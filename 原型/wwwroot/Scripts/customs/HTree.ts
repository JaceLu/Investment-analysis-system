class Button {
    btnAct: string;
    title: string;
    iconClass: string;
    minLevel: number;
    constructor(act, title, icon, minLevel) {
        this.btnAct = act;
        this.title = title;
        this.iconClass = icon;
        this.minLevel = minLevel;
    }
}

module Sail {
    export interface ITreeBtn {
        title: string,
        text: string,
        btnAct: string,
        class: string,
        iconClass: string,
        minLevel: number,
    }

    export interface ITreeSet {
        expanded: boolean,
        buttons: Array<ITreeBtn>,
        icons: Array<string>,
        isCheckbox: boolean,
        multi: boolean,
        openLevel: number;//展开层级
    };

    export interface ITree {
        Id: string,
        Title: string,
        Icon: string,
        IsDisabled: boolean,
        IsNotAllowButton: boolean,
        SubItems: Array<ITree>,
        Data?: any
    }

    export var TreeTmpl = {
        openIcon: "icon-caret-down",
        closeIcon: "icon-caret-right",
        preIcon: "icon",

    };

    export class HTree {
        protected setting: ITreeSet;
        protected icons: Array<string>;

        public $root: JQuery;
        public $target: JQuery;



        public constructor(target: JQuery, set?: ITreeSet) {
            var tool = this;
            this.$target = $(target);
            this.setting = $.extend({
                multi: false,
                icons: [
                    "icon icon-briefcase icon-state-success",
                    "icon icon-folder icon-state-warning",
                    "icon icon-file icon-state-warning"
                ],
                expanded: true,
                buttons: [],
                openLevel: 0
            },
                set);

            this.$target.addClass("treeview");

            this.$root = $("<ul class='treeview-list'></ul>").appendTo(target);


            this.$target.on("click", ".toggleNode", function () {
                var $btn: JQuery = $(this);
                $btn.toggleClass(Sail.TreeTmpl.openIcon).toggleClass(Sail.TreeTmpl.closeIcon);
                $btn.closest(".treeView-Node").children(".treeview-list").toggleClass("close");
                return false;
            })
                .on("click", ".treeview-itemname", function () {
                    var $btn: JQuery = $(this);
                    if ($btn.closest(".treeview-item").hasClass("disable")) return false;
                    var $check = $btn.find(".tree-checkbox");
                    if ($check.length > 0) {
                        $check.toggleClass("tree-checkbox-checked");
                        return false;
                    }
                    var data = $btn.closest(".treeView-Node").data("data");
                    if (!tool.setting.multi) {
                        tool.$target.find(".treeview-itemname.active").removeClass("active");
                        $btn.toggleClass("active");
                    } else {
                        $btn.addClass("active");
                    }
                    tool.$target.trigger("tree.selected", data);
                    return false;
                })
                .on("click", ".btnAct", function () {
                    var $btn = $(this);
                    var act = $btn.data("act");
                    var data = $btn.closest(".treeView-Node").data("data");
                    tool.$target.trigger("tree.btnAct", [act, data]);
                    return false;
                });
        }

        /**
        * 给content注册事件
        * @param event
        * @param selector
        * @param data
        */
        public on(event: string, selector?: any, data?: any) {
            return this.$target.on(event, selector, data);
        }

        /**
         * 点击选中事件
         * @param act
         */
        public onSelected(act: any) {
            this.$target.on("tree.selected", function (sender, data) {
                act(data);
            });
        }

        /**
         * 按钮点击事件
         * @param func
         */
        public onButtonClick(func: any) {
            this.$target.on("tree.btnAct", function (sender, act, data) {
                act(act, data);
            })
        }

        /**
         * 展开或收缩某节点
         * @param {JQuery} node
         * @param {boolean} isExpand
         */
        public Expand(node: JQuery, isExpand: boolean) {

            var $btn = node.find(".toggleNode");
            $btn.toggleClass(Sail.TreeTmpl.openIcon, isExpand)
                .toggleClass(Sail.TreeTmpl.closeIcon, !isExpand);
            $btn.closest(".treeView-Node").children(".treeview-list").toggleClass("close", !isExpand);
        }

        /**
         * 重置选中
         */
        public resetChecked() { this.$target.find(".tree-checkbox-checked").removeClass("tree-checkbox-checked"); }

        /**
         * 设置为选中
         * @param {string} id
         */
        public setChecked(id: string) { this.findNode(id).children(".treeview-item").find(".tree-checkbox").addClass("tree-checkbox-checked"); }


        public getCheckedData() { return this.$target.find(".tree-checkbox-checked").closest(".treeView-Node").map((i, o) => { return $(o).data("data") }).toArray(); }

        public initTree(obj: ITree) {
            this.clearNodes();
            this.createNode(obj, null, null);
        }

        public initMulitTree(obj: Array<ITree>) {
            this.clearNodes();
            $.each(obj, (i, o) => { this.createNode(o, null, null); });
        }

        /**
         * 创建节点
         * @param {any} obj
         * @param {any} parent
         * @param {number} level 
         */
        public createNode(obj: ITree, parent: any, level: number) {
            if (!parent) parent = this.$root;
            if (level == undefined || level == NaN || level == null) level = 0;
            if (!obj) return;
            var icon = obj.Icon;

            //是否展开
            var isOpen = this.setting.expanded && (this.setting.openLevel >= level);

            var $subItems = $(`<ul class='treeview-list  ${isOpen ? "" : "close"}'></ul>`);
            var node = $(`<li class='treeView-Node' data-nodeid='${obj.Id}'>`).appendTo(parent);
            var item = $(`<div class="treeview-item  ${obj.IsDisabled ? "disable" : "enable"}">`).appendTo(node);

            if (obj.SubItems && obj.SubItems.length > 0)
                $(`<i class="${Sail.TreeTmpl.preIcon} ${isOpen ? Sail.TreeTmpl.openIcon : Sail.TreeTmpl.closeIcon} toggleNode"></i>`).appendTo(item);
            const itemName = $('<div class="treeview-itemname">').appendTo(item);

            if (this.setting.isCheckbox) itemName.append(`<span class="tree-checkbox" ><span class="tree-checkbox-inner" ></span></span>`);


            if (!icon && level < this.setting.icons.length) icon = this.setting.icons[level];
            $(`<i class="${icon}"></i>`).appendTo(itemName);
            itemName.append($("<span class='text'>").text(obj.Title));

            var $btns = $("<span class='tree-task-config'></span>");
            var btns = _.filter(this.setting.buttons, (x) => { return level >= x.minLevel });
            if (!obj.IsNotAllowButton ) {

                
                
                var bs = 0;
                $.each(btns, (i, btn) => {
                    this.createButton(btn, $btns);
                    bs++;
                });
                if (bs > 0) $btns.appendTo(itemName)
            }


            if (obj.SubItems && obj.SubItems.length > 0) $subItems.appendTo(node);
            node.data("data", obj);
            if (obj.SubItems) $.each(obj.SubItems, (i, subitem) => { this.createNode(subitem, $subItems, level + 1); });
            this.$target.trigger("tree.rendered", [node, $btns, obj]);
        }

        /**
         * 创建按钮
         * @param {ITreeBtn} btn
         * @param {JQuery} $btns
         */
        public createButton(btn: ITreeBtn, $btns: JQuery) {
            if (!btn.class) btn.class = "btn-cl";
            if (!btn.text) btn.text = "";
            var btnTmpl = `<button title='${btn.title}' class='btnAct ${btn.class}' data-act='${btn.btnAct}'>`;
            if (btn.iconClass) btnTmpl += `<i class='${btn.iconClass}'></i>`;
            btnTmpl += `${btn.text} </button>`;
            $(btnTmpl).appendTo($btns);
        }


        public findNode(id: string): JQuery {
            return this.$root.find(`[data-nodeid=${id}]`);
        }

        public setNode(id: string, active?: boolean) {
            if (active == undefined) active = true;
            if (active)
                this.findNode(id).find(".treeview-itemname:eq(0)").trigger("click");
            else {
                this.findNode(id).find(".treeview-itemname:eq(0)").toggleClass("active", active);
            }
            console.log(222);
        }

        public clearNodes() {
            this.$root.empty();
        }

        public getSelected() {
            return this.$root.find(".treeview-itemname.active").closest(".treeView-Node");
        }

        public getSelectedData() {
            return this.getSelected().data("data");
        }



        /**
         * 更新节点
         * @param {string} id
         * @param {ITree} obj
         */
        public updateNode(id: string, obj: ITree) {
            var $node = this.findNode(id);
            $node.attr("data-nodeid", obj.Id);
            $node.find(".treeview-itemname b").text(obj.Title);
            $node.data("data", obj);
        }

        public deleteNode(id: string) {
            var $node = this.findNode(id);
            $node.remove();
        }


    }


}

