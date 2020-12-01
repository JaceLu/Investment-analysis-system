using Microsoft.AspNetCore.Razor.TagHelpers;
using Sail.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investment.Code.TagHelpers
{
    /// <summary>
    /// 
    /// </summary>
    [HtmlTargetElement("formGroup")]
    public class FormGroupTagHelper : TagHelper, IFormGroupTagHelper
    {
        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(Element))]
        public Element Element { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(Setting))]
        public FormSettings Setting { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(IsMulti))]
        public bool IsMulti { set; get; } = false;

        /// <summary>
        /// 
        /// </summary>
        public bool IsView { set; get; } = false;

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(ExtendClass))]
        public string ExtendClass { set; get; } = "";

        /// <summary>
        /// 
        /// </summary>
        public bool HasGroup { set; get; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool HasTips { set; get; } = true;

        /// <summary>
        /// 
        /// </summary>
        //[HtmlAttributeName(nameof(HasLabel))]
        public bool HasLabel { set; get; } = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            this.Setting = this.Setting ?? new FormSettings();
            this.HasLabel = !(context.HasAttr("noLabel"));

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.SetAttribute(new TagHelperAttribute("class", $"{this.Setting[FormElements.Group]} {this.ExtendClass}"));
            var group = this.Setting.FromGroup?.Invoke(this);
            output.PreContent.AppendHtml(group?.Item1);
            output.PostContent.AppendHtml(group?.Item2);
            //output.PostContent.AppendHtml($"</div>");
        }
    }
}
