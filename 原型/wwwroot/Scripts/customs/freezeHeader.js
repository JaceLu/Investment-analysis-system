/* ------------------------------------------------------------------------
Class: freezeHeader
Use:freeze header row in html table
Example 1:  $('#tableid').freezeHeader();
Example 2:  $("#tableid").freezeHeader({ 'height': '300px' });
Example 3:  $("table").freezeHeader();
Example 4:  $(".table2").freezeHeader();
Example 5:  $("#tableid").freezeHeader({ 'offset': '50px' });
Author(s): Laerte Mercier Junior, Larry A. Hendrix
Version: 1.0.8
-------------------------------------------------------------------------*/
(function ($) {
    var TABLE_ID = 0;
    $.fn.freezeHeader = function (params) {
        var copiedHeader = false;
        function freezeHeader(elem) {
            var idObj = elem.attr('id') || ('tbl-' + (++TABLE_ID));
            if (elem.length > 0 && elem[0].tagName.toLowerCase() == "table") {
                var obj = {
                    id: idObj,
                    grid: elem,
                    container: null,
                    header: null,
                    divScroll: null,
                    openDivScroll: null,
                    closeDivScroll: null,
                    scroller: null
                };
                if (params && params.height !== undefined) {
                    obj.divScroll = '<div id="hdScroll' + obj.id + '" style="height: ' + params.height + '; overflow-y: scroll">';
                    obj.closeDivScroll = '</div>';
                }
                obj.header = obj.grid.find('thead');
                if (params && params.height !== undefined) {
                    if ($('#hdScroll' + obj.id).length == 0) {
                        obj.grid.wrapAll(obj.divScroll);
                    }
                }
                obj.scroller = params && params.height !== undefined
                    ? $('#hdScroll' + obj.id)
                    : $(window);
                if (params && params.scrollListenerEl !== undefined) {
                    obj.scroller = params.scrollListenerEl;
                }
                obj.scroller.on('scroll', function () {
                    if ($('#hd' + obj.id).length == 0) {
                        obj.grid.before('<div id="hd' + obj.id + '"></div>');
                    }
                    obj.container = $('#hd' + obj.id);
                    if (obj.header.offset() != null) {
                        if (limiteAlcancado(obj, params)) {
                            elem.trigger("freeze:on");
                            if (!copiedHeader) {
                                cloneHeaderRow(obj);
                                copiedHeader = true;
                            }
                        }
                        else {
                            if (($(document).scrollTop() > obj.header.offset().top)) {
                                obj.container.css("position", "absolute");
                                obj.container.css("top", (obj.grid.find("tr:last").offset().top - obj.header.height()) + "px");
                            }
                            else {
                                elem.trigger("freeze:off");
                                obj.container.css("visibility", "hidden");
                                obj.container.css("top", "0px");
                                obj.container.width(0);
                            }
                            copiedHeader = false;
                        }
                    }
                });
            }
        }
        function limiteAlcancado(obj, params) {
            if (params && (params.height !== undefined || params.scrollListenerEl !== undefined)) {
                return (obj.header.offset().top <= obj.scroller.offset().top);
            }
            else {
                var top = obj.header.offset().top;
                if (params) {
                    if (params.offset !== undefined) {
                        top -= parseInt(params.offset.replace('px', ''), 10);
                    }
                }
                var gridHeight = (obj.grid.height() - obj.header.height() - obj.grid.find("tr:last").height()) + obj.header.offset().top;
                return ($(document).scrollTop() > top && $(document).scrollTop() < gridHeight);
            }
        }
        function cloneHeaderRow(obj) {
            obj.container.html('');
            obj.container.val('');
            var tabela = $('<table style="margin: 0 0;"></table>');
            var atributos = obj.grid.prop("attributes");
            $.each(atributos, function () {
                if (this.name != "id") {
                    tabela.attr(this.name, this.value);
                }
            });
            var clone = obj.header.clone(true);
            clone.appendTo(tabela);
            obj.container.append(tabela);
            obj.container.width(obj.header.width());
            obj.container.height(obj.header.height);
            obj.container.find('th').each(function (index) {
                var cellWidth = obj.grid.find('th').eq(index).width();
                $(this).css('width', cellWidth);
            });
            obj.container.css("visibility", "visible");
            if (params && params.height !== undefined) {
                if (params.offset !== undefined) {
                    obj.container.css("top", obj.scroller.offset().top + (params.offset.replace("px", "") * 1) + "px");
                }
                else {
                    obj.container.css("top", obj.scroller.offset().top + "px");
                }
                obj.container.css("position", "absolute");
            }
            else if (params && params.scrollListenerEl !== undefined) {
                obj.container.css("top", obj.scroller.find("thead > tr").innerHeight() + "px");
                obj.container.css("position", "absolute");
                obj.container.css("z-index", "2");
            }
            else if (params && params.offset !== undefined) {
                obj.container.css("top", params.offset);
                obj.container.css("position", "fixed");
            }
            else {
                obj.container.css("top", "0px");
                obj.container.css("position", "fixed");
            }
        }
        return this.each(function (i, e) {
            freezeHeader($(e));
        });
    };
    $.fn.tableHeadFixer = function (param) {
        var defaults = {
            head: true,
            foot: false,
            left: 0,
            right: 0
        };
        var settings = $.extend({}, defaults, param);
        return this.each(function () {
            settings.table = this;
            settings.parent = $("<div></div>");
            setParent();
            if (settings.head == true)
                fixHead();
            if (settings.foot == true)
                fixFoot();
            if (settings.left > 0)
                fixLeft();
            if (settings.right > 0)
                fixRight();
            // self.setCorner();
            $(settings.parent).trigger("scroll");
            $(window).resize(function () {
                $(settings.parent).trigger("scroll");
            });
        });
        function setTable(table) {
        }
        function setParent() {
            var container = $(settings.table).parent();
            var parent = $(settings.parent);
            var table = $(settings.table);
            table.before(parent);
            parent.append(table);
            parent
                .css({
                'width': '100%',
                'height': container.css("height"),
                'overflow': 'scroll',
                'max-height': container.css("max-height"),
                'min-height': container.css("min-height"),
                'max-width': container.css('max-width'),
                'min-width': container.css('min-width')
            });
            parent.scroll(function () {
                var scrollWidth = parent[0].scrollWidth;
                var clientWidth = parent[0].clientWidth;
                var scrollHeight = parent[0].scrollHeight;
                var clientHeight = parent[0].clientHeight;
                var top = parent.scrollTop();
                var left = parent.scrollLeft();
                if (settings.head)
                    this.find("thead tr > *").css("top", top);
                if (settings.foot)
                    this.find("tfoot tr > *").css("bottom", scrollHeight - clientHeight - top);
                if (settings.left > 0)
                    settings.leftColumns.css("left", left);
                if (settings.right > 0)
                    settings.rightColumns.css("right", scrollWidth - clientWidth - left);
            }.bind(table));
        }
        function fixHead() {
            var thead = $(settings.table).find("thead");
            var tr = thead.find("tr");
            var cells = thead.find("tr > *");
            setBackground(cells);
            cells.css({
                'position': 'relative'
            });
        }
        function fixFoot() {
            var tfoot = $(settings.table).find("tfoot");
            var tr = tfoot.find("tr");
            var cells = tfoot.find("tr > *");
            setBackground(cells);
            cells.css({
                'position': 'relative'
            });
        }
        function fixLeft() {
            var table = $(settings.table);
            var fixColumn = settings.left;
            settings.leftColumns = $();
            for (var i = 1; i <= fixColumn; i++) {
                settings.leftColumns = settings.leftColumns
                    .add(table.find("tr > *:nth-child(" + i + ")"));
            }
            var column = settings.leftColumns;
            column.each(function (k, cell) {
                var cell = $(cell);
                setBackground(cell);
                cell.css({
                    'position': 'relative'
                });
            });
        }
        function fixRight() {
            var table = $(settings.table);
            var fixColumn = settings.right;
            settings.rightColumns = $();
            for (var i = 1; i <= fixColumn; i++) {
                settings.rightColumns = settings.rightColumns
                    .add(table.find("tr > *:nth-last-child(" + i + ")"));
            }
            var column = settings.rightColumns;
            column.each(function (k, cell) {
                var cellx = $(cell);
                setBackground(cellx);
                cellx.css({
                    'position': 'relative'
                });
            });
        }
        function setBackground(elements) {
            elements.each(function (k, element) {
                var element = $(element);
                var parent = $(element).parent();
                var elementBackground = element.css("background-color");
                elementBackground = (elementBackground == "transparent" || elementBackground == "rgba(0, 0, 0, 0)") ? null : elementBackground;
                var parentBackground = parent.css("background-color");
                parentBackground = (parentBackground == "transparent" || parentBackground == "rgba(0, 0, 0, 0)") ? null : parentBackground;
                var background = parentBackground ? parentBackground : "white";
                background = elementBackground ? elementBackground : background;
                element.css("background-color", background);
            });
        }
    };
})(jQuery);
//# sourceMappingURL=freezeHeader.js.map