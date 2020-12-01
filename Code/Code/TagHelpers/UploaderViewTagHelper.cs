using Investment.Code.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sail.Common;
using Sail.Web;

namespace Sail.Tags
{
    /// <summary>
    /// 文件预览
    /// </summary>
    [HtmlTargetElement("fileList")]
    public class UploaderViewTagHelper : TagHelper
    {
        /// <summary>
        /// 上传元素
        /// </summary>
        [HtmlAttributeName(nameof(Element))]
        public Element Element { set; get; }

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
        public UploadType Type { set; get; } = UploadType.多附件;


        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName(nameof(ExtendClass))]
        public string ExtendClass { set; get; } = "";


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            this.Setting = this.Setting ?? (this.Element?.Settings ?? new FormSettings());
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;




            if (Element != null)
            {
                context.AllAttributes.TryGetAttribute("id", out var idAttr);

                context.AllAttributes.TryGetAttribute("class", out var classAttr);

                var dict = this.Element.MakeAttributies("", MDataType.视图, needValidate: false);

                if (classAttr != null) dict["class"] += $" {classAttr.Value}";
                dict["class"] += $" zqui-upload-list zqui-upload-list_pic ";

                output.SetAttributes(dict);
            }

            output.Attributes.SetAttribute("data-tmpl", "inline");
            output.Attributes.SetAttribute("name", "");
            output.Attributes.SetAttribute("id", "");
            output.Attributes.SetAttribute("data-valuefield", this.IsFileDb ? "FileDbId" : "FilePath");


            var text = "";

            switch (this.Type)
            {
                case UploadType.单附件:
                case UploadType.单图片:

                    text = $@"
                            {{{{if {this.Element.Id}.FileDbId}}}}
                                <div class='zqui-upload__item'>                                   
                                    <div class='zqui-upload__bd'>                                       
                                        <a class='zqui-upload__name'>{{{{:{this.Element.Id}.FileName}}}}</a>                                    
                                    </div>                                   
                                    <div class='zqui-upload__ft'>
                                        <a class='zqui-text_green' target='_blank' href='{{{{:{this.Element.Id}.FilePath}}}}'>预览</a>        
                                        <a class='zqui-text_green' href='/api/File/DownLoad?id={{{{:{this.Element.Id}.FileDbId}}}}'>下载</a>                                    
                                    </div>                               
                                </div>
                            {{{{/if }}}}
                    ";
                    break;
                case UploadType.多附件:
                case UploadType.多图片:
                    text = $@" {{{{for {this.Element.Id} }}}} 
                                <div class='zqui-upload__item'>                                   
                                    <div class='zqui-upload__bd'>                                       
                                        <a class='zqui-upload__name'>{{{{:FileName}}}}</a>                                    
                                    </div>                                   
                                    <div class='zqui-upload__ft'>
                                        <a class='zqui-text_green' target='_blank' href='{{{{:FilePath}}}}'>预览</a>    
                                        <a class='zqui-text_green' href='/api/File/DownLoad?id={{{{:FileDbId}}}}'>下载</a>                                    
                                    </div>                               
                                </div>
                                {{{{/for}}}}
                            ";

                    break;
                default:
                    break;
            }

            output.PreContent.AppendHtml(text);
            base.Process(context, output);
        }



    }
}
