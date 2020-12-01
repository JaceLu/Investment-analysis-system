using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Investment.Code;
using Investment.Models.Abstract;
using Sail.Common;

namespace Investment.Models
{
    /// <summary>
    /// 历次展会
    /// </summary>
    public class HistoryShows : SortableInfo, IModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string Title { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string Description { set; get; }

        /// <summary>
        /// 内容
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string Content { set; get; }

        /// <summary>
        /// 展馆名称
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string PavilionName { set; get; }

        /// <summary>
        /// 图片
        /// </summary>
        [HColumn]
        public string ImageUrl { set; get; }

        /// <summary>
        /// 展出时间
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public DateTime? ShowsTime { set; get; }

        /// <summary>
        /// 展出结束时间
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public DateTime? ShowsEndTime { set; get; }


        [HColumn]
        public Languages Language { set; get; }
    }
}
