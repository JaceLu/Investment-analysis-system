using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sail.Web;
using System.Collections.Generic;
using System.Linq;
using Sail.Common;
namespace Sail.Common
{
    public enum OptionMode
    {
        Replace,
        After,
        Before
    }

}
namespace Sail.Tags
{

    /// <summary>
    /// 添加element属性
    /// </summary>
    [HtmlTargetElement("input")]
    [HtmlTargetElement("textarea")]
    [HtmlTargetElement("select")]
    [HtmlTargetElement("img")]
    [HtmlTargetElement("div")]
    [HtmlTargetElement("fileList")]
    public class ElementTagHelper : TagHelper, IFormGroupTagHelper
    {
        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(Element))]
        public Element Element { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public FormSettings Setting { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(IsMulti))]
        public bool IsMulti { set; get; } = false;

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(ExtendClass))]
        public string ExtendClass { set; get; } = "";

        /// <summary>
        /// 
        /// </summary>
        public bool IsView { set; get; } = false;

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(HasTips))]
        public bool HasTips { set; get; } = true;

        /// <summary>
        /// 
        /// </summary>

        public bool HasLabel { set; get; } = true;

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
        public bool HasGroup { set; get; } = true;


        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName("options")]
        public IEnumerable<KeyValuePair<string, string>> Options { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName("optionmode")]
        public Sail.Web.OptionMode OptionMode { set; get; } = Sail.Web.OptionMode.After;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            this.Setting = this.Setting ?? (this.Element?.Settings ?? new FormSettings());
            this.HasLabel = !(context.HasAttr("nolable")  );
            this.IsView = context.HasAttr("view") || output.TagName == "div" || output.TagName == "fileList";


            this.HasGroup = !(context.HasAttr("noGroup") || context.HasAttr("filter"));
            if (this.Element == null) this.HasGroup = false;
            context.AllAttributes.TryGetAttribute("type", out var typeAttr);
            if (typeAttr != null && typeAttr.Value.ToString() == "hidden") this.HasGroup = false;

            if (Element != null)
            {
                context.AllAttributes.TryGetAttribute("id", out var idAttr);
                context.AllAttributes.TryGetAttribute("class", out var classAttr);
                bool isRenderId = !context.HasAttr("tmpl") && idAttr == null && !IsView;

                var controllClass = (IsView ? Setting[FormElements.View] : Setting[FormElements.Controller]);
                var dict = this.Element.MakeAttributies(controllClass, isRenderId: isRenderId, needValidate: !IsView);

                if (classAttr != null)
                    dict["class"] += $" {classAttr.Value.ToString()}";

                if (IsView && !context.HasAttr("tmpl"))
                    dict["class"] += " prop";

                if (context.HasAttr("tmpl"))
                {
                    output.Content.SetHtmlContent($"{{{{:{Element.Id}}}}}");
                    dict.Remove("name");
                }

                output.SetAttributes(dict);
                //dict.ToList().ForEach(d => output.Attributes.SetAttribute(d.Key, d.Value));
            }

            if (!string.IsNullOrEmpty(this.DefaultValue))
                output.Attributes.SetAttribute("data-defaultvalue", this.DefaultValue);

            if (!string.IsNullOrEmpty(this.DefaultText))
                output.Attributes.SetAttribute("data-defaulttext", this.DefaultText);


            switch (output.TagName)
            {
                case "input":
                    break;
                case "textarea":
                    output.TagMode = TagMode.StartTagAndEndTag;
                    output.Attributes.MergeAttribute("class", Setting[FormElements.Textarea]);
                    break;
                case "select":
                    if (this.Options?.Any() ?? false)
                    {
                        if (this.OptionMode == Sail.Web.OptionMode.Replace) output.Content.SetContent("");
                        var contents = new Dictionary<Sail.Web.OptionMode, TagHelperContent> {
                            {Sail.Web.OptionMode.Replace ,output.Content},
                            {Sail.Web.OptionMode.After ,output.PostContent},
                            {Sail.Web.OptionMode.Before ,output.PreContent},
                        };
                        this.Options.ForEach(x => contents[this.OptionMode].AppendHtml($"<option value={x.Key}>{x.Value}</option>"));
                    }
                    break;
            }
            if (this.HasGroup)
            {

                output.PreElement.AppendHtml($"<div class=\"{this.Setting[FormElements.Group]} {this.ExtendClass}\">");
                var group = this.Setting.FromGroup?.Invoke(this);
                output.PreElement.AppendHtml(group?.Item1);
                output.PostElement.AppendHtml(group?.Item2);
                output.PostElement.AppendHtml($"</div>");
            }
            base.Process(context, output);
        }
    }
}
