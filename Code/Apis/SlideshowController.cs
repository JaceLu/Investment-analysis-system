using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Investment.Code;
using Investment.Models;
using Microsoft.AspNetCore.Mvc;
using Sail.Common;
using Sail.Web;

namespace Investment.Apis
{
    /// <summary>
    /// 首页大图控制器
    /// </summary>
    public class SlideshowController : BaseController<Slideshow> 
    {
        /// <summary>
        /// 筛选查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip BaseWhere(string key)
        {
            return this.Where(x => x.Title.Like(key));
        }

        /// <summary>
        ///排序
        /// </summary>
        protected override Clip OrderBy => this.Order(x => x.OrderByNo.Asc());

        /// <summary>
        ///  
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetListInfo(int pageIndex, int pageSize, string key)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = this.MakeWhere(key);
                where &= this.Where(x => x.Language ==I18N.CurrentWEBLanguage);
                return db.GetPageList<Slideshow>(pageIndex, pageSize, where, OrderBy);
            });
        }

        /// <summary>
        /// 前台获取轮播图数据（根据语言）
        /// </summary>
        /// <returns></returns>
        public AjaxResult GetPictures()
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = this.Where(x => x.Language == I18N.CurrentLanguage);
                var OrderBy = Clip.OrderBy<Slideshow>(x => x.OrderByNo.Asc() && x.CreateTime.Desc());
                return db.GetList<Slideshow>(where, OrderBy);
            });
        }
    }
}
