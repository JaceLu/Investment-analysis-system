using NPOI.SS.Formula.Functions;
using Sail.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investment.Models
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class Role : ModelBase, IModel
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [HColumn(Length = 200, Remark = "角色名称", IsUniqueness = true)]
        [Form(IsRequired = true)]
        public string RoleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [HColumn(Remark = "备注", Length = 500)]
        public string Memo { get; set; }

        /// <summary>
        /// 功能权限
        /// </summary>
        [HColumn(Remark = "功能权限")]
        public List<Permission> Powers { get; set; } = new List<Permission>();

        /// <summary>
        /// 功能权限
        /// </summary>
        public string PowersStr => Powers.Select(x => x.Name).JoinToString();

        /// <summary>
        /// 可以新增/编辑
        /// </summary>
        [HColumn]
        public bool CanEdit { set; get; }

        /// <summary>
        /// 可以删除
        /// </summary>
        [HColumn]
        public bool CanDelete { set; get; }

        /// <summary>
        /// 排序
        /// </summary>
        [HColumn]
        [Form(Tips = "默认按从小到大排序，如不填写，将默认排序号为0")]
        public int OrderByNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [HColumn]
        public InfoStatus Status { get; set; }

        /// <summary>
        /// 读取全部角色
        /// </summary>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> GetAllRole()
        {
            using (var db = new DataContext())
            {
                return db.GetList<Role>().Select(x => new KeyValuePair<string, string>(x.Id, x.RoleName)).ToList();
            }
        }

        /// <summary>
        /// 读取全部角色
        /// </summary>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> GetRoles()
        {
            using (var db = new DataContext())
            {
                var userRoles = db.GetList<Role>(y => y.Status == InfoStatus.正常, y => y.OrderByNo.Asc() && y.CreateTime.Desc())
                         .Select(x => new KeyValuePair<string, string>(x.Id, x.RoleName)).ToList();

                var superAdmin = userRoles.Find(x => x.Key.IsEqualTo("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF") && x.Value == "超级管理员");
                userRoles.Remove(superAdmin);
                userRoles.Add(superAdmin);  //加到最后
                return userRoles;
            }
        }

        /// <summary>
        ///  删除角色前，进行数据检查
        /// </summary>
        /// <param name="db"></param>
        /// <param name="e"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public static void BeforeAction(DataContext db, Sail.Web.ControllerEvent e, object o)
        {
            if (e == Sail.Web.ControllerEvent.Delete)
            {
                var model = o as Role;
                var where = /*nameof(Lamtip.Model.User.Role).ToField().Contains(model.Id) & */ nameof(Investment.Models.Admin.IsDisabled).ToField() == false;
                var count = db.GetCount<Admin>(where);
                if (count > 0)
                {
                    throw new SailCommonException("操作失败！此角色正在使用中");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Clip BaseWhere(string key)
        {
            return key.IsNull() ? null : Clip.Where<Role>(x => x.RoleName.Like(key)).Bracket();
        }

        /// <summary>
        /// 过滤超级管理员
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static PageResult GetList(int pageIndex, int pageSize, string key, DataContext db)
        {
            var where = BaseWhere(key);
            where &= Clip.Where<Role>(x => x.Id != "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");//ModelHelper.Root
            return db.GetPageList<Role>(pageIndex, pageSize, where, Clip.OrderBy<Role>(x => x.CreateTime.Desc()));
        }

        public override string ToString()
        {
            return this.RoleName?.ToString();
        }
    }
}
