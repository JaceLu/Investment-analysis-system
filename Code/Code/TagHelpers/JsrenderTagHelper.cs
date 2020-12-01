using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investment.Code.TagHelpers
{
    /// <summary>
    /// JsViews模板
    /// </summary>
    [HtmlTargetElement("tmpl")]
    public class JsrenderTagHelper : TagHelper
    {

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "script";
            output.Attributes.SetAttribute("type", "text/x-jsrender");

            //var result = await output.GetChildContentAsync(NullHtmlEncoder.Default);
            //output.Content.SetHtmlContent(result);

        }


    }
}
