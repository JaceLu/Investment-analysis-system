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
    /// 大图
    /// </summary>
    public class Slideshow : SortableInfo, IModel, IMultiLanguages
    {
        /// <summary>
        /// 图片
        /// </summary>
        [HColumn]
        [Form(IsReadOnly = true, Tips = "建议宽高比例16：9,建议尺寸:1920*1080(px)")]
        public string ImageUrl { set; get; }

        /// <summary>
        /// 标题
        /// </summary>
        [HColumn]
        [Form]
        public string Title { set; get; }

        /// <summary>
        /// 副标题
        /// </summary>
        [HColumn]
        [Form]
        public string Subtitle { set; get; }

        [HColumn]
        public Languages Language { set; get; }

        public string LanguageStr => Language.ToString();

        public static IList<Slideshow> GetPictures()
        {
            using var db = new DataContext();
            Clip where = Clip.Where<Slideshow>(x => x.Language == I18N.CurrentLanguage);
            var OrderBy = Clip.OrderBy<Slideshow>(x => x.OrderByNo.Asc() && x.CreateTime.Desc());
            return db.GetList<Slideshow>(where, OrderBy);
        }
    }

}
