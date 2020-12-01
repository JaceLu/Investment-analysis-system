using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sail.Common;
using Investment.Models.Abstract;
using Investment.Code;
using System.Security.Permissions;
using Investment.Models.Interface;
using Sail.Web;

namespace Investment.Models
{
    /// <summary>
    /// 首页设置
    /// </summary>
    public class PageConfig : ModelBase, IModel, IMultiLanguages
    {
        /// <summary>
        /// 
        /// </summary>
        [HColumn]
        public Languages Language { set; get; }

        public string LanguageStr => Language.ToString();

        /// <summary>
        /// 网页顶部彩色Logo
        /// </summary>
        [HColumn]
        public string ColourfulLogo { set; get; }

        /// <summary>
        /// 网页顶部白色Logo
        /// </summary>
        [HColumn]
        public string WhiteLogo { set; get; }

        /// <summary>
        /// 网站底部Logo
        /// </summary>
        [HColumn]
        public string BottomLogo { set; get; }

        /// <summary>
        /// 微信二维码
        /// </summary>
        [HColumn]
        public string WXErcode { set; get; }

        /// <summary>
        /// 客服QQ
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string QQ { set; get; }

        /// <summary>
        /// 网站名称
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string SiteName { set; get; } 

        /// <summary>
        /// 地址
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string Location { set; get; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string Phone { set; get; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public List<string> Phones
        {
            get
            {
                return Phone.IsNull() ? new List<string>() : Phone.Split("\n").ToList();
            }
        }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string Mail { set; get; }

        ///// <summary>
        ///// 备案号
        ///// </summary>
        //[HColumn]
        //[Form(IsRequired = true)]
        //public string Icp { set; get; }

        /// <summary>
        /// 版权名称
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string Copyright { set; get; }

        

        /// <summary>
        /// 网站默认设置
        /// </summary>
        public static PageConfig WEBDefault
        {
            get;
            private set;
        } = initWEB();

        public static PageConfig initWEB()
        {
            using var db = new DataContext();
            return GetWEB(db);
        }
        public static void WEBConfig(PageConfig config)
        {
            WEBDefault = config;
        }

        /// <summary>
        /// 获取默认参数
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static PageConfig GetWEB(IDataContext db)
        {
            return
                db.GetModel<PageConfig>(Clip.Where<PageConfig>(x => x.Id != NewId && x.Language ==I18N.CurrentWEBLanguage)) ?? new PageConfig
                {
                    SiteName = "芜湖宜居国际博览中心有限公司",
                    Language = Languages.CHINESE_CN
                };
        }

        /// <summary>
        /// 获取默认参数
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static PageConfig Get()
        {
            using var db = new DataContext();
            return db.GetModel<PageConfig>(Clip.Where<PageConfig>(x => x.Language == I18N.CurrentLanguage));
        }
    }
}
