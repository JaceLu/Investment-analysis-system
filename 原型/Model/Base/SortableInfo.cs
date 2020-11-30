using Sail.Common;
using System;

namespace Lamtip.Model
{
    /// <summary>
    /// 可排序对象
    /// </summary>
    public class SortableInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string Id { set; get; }


        /// <summary>
        /// 排序号
        /// </summary>
        [HColumn(Remark = "排序号", Length = 14, Precision = 2)]
        [Form(IsRequired = true, CustomValidate = "decimal")]
        public virtual decimal OrderByNo { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [HColumn]
        public DateTime CreateTime { set; get; }
    }


}