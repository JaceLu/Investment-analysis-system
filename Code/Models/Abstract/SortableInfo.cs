using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sail.Common;

namespace Investment.Models.Abstract
{
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
