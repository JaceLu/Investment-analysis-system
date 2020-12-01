using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sail.Common;
using Investment.Models.Abstract;
using Investment.Code;
using System.Security.Permissions;
using Investment.Models.Interface;

namespace Investment.Models
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public class FriendlyLink : BaseDict
    {
        /// <summary>
        /// 名称
        /// </summary>
        [HColumn(Length = 200)]
        public new string Name { set; get; }

        [HColumn]
        public Languages Language { set; get; }

        /// <summary>
        /// 友链地址
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string Url { set; get; }

        /// <summary>
        /// 排序号
        /// </summary>
        [HColumn(Remark = "排序号", Length = 14, Precision = 2)]
        [Form(CustomValidate = "decimal")]
        public virtual decimal OrderByNo { set; get; }

        /// <summary>
        /// logo
        /// </summary>
        [HColumn]
        [Form(IsRequired = true,Tips = "建议宽高比例3：1,建议尺寸:600*200(px)")]
        public string ImageUrl { set; get; }

        public static IList<FriendlyLink> GetLinks()
        {
            using var db = new DataContext();
            var order = Clip.OrderBy<FriendlyLink>(x => x.OrderByNo.Asc() && x.CreateTime.Asc());
            var where = Clip.Where<FriendlyLink>(x => x.Language == I18N.CurrentLanguage);
            return db.GetList<FriendlyLink>(where, order);
        }
    }
}
