using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sail.Common;
using Sail.Web;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Investment.Models;

namespace Investment.Apis
{
    public class FileController : BaseUploadController
    {
        /// <summary>
        /// 上传文件默认路径，可以重写，默认是"~/uploads/yyyyMM/"
        /// </summary>
        /// <value>The upload root.</value>
        protected override string UploadRoot => $"~/uploads/{DateTime.Now:yyyyMM}/";

        protected virtual string UploadPre => $"~/uploads/{DateTime.Now:yyyyMM}/Pre/";

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override async Task<dynamic> Uploaded(IFormFile file, HttpRequest request)
        {
            var name = Guid.NewGuid().ToString("N");
            var ext = Path.GetExtension(file.FileName);
            var filename = "{0}{1}".FormatWith(name, ext);
            var webRootPath = this._env.WebRootPath;
            var root = webRootPath + this.UploadRoot.Replace("~", "");
            var filePath = Path.Combine(root, filename);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var filedb = new FileDb { DisPlayName = file.FileName, FileName = file.FileName, FilePath = $"{this.UploadRoot.Substring(1)}{filename}" };
            using (var db = new DataContext())
            {
                db.Save(filedb);
            }
            return filedb;
        }
        [RequestSizeLimit(1000_000_000)]
        public override Task<AjaxResult> UploadFile()
        {
            return base.UploadFile();
        }
        [HttpPost]
        public async Task<object> WangUploadFile()
        {
            var webRootPath = this._env.WebRootPath;
            var root = webRootPath + this.UploadRoot.Replace("~", "");
            root.CreateDir();
            try
            {
                var file = this.Request.Form.Files[0];
                var obj = await this.Uploaded(file, this.Request);
                return obj;
            }
            catch (System.Exception e)
            {
                return this.GetFailResult(e);
            }
        }



        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public FileResult DownLoad(string id)
        {
            using var db = new DataContext();
            var model = db.GetModelById<FileDb>(id);
            if (model.IsNull()) throw new SailCommonException("无效文件");
            var webRootPath = this._env.WebRootPath;
            var FilePath = webRootPath + model.FilePath.FullFileName().Replace("~", "");
            var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/octet-stream", model.FileName);
        }

        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Rename(string id, [FromBody] string value)
        {
            return HandleHelper.TryAction(db =>
            {
                var img = db.GetModelById<FileDb>(id);
                img.DisPlayName = value;
                db.Save(img);
                return img;
            });
        }
    }
}
