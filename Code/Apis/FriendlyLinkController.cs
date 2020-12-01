using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Investment.Code;
using Investment.Models;
using Microsoft.AspNetCore.Mvc;
using Sail.Common;
using Sail.Web;

namespace Investment.Apis
{
    /// <summary>
    /// 友链控制器
    /// </summary>
    public class FriendlyLinkController : BaseController<FriendlyLink>
    {
        /// <summary>
        /// 筛选查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip BaseWhere(string key)
        {
            return this.Where(x => x.OrderByNo.Like(key) || x.Name.Like(key)).Bracket();
        }

        /// <summary>
        ///排序
        /// </summary>
        protected override Clip OrderBy => this.Order(x => x.OrderByNo.Asc());

        /// <summary>
        /// 获取数据（后台）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetListInfo(int pageIndex, int pageSize, string key)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = this.MakeWhere(key);
                where &= this.Where(x => x.Language == I18N.CurrentWEBLanguage);
                return db.GetPageList<FriendlyLink>(pageIndex, pageSize, where, OrderBy);
            });
        }

        /// <summary>
        /// 前台获取数据（根据语言)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetLinks()
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = this.Where(x => x.Language == I18N.CurrentLanguage);
                var OrderBy = Clip.OrderBy<FriendlyLink>(x => x.OrderByNo.Asc() && x.CreateTime.Asc());
                return db.GetList<FriendlyLink>(where, OrderBy);
            });
        }
    }
}
