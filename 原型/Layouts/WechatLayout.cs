using Lamtip.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sail.Common;
using Sail.Web;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Web;

namespace Lamtip.Code
{
    /// <summary>
    /// 
    /// </summary>
    public class WechatLayout : LayoutBase<User>
    {
        /// <summary>
        /// 
        /// </summary>
        public override User CurrentUser { get => WebHelper.CurrentUser as User; set => WebHelper.CurrentUser = value; }

        /// <summary>
        /// 
        /// </summary>
        public override IList<HMenuItem> Menus => throw new NotImplementedException();

        /// <summary>
        /// 
        /// </summary>

        [Inject]
        private readonly IHttpContextAccessor _httpAccessor;
        [Inject]
        private readonly ILogger<WechatLayout> _logger;

        [Inject]
        private IOptions<SenparcWeixinSetting> senparcWeixinSetting;

        /// <summary>
        /// 
        /// </summary>
        public string UserAgent => SailHttpContext.Current.Request.Headers["User-Agent"].ToString();

        /// <summary>
        /// 是否在微信中
        /// </summary>
        public bool IsInWechat => this.UserAgent.IndexOf("micromessenger", StringComparison.OrdinalIgnoreCase) >= 0;

        public bool Subscribed => this.CurrentUser?.Subscribed ?? false;
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
        /// <param name="httpAccessor"></param>
        public WechatLayout()
        {
            this.Inject();
            try
            {
                
                if (IsInWechat)
                { 
                    ticket = JsApiTicketContainer.TryGetJsApiTicket(Param.Default.AppId, Param.Default.AppSecret);
                }
                this.ValidateUser();
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "", null);
                throw e;
            }
#if !DEBUG
            
#endif
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timeStamp { set; get; } = TenPayV3Util.GetTimestamp();

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonceStr { set; get; } = TenPayV3Util.GetNoncestr();

        ///// <summary>
        ///// 票据
        ///// </summary>
        public string ticket { set; get; }

        ///// <summary>
        ///// 签名
        ///// </summary>
        public string Signature => JSSDKHelper.GetSignature(this.ticket, this.nonceStr, this.timeStamp, this.CurrentUrl);

        public string CurrentUrl
        {
            get
            {
                var context = this._httpAccessor.HttpContext;
                return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathAndQuery()}";
            }
        }

        internal void ValidateUser()
        {
             var context = this._httpAccessor.HttpContext;
            var code = context.Request.Query["code"].ToString();
            var cfg = senparcWeixinSetting.Value;
            Param.Default.Set(senparcWeixinSetting.Value);

            if (!code.IsNullOrEmpty() && this.IsInWechat)
            {
                var res = OAuthApi.GetAccessToken(cfg.WeixinAppId, cfg.WeixinAppSecret, code);
                var info = UserApi.Info(cfg.WeixinAppId, res.openid);

                using (var db = new DataContext())
                {
                    var user = db.GetModel<User>(x => x.WxOpenId == res.openid);

                    this.CurrentUser = user ?? new User
                    {
                        UserName = info?.nickname,
                        WxOpenId = res.openid,
                        HeadImg=info?.headimgurl,
                        NickName=info?.nickname
                        //Subscribed = info.subscribe != 0
                    };
                    this.CurrentUser.Subscribed = info.subscribe != 0;
                }
                var path = context.Session.GetString("lastUrl");// $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString.ToString().Replace("code", "usedcode")}";
                context.Session.Remove("lastUrl");
                context.Response.Redirect(path);
            }

            if (this.CurrentUser.IsNull())
            {
                if (this.IsInWechat)
                {
                    var path = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                    context.Session.SetString("lastUrl", path);
                    
                    var res = OAuthApi.GetAuthorizeUrl(cfg.WeixinAppId, path, "STATE", OAuthScope.snsapi_userinfo);
                    this._logger.LogWarning($"当前使用的appid:{cfg.WeixinAppId}\n url:{res}");
                    context.Response.Redirect(res);
                }
                else
                {
                    context.Response.Redirect("/wechat/login");
                }

            }
            else
            {
                if (this.IsInWechat)
                {
                    var info = UserApi.Info(cfg.WeixinAppId, this.CurrentUser.WxOpenId);
                    this.CurrentUser.Subscribed = info.subscribe != 0;
                   
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void checkLogin()
        {
            //do nothing
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        protected override void beforeCheckLogin(DataContext db)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadMenu()
        {

        }
    }
}