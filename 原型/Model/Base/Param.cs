using Sail.Common;

using System.Threading.Tasks;

namespace Lamtip.Model
{
    /// <summary>
    /// 系统参数表
    /// </summary>
    public class Param : BaseConfig<Param>, IModel
    {
        /// <summary>
        /// 
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string Id { set; get; }

        /// <summary>
        /// 域名
        /// </summary>
        [HColumn(Length = 200)]
        [Form(CustomValidate = "url")]
        public string Host { set; get; }

        /// <summary>
        /// Logo
        /// </summary>
        [HColumn]
        public string Logo { get; set; }

        /// <summary>
        /// 短信平台账号
        /// </summary>
        [HColumn(Length = 50)]
        public string SmsAccount { get; set; }

        /// <summary>
        /// 短信平台密码
        /// </summary>
        [HColumn(Length = 50)]
        public string SmsPassword { get; set; }

        /// <summary>
        /// 短信签名
        /// </summary>
        [HColumn(Length = 10)]
        public string SmsSign { set; get; }


        /// <summary>
        /// 微信app id
        /// </summary>
        [HColumn(Length = 200)]
        public string AppId { set; get; }

        /// <summary>
        /// 微信app secret
        /// </summary>
        [HColumn(Length = 200)]
        public string AppSecret { set; get; }


        /// <summary>
        /// 联系电话
        /// </summary>
        [HColumn(Length = 20)]
        public string Tel { get; set; }

        /// <summary>
        /// 服务器ip地址
        /// </summary>
        [HColumn(Length = 20)]
        public string ServerIp { get; set; }

        /// <summary>
        /// 用户关注公众号的地址
        /// </summary>
        [HColumn(Length = 200)]
        public string SubscribeQrCode { set; get; }

        /// <summary>
        /// 微信公众号
        /// </summary>
        [HColumn(Length = 200)]
        public string WechatMPName { set; get; }

        /// <summary>
        /// 微信公众号二维码
        /// </summary>
        [HColumn(Length = 200)]
        public string QrCode { set; get; }

        /// <summary>
        /// 系统名称
        /// </summary>
        [HColumn(Length = 200)]
        public string SiteName { set; get; }


        /// <summary>
        /// QQ
        /// </summary>
        [HColumn(Length = 50)]
        [Form(IsRequired = true)]
        public string QQ { set; get; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [HColumn(Length = 50)]
        [Form(IsRequired = true, CustomValidate = "pinteger")]
        public string ContactNumber { set; get; }

        /// <summary>
        /// 拜访地址
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string Address { set; get; }

        /// <summary>
        /// 关于我们
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string AboutUs { set; get; }

        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static Param Get(IDataContext db) => db.GetModel<Param>(Clip.Where<Param>(x => x.Id != ModelBase.NewId)) ?? new Param
        {
            SiteName = "德鑫库存项目",
            SmsAccount = "whqzkj",
            SmsPassword = "3827110",
            SmsSign = "智擎网络科技"
        };
    }
}
