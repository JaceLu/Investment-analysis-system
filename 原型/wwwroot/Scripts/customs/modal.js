/// <reference path="../tsd/jquery.d.ts" />
/// <reference path="../tsd/jsrender.d.ts" />
/// <reference path="../tsd/sail.javascript.d.ts" />
$.fn.ModalSelect = function (set) {
    var selecter = new Sail.ModalSelect($(this), set);
    $(this).data("selecter", selecter);
    return selecter;
};
var Sail;
(function (Sail) {
    var ModalSelect = /** @class */ (function () {
        function ModalSelect(target, set) {
            var _this = this;
            this.title = set.title;
            this.api = new Sail.ApiHelper(set.api);
            this.target = $(target);
            var tool = this;
            tool.setting = $.extend({
                defaultId: "00000000-0000-0000-0000-000000000000",
                resultTarget: ".select-result[data-target=" + this.target.data("target") + "-result]",
                resultTmpl: "#" + this.target.data("target") + "ResultTmpl",
                modalTmpl: "#SearcModalTmpl",
                isAllowClear: true,
                onSelect: function () { },
                onClear: function () { },
            }, set);
            this.$Text = tool.target.find('input[type=text]');
            this.$Value = tool.target.find('input[type=hidden]');
            this.$btnClear = tool.target.find("a.zqui-input-clear");
            this.modal = new Sail.Modal(tool.setting.id, {
                title: tool.setting.title,
                tmplName: tool.setting.modalTmpl,
                cssClass: tool.setting.cssClass,
                okTitle: "",
                cancelTitle: "关闭",
                init: function (modal) {
                    tool.setting.pageSet = $.extend({
                        handleName: function () { return tool.api.GetApi("Query"); },
                        getPostKey: function () {
                            var data = modal.find(".table-toolbar").GetJson();
                            if (tool.setting.queryDataExt)
                                data = $.extend(data, tool.setting.queryDataExt());
                            return data;
                        }
                    }, tool.setting.pageSet);
                    console.log(modal.find(".page"));
                    tool.page = modal.SetPagination(tool.setting.pageSet);
                    modal.find(".btnReset").click(function () {
                        modal.find(".table-toolbar").ResetForm();
                        tool.page.Query(1);
                    });
                    modal.find("a.btnAdd").toggle(!!tool.setting.href)
                        .on("click", function () {
                        if (typeof (tool.setting.href) == "function")
                            window.open(tool.setting.href());
                        else
                            window.open(tool.setting.href);
                    });
                    tool.page.Container.on("click", ".btnSelect", function () {
                        var data = $.view(this).data;
                        tool.$Value.val(data[tool.cfg.id]).trigger("change");
                        tool.$Text.val(data[tool.cfg.name]);
                        tool.setting.onSelect(data, this, tool);
                        tool.setData(data);
                        tool.$Value.trigger("keyup");
                        tool.modal.Hide();
                    });
                }
            });
            this.cfg = $.extend({ id: "Id", name: "Name" }, this.target.data());
            this.target.on("click", "button,a.button", function () {
                tool.query();
            });
            this.$btnClear.click(function () {
                //this.$Value.val(this.setting.defaultId).trigger("change");
                _this.$Value.val(null).trigger("change");
                _this.$Text.val("");
                tool.setting.onClear(tool.target);
                tool.setData(null);
                tool.$Value.trigger("keyup");
                tool.$btnClear.hide(); //清除后隐藏按钮
            });
            this.$Value.change(function () {
                if ($(this).val() && tool.setting.isAllowClear)
                    tool.$btnClear.show();
                else
                    tool.$btnClear.hide();
            });
        }
        /**
         * 查询并弹窗
         */
        ModalSelect.prototype.query = function () {
            this.modal.modal.find(".btnReset").click();
            console.log("show");
            this.modal.Show();
        };
        ModalSelect.prototype.setData = function (data) {
            $(this.setting.resultTarget).empty();
            if (data) {
                this.$Value.val(data[this.cfg.id]);
                this.$Text.val(data[this.cfg.name]);
                if (data[this.cfg.name])
                    $(this.setting.resultTarget).Render(this.setting.resultTmpl, data);
            }
        };
        return ModalSelect;
    }());
    Sail.ModalSelect = ModalSelect;
})(Sail || (Sail = {}));
/**
 * 小区弹窗
 * @param marketId
 */
function GetCommunityModal(title) {
    return {
        title: title || "小区列表",
        modalTmpl: "#SearchCommunityModalTmpl",
        cssClass: 'zqui-dialog_lg',
        api: "Community",
        href: function () { return "/Views/BaseInfo/Community?action=add"; },
        pageSet: {
            titles: ["小区名称", /*"所属街道",*/ "小区地址", "物业联系人", "物业联系方式"],
            bodyTmpl: "#CommunityListTmpl",
        },
        onSelect: function (data, obj, tool) {
            console.log(data);
        }
    };
}
function GetApartmentCommunityModal(title) {
    return {
        title: title || "小区列表",
        modalTmpl: "#SearchCommunityModalTmpl",
        cssClass: 'zqui-dialog_lg',
        api: "ApartmentCommunity",
        href: function () { return "/Views/BaseInfo/ApartmentCommunity?action=add"; },
        pageSet: {
            titles: ["小区名称", /* "所属街道",*/ "小区地址", "物业联系人", "物业联系方式"],
            bodyTmpl: "#CommunityListTmpl",
        },
        onSelect: function (data, obj, tool) {
            console.log(data);
        }
    };
}
/**
 * 公租房弹窗
 * @param title
 */
function GetRentalHousingModal(title) {
    return {
        title: title || "公租房列表",
        modalTmpl: "#SearchRentalHousingModalTmpl",
        cssClass: 'zqui-dialog_lg',
        api: "RentalHousing",
        href: function () { return "/Views/BaseInfo/RentalHousing?action=add"; },
        pageSet: {
            titles: ["房号", /* "所属街道",*/ "所属小区", "房屋面积", "房屋状态"],
            bodyTmpl: "#RentalHousingListTmpl",
        },
        onSelect: function (data, obj, tool) {
            console.log(data);
        }
    };
}
/**
 * 合同弹窗
 * @param title
 */
function GetContractModal(title) {
    return {
        title: title || "合同列表",
        modalTmpl: "#SearchContractModalTmpl",
        cssClass: 'zqui-dialog_lg',
        api: "Contract",
        pageSet: {
            titles: ["合同编号/公租房", "承租人", "支付周期", "应付金额", "已付金额"],
            bodyTmpl: "#ContractListTmpl",
        },
        onSelect: function (data, obj, tool) {
            console.log(data);
        }
    };
}
//# sourceMappingURL=modal.js.map