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
    /// 管理员表
    /// </summary>
    public class AdminController : BaseController<Admin>
    {
        /// <summary>
        /// 条件过滤
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip BaseWhere(string key)
        {
            return this.Where(x => x.UserName.Like(key));
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Login([FromForm]string str)
        {
            return HandleHelper.TryAction(db =>
            {
                var value = DynamicJson.Parse(str);
                var loginId = ((string)(value.LoginId)).Trim();
                var result = Common.Login<Admin>(db, loginId, value.Password.Trim());

                if (result.Status == Common.LoginStatus.成功)
                {
                    if (result.User.IsDisabled) throw new SailCommonException("该用户已停用");
                    var user = result.User as Admin;
                    WebHelper.CurrentAdmin = result.User;
                    return user;
                }
                throw new SailCommonException(result.Status.ToString());
            });
        }

        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Logout()
        {
            return HandleHelper.TryAction(() =>
            {
                WebHelper.CurrentAdmin = null;
            });
        }

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Enable([FromForm] string id)
        {
            return this.ActToModel(id, m => { m.IsDisabled = false; });
        }

        /// <summary>
        /// 停用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Disable([FromForm] string id)
        {
            return this.ActToModel(id, m => { m.IsDisabled = true; });
        }

        /// <summary>
        /// 用户密码重置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult ResetPwd([FromForm] string id)
        {
            return HandleHelper.TryAction(db =>
            {
                var model = this.GetModel(db, id);
                model.Password = Admin.DefaultPassword;
                db.Save(model);
            }, $"操作成功，密码已重置为:{Admin.DefaultPassword}");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult ChangePwd([FromForm] string value)
        {
            return HandleHelper.TryAction(db =>
            {
                var user = WebHelper.CurrentAdmin;
                IUser getUser() => user;
                var cpd = DynamicJson.Parse(value);
                string oldPwd = cpd.OldPwd.ToString();
                string newPwd = cpd.NewPwd.ToString();
                string rptPwd = cpd.RptPwd.ToString();
                new CheckValidate()
                .Check(() => WebHelper.CurrentAdmin.IsNull(), "请先登录")
                .Check(() => oldPwd.Equals(newPwd), "新密码不能和旧密码一样")
                .Check(() => !rptPwd.Equals(newPwd), "新密码不能和重复密码不一样")
                .Check(() => newPwd.Equals(Admin.DefaultPassword), "不能使用默认密码");
                Common.ChangePwd<Admin>(db, user.LoginId, cpd.OldPwd, cpd.NewPwd, (Func<IUser>)getUser);
            });
        }

        /// <summary>
        /// 调用方法之前的调用
        /// </summary>
        /// <param name="db"></param>
        /// <param name="e"></param>
        /// <param name="o"></param>
        protected override void BeforeAction(DataContext db, ControllerEvent e, object o)
        {
            switch (e)
            {
                case ControllerEvent.Put:
                    if (o is Admin model)
                    {
                        if (model.IsNew())
                            model.Password = Admin.DefaultPassword;
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetUserList(int pageIndex, int pageSize, string key, int status)
        {
            return HandleHelper.TryAction(db =>
            {
                var user = WebHelper.CheckAdmin<Admin>();
                var where = Where(x => x.UserId != ModelBase.DefaultId);//ModelHelper.Root
                where &= MakeWhere(key);
                if (status > -1) where &= Where(x => x.IsDisabled == (status != 0));
                return GetPageList(db, pageIndex, pageSize, where, this.OrderBy);
            });
        }

        /// <summary>
        /// 过滤超级管理员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult Ask()
        {
            return HandleHelper.TryAction(db =>
            {
                WebHelper.CurrentAdmin = WebHelper.CurrentAdmin;
            });
        }

        ///// <summary>
        ///// 修改当前后台语音
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public AjaxResult ChangeLan()
        //{
        //    return HandleHelper.TryAction(db => {
        //        switch (I18N.CurrentWEBLanguage) {
        //            case Languages.CHINESE_CN:
        //                I18N.SetCurrentWEBLang(Languages.ENGLISH);
        //                break;
        //            case Languages.ENGLISH:
        //                I18N.SetCurrentWEBLang(Languages.CHINESE_CN);
        //                break;
        //        }
        //    });
        //}

        /// <summary>
        /// 修改当前后台语音为中文
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult ChangeLanCN()
        {
            return HandleHelper.TryAction(db => {
                I18N.SetCurrentWEBLang(Languages.CHINESE_CN);
            });
        }

        /// <summary>
        /// 修改当前后台语音为英文
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult ChangeLanEN()
        {
            return HandleHelper.TryAction(db => {
                I18N.SetCurrentWEBLang(Languages.ENGLISH);
            });
        }
    }
}
