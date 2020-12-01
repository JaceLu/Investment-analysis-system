using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sail.Common;

namespace Investment.Models.Abstract
{
    /// <summary>
    /// 日志基类
    /// </summary>
    public abstract class BaseLog : ModelBase, IModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [HColumn(IsGuid = true)]
        public string UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [HColumn(Length = 200)]
        public string UserName { set; get; }

        /// <summary>
        /// 登录时间
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string LoginIP { get; set; }

        /// <summary>
        /// 保存登录日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="user"></param>
        /// <param name="LoginTime"></param>
        /// <param name="LoginIP"></param>
        /// <returns></returns>
        public static T CreateLog<T>(DataContext db, IUser user, DateTime? LoginTime, string LoginIP) where T : BaseLog, new()
        {
            var t = new T
            {
                UserId = user.UserId,
                UserName = user.UserName,
                LoginTime = LoginTime,
                LoginIP = LoginIP
            };
            db.Save(t);
            return t;
        }
    }
}
