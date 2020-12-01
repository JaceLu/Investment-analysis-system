using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Investment.Models;
using Sail.Common;
using Sail.Web;
using Investment.Code;

namespace Investment.Apis
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class UserController : BaseController<User>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip BaseWhere(string key)
        {
            return this.Where(x => x.UserName.Like(key) || x.LoginId.Like(key)).Bracket();
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
                WebHelper.CurrentUser = WebHelper.CurrentUser;
                return WebHelper.CurrentUser;
            });
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult Login([FromForm] string str)
        {
            return HandleHelper.TryAction(db =>
            {
                var value = DynamicJson.Parse(str);
                string loginId = value.LoginId.ToString();
                string password = value.Password.ToString();

                Clip where = Clip.Where<User>(x => x.LoginId == loginId);
                var User = db.GetModel<User>(where);
                //if (User.IsNull()) throw new SailCommonException(I18N.GetText("AccountNot"));
                //else if (User?.Password != password.ToMd5().Encrypt()) throw new SailCommonException(I18N.GetText("WrongPassword"));
                WebHelper.GetRealIp();
                WebHelper.CurrentUser = User as User;
                return User;
            });
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
                var user = WebHelper.CurrentUser;
                IUser getUser() => user;
                var cpd = DynamicJson.Parse(value);
                string oldPwd = cpd.OldPwd.ToString();
                string newPwd = cpd.NewPwd.ToString();
                string rptPwd = cpd.RptPwd.ToString();
                new CheckValidate()
                .Check(() => WebHelper.CurrentUser.IsNull(), "请先登录")
                .Check(() => !rptPwd.Equals(newPwd), "新密码不能和重复密码不一样");
                Common.ChangePwd<User>(db, user.LoginId, cpd.OldPwd, cpd.NewPwd, (Func<IUser>)getUser);
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
                WebHelper.CurrentUser = null;
            });
        }
        /// <summary>
        /// 修改语言包
        /// </summary>
        [HttpPost]
        public AjaxResult UpDateLangluage(int langluage )
        {
            return HandleHelper.TryAction(db => {
                I18N.SetCurrentLang((Languages)langluage);
                //PageConfig.Config(PageConfig.Get(db));
            });
        }

       
    }
}
