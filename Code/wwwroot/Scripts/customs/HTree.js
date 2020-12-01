var Button = /** @class */ (function () {
    function Button(act, title, icon, minLevel) {
        this.btnAct = act;
        this.title = title;
        this.iconClass = icon;
        this.minLevel = minLevel;
    }
    return Button;
}());
var Sail;
(function (Sail) {
    ;
    Sail.TreeTmpl = {
        openIcon: "icon-caret-down",
        closeIcon: "icon-caret-right",
        preIcon: "icon",
    };
    var HTree = /** @class */ (function () {
        function HTree(target, set) {
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
            }, set);
            this.$target.addClass("treeview");
            this.$root = $("<ul class='treeview-list'></ul>").appendTo(target);
            this.$target.on("click", ".toggleNode", function () {
                var $btn = $(this);
                $btn.toggleClass(Sail.TreeTmpl.openIcon).toggleClass(Sail.TreeTmpl.closeIcon);
                $btn.closest(".treeView-Node").children(".treeview-list").toggleClass("close");
                return false;
            })
                .on("click", ".treeview-itemname", function () {
                var $btn = $(this);
                if ($btn.closest(".treeview-item").hasClass("disable"))
                    return false;
                var $check = $btn.find(".tree-checkbox");
                if ($check.length > 0) {
                    $check.toggleClass("tree-checkbox-checked");
                    return false;
                }
                var data = $btn.closest(".treeView-Node").data("data");
                if (!tool.setting.multi) {
                    tool.$target.find(".treeview-itemname.active").removeClass("active");
                    $btn.toggleClass("active");
                }
                else {
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
        HTree.prototype.on = function (event, selector, data) {
            return this.$target.on(event, selector, data);
        };
        /**
         * 点击选中事件
         * @param act
         */
        HTree.prototype.onSelected = function (act) {
            this.$target.on("tree.selected", function (sender, data) {
                act(data);
            });
        };
        /**
         * 按钮点击事件
         * @param func
         */
        HTree.prototype.onButtonClick = function (func) {
            this.$target.on("tree.btnAct", function (sender, act, data) {
                act(act, data);
            });
        };
        /**
         * 展开或收缩某节点
         * @param {JQuery} node
         * @param {boolean} isExpand
         */
        HTree.prototype.Expand = function (node, isExpand) {
            var $btn = node.find(".toggleNode");
            $btn.toggleClass(Sail.TreeTmpl.openIcon, isExpand)
                .toggleClass(Sail.TreeTmpl.closeIcon, !isExpand);
            $btn.closest(".treeView-Node").children(".treeview-list").toggleClass("close", !isExpand);
        };
        /**
         * 重置选中
         */
        HTree.prototype.resetChecked = function () { this.$target.find(".tree-checkbox-checked").removeClass("tree-checkbox-checked"); };
        /**
         * 设置为选中
         * @param {string} id
         */
        HTree.prototype.setChecked = function (id) { this.findNode(id).children(".treeview-item").find(".tree-checkbox").addClass("tree-checkbox-checked"); };
        HTree.prototype.getCheckedData = function () { return this.$target.find(".tree-checkbox-checked").closest(".treeView-Node").map(function (i, o) { return $(o).data("data"); }).toArray(); };
        HTree.prototype.initTree = function (obj) {
            this.clearNodes();
            this.createNode(obj, null, null);
        };
        HTree.prototype.initMulitTree = function (obj) {
            var _this = this;
            this.clearNodes();
            $.each(obj, function (i, o) { _this.createNode(o, null, null); });
        };
        /**
         * 创建节点
         * @param {any} obj
         * @param {any} parent
         * @param {number} level
         */
        HTree.prototype.createNode = function (obj, parent, level) {
            var _this = this;
            if (!parent)
                parent = this.$root;
            if (level == undefined || level == NaN || level == null)
                level = 0;
            if (!obj)
                return;
            var icon = obj.Icon;
            //是否展开
            var isOpen = this.setting.expanded && (this.setting.openLevel >= level);
            var $subItems = $("<ul class='treeview-list  " + (isOpen ? "" : "close") + "'></ul>");
            var node = $("<li class='treeView-Node' data-nodeid='" + obj.Id + "'>").appendTo(parent);
            var item = $("<div class=\"treeview-item  " + (obj.IsDisabled ? "disable" : "enable") + "\">").appendTo(node);
            if (obj.SubItems && obj.SubItems.length > 0)
                $("<i class=\"" + Sail.TreeTmpl.preIcon + " " + (isOpen ? Sail.TreeTmpl.openIcon : Sail.TreeTmpl.closeIcon) + " toggleNode\"></i>").appendTo(item);
            var itemName = $('<div class="treeview-itemname">').appendTo(item);
            if (this.setting.isCheckbox)
                itemName.append("<span class=\"tree-checkbox\" ><span class=\"tree-checkbox-inner\" ></span></span>");
            if (!icon && level < this.setting.icons.length)
                icon = this.setting.icons[level];
            $("<i class=\"" + icon + "\"></i>").appendTo(itemName);
            itemName.append($("<span class='text'>").text(obj.Title));
            var $btns = $("<span class='tree-task-config'></span>");
            var btns = _.filter(this.setting.buttons, function (x) { return level >= x.minLevel; });
            if (!obj.IsNotAllowButton) {
                var bs = 0;
                $.each(btns, function (i, btn) {
                    _this.createButton(btn, $btns);
                    bs++;
                });
                if (bs > 0)
                    $btns.appendTo(itemName);
            }
            if (obj.SubItems && obj.SubItems.length > 0)
                $subItems.appendTo(node);
            node.data("data", obj);
            if (obj.SubItems)
                $.each(obj.SubItems, function (i, subitem) { _this.createNode(subitem, $subItems, level + 1); });
            this.$target.trigger("tree.rendered", [node, $btns, obj]);
        };
        /**
         * 创建按钮
         * @param {ITreeBtn} btn
         * @param {JQuery} $btns
         */
        HTree.prototype.createButton = function (btn, $btns) {
            if (!btn.class)
                btn.class = "btn-cl";
            if (!btn.text)
                btn.text = "";
            var btnTmpl = "<button title='" + btn.title + "' class='btnAct " + btn.class + "' data-act='" + btn.btnAct + "'>";
            if (btn.iconClass)
                btnTmpl += "<i class='" + btn.iconClass + "'></i>";
            btnTmpl += btn.text + " </button>";
            $(btnTmpl).appendTo($btns);
        };
        HTree.prototype.findNode = function (id) {
            return this.$root.find("[data-nodeid=" + id + "]");
        };
        HTree.prototype.setNode = function (id, active) {
            if (active == undefined)
                active = true;
            if (active)
                this.findNode(id).find(".treeview-itemname:eq(0)").trigger("click");
            else {
                this.findNode(id).find(".treeview-itemname:eq(0)").toggleClass("active", active);
            }
            console.log(222);
        };
        HTree.prototype.clearNodes = function () {
            this.$root.empty();
        };
        HTree.prototype.getSelected = function () {
            return this.$root.find(".treeview-itemname.active").closest(".treeView-Node");
        };
        HTree.prototype.getSelectedData = function () {
            return this.getSelected().data("data");
        };
        /**
         * 更新节点
         * @param {string} id
         * @param {ITree} obj
         */
        HTree.prototype.updateNode = function (id, obj) {
            var $node = this.findNode(id);
            $node.attr("data-nodeid", obj.Id);
            $node.find(".treeview-itemname b").text(obj.Title);
            $node.data("data", obj);
        };
        HTree.prototype.deleteNode = function (id) {
            var $node = this.findNode(id);
            $node.remove();
        };
        return HTree;
    }());
    Sail.HTree = HTree;
})(Sail || (Sail = {}));
//# sourceMappingURL=HTree.js.map