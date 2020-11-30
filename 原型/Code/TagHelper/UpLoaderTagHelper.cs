using Microsoft.AspNetCore.Razor.TagHelpers;
using Sail.Common;
using Sail.Web;

namespace Sail.Tags
{
    /// <summary>
    /// 上传类型
    /// </summary>
    public enum UploadType
    {
        /// <summary>
        /// 
        /// </summary>
        单附件,
        /// <summary>
        /// 
        /// </summary>
        多附件,
        /// <summary>
        /// 
        /// </summary>
        单图片,
        /// <summary>
        /// 
        /// </summary>
        多图片,
    }

    /// <summary>
    /// 上传
    /// </summary>
    [HtmlTargetElement("upLoader")]
    public class UpLoaderTagHelper : TagHelper
    {
        /// <summary>
        /// 上传元素
        /// </summary>
        [HtmlAttributeName(nameof(Element))]
        public Element Element { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(DefaultValue))]
        public string DefaultValue { set; get; } = "";

        /// <summary>
        /// 是否关联文件数据库
        /// </summary>
        [HtmlAttributeName(nameof(IsFileDb))]
        public bool IsFileDb { set; get; } = true;

        /// <summary>
        /// 
        /// </summary>
        public FormSettings Setting { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(Type))]
        public UploadType Type { set; get; }

        /// <summary>
        /// 上传按钮名称
        /// </summary>
        [HtmlAttributeName(nameof(Title))]
        public string Title { set; get; } = "上传附件";


        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(ExtendClass))]
        public string ExtendClass { set; get; } = "";


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            this.Setting = this.Setting ?? (this.Element?.Settings ?? new FormSettings());
            output.TagName = "input";
            output.TagMode = TagMode.SelfClosing;


            if (Element != null)
            {
                context.AllAttributes.TryGetAttribute("id", out var idAttr);
                context.AllAttributes.TryGetAttribute("class", out var classAttr);


                var dict = this.Element.MakeAttributies("", MDataType.全部);

                if (classAttr != null)
                    dict["class"] += $" {classAttr.Value}";

                dict["data-text"] = Title;

                switch (this.Type)
                {
                    case UploadType.单附件:
                        //context.AllAttributes.TryGetAttribute("data-text", out var buttonTitle);
                        dict["class"] += $" singleFileUpload";
                        break;
                    case UploadType.多附件:
                        dict["class"] += $" multiFilesUpload";
                        output.Attributes.SetAttribute("data-multi", true);
                        break;
                    case UploadType.单图片:
                        break;
                    case UploadType.多图片:
                        output.Attributes.SetAttribute("data-multi", true);
                        break;
                    default:
                        break;
                }

                output.SetAttributes(dict);
            }

            if (!string.IsNullOrEmpty(this.DefaultValue))
                output.Attributes.SetAttribute("data-defaultvalue", this.DefaultValue);

            output.Attributes.SetAttribute("data-valuefield", this.IsFileDb ? "FileDbId" : "FilePath");

            var groupHelper = new GroupHelper()
            {
                Element = this.Element,
                Setting = this.Setting
            };

            var group = this.Setting.FromGroup?.Invoke(groupHelper);


            var preText = $@"<div class='zqui-box'>
            <div class='zqui-box__hd zqui-font_bold'>";
            if (this.Element.IsRequired)
            {
                preText += "<span style='color:red'>*</span> ";
            }
            preText += $@"{Element.Label}</div>
             <div class='zqui-box__bd'>
                <div class='zqui-form'>
                {group?.Item1}";

            switch (this.Type)
            {
                case UploadType.单附件:
                case UploadType.单图片:
                    preText += "<div class='clearfix'><div class='zqui-upload'>";
                    break;
                default:
                    preText += "<div class='zqui-btn zqui-btn_default zqui-btn_file'>";
                    break; 
            }

            var postText = $@"</div>
                        </div>
                        <div class='zqui-upload-list zqui-upload-list_pic fileList'>
                    {group?.Item2}
              </div>
             </div>
            </div></div>";

            output.PreElement.SetHtmlContent(preText);
            output.PostElement.SetHtmlContent(postText);
            base.Process(context, output);
        }
    }
}
