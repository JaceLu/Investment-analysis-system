using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sail.Common;
using Investment.Models.Abstract;
using Investment.Code;
using System.Security.Permissions;
using Investment.Models.Interface;
using System.Data;

namespace Investment.Models
{
    public enum PageCategory
    {
        News,
        Navigation,
        Schedule
    }

    /// <summary>
    /// 类别
    /// </summary>
    public class Category : SortableInfo, IModel, IMultiLanguages,ITree
    {
        /// <summary>
        /// 编号
        /// </summary>
        [HColumn(Length = 20)]
        [Form(IsRequired = true )]
        public string Code { set; get; }

        /// <summary>
        /// 上级类别
        /// </summary>
        [HColumn(Length = 200)]
        public string ParentId { set; get; }
        /// <summary>
        /// 上级类别
        /// </summary>
        [HColumn(Length = 200)]
        public string ParentName { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string Name { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        [HColumn]
        public string Description { set; get; }

        /// <summary>
        /// 说明
        /// </summary>
        [HColumn]
        public string Demo { set; get; }

        /// <summary>
        /// 大图
        /// </summary>
        [HColumn]
        [Form(Tips = "建议宽高比例16：5,建议尺寸:1920*600(px)")]
        public string ImgUrl { set; get; }

        /// <summary>
        /// 大图主标题
        /// </summary>
        [HColumn]
        public string ImgTitle { set; get; }

        /// <summary>
        /// 大图副标题
        /// </summary>
        [HColumn]
        public string ImgSubTitle { set; get; }
        /// <summary>
        /// 页面类型
        /// </summary>
        [HColumn]
        [Form(IsRequired = true,Tips = "News(新闻，动态，公告),Navigation(概况，服务，关于),Schedule(日程)")]
        public PageCategory PageCategory { set; get; }
        /// <summary>
        /// 页面类型
        /// </summary>
        public string PageCategoryStr => PageCategory.ToString();
        /// <summary>
        /// 图标
        /// </summary>
        [HColumn(Length = 500)]
        public string Icon { set; get; }

       
        /// <summary>
        /// 视频vid
        /// </summary>
        [HColumn(Length = 500)]
        [Form(Tips = "填写腾讯点播视频vid")]
        public string VideoUrl { set; get; }

        public string Url
        {
            get
            {
                var pages = new Dictionary<PageCategory, string> {
                    { PageCategory.News,"/news" },
                    { PageCategory.Navigation,"/Survey" },
                    { PageCategory.Schedule,"/Schedule" }
                };
                return $"{pages[this.PageCategory]}?type={this.Id}&title={this.Name}";
            }
        }

        public List<Category> SubItem { set; get; }

        /// <summary>
        /// 语言
        /// </summary>
        [HColumn(IsNotNeedUniqueness = true)]
        public Languages Language { get; set; }

        /// <summary>
        /// 获取父节点下的子节点
        /// </summary>
        /// <returns></returns>
        public static List<Category> CategoryByParent(string parentId)
        {
            using var db = new DataContext();
            return db
            .GetList<Category>(x => x.Language == I18N.CurrentLanguage && x.ParentId == parentId, x => x.OrderByNo.Asc() && x.CreateTime.Desc())
            .ToList();
        }


        /// <summary>
        /// 获取所有字典
        /// </summary>
        /// <returns></returns>
        public static List<Category> Categorys()
        {
            using var db = new DataContext();
            return db
            .GetList<Category>(x => x.Language==I18N.CurrentLanguage && x.ParentId == "", x => x.OrderByNo.Asc() && x.CreateTime.Desc())
            .ToList();
        }
        public static List<Category> CategorysByWeb(int type)
        {
            using var db = new DataContext();
            var order = Clip.OrderBy<Category>(x => x.OrderByNo.Asc() && x.CreateTime.Desc());
            var where = Clip.Where<Category>(x => x.Language == I18N.CurrentWEBLanguage&& x.ParentId=="" && x.PageCategory==(PageCategory)type);
            var list = db
            .GetList<Category>(where, order)
            .ToList();
            list.ForEach(x =>
            {
                Clip itemWhere = Clip.Where<Category>(t => t.ParentId == x.Id);
                x.SubItem = db.GetList<Category>(itemWhere, order).ToList();
            });
            return list;
        }
        /// <summary>
        /// 获取news页面类型的数据
        /// </summary>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> ParentNodes()
        {
            using var db = new DataContext();
            Clip where = Clip.Where<Category>(x => x.Language == I18N.CurrentWEBLanguage && x.PageCategory == PageCategory.News);
            Clip order = Clip.OrderBy<Category>(x => x.OrderByNo.Asc() && x.CreateTime.Desc());
            return db
            .GetList<Category>(where, order)
            .Select(x=>new KeyValuePair<string,string>(x.Id,x.Name))
            .ToList().Prepend(new KeyValuePair<string, string>("", "请选择上级类别")).ToList();
        }

        public static Category GetNews(string code)
        {
            using var db = new DataContext();
            return db.GetModel<Category>(x => x.Code == code&&x.Language==I18N.CurrentLanguage);
        }

        public static Category GetNewsById(string id)
        {
            using var db = new DataContext();
            return db.GetModelById<Category>(id);
        }
    }
}
