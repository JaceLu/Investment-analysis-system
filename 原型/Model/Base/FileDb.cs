using Sail.Common;
using System;

namespace Lamtip.Model
{

    /// <summary>
    /// 文件表
    /// </summary>
    public class FileDb : IModel
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string FileDbId { set; get; }

        /// <summary>
        /// 文件名
        /// </summary>
        [HColumn(Length = 200)]
        public string FileName { set; get; }

        /// <summary>
        /// 显示文件名
        /// </summary>
        [HColumn(Length = 200)]
        public string DisPlayName { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [HColumn]
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [HColumn(Length = 200)]
        public string FilePath { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            FileDb o = obj as FileDb;
            if (o == null)
                return false;
            return this.FileDbId.Equals(o.FileDbId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.FileDbId.GetHashCode();
        }
    }
}