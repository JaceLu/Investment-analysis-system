using Sail.Common;
using System;
using System.Linq;

namespace Lamtip.Model
{
    /// <summary>
    /// 
    /// </summary>
    [HTable(TableName = "t_StockUser")]
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
        /// 注册手机
        /// </summary>
        [HColumn(Length = 20, IsUniqueness = true)]
        [Form(IsRequired = true, CustomValidate = "mobile")]
        public string Phone { set; get; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [HColumn(Length = 20)]
        [Form(CustomValidate = "pinteger")]
        public string ContactNumber { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string Password { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [HColumn]
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 是否已停用
        /// </summary>
        [HColumn]
        public bool IsDisabled { set; get; }

        /// <summary>
        /// 是否企业根用户
        /// </summary>
        [HColumn]
        public bool IsRoot { set; get; }

        /// <summary>
        /// 角色权限
        /// </summary>
        [HColumn]
        [ModelData("prop", nameof(UserRole.Id))]
        [Form(IsRequired = true)]
        public UserRole Role { set; get; }

        /// <summary>
        /// 微信OpenId
        /// </summary>
        [HColumn]
        public string WxOpenId { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [HColumn]
        public string HeadImg { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [HColumn(Length = 50)]
        public string NickName { get; set; }

        /// <summary>
        /// 用户身份
        /// </summary>
        [HColumn]
        public UserType UserType { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        [HColumn]
        public int SupplierId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [HColumn(Length =100)]
        public string SupplierName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuperAdmin => string.Compare(this.UserId, UserRole.DefaultId, ignoreCase: true) == 0 ||
          string.Compare(this.Role?.Id, UserRole.DefaultId, ignoreCase: true) == 0;

        /// <summary>
        /// 
        /// </summary>
        public bool Subscribed { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="powerName"></param>
        /// <returns></returns>
        public bool IsHasPower(string powerName)
        {
            if (IsSuperAdmin || string.IsNullOrEmpty(powerName))
                return true;
            return Role?.Powers?.Select(x => x.Key)?.Contains(powerName) ?? false;
        }

#if DEBUG
        /// <summary>
        /// 当前自动登录的用户
        /// </summary>
        [HColumn]
        public bool IsAutoLogin { get; set; }
#endif
    }

    /// <summary>
    /// 用户身份
    /// </summary>
    public enum UserType
    {
        供应商=1,
        仓管员,
        质检员
    }
}
