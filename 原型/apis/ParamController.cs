using Microsoft.AspNetCore.Mvc;
using Sail.Common;
using Sail.Web;
using Lamtip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lamtip.apis
{
    /// <summary>
    /// 参数
    /// </summary>
    public class ParamController:BaseController<Param>
    {
        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetParam()
        {
            return HandleHelper.TryAction(db => Param.Default);
        }
        /// <summary>
        /// 修改系统参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Save([FromForm] string value)
        {
            return HandleHelper.TryAction(db =>
            {
                var user= WebHelper.CheckUser<User>();
                if (!user.IsSuperAdmin) throw new SailCommonException("只有管理员才能设置");
                var model = Param.Default;
                model = db.LoadModalByJson<Param>(value, model.Id);
                db.Save(model);
                Param.Config(model);
                return model;
            });
        }
    }
}
