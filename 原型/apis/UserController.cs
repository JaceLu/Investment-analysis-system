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
    /// 用户
    /// </summary>
    public class UserController : BaseController<User>
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
                var result = Common.Login<User>(db, loginId, value.Password.Trim());
                if (result.Status == Common.LoginStatus.成功)
                {
                    if (result.User.IsDisabled) throw new SailCommonException("该用户已停用"); 
                    var user = result.User as User;
                    WebHelper.CurrentUser = result.User;
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
                WebHelper.CurrentUser = null;
            });
        }

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Enable([FromForm]string id)
        {
            return this.ActToModel(id, m => { m.IsDisabled = false; });
        }

        /// <summary>
        /// 停用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Disable([FromForm]string id)
        {
            return this.ActToModel(id, m => { m.IsDisabled = true; });
        }

        /// <summary>
        /// 用户密码重置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult ResetPwd([FromForm]string id)
        {
            return HandleHelper.TryAction(db =>
            {
                var model = this.GetModel(db, id);
                model.Password = Lamtip.Model.User.DefaultPassword;
                db.Save(model);
            }, $"操作成功，密码已重置为:{Lamtip.Model.User.DefaultPassword}");
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult changePwd([FromForm]string value)
        {
            return HandleHelper.TryAction(db =>
            {
                var user = WebHelper.CurrentUser;
                Func<IUser> getUser = () => user;
                var cpd = DynamicJson.Parse(value);
                string oldPwd = cpd.OldPwd.ToString();
                string newPwd = cpd.NewPwd.ToString();
                string rptPwd = cpd.RptPwd.ToString();
                new CheckValidate()
                .Check(() => WebHelper.CurrentUser.IsNull(), "请先登录")
                .Check(() => oldPwd.Equals(newPwd), "新密码不能和旧密码一样")
                .Check(() => !rptPwd.Equals(newPwd), "新密码不能和重复密码不一样")
                .Check(() => newPwd.Equals(Lamtip.Model.User.DefaultPassword), "不能使用默认密码");
                Common.ChangePwd<User>(db, user.LoginId, cpd.OldPwd, cpd.NewPwd, getUser);
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
            var model = o as User;
            switch (e)
            {
                case ControllerEvent.Put:
                    if (model != null)
                    {
                        if (model.IsNew())
                            model.Password = Lamtip.Model.User.DefaultPassword;
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
        public AjaxResult GetList(int pageIndex, int pageSize, string key, int status)
        {
            return HandleHelper.TryAction(db =>
            {
                var where = this.Where(x => x.UserId != ModelBase.DefaultId);//ModelHelper.Root
                where &= MakeWhere(key);
                if (status > -1)
                {
                    where &= this.Where(x => x.IsDisabled == (status != 0));
                }
                return this.GetPageList(db, pageIndex, pageSize, where, this.OrderBy);
            });
        }
    }
}
