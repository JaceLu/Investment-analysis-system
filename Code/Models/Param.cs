using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sail.Common;

namespace Investment.Models
{
    /// <summary>
    /// 系统参数表
    /// </summary>
    public class Param : BaseConfig<Param>, IModel
    {
        /// <summary>
        /// Id
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
        /// 中文名称
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string SiteName { set; get; }

        /// <summary>
        /// 英文名称
        /// </summary>
        [HColumn(Length = 200)]
        [Form(IsRequired = true)]
        public string SiteName_EN { set; get; }

        /// <summary>
        /// 系统Logo
        /// </summary>
        [HColumn]
        public string Logo { set; get; }

        /// <summary>
        /// 网页Icon
        /// </summary>
        [HColumn]
        public string Icon { set; get; }

        /// <summary>
        /// 网站介绍
        /// </summary>
        [HColumn]
        [Form(IsRequired = true)]
        public string Description { set; get; }

        /// <summary>
        /// 网站搜索关键字
        /// </summary>
        [HColumn]
        [Form(IsRequired = true,Tips = "便于客户利用搜索引擎搜索到此网站，建议设置多个关键词并用逗号分隔，示例：芜湖宜居，芜湖博览中心，宜居博览中心")]
        public string Keywords { set; get; }

        /// <summary>
        /// 访问量
        /// </summary>
        [HColumn]
        public int Visitors { set; get; }

        /// <summary>
        /// 访客量基础值
        /// </summary>
        [HColumn]
        [Form(IsRequired = true, Tips = "请输入正整数")]
        public int BaseValue { set; get; }

        /// <summary>
        /// 访客量系数值
        /// </summary>
        [HColumn]
        [Form(Tips = "请输入正整数，默认值为1（访问总次数 = 访客量基础值 + 访客量系数值*点击一次）")]
        public int CoefficientValues { set; get; }
    }
}
