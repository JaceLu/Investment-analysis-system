using System.Collections.Generic;
using System.Linq;
using Investment.Models;
using Sail.Common;
using System;

namespace Investment.Code
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extension
    {
        public static bool IsHasSubPower(this Admin user, string powerId)
        {
            var check = !user.IsNull() && !user.Role.IsNull() && user.IsHasPermission(powerId);
            return check || (user?.IsSuperAdmin ?? false);
        }
    }
}
