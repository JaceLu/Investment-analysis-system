using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Investment.Code;
using Investment.Models.Abstract;
using NPOI.OpenXmlFormats.Shared;
using Sail.Common;

namespace Investment.Models
{
    /// <summary>
    /// 新闻资讯
    /// </summary>
    public class News : SortableInfo, IModel, IMultiLanguages
    {
        /// <summary>
        /// 类型
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public Category Category { set; get; }
       
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
        [Form]
        public string Description { set; get; }

        /// <summary>
        /// 内容
        /// </summary>
        [HColumn]
        public string Content { set; get; }

        /// <summary>
        /// 图片
        /// </summary>
        [HColumn]
        [Form(Tips = "新闻管理图片：建议宽高比例3：2,建议尺寸:300*200(px); 展会服务图片：建议宽高比例10：16,建议尺寸:300*480(px)")]
        public string ImageUrl { set; get; }

        /// <summary>
        /// 时间
        /// </summary>
        [HColumn]
        [Form(IsRequired = true,Class = "datetime")]
        public DateTime? PublishTime { set; get; }
        public string PublishDate => PublishTime?.ToString("yyyy-MM-dd");

        /// <summary>
        /// 作者
        /// </summary>
        [HColumn]
        public string Author { set; get; }

        /// <summary>
        /// 外链路径
        /// </summary>
        [HColumn]
        public string OutUrl { set; get; }

        /// <summary>
        /// 使用外链路径
        /// </summary>
        [HColumn]
        [Form(Tips = "默认不勾选使用内容，勾选则选择使用外链路径内容")]
        public bool IsOutUrl { get; set; }

        public string Url
        {
            get
            {
                if (OutUrl.IsNotNull()) return OutUrl;
                var pageUrl = "";
                switch (this.Category.PageCategory) {
                    case PageCategory.News:
                        pageUrl = $"article?id={this.Id}&category={this.Category.Id.ToLower()}&parentcate={this.Category.ParentId}";
                        break;
                    case PageCategory.Navigation:
                        pageUrl  = $"Survey?id={this.Id}&type={this.Category.Id.ToLower()}";
                        break;
                }
                return pageUrl;
            }
        }

        /// <summary>
        /// 语言
        /// </summary>
        [HColumn]
        public Languages Language { set; get; }

        public static IList<News> NewsList(string type)
        {
            using var db = new DataContext();
            var order = Clip.OrderBy< News >(x=>x.OrderByNo.Asc()&&x.CreateTime.Desc());
            var where = Clip.Where<News>(x => x.Language == I18N.CurrentLanguage && x.Category.Id == type);
            return db.GetList<News>(where, order);
        }

        public static News Detail(string id)
        {
            using var db = new DataContext();
            return db.GetModelById<News>(id);
        }

        public static IList<News> GetService(string category)
        {
            using var db = new DataContext();
            var where = Clip.Where<News>(x => x.Category.Id == category && x.ImageUrl != "");
            var order = Clip.OrderBy<News>(x => x.OrderByNo.Asc() && x.CreateTime.Desc());
            return db.GetList<News>(where, order).Take(3).ToList();
        }

        public static IList<News> GetNewsList(string category)
        {
            using var db = new DataContext();
            var where = Clip.Where<News>(x => x.Category.ParentId == category);
            var OrderBy = Clip.OrderBy<News>(x => x.PublishTime.Desc());
            return db.GetList<News>(where, OrderBy);
        }

        public static IList<News> GetNewsPictureList(string category)
        {
            using var db = new DataContext();
            var where = Clip.Where<News>(x => x.Category.ParentId == category);
            var OrderBy = Clip.OrderBy<News>(x => x.PublishTime.Desc());
            where &= Clip.Where<News>(x => x.ImageUrl != "");
            return db.GetList<News>(where, OrderBy).Take(3).ToList();
        }
    }
}
