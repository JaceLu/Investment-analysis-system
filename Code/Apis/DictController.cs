using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sail.Common;
using Sail.Web;
using Investment.Models;
using Investment.Code;

namespace Investment.Apis
{
    /// <summary>
    /// 字典设置
    /// </summary>
    public class DictController : BaseController<Dict>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip BaseWhere(string key)
        {
            return this.Where(x => x.Name.Like(key));
        }

        /// <summary>
        /// 
        /// </summary>
        protected override Clip OrderBy => this.Order(x =>x.OrderByNo.Asc()&& x.CreateTime.Desc());

        /// <summary>
        /// 获取信息类型
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetInfoType(int pageIndex, int pageSize, string key, DictType type,int status)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = MakeWhere(key);
                where &= this.Where(x => x.Language == I18N.CurrentWEBLanguage);
                if(type.IsNotNull()) where &= this.Where(x => x.Type == type);
                if(status>-1) where &= this.Where(x => x.Status == (InfoStatus)status);
                return db.GetPageList<Dict>(pageIndex, pageSize, where, OrderBy);
            });
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Enable([FromForm] string id)
        {
            return this.ActToModel(id, m => { m.Status = InfoStatus.正常; });
        }

        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Disable([FromForm] string id)
        {
            return HandleHelper.TryAction(db =>
            {
                return this.ActToModel(id, m => { m.Status = InfoStatus.停用; });
            });
        }
    }
}
