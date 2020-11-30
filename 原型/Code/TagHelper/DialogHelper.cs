using Microsoft.AspNetCore.Razor.TagHelpers;
using Sail.Common;
using System;
using System.Collections.Generic;
using System.Web;

namespace Sail.Tags
{

    /// <summary>
    /// 弹出层
    /// </summary>
    [HtmlTargetElement("dialog")]
    public class DialogHelper : TagHelper
    {
        /// <summary>
        /// 弹出层标题
        /// </summary>
        [HtmlAttributeName(nameof(Title))]
        public string Title { set; get; }

        /// <summary>
        /// 确定按钮文字
        /// </summary>
        [HtmlAttributeName(nameof(OkTitle))]
        public string OkTitle { set; get; } = "确定";

        /// <summary>
        /// 取消按钮文字
        /// </summary>
        [HtmlAttributeName(nameof(CancelTitle))]
        public string CancelTitle { set; get; } = "取消";

        /// <summary>
        /// 尺寸
        /// </summary>
        [HtmlAttributeName(nameof(Size))]
        public DialogSize Size { set; get; } = DialogSize.Default;

        /// <summary>
        /// 设置
        /// </summary>
        [HtmlAttributeName("Setting")]
        public DialogSetting DialogSetting { set; get; } = new DialogSetting();

        /// <summary>
        /// 扩展按钮
        /// </summary>
        [HtmlAttributeName("buttons")]
        public List<HtmlElement> ExtendButtons { set; get; }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            context.AllAttributes.TryGetAttribute("class", out var classAttr);

            context.AllAttributes.TryGetAttribute("id", out var idAttr);
            var id = idAttr?.Value.ToString() ?? "";
            id = id.IsNotNull() ? id : Guid.NewGuid().ToString("N");

            //本体的id属性移除,加到
            output.Attributes.RemoveAttribute("id");

            output.Attributes.MergeAttribute("class", this.DialogSetting.BodyClass?.Invoke());

            var before = this.DialogSetting?.PreBody.Invoke(this.DialogSetting, this.Size, id);
            before += this.DialogSetting?.Header.Invoke(this.Title);

            var post = this.DialogSetting?.Foot(() =>
            {
                var text = "";
                if (this.CancelTitle.IsNotNull()) text += this.DialogSetting?.CancelButton.Invoke(this.CancelTitle);
                if (this.OkTitle.IsNotNull()) text += this.DialogSetting?.OkButton.Invoke(this.OkTitle);
                this.ExtendButtons?.ForEach(btn =>
                {
                    text += this.DialogSetting?.CreateButton.Invoke(btn);
                });
                return text;
            });

            post += this.DialogSetting?.PostBody.Invoke(); 

            output.PreElement.SetHtmlContent(before);
            output.PostElement.SetHtmlContent(post);
            base.Process(context, output);
        }

    }
}
