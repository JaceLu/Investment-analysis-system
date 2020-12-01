using Microsoft.AspNetCore.Mvc;
using Investment.Code;
using Investment.Models;
using Sail.Common;
using Sail.Web;
using System.Collections.Generic;
using System.Linq;

namespace Investment.Apis
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class RoleController : BaseController<Role>
    {
        /// <summary>
        /// 获取角色权限菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult Menus() => HandleHelper.TryAction(db =>
        {
            var filename = $"~/config/menu.json";
            var menuText = FileHelper.ReadTextFile(filename.FullFileName());
            return menuText.JsonToObj<List<HMenuItem>>().ToList().Select(x => x.ToNode());
        });

        /// <summary>
        /// 角色菜单更新以后更新
        /// </summary>
        /// <returns></returns>
        protected override void AfterSave(DataContext db, Role oldModel, Role newModel, dynamic json, bool isNew)
        {
            var user = WebHelper.CurrentAdmin as Admin;
            if (!isNew && user.Role.Id == newModel.Id)
            {
                user.Role = newModel;
                db.Save(user);
            }
        }

        /// <summary>
        /// 过滤超级管理员
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public override AjaxResult GetList(int pageIndex, int pageSize, string key)
        {
            return HandleHelper.TryAction(db =>
            {
                return Role.GetList(pageIndex, pageSize, key, db);
            });
        }

        /// <summary>
        /// 过滤超级管理员
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetUserRoleList(int pageIndex, int pageSize, string key)
        {
            return HandleHelper.TryAction(db =>
            {
                var where = MakeWhere(key);
                where &= Clip.Where<Role>(x => x.Id != Role.DefaultId);//ModelHelper.Root
                return db.GetPageList<Role>(pageIndex, pageSize, where, Clip.OrderBy<Role>(x => x.OrderByNo.Asc() && x.CreateTime.Desc()));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Enable([FromForm] string id) => ActToModel(id, m => { m.Status = InfoStatus.正常; });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Disable([FromForm] string id) => ActToModel(id, m => { m.Status = InfoStatus.停用; });
    }
}
