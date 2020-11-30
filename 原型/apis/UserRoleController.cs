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
    /// 角色权限
    /// </summary>
    public class UserRoleController : BaseController<UserRole>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip BaseWhere(string key)
        {
            return this.Where(x => x.RoleName.Like(key)).Bracket();
        }

        /// <summary>
        /// 过滤超级管理员
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public override AjaxResult GetList(int pageIndex, int pageSize, string key)
        {
            return HandleHelper.TryAction(db =>
            {
                var where = MakeWhere(key);
                where &= Clip.Where<UserRole>(x => x.Id != UserRole.DefaultId);//ModelHelper.Root
                return db.GetPageList<UserRole>(pageIndex, pageSize, where, Clip.OrderBy<UserRole>(x => x.CreateTime.Desc()));
            });
        }


        /// <summary>
        /// 获取角色权限菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult Menus()
        {
            return HandleHelper.TryAction(db =>
            {
                var menuText = FileHelper.ReadTextFile("~/config/menu.json".FullFileName());
                return menuText.JsonToObj<List<HMenuItem>>().ToList();
            });
        }

        /// <summary>
        /// 删除角色前，进行数据检查
        /// </summary>
        /// <param name="db"></param>
        /// <param name="e"></param>
        /// <param name="o"></param>
        //protected override void BeforeAction(DataContext db, ControllerEvent e, object o)
        //{
        //    if (e == ControllerEvent.Delete)
        //    {
        //        var model = o as UserRole;
        //        var where = nameof(User.Role).ToField().Contains(model.Id) & nameof(User.IsDisabled).ToField() == false;
        //        if (db.Any<User>(where))
        //        {
        //            throw new SailCommonException("操作失败！此角色正在使用中");
        //        }
        //    }
        //}
    }
}
