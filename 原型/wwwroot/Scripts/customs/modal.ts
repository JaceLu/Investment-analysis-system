/// <reference path="../tsd/jquery.d.ts" />
/// <reference path="../tsd/jsrender.d.ts" />
/// <reference path="../tsd/sail.javascript.d.ts" />


//namespace Sail {
//    export interface IEvent {
//        handle: string;
//        act: Function;
//    }

//    export interface IPaginationSetting {
//        isPaging: boolean;
//        bodyContainer?: any;
//        getPostKey?: Function;
//        pageSize?: number;
//        handleName: any;
//        headContainer?: any;
//        footContainer?: any;
//        ajaxType?: Function;
//        bodyTmpl?: any;
//        footTmpl?: any;
//        queryed?: any;
//        titles?: Array<ITHead>;
//        titleWidth?: Array<number>;
//        events?: Array<IEvent>;
//        reQueryHandle?: string;
//    }
//}

interface JQuery {
    ModalSelect(set: Sail.IModalSelectSetting): Sail.Modal;
}

$.fn.ModalSelect = function (set: Sail.IModalSelectSetting) {
    var selecter = new Sail.ModalSelect($(this), set);
    $(this).data("selecter", selecter);
    return selecter;
};

module Sail {



    export interface IModalSelectSetting {
        defaultId: string,
        id: string;
        cssClass: string;
        resultTarget: string;
        resultTmpl: string;
        title: string;
        modalTmpl: string;
        api: string;
        href: any;
        pageSet: Sail.IPaginationSetting;
        onSelect: Function;
        onClear: Function;
        isAllowClear: boolean;
        queryDataExt: Function;
    }

    export interface ITargetCfg {
        id: string;
        name: string;
        idtarget: string;
        nametarget: string;
    }

    export class ModalSelect {
        public modal: Modal;
        public page: Pagination;
        private api: ApiHelper;

        private title: string;
        private target: JQuery;
        private $Text: JQuery;
        private $Value: JQuery;
        private $btnClear: JQuery;

        private cfg: ITargetCfg;
        private setting: IModalSelectSetting;
        constructor(target: any, set: IModalSelectSetting) {

            this.title = set.title;
            this.api = new ApiHelper(set.api);
            this.target = $(target);
            var tool = this;
            tool.setting = $.extend({
                defaultId: "00000000-0000-0000-0000-000000000000",
                resultTarget: ".select-result[data-target=" + this.target.data("target") + "-result]",
                resultTmpl: "#" + this.target.data("target") + "ResultTmpl",
                modalTmpl: "#SearcModalTmpl",
                isAllowClear: true,
                onSelect() { },
                onClear() { },
            }, set);

            this.$Text = tool.target.find('input[type=text]');
            this.$Value = tool.target.find('input[type=hidden]');
            this.$btnClear = tool.target.find("a.zqui-input-clear");


            this.modal = new Modal(tool.setting.id, {
                title: tool.setting.title,
                tmplName: tool.setting.modalTmpl,
                cssClass: tool.setting.cssClass,
                okTitle: "",
                cancelTitle: "关闭",
                init(modal: JQuery) {

                    tool.setting.pageSet = $.extend(
                        {
                            handleName() { return tool.api.GetApi("Query") },
                            getPostKey() {
                                var data = modal.find(".table-toolbar").GetJson();
                                if (tool.setting.queryDataExt)
                                    data = $.extend(data, tool.setting.queryDataExt());
                                return data;
                            }
                        }, tool.setting.pageSet);

                    console.log(modal.find(".page"));
                    tool.page = modal.SetPagination(tool.setting.pageSet);


                    modal.find(".btnReset").click(() => {
                        modal.find(".table-toolbar").ResetForm();
                        tool.page.Query(1);
                    });

                    modal.find("a.btnAdd").toggle(!!tool.setting.href)
                        .on("click", () => {
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

            this.target.on("click", "button,a.button", () => {
                tool.query();
            });

            this.$btnClear.click(() => {
                //this.$Value.val(this.setting.defaultId).trigger("change");
                this.$Value.val(null).trigger("change");
                this.$Text.val("");
                tool.setting.onClear(tool.target);
                tool.setData(null);
                tool.$Value.trigger("keyup");
                tool.$btnClear.hide();//清除后隐藏按钮

            });

            this.$Value.change(function () {
                if ($(this).val() && tool.setting.isAllowClear) tool.$btnClear.show();
                else tool.$btnClear.hide();
            });
        }

        /**
         * 查询并弹窗
         */
        public query() {
            this.modal.modal.find(".btnReset").click();
            console.log("show");
            this.modal.Show();
        }

        public setData(data) {
            $(this.setting.resultTarget).empty();

            if (data) {
                this.$Value.val(data[this.cfg.id]);
                this.$Text.val(data[this.cfg.name]);
                if (data[this.cfg.name])
                    $(this.setting.resultTarget).Render(this.setting.resultTmpl, data);
            }
        }
    }
}


/**
 * 小区弹窗
 * @param marketId
 */
function GetCommunityModal(title?: string) {
    return {
        title: title || "小区列表",
        modalTmpl: "#SearchCommunityModalTmpl",
        cssClass: 'zqui-dialog_lg',
        api: "Community",
        href: () => "/Views/BaseInfo/Community?action=add",
        pageSet: {
            titles: ["小区名称", /*"所属街道",*/ "小区地址", "物业联系人", "物业联系方式"],
            bodyTmpl: "#CommunityListTmpl",
        },
        onSelect: function (data, obj, tool) {
            console.log(data);
        }
    }
}

function GetApartmentCommunityModal(title?: string) {
    return {
        title: title || "小区列表",
        modalTmpl: "#SearchCommunityModalTmpl",
        cssClass: 'zqui-dialog_lg',
        api: "ApartmentCommunity",
        href: () => "/Views/BaseInfo/ApartmentCommunity?action=add",
        pageSet: {
            titles: ["小区名称",/* "所属街道",*/ "小区地址", "物业联系人", "物业联系方式"],
            bodyTmpl: "#CommunityListTmpl",
        },
        onSelect: function (data, obj, tool) {
            console.log(data);
        }
    }
}


/**
 * 公租房弹窗
 * @param title
 */
function GetRentalHousingModal(title?: string) {
    return {
        title: title || "公租房列表",
        modalTmpl: "#SearchRentalHousingModalTmpl",
        cssClass: 'zqui-dialog_lg',
        api: "RentalHousing",
        href: () => "/Views/BaseInfo/RentalHousing?action=add",
        pageSet: {
            titles: ["房号",/* "所属街道",*/ "所属小区", "房屋面积", "房屋状态"],
            bodyTmpl: "#RentalHousingListTmpl",
        },
        onSelect: function (data, obj, tool) {
            console.log(data);
        }
    }
}

/**
 * 合同弹窗
 * @param title
 */
function GetContractModal(title?: string) {
    return {
        title: title || "合同列表",
        modalTmpl: "#SearchContractModalTmpl",
        cssClass: 'zqui-dialog_lg',
        api: "Contract",
        pageSet: {
            titles: ["合同编号/公租房","承租人", "支付周期", "应付金额", "已付金额"],
            bodyTmpl: "#ContractListTmpl",
        },
        onSelect: function (data, obj, tool) {
            console.log(data);
        }
    }
}