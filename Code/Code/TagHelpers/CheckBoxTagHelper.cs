using Microsoft.AspNetCore.Razor.TagHelpers;
using Sail.Common;
using Sail.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investment.Code.TagHelpers
{
    public class GroupHelper : IFormGroupTagHelper
    {
        public Element Element { set; get; }
        public string ExtendClass { set; get; }
        public bool HasLabel { set; get; }
        public bool HasTips { set; get; }
        public bool IsMulti { set; get; }
        public bool IsView { set; get; }
        [HtmlAttributeName(nameof(HasGroup))]
        public bool HasGroup { set; get; }
        public FormSettings Setting { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    [HtmlTargetElement("checkbox")]
    public class CheckBoxTagHelper : FormGroupTagHelper, IFormGroupTagHelper
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

        private string CheckBoxHtml(string name, KeyValuePair<string, string> v)
        {
            return $@"<label class='zqui-checkbox-wrap' >
                                                                                    <span class='zqui-checkbox'>
                                                                                        <input type='checkbox' id='{v.Key}' name='{name}' data-name='{v.Value}'data-defaultvalue='{v.Key}'>
                                                                                        <span class='zqui-checkbox__inner'></span>
                                                                                    </span>
                                                                                    <span>{v.Value}</span>
                                                                                </label>";
        }

        private string SingleCheckBox(string name, string lable)
        {
            return $@"<label class='zqui-checkbox-wrap' >
                                                                    <span class='zqui-checkbox'>
                                                                        <input type='checkbox' id='{name}'  data-valuetype='bool' >
                                                                        <span class='zqui-checkbox__inner'></span>
                                                                    </span>
                                                                    <span>{lable}</span>
                                                                </label>";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
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
            output.Attributes.SetAttribute("class", "zqui-checkbox-group");

            if (this.Options == null)
            {
                output.Content.SetHtmlContent(this.SingleCheckBox(nameAttr.Value.ToString(), this.Element?.Label ?? this.DefaultText));
            }
            else
            {
                output.Attributes.MergeAttribute("class", " checkList");
                output.Attributes.SetAttribute("data-prop", nameAttr.Value);
                this.Options.ForEach(v =>
                {
                    output.Content.AppendHtml(this.CheckBoxHtml(nameAttr.Value.ToString(), v));
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
