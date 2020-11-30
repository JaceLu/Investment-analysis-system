using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sail.Tags
{

    /// <summary>
    /// box结构
    /// </summary>
    [HtmlTargetElement("zq-box")]
    public class ZqBoxTagHelper : TagHelper
    {
        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(Title))]
        public string Title { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(HasMore))]
        public bool HasMore { set; get; } = true;
        /// <summary>
        /// /
        /// </summary>
        [HtmlAttributeName(nameof(MoreText))]
        public string MoreText { set; get; } = "更多...";


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            context.AllAttributes.TryGetAttribute("class", out var classAttr);

            //output.Attributes.MergeAttribute("class", " zqui-box__bd ");
            var before = "<div class=\"zqui-box\">";

            before += $@" <div class='zqui-box__hd'>
                          <div class='zqui-caption zqui-title'><b>{this.Title}</b></div>
                            { (!HasMore ? "" : $" <a class='zqui-actions'>{this.MoreText }</a>")}
                    </div>";
            output.PreContent.SetHtmlContent(before);
            output.PostContent.SetHtmlContent("</div>");
        }
    }
}
