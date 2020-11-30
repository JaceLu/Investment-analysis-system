using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Sail.Tags
{
    /// <summary>
    /// JsViews模板
    /// </summary>
    [HtmlTargetElement("tmpl")]
    public class JsrenderTagHelper : TagHelper
    {

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "script";
            output.Attributes.SetAttribute("type", "text/x-jsrender");

            //var result = await output.GetChildContentAsync(NullHtmlEncoder.Default);
            //output.Content.SetHtmlContent(result);

        }


    }
}
