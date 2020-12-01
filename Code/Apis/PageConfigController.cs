using Investment.Code;
using Investment.Models;
using Microsoft.AspNetCore.Mvc;
using Sail.Common;
using Sail.Web;

namespace Investment.Apis
{
    /// <summary>
    /// 关于我们控制器
    /// </summary>
    public class PageConfigController : BaseController<PageConfig>
    {
        /// <summary>
        /// 筛选查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip BaseWhere(string key)
        {
            return this.Where(x => x.SiteName.Like(key));
        }

        /// <summary>
        ///排序
        /// </summary>
        protected override Clip OrderBy => this.Order(x => x.CreateTime.Asc());

        /// <summary>
        /// 获取数据（后台）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetInfo()
        {
            return HandleHelper.TryAction(db =>
            {
                var info = db.GetModel<PageConfig>(x => x.Language == I18N.CurrentWEBLanguage);
                return info;
            });
        }

        ///// <summary>
        ///// 获取数据（前台）
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public AjaxResult GetAboutUs() 
        //{
        //    return HandleHelper.TryAction(db =>  PageConfig.Default);
        //}

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Save(string id, [FromForm] string value)
        {
            return HandleHelper.TryAction(db =>
            {
                var model = db.LoadModelByJson<PageConfig>(value, id);
                db.Save(model);
                PageConfig.WEBConfig(model);
                return model;
            });
        }
    }
}
