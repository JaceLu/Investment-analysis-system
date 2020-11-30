using Microsoft.AspNetCore.Razor.TagHelpers;
using Sail.Web;
using System.Collections.Generic;
using System.Linq;
using Sail.Common;
namespace Sail.Tags
{
    /// <summary>
    /// 
    /// </summary>
    [HtmlTargetElement("radio")]
    public class RadioTagHelper : FormGroupTagHelper, IFormGroupTagHelper
    {
        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(DefaultValue))]
        public string DefaultValue { set; get; } = "";

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(DefaultText))]
        public string DefaultText { set; get; } = "";

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName("options")]
        public IEnumerable<KeyValuePair<string, string>> Options { set; get; }


        string RidioHtml(string name, KeyValuePair<string, string> v) => $@"<label class='zqui-radio-wrap' >
                                                                                    <span class='zqui-radio'>
                                                                                        <input type='radio' id='{v.Key}' name='{name}' data-name='{v.Value}'data-defaultvalue='{v.Key}'>
                                                                                        <span class='zqui-radio__inner'></span>
                                                                                    </span>
                                                                                    <span>{v.Value}</span>
                                                                                </label>";

        string SingleRidioBox(string name, string lable) => $@"<label class='zqui-radio-wrap' >
                                                                    <span class='zqui-radio'>
                                                                        <input type='radio' id='{name}' >
                                                                        <span class='zqui-checkbox__inner'></span>
                                                                    </span>
                                                                    <span>{lable}</span>
                                                                </label>";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            this.Setting = this.Setting ?? (this.Element?.Settings ?? new FormSettings());

            if (this.Element != null)
            {
                context.AllAttributes.TryGetAttribute("id", out var idAttr);
                context.AllAttributes.TryGetAttribute("class", out var classAttr);

                var dict = this.Element.MakeAttributies("", isRenderId: false);

                if (classAttr != null)
                    dict["class"] += classAttr.Value.ToString();



                dict.ToList().ForEach(d => output.Attributes.SetAttribute(d.Key, d.Value));
            }

            if (!string.IsNullOrEmpty(this.DefaultValue)) output.Attributes.SetAttribute("data-defaultvalue", this.DefaultValue);
            if (!string.IsNullOrEmpty(this.DefaultValue)) output.Attributes.SetAttribute("data-defaulttext", this.DefaultText);

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.TryGetAttribute("name", out var nameAttr);
            output.Attributes.RemoveAll("name");
            output.Attributes.SetAttribute("class", "zqui-radio-group");

            if (this.Options == null)
            {
                output.Content.SetHtmlContent(SingleRidioBox(nameAttr.Value.ToString(), Element?.Label ?? DefaultText));
            }
            else
            {
                output.Attributes.SetAttribute("data-prop", nameAttr.Value);
                this.Options.ForEach(v =>
                {
                    output.Content.AppendHtml(RidioHtml(nameAttr.Value.ToString(), v));
                });
            }

            if (this.HasGroup && this.Element != null)
            {
                output.PreElement.AppendHtml($"<div class=\"{this.Setting[FormElements.Group]} {this.ExtendClass}\">");
                var group = this.Setting.FromGroup?.Invoke(this);
                output.PreElement.AppendHtml(group?.Item1);
                output.PostElement.AppendHtml(group?.Item2);
                output.PostElement.AppendHtml($"</div>");
            }
        }
    }
}
