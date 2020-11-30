using Lamtip.Model;
using Sail.Web;
using System.Collections.Generic;
using System.Linq;
using Senparc.Weixin.Entities;
using Sail.Common;

namespace Lamtip.Code
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> ToPairList(this IEnumerable<IDict> source)
        {
            return source.Select(v => v.ToKvPair()).ToList();
        }

        public static SenparcWeixinSetting Set(this Param param, SenparcWeixinSetting set)
        {
            set.TenPayV3_AppId = param.AppId;
            set.TenPayV3_AppSecret = param.AppSecret;
            //set.TenPayV3_Key = param.WeixinPay_Key;
            //set.TenPayV3_MchId = param.WeixinPay_MchId;

            set.WeixinAppId = param.AppId;
            set.WeixinAppSecret = param.AppSecret;

            set.WeixinPay_AppId = param.AppId;
            set.WeixinPay_AppKey = param.AppSecret;
            // set.WeixinPay_Key = param.WeixinPay_Key;
            //set.WeixinPay_PartnerId = param.WeixinPay_MchId;
            //var env = SailContext.GetService<IHostingEnvironment>();
            //RegisterService.Start(null).UseSenparcWeixin(set, null)
            //    .RegisterMpAccount(set, null);
            return set;
             
        }



        
    }
}
