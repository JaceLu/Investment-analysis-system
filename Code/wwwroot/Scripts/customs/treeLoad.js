"use strict";
// import { Validate } from "./Validate";
var Mainyf;
(function (Mainyf) {
    function notNull(obj, message) {
        if (!obj)
            console.error(message);
    }
    function isTree(bool, message) {
        if (!bool)
            console.error(message);
    }
    var TreeGraph = /** @class */ (function () {
        function TreeGraph(settings) {
            this._active = "active";

            this._checked = "checked";

            this._timerId = null;
            this._settings = $.extend({
                tmplName: "#treeTmpl",
                treeList: "#treeList",
                treeNode: ".treeNode",
                treeItem: ".zqui-tree__item",

                sumTmplName: "#sumTmpl",
                sumList: "#sumList",
                checkBox:".checkbox",

                events: {}
            }, settings);
            this._tmplName = this._settings.tmplName;
            this.$treeList = $(this._settings.treeList);

            this.$sumList = $(this._settings.sumList);
            this._sumTmplName = this._settings.sumTmplName;
            this._checkBox = this._settings.checkBox;
            this._checkData = [];

            this._treeNode = this._settings.treeNode;
            this._treeItem = this._settings.treeItem;
            this._events = this._settings.events;
            this._clickCount = 0;
        }
        /**
         * constructor before execute
         */
        TreeGraph.prototype.initialization = function (data) {
            notNull(data, "param 'data' cannot not null");
            this._rootData = data;
            //$(this._settings.treeList).Link(this._tmplName, this._rootData);

            //this._initEvents();
            this._initVisualFeedback();
            $(this._settings.treeList).find(this._treeNode).eq(0).trigger("click");
        };
        TreeGraph.prototype.initTreeData = function (data, nodeId,minTreeCodeLen) {
            notNull(data, "param 'data' cannot not null");
            this._rootData = data;
            $(this._settings.treeList).Link(this._tmplName, this._rootData);

            this._sumData = [];
            $(this._settings.sumList).Link(this._sumTmplName, this._sumData);
            this.minTreeCodeLen = minTreeCodeLen;

            this._initEvents();
            this._initVisualFeedback(minTreeCodeLen);
            $(`li[data-id='${nodeId}']`).parents("li").addClass(this._active);
            $(`li[data-id='${nodeId}']`).find(this._treeNode).addClass(this._active);
        };
        /**
         * initialization visual feedback
         */
        TreeGraph.prototype._initVisualFeedback = function () {
            var that = this;
            //$(that._settings.treeList).on("dblclick", that._treeNode, function () {
            //    var item = $(this);
            //    item.closest(that._treeItem).toggleClass(that._active);
            //    if (item.closest(that._treeItem).hasClass(that._active) && minTreeCodeLen !== -1) {
            //        $(that._settings.treeList).prepend("<div class='loading zqui-modal modal in' ><i></i></div>");
            //    }
                
            //});
            $(that._settings.treeList).on("click", that._treeNode, function () {
                var item = $(this);
                var nodes = $(that._treeNode);
                nodes.removeClass(that._active);
                item.addClass(that._active);
                $(that._settings.treeList).trigger("node:click:after", item);
               
            });

        };
       
        TreeGraph.prototype.closeAllActive = function () {
            var that = this;
            $(that._treeItem).each(function (i, v) {
                $(v).removeClass(that._active);
            });
            $(that._treeNode).each(function (i, v) {
                $(v).removeClass(that._active);
            });
        };
        TreeGraph.prototype._initEvents = function () {
            var that = this;
            var _loop_1 = function (k) {
                var item = k;
                var act = that._events[k];
                
                $(that._settings.treeList).on("click", item, function (e) {
                    //this_1.$treeList.on("click", item, function (e) {
                    return that._events[item].call(this, $(e.target).view());
                });
            };
            for (var k in this._events) {
                _loop_1(k);
            }
        };
        TreeGraph.prototype.addSubNode = function (element, data) {
            notNull(data, "param 'data' cannot not null");
            notNull(element, "param 'element' cannot not null");
            if ($(element).view().data.SubItem && $(element).view().data.SubItem.length > 0)
                return;
            $.observable(element.view().data).setProperty("SubItem", data);
            
        };
        TreeGraph.prototype.getActive = function () {
            return $(this._settings.treeList).find(this._treeNode + "." + this._active);
        };
        TreeGraph.prototype.setActive = function (element) {
            element.addClass(this._active);
        };
        TreeGraph.prototype.setActiveToFirst = function () {
            if (this.getRootNode().length == 0) {
                return;
            }
            var firstElement = this.getRootNode().eq(0);
            this.setActive(firstElement);
        };
        TreeGraph.prototype.getActiveView = function () {
            return (this.getActive() && this.getActive().view()) || {};
        };
        TreeGraph.prototype.getActiveData = function () {
            return (this.getActive() && this.getActive().view() && this.getActive().view().data) || {};
        };
        TreeGraph.prototype.getTreeData = function () {
            //return (this.$treeList && this.$treeList.view()) || {};
            return ($(this._settings.treeList) && $(this._settings.treeList).view()) || {};
        };
        TreeGraph.prototype.getNodes = function () {
            //return this.$treeList.find(this._treeNode);
            return $(this._settings.treeList).find(this._treeNode);
        };
        TreeGraph.prototype.getRootNode = function () {
            return $(this._treeNode);
        };
        //TODO 
        TreeGraph.prototype.getChildNodes = function (parentId) {
        };
        TreeGraph.prototype.addNode = function (parentNode, data) {
            if (!parentNode) {
                $.observable(this._rootData).insert(data);
            }
            else {
                if (!parentNode.SubItem)
                    parentNode.SubItem = [];
                $.observable(parentNode.SubItem).insert(data);
            }
            //this.$treeList.trigger("node:add:after", { parentNode: parentNode, data: data });
            $(this._settings.treeList).trigger("node:add:after", { parentNode: parentNode, data: data });
        };
        TreeGraph.prototype.isRemove = function (viewData) {
            if (viewData.data.SubItem && viewData.data.SubItem.length !== 0) {
                MsgBox.Error("该节点下有其他节点，不允许删除");
                return false;
            }
            return true;
        };
        TreeGraph.prototype.removeNode = function (viewData) {
            var data = viewData.data;
            var index = viewData.index;
            this.setActiveToFirst();
            if (!data.ParentId) {
                $.observable(this._rootData).remove(index);
            }
            else {
                $.observable(viewData.parent.parent.data).remove(viewData.parent.parent.data.indexOf(data));
            }
        };
        TreeGraph.prototype.updateNode = function (targetData, newData) {
            for (var tK in targetData) {
                for (var nK in newData) {
                    if (tK.toString() == nK.toString()) {
                        $.observable(targetData).setProperty(tK, newData[tK]);
                    }
                }
            }
        };
        Object.defineProperty(TreeGraph.prototype, "treeList", {
            get: function () {
                return this.$treeList;
            },
            set: function (value) {
                this.$treeList = value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(TreeGraph.prototype, "tmplName", {
            get: function () {
                return this._tmplName;
            },
            set: function (value) {
                this._tmplName = value;
            },
            enumerable: true,
            configurable: true
        });
        return TreeGraph;
    }());
    Mainyf.TreeGraph = TreeGraph;
})(Mainyf || (Mainyf = {}));
//# sourceMappingURL=tree.js.map