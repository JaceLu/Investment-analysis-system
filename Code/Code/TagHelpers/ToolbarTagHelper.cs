using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investment.Code.TagHelpers
{
    /// <summary>
    /// 工具栏
    /// </summary>
    [HtmlTargetElement("toolbar")]
    public class ToolbarTagHelper : TagHelper
    {
        [HtmlAttributeName("key")]
        public string Keyword { set; get; }

        [HtmlAttributeName("export")]
        public string Export { get; set; }

        [HtmlAttributeName("buttons")]
        public IEnumerable<HtmlElement> Buttons { set; get; }

        private IEnumerable<HtmlElement> _buttons = new List<HtmlElement>
        {
            new HtmlElement("btnQuery","zqui-btn_primary btnQuery","btnQuery","搜索"),
            new HtmlElement("zqui-btn_default btnReset","重置")

        };

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;



            if (!context.AllAttributes.TryGetAttribute("id", out var idAttr))
                output.Attributes.SetAttribute("id", "toolbar");

            //if (context.AllAttributes.TryGetAttribute("class", out var classAttr))
            //    output.Attributes.MergeAttribute("class", classAttr);

            output.Attributes.MergeAttribute("class", "zqui-table-toolbar zqui-filter");

            var key = string.IsNullOrEmpty(this.Keyword) ? "" : $@"
            <div class='zqui-filter__item'>
              <div class='zqui-input-group'>
                <input type='text' class='zqui-input-group__input' placeholder='{Keyword}' name='key'>
              </div>
            </div>";

            var buttons = "<div class='zqui-filter__item'>";
            var btns = this._buttons.ToList();
            if (!string.IsNullOrEmpty(this.Export))
            {
                btns.Add(new HtmlElement("zqui-btn_warning btnExport", "导出"));
            }


            if (this.Buttons?.Any() ?? false)
            {
                btns.AddRange(this.Buttons);
            }

            btns.ForEach(ele =>
            {
                ele.Class += " zqui-btn";
                buttons += $"<button {ele.ToAttributesString()} >{ele.InnertHtml}</button>";
            });
            buttons += "</div>";

            output.PreContent.SetHtmlContent(key);
            output.PostContent.SetHtmlContent(buttons);
        }
    }
}
