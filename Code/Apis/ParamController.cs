using Microsoft.AspNetCore.Mvc;
using Sail.Common;
using Sail.Web;
using Investment.Models;
using Investment.Code;

namespace Investment.Apis
{
    /// <summary>
    /// 参数
    /// </summary>
    public class ParamController : BaseController<Param>
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
                var user = WebHelper.CheckAdmin<Admin>();
                if (!user.IsSuperAdmin) throw new SailCommonException("只有管理员才能设置");
                var model = Param.Default;
                model = db.LoadModelByJson<Param>(value, model.Id);
                db.Save(model);
                Param.Config(model);
                return model;
            });
        }


        [HttpPost]
        public AjaxResult SaveVisitors()
        {
            return HandleHelper.TryAction(db =>
            {
                var model = Param.Default;
                model = db.GetModelById<Param>(model.Id);
                if (model.CoefficientValues <= 0)
                {
                    model.CoefficientValues = 1;
                }
                model.Visitors = model.Visitors + model.CoefficientValues;
                db.Save(model);
                return model.Visitors + model.BaseValue;
            });
        }

        [HttpGet]
        public AjaxResult GetBaseValue()
        {
            return HandleHelper.TryAction(db =>
            {
                var model = Param.Default;
                model = db.GetModelById<Param>(model.Id);
                return model.Visitors + model.BaseValue;
            });
        }
    }
}
