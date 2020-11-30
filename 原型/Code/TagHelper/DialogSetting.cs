using System;
using System.Collections.Generic;
using System.Web;

namespace Sail.Tags
{
    /// <summary>
    /// 对话框设置
    /// </summary>
    public class DialogSetting
    {
        /// <summary>
        /// 输出头
        /// </summary>
        public Func<string, string> Header { set; get; } = (title) =>
        {
            return $@"
                    <div class='zqui-modal__hd modal-header'>
                        <div class='zqui-modal__title modal-title'>{title}</div>
                        <a class='zqui-close btnCancel' data-dismiss='modal' aria-hidden='true'>×</a>
                    </div>";
        };

        /// <summary>
        /// 输出脚步
        /// </summary>
        public Func<Func<string>, string> Foot { set; get; } = (act) =>
        {
            return $"<div class='zqui-modal__ft modal-footer'>{act?.Invoke()}</div></div>";
        };




        /// <summary>
        /// 前半截
        /// </summary>
        public Func<DialogSetting, DialogSize, string, string> PreBody { set; get; } = (setting, size, id) =>
        {
            return $@"
            <div class='zqui-modal modal' role='dialog'  id='{id}'>
                        <div class='zqui-modal__mask'></div>
                        <div class='zqui-modal__dialog modal-dialog {setting.Size[size]}'>
                            <div class='zqui-modal__content modal-content'>"
                               ;
        };



        /// <summary>
        /// 对话框体的class
        /// </summary>
        public Func<string> BodyClass { set; get; } = () => { return " zqui-modal__bd modal-body "; };

        /// <summary>
        /// 后半截
        /// </summary>
        public Func<string> PostBody { set; get; } = () =>
        {
            return "</div></div>";
        };


        /// <summary>
        /// 取消按钮
        /// </summary>
        public Func<string, string> CancelButton { set; get; } = (cancelTitle) =>
        {
            return $"<a class='zqui-btn btn zqui-btn_default btnCancel' data-dismiss='modal' aria-hidden='true'>{cancelTitle}</a>";
        };

        /// <summary>
        /// 确定按钮
        /// </summary>
        public Func<string, string> OkButton { set; get; } = (okTitle) =>
        {
            return $"<button class='zqui-btn btn zqui-btn_primary btnOk'>{okTitle}</button>";
        };

        /// <summary>
        /// 生成按钮
        /// </summary>
        public Func<HtmlElement, string> CreateButton { set; get; } = (htmlElement) =>
        {
            return $"<button class='zqui-btn btn {htmlElement.Class}' id='{htmlElement.Id}' title='{htmlElement.Name}' >{htmlElement.InnertHtml}</button>";
        };

        /// <summary>
        /// 对话框尺寸
        /// </summary>
        public Dictionary<DialogSize, string> Size { set; get; } = new Dictionary<DialogSize, string> {
            {  DialogSize.Default,""},
            {  DialogSize.Small,"zqui-dialog_sm"},
            {  DialogSize.Middle,"zqui-dialog_md"},
            {  DialogSize.Large,"zqui-dialog_lg"},
            {  DialogSize.FullScreen,""},
        };

    }
}
