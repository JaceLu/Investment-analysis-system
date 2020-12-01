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
    /// 预约留言
    /// </summary>
    public class AppointmentMessage : SortableInfo, IModel, IMultiLanguages
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string Name { set; get; }

        /// <summary>
        /// 电话
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string Tel { set; get; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [HColumn]
        [Form(IsRequired = true,Class ="date")]
        public DateTime? StartTime { set; get; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [HColumn]
        [Form(Class = "date")]
        public DateTime? EndTime { set; get; }

        /// <summary>
        /// 预约类型
        /// </summary>
        [HColumn]
        public AppointmentType Type { set; get; }

        /// <summary>
        /// 预约类型
        /// </summary>
        public string TypeStr => Type.ToString();

        /// <summary>
        /// 场馆
        /// </summary>
        [HColumn]
        [ModelData("prop", "Id")]
        [Form(IsRequired = true)]
        public List<Dict> Pavilion { set; get; }

        /// <summary>
        /// 展馆名称
        /// </summary>
        public string PavilionName => Pavilion?.Select(x => x.Name).JoinToString();

        /// <summary>
        /// 留言
        /// </summary>
        [HColumn]
        public string Message { set; get; }
        /// <summary>
        /// 语言
        /// </summary>
        [HColumn]
        public Languages Language { get; set; }
    }

    /// <summary>
    /// 预约类型
    /// </summary>
    public enum AppointmentType
    {
        场馆档期预约,
        展馆考察预约
    }
}
