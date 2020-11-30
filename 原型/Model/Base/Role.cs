using Sail.Common;
using System.Collections.Generic;
using System.Linq;

namespace Lamtip.Model
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public abstract class Role : ModelBase
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [HColumn(Length = 200)]
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
        public List<Power> Powers { get; set; } = new List<Power>();

        /// <summary>
        /// 读取全部角色
        /// </summary>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> GetAllRole()
        {
            using (var db = new DataContext())
            {
                return db.GetList<UserRole>().Select(x => new KeyValuePair<string, string>(x.Id, x.RoleName)).ToList();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Clip BaseWhere(string key)
        {
            return key.IsNull() ? null : Clip.Where<UserRole>(x => x.RoleName.Like(key)).Bracket();
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
            where &= Clip.Where<UserRole>(x => x.Id != "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");//ModelHelper.Root
            return db.GetPageList<UserRole>(pageIndex, pageSize, where, Clip.OrderBy<UserRole>(x => x.CreateTime.Desc()));
        }
    }
}
