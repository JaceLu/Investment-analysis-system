using Sail.Common;
using System.Collections.Generic;
using System.Linq;

namespace Lamtip.Model
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class UserRole : Role, IModel
    {
        internal static readonly string RootName = "管理员";
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

       
    }
}
