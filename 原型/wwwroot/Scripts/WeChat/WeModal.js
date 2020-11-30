var WeChat;
(function (WeChat) {
    var modalTmpl = {
        dialog: '<div class="zqui-modal">' +
            '<div class="zqui-modal-mask"></div>' +
            '<div class="zqui-modal-dialog distance-two zqui-modal-action">' +
            '<ul class="zqui-table-view buttons"></ul>' +
            ' <ul class="zqui-table-view">' +
            '<li class="zqui-table-view-cell"><a class="btnClose"><b>取消</b></a></li>' +
            '</ul>' +
            '</div>' +
            '</div>',
        Button: '<li class="zqui-table-view-cell"><a class="btnAction"></a></li>'
    };
    var SheetAction = /** @class */ (function () {
        function SheetAction(btns) {
            var _this = this;
            this.$modal = $(modalTmpl.dialog);
            this.reBind(btns);
            this.$modal.on("click", ".btnClose", function () { _this.$modal.hide(); });
        }
        SheetAction.prototype.addButton = function (btn) {
            var _this = this;
            var $buttons = this.$modal.find(".buttons");
            var $btn = $(modalTmpl.Button).appendTo($buttons);
            $btn.find("a").html(btn.title).addClass(btn.cssClass);
            if (btn.act) {
                $btn.click(btn.act).click(function () { _this.$modal.hide(); });
            }
        };
        SheetAction.prototype.reBind = function (btns) {
            var _this = this;
            //this.$modal = $(modalTmpl.dialog);
            this.$modal.find(".buttons").empty();
            btns.forEach(function (x) {
                _this.addButton(x);
            });
            this.$modal.hide();
        };
        return SheetAction;
    }());
    WeChat.SheetAction = SheetAction;
})(WeChat || (WeChat = {}));
$(function () {
    jQuery.each(jQuery('textarea[data-autoresize]'), function () {
        var offset = this.offsetHeight - this.clientHeight;
        var resizeTextarea = function (el) {
            jQuery(el).css('height', 'auto').css('height', el.scrollHeight + offset);
        };
        jQuery(this).on('keyup input', function () { resizeTextarea(this); }).removeAttr('data-autoresize');
    });
});
function ActOrLogin(isLogined, act) {
    var $this = $("#divLogin");
    if (!isLogined)
        $this.show();
    else if (act && typeof act == "function")
        act();
    var loginApi = new Sail.ApiHelper("wxuser");
    $this.off("click", "#btnLogin").on("click", "#btnLogin", function () {
        $.post(loginApi.GetApi("login"), { "": JSON.stringify($this.GetJson()) }, function (result) {
            ShowMessage(result, "登录成功", function () {
                $this.hide();
                if (act && typeof act == "function")
                    act();
            });
        });
    }).off("click", ".btnClose").on("click", ".btnClose", function () { $("#divLogin").hide(); });
}
//# sourceMappingURL=WeModal.js.map