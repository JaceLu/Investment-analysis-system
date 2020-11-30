using Qiniu.Storage;
using Qiniu.Util;
using Sail.Common;
using System;
using System.IO;

namespace Lamtip
{
    public class QiNiuService
    {
        public static readonly string Host = "http://images.yunbaowin.com";
        private static readonly Mac _mac = new Mac("rmt3BUbbMen1lny0tk1xol-Zcg_bpzCttIBgWhBG", "PzawdM40mPIb4bFk1Oxa5r-bcgf5tF228wjZyKLq");
        private static readonly string _scope = "image";

        public UploadResult Upload(Stream content, string fileName)
        {
            var result = CreateForm(out var token)
                .UploadStream(content, fileName, token, null);
            if (result.Code != 200)
                throw new Exception(result.ToJson());
            var fileInfo = new UploadResult
            {
                FilePath = $"{Host}/{fileName}",
                FileName = fileName
            };
            return fileInfo;
        }

        public UploadResult Upload(byte[] content, string fileName)
        {
            var result = CreateForm(out var token)
                .UploadData(content, fileName, token, null);
            if (result.Code != 200) throw new SailCommonException(result.ToJson());
            var fileInfo = new UploadResult
            {
                FilePath = $"{Host}/{fileName}",
                FileName = fileName
            };
            return fileInfo;
        }

        private static FormUploader CreateForm(out string token)
        {
            var putPolicy = new PutPolicy { Scope = _scope };
            putPolicy.SetExpires(3600);
            token = Auth.CreateUploadToken(_mac, putPolicy.ToJsonString());
            return new FormUploader(new Config());
        }
    }

    public class UploadResult
    {
        public string FilePath { set; get; }

        public string FileName { set; get; }
    }
}
