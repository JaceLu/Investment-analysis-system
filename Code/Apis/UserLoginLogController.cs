using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sail.Common;
using Sail.Web;
using Investment.Models;

namespace Investment.Apis
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserLoginLogController : BaseController<UserLoginLog>
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
    }
}
