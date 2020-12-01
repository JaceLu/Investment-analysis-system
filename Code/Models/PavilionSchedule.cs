using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sail.Common;
using Investment.Models.Abstract;
using Investment.Code;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Investment.Models
{
    /// <summary>
    /// 展览日程
    /// </summary>
    public class PavilionSchedule : SortableInfo, IModel
    {
        /// <summary>
        /// 展会名称
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string ExhibitionName { set; get; }

        /// <summary>
        /// 展馆名称
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
        /// 开始时间
        /// </summary>
        [HColumn]
        [Form(IsRequired = true, Class = "datetime")]
        public DateTime? StartTime { set; get; }

        /// <summary>
        /// 开始年份
        /// </summary>
        public string StartYear => StartTime?.ToString("yyyy");
        public string StartMonthDay => StartTime?.ToString("MM.dd");

        /// <summary>
        /// 结束时间
        /// </summary>
        [HColumn]
        [Form(Class = "datetime")]
        public DateTime? EndTime { set; get; }
        public string EndMonthDay => EndTime?.ToString("MM.dd");

        public string Url
        {
            get
            {
                return $"ScheduleDetail?id={Id}";
            }
        }
        [HColumn]
        public Languages Language { set; get; }

        /// <summary>
        /// 内容
        /// </summary>
        [HColumn]
        public string Content { set; get; }

        /// <summary>
        /// 图片
        /// </summary>
        [HColumn]
        [Form(Tips = "建议宽高比例16：9,建议尺寸:800*450(px)")]
        public string ImageUrl { set; get; }

        public static PavilionSchedule GetInfo()
        {
            using var db = new DataContext();
            var today = DateTime.Now;
            Clip where = Clip.Where<PavilionSchedule>(x => x.Language == I18N.CurrentLanguage);
            where &= Clip.Where<PavilionSchedule>(x => x.StartTime > today);
            var OrderBy = Clip.OrderBy<PavilionSchedule>(x => x.StartTime.Asc());
            return db.GetTopModel<PavilionSchedule>(where, OrderBy);
        }
    }
}
