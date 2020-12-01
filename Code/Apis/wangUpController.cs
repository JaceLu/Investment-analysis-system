using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sail.Common;
using Sail.Web;
using System.IO;

namespace Investment.Apis
{
    /// <summary>
    /// 
    /// </summary>
    public class wangUpController : BaseUploadController
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
            var filex = new FileInfo(file.FileName);
            var ext = Path.GetExtension(file.FileName);
            var filename = "{0}{1}".FormatWith(name, ext);
            var webRootPath = this._env.WebRootPath;
            var root = webRootPath + this.UploadRoot.Replace("~", "");
            var filePath = Path.Combine(root, filename);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return $"{this.UploadRoot.Substring(1)}{filename}";
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
    }
}
