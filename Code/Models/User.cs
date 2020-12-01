using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sail.Common;
using Investment.Models;

namespace Investment.Models
{

    /// <summary>
    /// 管理员表
    /// </summary>
    public class Admin : User, IUser, IModel
    {
        /// <summary>
        /// 角色权限
        /// </summary>
        [HColumn]
        [ModelData("prop", nameof(Models.Role.Id))]
        [Form(IsRequired = true)]
        public Role Role { set; get; }

        /// <summary>
        /// 是否已停用
        /// </summary>
        [HColumn]
        public bool IsDisabled { set; get; }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public new bool IsSuperAdmin => this.UserId.Equals(ModelBase.DefaultId, StringComparison.OrdinalIgnoreCase);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="powerName"></param>
        /// <returns></returns>
        public new bool IsHasPermission(string powerName)
        {
            if (IsSuperAdmin || string.IsNullOrEmpty(powerName))
                return true;
            return this.Role?.Powers?.Select(x => x.Key).Contains(powerName)??false;
        }
    }


    /// <summary>
    /// 用户表
    /// </summary>
    public class User : IUser, IModel
    {
        /// <summary>
        /// 默认密码
        /// </summary>
        public static string DefaultPassword => "123456";

        /// <summary>
        /// 用户ID
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string UserId { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        [HColumn(IsUniqueness = true, Length = 50)]
        [Form(IsRequired = true, Tips = "初始密码为123456")]
        public string LoginId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string UserName { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string Password { get; set; }

        /// <summary>
        /// 注册手机
        /// </summary>
        [HColumn(Length = 20, IsUniqueness = true)]
        [Form(IsRequired = true, CustomValidate = "mobile")]
        public string Phone { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [HColumn]
        [Form(Class = "datetime")]
        public DateTime CreateTime { set; get; }




#if DEBUG
        /// <summary>
        /// 是否自动登录
        /// </summary>
        [HColumn]
        public bool IsAutoLogin { get; set; }


#endif
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsSuperAdmin => true;

        /// <summary>
        /// 是否拥有权限
        /// </summary>
        /// <param name="powerName"></param>
        /// <returns></returns>
        public bool IsHasPermission(string powerName)
        {
            return false;
        }
    }

}
