using Sail.Web;
using System.Collections.Generic;
using System;
using Sail.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Investment.Models;

namespace Investment.Layouts
{
    /// <summary>
    /// 用户模板页
    /// </summary>
    public class UserLayout : LayoutBase<User>
    {
        public override User CurrentUser { get => WebHelper.CurrentUser as User; set => WebHelper.CurrentUser = value; }
        /// <summary>
        /// 
        /// </summary>

        [Inject]
        private readonly IHttpContextAccessor _httpAccessor;
        [Inject]
        private readonly ILogger<User> _logger;
        /// <summary>
        /// 
        /// </summary>
        public string UserAgent => SailHttpContext.Current.Request.Headers["User-Agent"].ToString();
        /// <summary>
        /// 是否在微信中
        /// </summary>
        public bool IsInWechat => this.UserAgent.IndexOf("micromessenger", StringComparison.OrdinalIgnoreCase) >= 0;

        public bool IsAndroid => this.UserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0
                                || this.UserAgent.IndexOf("Linux", StringComparison.OrdinalIgnoreCase) >= 0;

        public bool IsiOS => this.UserAgent.IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) >= 0
                                || this.UserAgent.IndexOf("iPad", StringComparison.OrdinalIgnoreCase) >= 0
                                || this.UserAgent.IndexOf("iPod", StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// pc端微信
        /// </summary>
        public bool IsPcWechat => this.IsInWechat && !this.IsAndroid && !this.IsiOS;

        /// <summary>
        /// 
        /// </summary>
        public override string IndexUrl => "/index";
        /// <summary>
        /// 
        /// </summary>
        public override string LoginUrl => "/index";
        /// <summary>
        /// 登陆后跳转路径
        /// </summary>
        public static string Referer { set; get; } = "/index";

        public override IList<HMenuItem> Menus => null;

        public override void LoadMenu()
        {
            return;
        }


        protected override void CheckLogin()
        {
            return;
        }
    }
}
