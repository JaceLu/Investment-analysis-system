using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investment.Code.TagHelpers
{
    /// <summary>
    /// 过滤
    /// </summary>
    [HtmlTargetElement(Attributes = "filter")]
    public class FilterTagHelper : TagHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string pre = $@"<div class='zqui-filter__item'>
                    <div class='zqui-form__element'>";
            string post = $@"</div><div class='zqui-form__tips'></div>
                      </div>";
            output.PreElement.SetHtmlContent(pre);
            output.PostElement.SetHtmlContent(post);
            output.Attributes.MergeAttribute("class", "zqui-form__input");
        }
    }
}
