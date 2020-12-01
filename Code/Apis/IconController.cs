using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sail.Common;
using Sail.Web;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Investment.Apis
{
    /// <summary>
    /// 图片上传
    /// </summary>
    public class IconController : BaseUploadController
    {
        /// <summary>
        /// 上传目录
        /// </summary>
        protected override string UploadRoot => $"~/uploads/icons/{DateTime.Now.ToString("yyyyMM")}/";

        /// <summary>
        /// 缩略图目录
        /// </summary>
        protected virtual string UploadPre => $"~/uploads/icons/{DateTime.Now.ToString("yyyyMM")}/Pre/";

        [Inject]
        public ILogger<IconController> Logger { set; get; }

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

            var filedb = new
            {
                FilePath = this.UploadRoot.Substring(1) + filename,
                FileName = filename
            };
            return filedb;
        }

        [RequestSizeLimit(100_000_000_000)]
        public override Task<AjaxResult> UploadFile()
        {

            return base.UploadFile();
        }
    }
}
