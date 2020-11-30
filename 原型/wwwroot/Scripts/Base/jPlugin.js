/// <reference path="../tsd/jquery.d.ts" />
/// <reference path="../tsd/jsrender.d.ts" />
/// <reference path="../tsd/underscore.d.ts" />
;
var Sail;
(function (Sail) {
    Sail.debug = false;

    Sail.enableDebug = function () { Sail.debug = true; };

    /*
     * 初始化编辑器
     * @returns {}
     */
    $.fn.initEditor = function () {
        $(this).find(".myeditor").each(function (i, o) {
            try {
                var editor = new baidu.editor.ui.Editor({ initialframewidth: "100%" });
                editor.render($(o).prop("id"));
                $(o).data("editor", editor);
            }
            catch (e) {
                console.log("编辑器初始化失败");
            }
        });
        return this;
    };
    ///重置编辑器
    $.fn.resetEditor = function () {
        try {
            $(this).find(".myeditor").each(function (i, o) {
                var editor = $(o).data("editor");
                editor.ready(function () {
                    editor.setContent('');
                });
            });
        }
        catch (e) {
            console.log("重置编辑器失败");
        }
        return this;
    };
    ///从编辑器取值到实体
    $.fn.editorToModel = function (model) {
        try {
            $(this).find(".myeditor").each(function (i, o) {
                var editor = $(o).data("editor");
                editor.ready(function () { model[$(o).data("item")] = editor.getContent(); });
            });
        }
        catch (e) {
            console.log("从编辑器取值失败");
        }
        return model;
    };
    ///设置值到编辑器
    $.fn.setToEditor = function (model) {
        try {
            $(this).find(".myeditor").each(function (i, o) {
                var $obj = $(o);
                var editor = $obj.data("editor");
                editor.ready(function () { editor.setContent(model[$obj.data("item")]); });
            });
        }
        catch (e) {
            console.log("设置编辑器内容失败");
        }
        return this;
    };
    //取val的int形式
    $.fn.valToInt = function () {
        var val = $(this).val();
        if (!val)
            return 0;
        var x = parseInt(val);
        if (isNaN(x))
            x = 0;
        return x;
    };
    //取val的float形式
    $.fn.valToFloat = function () {
        var val = $(this).val();
        if (!val)
            return 0;
        var x = parseFloat(val);
        if (isNaN(x))
            x = 0;
        return x;
    };
    ///给tbody增加合计行，skipStep可以指定每隔几行计算一次
    $.fn.AddSum = function (cols, title, skipStep) {
        var tbody = $(this);
        var table = tbody.parent();
        var tfoot = $("<tr>");
        if (!skipStep)
            skipStep = 1;
        var trs = table.find("thead tr:eq(0) th").length;
        tfoot.html("");
        $("<th>{0}</th>".format(title ? title : "合计")).appendTo(tfoot);
        for (var i = 1; i < trs; i++) {
            $("<th>&nbsp;</th>").appendTo(tfoot);
        }
        //console.log(tfoot.html());
        $.each(cols, function (i, o) {
            var sum = 0;
            var tdIndex = -1;
            var fixed = 2;
            if (typeof (o) == "number") {
                tdIndex = o;
            }
            else {
                tdIndex = o[0];
                fixed = o[1];
            }
            tbody.find("tr").each(function (trindex) {
                if (trindex % skipStep !== 0)
                    return true;
                var td = $(this).find("td:eq(" + tdIndex + ")");
                var istablesaw = td.find("span.tablesaw-cell-content").length > 0;
                var value = istablesaw ? td.find("span.tablesaw-cell-content").text() : td.text();
                $.each(value.split("\n"), function (index, str) {
                    if (str) {
                        var num = parseFloat(str);
                        if (!isNaN(num))
                            sum += num;
                    }
                });
                return true;
            });
            //console.log(tfoot.html());
            tfoot.appendTo(tbody);
            tfoot.find("th:eq(" + tdIndex + ")").text(sum.toFixed(fixed));
        });
        return tfoot;
    };
    $.fn.Display = function (isShow) {
        var $this = $(this);
        if (isShow)
            $this.show();
        else
            $this.hide();
        return $this;
    };
    //$.fn.MakeHead = function (heads, widths) {
    //    $(this).empty();
    //    var tr = $("<tr></tr>").appendTo(this);
    //    $.each(heads, function (i, o) {
    //        var $th = $("<th>");
    //        $th.text(o).appendTo(tr);
    //        try {
    //            if (widths && widths[i] > 0)
    //                $th.width(widths[i]);
    //        }
    //        catch (err) { }
    //    });
    //    return this;
    //};
    $.fn.MakeHead = function (heads) {
        $(this).empty();
        var tr = $("<tr></tr>").appendTo(this);
        try {
            heads.map(function (v, i) {
                var $th = $("<th>").appendTo(tr);
                if (typeof (v) === "string")
                    $th.text(v);
                else {
                    if (v.name)
                        $th.text(v.name);
                    if (v.class)
                        $th.addClass(v.class);
                    if (v.width)
                        $th.width(v.width);
                }
            });
        }
        catch (err) {
            console.log(err);
        }
        return this;
    };
    var clickOrTouch = (("ontouchend" in window)) ? "touchend" : "click";
    $.fn.ClickOrTouch = function (target, act) {
        var $this = $(this);
        $this.on(clickOrTouch, target, act);
        return $this;
    };
    ///设置弹出日期控件
    $.fn.SetDate = function () {
        $(this).on("focus", function () {
            WdatePicker({
                lang: "zh-cn",
                skin: "default",
                onpicked: function () {
                    $(this).blur();
                    $(this).change();
                },
                oncleared: function () {
                    $(this).blur();
                    $(this).change();
                }
            });
        }).prop("readonly", true);
        return this;
    };
    $.fn.SetDateTime = function () {
        $(this).on("focus", function () {
            WdatePicker({
                dateFmt: "yyyy-M-d HH:mm:ss",
                minDate: "%y-%M-%d 08:mm:ss",
                lang: "zh-cn",
                skin: "whyGreen",
                onpicked: function () {
                    $(this).blur();
                    $(this).change();
                },
                oncleared: function () {
                    $(this).blur();
                    $(this).change();
                }
            });
        }).prop("readonly", true);
        return this;
    };
    /*
     * 初始化表单
     * @returns
     */
    $.fn.initForm = function () {
        var form = $(this);
        form.find("input.date:enabled").SetDate();
        form.find("input.datetime:enabled").SetDateTime();
        form.find("select.ajaxSelect").each(function () {
            $(this).ajaxAddOptions();
        });
        $(".dateSwitch").each(function (i, o) {
            $(o).DateSwitch();
        });
        if ($.fn.bootstrapSwitch) {
            form.find("input[type=checkbox].checkSwitch").each(function () {
                var check = $(this);
                check.bootstrapSwitch({
                    onText: check.data("ontext"),
                    offText: check.data("offtext"),
                    labelText: check.data("label")
                });
            });
        }
        form.initEditor();
        if ($.registerWang)
            $.registerWang();
        form.trigger("after.InitForm", form);
        return form;
    };
    /*
     * 重置表单
     * @returns
     */
    $.fn.ResetForm = function () {
        var form = $(this);
        form.find("textarea,input:not([type=radio], [type=checkbox])").val("").trigger("change");
        form.find("input[type=checkbox],input[type=radio]").prop("checked", false).trigger("change");
        form.find(".has-error").removeClass("has-error");
        if ($.fn.bootstrapSwitch) {
            form.find("input[type=checkbox].checkSwitch").each(function () {
                $(this).bootstrapSwitch("state", false).prop("checked", false);
            });
        }
        form.find("input,textarea").each(function () {
            var $this = $(this);
            var defaultValue = $this.data("defaultvalue");
            ///通过文本赋值默认值
            if (defaultValue != undefined)
                $this.val(defaultValue);
            else
                $this.val("");
            $this.trigger("change");
        });
        form.find("img").each(function () {
            var $this = $(this);
            var defaultValue = $this.data("defaultvalue");
            ///通过文本赋值默认图片
            if (defaultValue != undefined)
                $this.prop("src", defaultValue);
        });
        form.find("select").each(function () {
            var $this = $(this);
            var defaultValue = $this.data("defaultvalue");

            if (defaultValue == undefined)
                defaultValue = $this.find("option:eq(0)").val();
            $this.val(defaultValue).trigger("change");
        });
        form.resetEditor();
        $("body").trigger("after.ResetForm", form);
        form.trigger("after.form.ResetForm", form);
        return form;
    };
    $.fn.tagName = function () {
        return this.prop("tagName");
    };
    /**
     * 序列化表单
     * @returns
     */
    $.fn.GetJson = function () {
        var model = {};
        var form = $(this);
        var items = form.find("input,textarea,select");
        items.each(function () {
            var obj = $(this);
            var name = obj.prop("name");
            var id = obj.prop("id");
            if (!id || id.indexOf("s2id_") === 0)
                return true;
            if (!name) {
                name = id;
                obj.prop("name", name);
            }
            ;
            return true;
        });
        var o = items.serializeArray();
        for (var i in o) {
            if (o.hasOwnProperty(i)) {
                var name = o[i].name;
                var value = o[i].value;
                if (typeof (model[name]) == "undefined")
                    model[name] = value;
                else
                    model[name] += "," + value;
            }
        }
        for (var p in model) {
            if (model.hasOwnProperty(p)) {
                model[p] = $.trim(model[p]);
            }
        }
        form.find("input[type=checkbox][data-valuetype=bool]").each(function (index, obj) {
            var checkbox = $(this);
            var id = checkbox.prop("id");
            model[id] = checkbox.is(":checked");
        });
        model = form.editorToModel(model);
        form.trigger("after.GetJson", model);
        return model;
    };
    /***
    *添加options
    *
    */
    $.fn.AddOptions = function (data, defaulttext, defaultvalue, setting) {
        var set = $.extend({ key: "Key", value: "Value" }, setting);
        var $select = $(this);
        var optStr = "<option>";
        if (!defaultvalue)
            defaultvalue = "";
        $select.empty();
        if (defaulttext)
            $(optStr).val(defaultvalue).text(defaulttext).appendTo($select);
        $.each(data, function (i, o) {
            $(optStr).val(o[set.key]).text(o[set.value]).appendTo($select).data(o);
        });
    };
    /**
     *
     * @param {string} url?
     * @param {string} id?
     * @param {string} text?
     * @param {string} defaultId?
     * @param {string} defaultText?
     * @param {Function} init?
     * @returns
     */
    $.fn.ajaxAddOptions = function (url, id, text, defaultId, defaultText) {
        var item = $(this);
        var $this = item;
        if (!url)
            url = $this.data("url");
        if (!url)
            return item;
        if (url.indexOf("/") !== 0)
            url = $.SailConfig.ApiRoot + url;
        if (!id) {
            id = $this.data("value") || "id";
            text = $this.data("text") || "Name";
            defaultId = $this.data("defaultvalue");
            defaultText = $this.data("defaulttext");
        }
        ;
        var optstr = "<option value='{0}'>{1}</<option>";
        item.empty();
        if (defaultText)
            item.append(optstr.format(defaultId, defaultText));
        $.ajax({
            //async: false,
            url: url,
            success: function (json) {
                if (json.IsSuccess) {
                    $(json.Data).each(function (index, element) {
                        var option = $(optstr.format(element[id], element[text]));
                        option.data("item", element);
                        item.append(option);
                    });
                    item.trigger("ajax.Done", json.Data);
                }
            }
        });
        return item;
    };
    /**
     * 关闭自定义弹窗
     */
    $.fn.closeModal = function () {
        $("#modal_overlay").fadeOut(200);
        $(this).trigger("hidden.bs.modal");
        $(this).hide();
    };
    /**
     * 兼容bootstrap弹窗插件
     * @param options
     */
    $.fn.myModal = function (options) {
        var modal = $(this);
        var overlay = $("#modal_overlay");
        if (overlay.length === 0) {
            $("body").append($("<div id='modal_overlay'></div>"));
            overlay = $("#modal_overlay");
        }
        switch (options) {
            case "hide":
                modal.closeModal();
                break;
            default:
                modal.off("click", "[aria-hidden=true]")
                    .on("click", "[aria-hidden=true]", function () {
                        modal.closeModal();
                    });
                overlay.css({ display: "block", opacity: 0 });
                overlay.fadeTo(100, 0.6);
                modal.fadeTo(100, 1);
        }
    };
    /**
     * 双向绑定工具
     * @param {type} object
     * @returns
     */
    $.fn.ItemBinder = function (object) {
        var $this = $(this);
        var binder = $this.data("binder");
        if (!binder) {
            binder = new Sail.Binder($(this), object);
            $this.data("binder", binder);
        }
        if (object)
            binder.SetObject(object);
        return binder;
    };
    //验证表单
    $.fn.CheckValidation = function () {
        var r = $(this).validationEngine({ returnIsValid: true });
        return r;
    };
    /*回车触发按钮的点击*/
    $.fn.EnterToClick = function (btn) {
        $(this).keydown(function (event) {
            if (event.keyCode === 13)
                $(btn).click();
        });
        return this;
    };
    //给表格中的空白单元格加上一个空格
    $.fn.TableChange = function () {
        var $this = $(this);
        $this.find("td").each(function (i, o) {
            if (!$(o).html())
                $(o).html("&nbsp;");
        });
        return $this;
    };
    //根据src的路径获取远程脚本内容,或直接返回内部的html元素
    $.fn.loadRemoteTmpl = function () {
        var $this = $(this);
        var html = $this.html();
        if (!html || !html.trim()) {
            if ($this.prop("src")) {
                $.ajax({
                    url: $this.prop("src"),
                    async: false,
                    cache: false,
                    success: function (template) { $this.html(template); }
                });
            }
        }
        return $this;
    };
    //绑定模板和数据
    $.fn.Link = function (tmpl, data) {
        if (!$.isArray(data))
            data = [data];
        $.Loadtmpl(tmpl).link(this, data);
        if ($.fn.tooltip)
            $(this).find("[data-toggle=tooltip]").tooltip();
        return $(this).TableChange();
    };
    //用模板和数据生成html
    $.fn.Render = function (tmpl, data) {
        if (!$.isArray(data))
            data = [data];
        if ($.fn.tooltip)
            $(this).find("[data-toggle=tooltip]").tooltip();
        $(this).html($.Loadtmpl(tmpl).render(data));
        return $(this).TableChange();
    };
    //增加日期选择方式
    $.fn.DateSwitch = function () {
        var $this = $(this);
        var $ipts = $this.find("input[type=text]");
        var $start = $ipts.eq(0);
        var $end = $ipts.eq(1);
        var dayFormat = "yyyy-MM-dd";
        [".today", ".thisWeek", ".thisMonth", ".preDay", ".nextDay", ".preWeek", ".nextWeek", ".preMonth", ".nextMonth"].forEach(function (x) {
            $this.off("click", x);
        });
        $this.on("click", ".today", function () {
            var date = new Date().format(dayFormat);
            $start.val(date);
            $end.val(date);
        }).on("click", ".thisWeek", function () {
            $start.val(new Date().WeekFirst().format(dayFormat));
            $end.val(new Date().WeekEnd().format(dayFormat));
        }).on("click", ".thisMonth", function () {
            $start.val(new Date().MonthFirst().format(dayFormat));
            $end.val(new Date().MonthEnd().format(dayFormat));
        }).on("click", ".preDay", function () {
            if (!$start.val())
                $start.val(new Date().format(dayFormat));
            var date = DateTime.Parse($start.val()).AddDays(-1).format(dayFormat);
            $start.val(date);
            $end.val(date);
        }).on("click", ".nextDay", function () {
            if (!$start.val())
                $start.val(new Date().format(dayFormat));
            var date = DateTime.Parse($start.val()).AddDays(1).format(dayFormat);
            $start.val(date);
            $end.val(date);
        }).on("click", ".preWeek", function () {
            if (!$start.val())
                $start.val(new Date().format(dayFormat));
            var date = DateTime.Parse($start.val()).WeekFirst().AddDays(-7);
            $start.val(date.format(dayFormat));
            $end.val(date.WeekEnd().format(dayFormat));
        }).on("click", ".nextWeek", function () {
            if (!$start.val())
                $start.val(new Date().format(dayFormat));
            var date = DateTime.Parse($start.val()).WeekFirst().AddDays(7);
            $start.val(date.format(dayFormat));
            $end.val(date.WeekEnd().format(dayFormat));
        }).on("click", ".preMonth", function () {
            if (!$start.val())
                $start.val(new Date().format(dayFormat));
            var date = DateTime.Parse($start.val()).MonthFirst().AddMonths(-1);
            $start.val(date.format(dayFormat));
            $end.val(date.MonthEnd().format(dayFormat));
        }).on("click", ".nextMonth", function () {
            if (!$start.val())
                $start.val(new Date().format(dayFormat));
            var date = DateTime.Parse($start.val()).MonthFirst().AddMonths(1);
            $start.val(date.format(dayFormat));
            $end.val(date.MonthEnd().format(dayFormat));
        });
        var isInit = $this.data("init");
        if (isInit !== false)
            $this.find(".thisMonth").click();
        return $this;
    };
    /*    $.fn.InitLogin = function (set) {
            var $this = $(this);
            var settings = $.extend({
                inputId: "#LoginId",
                inputPwd: "#Password",
                ckRember: "#ckRemember",
                btnLogin: "#btnLogin",
                cookieName: "_LoginId"
            }, set);
            var $loginId = $this.find(settings.inputId);
            var $Password = $this.find(settings.inputPwd);
            var $ckRemember = $this.find(settings.ckRember);
            var $btnLogin = $this.find(settings.btnLogin);
            var cookieName = settings.path + settings.cookiesName;
            $loginId.focus();
            $Password.EnterToClick($btnLogin);
            var loginId = $.cookie(cookieName);
            if (loginId) {
                $ckRemember.attr("checked", true);
                console.log(loginId);
                $loginId.val(loginId["/LoginId"]);
                $Password.focus();
            }
            var api = new Sail.ApiHelper(settings.controller);
            $btnLogin.click(function () {
            //wtf
                api.get("login", { "": JSON.stringify($this.GetJson()) }, function (result) {
                    if (!result.IsSuccess) {
                        ShowError(result.Msg);
                        return;
                    }
                    if ($ckRemember.is(":checked"))
                        $.cookie(cookieName, $loginId.val(), { expires: 30, path: settings.path });
                    else
                        $.removeCookie(cookieName, { path: settings.path });
                    window.location.href = settings.path;
                });
            });
        };*/
    $.fn.InitLogin = function (controll, path) {
        var $this = $(this);
        var $loginId = $this.find("#LoginId");
        var $Password = $this.find("#Password");
        var $ckRemember = $this.find("#ckRemember");
        console.log($ckRemember);
        var $btnLogin = $this.find("#btnLogin");
        var cookieName = path + 'LoginId';
        console.log(cookieName);
        $loginId.focus();
        $Password.EnterToClick($btnLogin);
        var loginId = $.cookie(cookieName);
        console.log(loginId);
        if (loginId) {
            $ckRemember.attr("checked", true);
            $loginId.val(loginId);
            $Password.focus();
        }
        $btnLogin.click(function () {
            $.post($.SailConfig.ApiRoot + controll + "/login", { "": JSON.stringify($this.GetJson()) }, function (result) {
                if (!result.IsSuccess) {
                    ShowError(result.Msg);
                    return;
                }
                if ($ckRemember.is(":checked")) {
                    console.log($loginId.val());
                    $.cookie(cookieName, $loginId.val(), { expires: 30, path: path });
                    console.log($.cookie(cookieName));
                }
                else
                    $.removeCookie(cookieName, { path: path });
                window.location.href = path;
            });
        });
    };
    //导出excel
    $.fn.Table2Excel = function (options) {
        var e = this;
        e.each(function () {
            if (!$.data(e, "plugin_table2excel")) {
                $.data(e, "plugin_table2excel", new Sail.ExportExcel(this, options));
            }
        });
        return e;
    };
    //导出excel的class
    var ExportExcel = (function () {
        function ExportExcel(element, options) {
            this.element = element;
            this.defaults = {
                exclude: ".noExl",
                name: "Table2Excel"
            };
            this.settings = $.extend({}, this.defaults, options);
            this.init();
        }
        ExportExcel.prototype.init = function () {
            var _this = this;
            var e = this;
            e.uri = "data:application/vnd.ms-excel;base64,";
            e.template = {
                head: "<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets>",
                sheet: {
                    head: "<x:ExcelWorksheet><x:Name>",
                    tail: "</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet>"
                },
                mid: "</x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body>",
                table: {
                    head: "<table>",
                    tail: "</table>"
                },
                foot: "</body></html>"
            };
            e.tableRows = [];
            // get contents of table except for exclude
            $(e.element).each(function (i, o) {
                var tempRows = "";
                $(o).find("tr").not(_this.settings.exclude).each(function (i, o) {
                    tempRows += "<tr>" + $(o).html() + "</tr>";
                });
                _this.tableRows.push(tempRows);
            });
            e.tableToExcel(e.tableRows, e.settings.name);
        };
        ExportExcel.prototype.format = function (s, c) {
            return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; });
        };
        ;
        ExportExcel.prototype.base64 = function (s) {
            return window.btoa(unescape(encodeURIComponent(s)));
        };
        ;
        ExportExcel.prototype.getFileName = function (settings) {
            return (settings.filename ? settings.filename : "table2excel") + ".xls";
        };
        ExportExcel.prototype.tableToExcel = function (table, name) {
            var e = this;
            e.ctx = {
                worksheet: name || "Worksheet",
                table: table
            };
            var fullTemplate = e.template.head;
            if ($.isArray(table)) {
                for (var i in table) {
                    fullTemplate += e.template.sheet.head + "Table" + i + "" + e.template.sheet.tail;
                }
            }
            fullTemplate += e.template.mid;
            if ($.isArray(table)) {
                for (var i in table) {
                    fullTemplate += e.template.table.head + "{table" + i + "}" + e.template.table.tail;
                }
            }
            fullTemplate += e.template.foot;
            for (var i in table) {
                if (table.hasOwnProperty(i)) {
                    e.ctx["table" + i] = table[i];
                }
            }
            delete e.ctx.table;
            var link = e.uri + e.base64(e.format(fullTemplate, e.ctx));
            var a = document.createElement("a");
            a.download = e.getFileName(e.settings);
            a.href = link;

            a.setAttribute("hidden", "hidden");
            document.body.append(a);
            a.click();
            return true;
        };
        return ExportExcel;
    }());
    Sail.ExportExcel = ExportExcel;
})(Sail || (Sail = {}));
;

$().ready(function () {
    $('body').initForm().validationEngine();
});
;
function guid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
var Sail;
(function (Sail) {
    $.setSailConfig = function (set) {
        $.SailConfig = $.extend({
            Root: "/",
            ApiRoot: "/Api/",
            PageContent: ".flexbox-content"
        }, set);
    };
    $.setSailConfig(null);
    $.CreateModal = function (id, set) { return new Sail.Modal(id, set); };
    //读取参数
    $.LoadParam = function (obj) { return JSON.parse(decodeURIComponent(btoa($(obj).val()))); };
    ///把url后面的参数序列化成对象
    $.decodeParam = function () {
        if (!location.search)
            return {};
        var search = location.search.substring(1);
        return JSON.parse("{\"" + decodeURI(search).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + "\"}");
    };
    //把对象编码成字符串
    $.encodeObject = function (obj) { return atob(encodeURIComponent(JSON.stringify(obj))); };
    //把编码后的字符串还原成对象
    $.decodeObject = function (str) { return JSON.parse(decodeURIComponent(btoa(str))); };
    /**
     * 获取url参数
     * @param name
     * @returns
     */
    $.Request = function (name) {
        var url = window.location.href;
        var reg = new RegExp("(\\?|&)" + name + "=([^&?#]*)", 'i');
        var arr = url.match(reg);
        if (arr)
            return decodeURIComponent(arr[2]);
        return "";
    };
    /**
     * 双向绑定工具
     * @param container
     * @param {Object} objItem
     * @returns
     */
    $.ItemBinder = function (container, objItem) {
        var result = new Sail.Binder(container, objItem);
        return result;
    };
    $.Delete = function (url, data, act) {
        if (typeof data == "function") {
            act = data;
            data = null;
        }
        return $.ajax({
            url: url,
            type: "delete",
            data: data,
            dataType: "json",
            success: act
        });
    };
    $.Put = function (url, data, act) {
        if (typeof data == "function") {
            act = data;
            data = null;
        }
        return $.ajax({
            type: "put",
            url: url,
            data: data,
            dataType: "json",
            success: act
        });
    };
    ///加载html内容
    $.LoadHtml = function (tmplName) {
        //获取真实模版文件名
        function formatTemplatePath(name) {
            return $.SailConfig.Root + "templates/" + name + ".tmpl.html";
        }
        ;
        var tmplHtml = "";
        if (tmplName) {
            if (tmplName.indexOf("#") == 0) {
                var template = $(tmplName).loadRemoteTmpl().html();
                tmplHtml = template;
            }
            else {
                tmplHtml = $(document).data(tmplName + "_Tmpl");
                if (!tmplHtml)
                    $.ajax({
                        url: formatTemplatePath(tmplName),
                        async: false,
                        cache: false,
                        success: function (template) {
                            tmplHtml = template;
                            $(document).data(tmplName + "_Tmpl", template);
                        }
                    });
            }
        }
        return tmplHtml;
    };
    $.Loadtmpl = function (tmplName) {
        var tmplHtml = $.LoadHtml(tmplName);
        return $.templates(tmplHtml);
    };
    $.Pagination = function (set) { return new Sail.Pagination(set); };
})(Sail || (Sail = {}));
var DateTime;
(function (DateTime) {
    Date.prototype.dayofWeek = function () {
        var week = ["周日", "周一", "周二", "周三", "周四", "周五", "周六"];
        return week[this.getDay()];
    };
    Date.prototype.format = function (format) {
        var o;
        o = {
            "M+": this.getMonth() + 1,
            "d+": this.getDate(),
            "h+": this.getHours(),
            "m+": this.getMinutes(),
            "s+": this.getSeconds(),
            "q+": Math.floor((this.getMonth() + 3) / 3),
            "S": this.getMilliseconds(),
            "ddd": this.dayofWeek()
        };
        if (/(y+)/.test(format))
            format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o) {
            if (o.hasOwnProperty(k)) {
                if (new RegExp("(" + k + ")").test(format)) {
                    var tmp = RegExp.$1.length === 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length);
                    format = format.replace(RegExp.$1, tmp);
                }
            }
        }
        return format;
    };
    Date.prototype.MonthFirst = function () {
        var result = new Date(this.getTime());
        result.setDate(1);
        return result;
    };
    Date.prototype.MonthEnd = function () {
        var result = new Date(this.getTime());
        result.setDate(DaysCount(this.getFullYear(), this.getMonth() + 1));
        return result;
    };
    Date.prototype.WeekFirst = function () {
        var day = this.getDay();
        if (day === 0)
            return this.AddDays(-6);
        else
            return new Date(this - (this.getDay() - 1) * 86400000);
    };
    Date.prototype.WeekEnd = function () {
        var weekFirstDay = this.WeekFirst();
        return new Date((weekFirstDay / 1000 + 6 * 86400) * 1000);
    };
    /**
     * 时间变化核心函数
     * @param {any[]} ...args
     * @returns
     */
    function AddDateTime(addType, numbers, dateTime) {
        if (numbers == 0 || !addType)
            return dateTime;
        addType = addType.replace(/(^[\s　]*)|([\s　]*$)/g, "");
        addType = addType.toLowerCase();
        try {
            var giYear = dateTime.getFullYear();
            var giMonth = dateTime.getMonth();
            var giDay = dateTime.getDate();
            var giHour = dateTime.getHours();
            var giMinute = dateTime.getMinutes();
            var giSecond = dateTime.getSeconds();
            var giMillisecond = dateTime.getMilliseconds();
            switch (addType) {
                case "millisecond":
                    {
                        giMillisecond += numbers;
                        break;
                    }
                case "second":
                    {
                        giSecond += numbers;
                        break;
                    }
                case "minute":
                    {
                        giMinute += numbers;
                        break;
                    }
                case "hour":
                    {
                        giHour += numbers;
                        break;
                    }
                case "day":
                    {
                        giDay += numbers;
                        break;
                    }
                case "month":
                    {
                        giMonth += numbers;
                        break;
                    }
                case "year":
                    {
                        giYear += numbers;
                        break;
                    }
            }
            dateTime.setFullYear(giYear);
            dateTime.setMonth(giMonth);
            dateTime.setDate(giDay);
            dateTime.setHours(giHour);
            dateTime.setMinutes(giMinute);
            dateTime.setSeconds(giSecond);
            dateTime.setMilliseconds(giMillisecond);
        }
        catch (e) { }
        return dateTime;
    }
    ;
    Date.prototype.AddSeconds = function (seconds) {
        return AddDateTime("second", seconds, this);
    };
    Date.prototype.AddMinutes = function (minutes) {
        return AddDateTime("minute", minutes, this);
    };
    Date.prototype.AddHours = function (hours) {
        return AddDateTime("hour", hours, this);
    };
    Date.prototype.AddDays = function (days) {
        return AddDateTime("day", days, this);
    };
    Date.prototype.AddMonths = function (months) {
        return AddDateTime("month", months, this);
    };
    Date.prototype.AddYears = function (years) {
        return AddDateTime("year", years, this);
    };
    /**
     * 把字符串解析成日期时间格式
     * @param {string} str
     * @returns {Date} 时间
     */
    function Parse(str) {
        if (!str)
            return null;
        var regexIso8601 = /^(?:-([0-9]{1,2})|([0-9]{4}))?(?:-?(?:([0-9]{2})?(?:-?([0-9]{2}))?|W([0-9]{2})(?:-?([1-7]))?|([0-9]{3})))?(?:T([0-9]{2})(?::?([0-9]{2})(?::?([0-9]{2}))?)?(?:[,\.]([0-9]+))?(?:(Z)|([+-])([0-9]{2})(?::?([0-9]{2}))?)?)?$/;
        var m = regexIso8601.exec(str);
        if (m) {
            return new Date(m[2], (m[3] || 1) - 1, m[4] || 1, m[8] || 0, m[9] || 0, m[10] || 0, m[11] || 0);
        }
    }
    DateTime.Parse = Parse;
    ;
    /**
     * 判断指定年是否为闰年
     * @param {number} year
     * @returns
     */
    function IsLeapYear(year) {
        return (year % 4 === 0) || (year % 100 === 0) || (year % 400 === 0);
    }
    DateTime.IsLeapYear = IsLeapYear;
    /**
     * 获取某年或某月的总天数
     * @param {number} year 年份
     * @param {number} month? 月份
     * @return {number} 天数
     */
    function DaysCount(year, month) {
        var months = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (!month)
            return IsLeapYear(year) ? 366 : 365;
        var days = months[month - 1];
        if (IsLeapYear(year) && month == 2)
            days = 29;
        return days;
    }
    DateTime.DaysCount = DaysCount;
    /**
     * 返回某年每个月的天数
     * @param {number} year
     */
    function MonthDaysList(year) {
        var months = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (IsLeapYear(year))
            months[1] = 29;
        return months;
    }
    DateTime.MonthDaysList = MonthDaysList;
})(DateTime || (DateTime = {}));
var Sail;
(function (Sail) {
    (function (ControlType) {
        ControlType[ControlType["Val"] = 0] = "Val";
        ControlType[ControlType["Checked"] = 1] = "Checked";
        ControlType[ControlType["Src"] = 2] = "Src";
        ControlType[ControlType["Html"] = 3] = "Html"; //内容
    })(Sail.ControlType || (Sail.ControlType = {}));
    var ControlType = Sail.ControlType;
    ;
    $.fn.GetControlType = function () {
        var control = this;
        switch (control.prop("tagName")) {
            case "IMG":
                return ControlType.Src;
            case "SELECT":
            case "TEXTAREA":
                return ControlType.Val;
            case "INPUT":
                switch (control.attr("type")) {
                    case "checkbox":
                    case "radio":
                        return ControlType.Checked;
                    default:
                        return ControlType.Val;
                }
            default:
                return ControlType.Html;
        }
    };
    $.fn.SetValue = function (value) {
        function formatValue(control, value) {
            var helper = control.data("helper");
            if (helper) {
                var func = Sail.Binder.Helpers[helper];
                return func(value);
            }
            return value;
        }
        var control = this;
        if (control && typeof (value) != "undefined") {
            switch (control.GetControlType()) {
                case ControlType.Checked:
                    control.prop("checked", value);
                    if ($.fn.bootstrapSwitch && control.hasClass("checkSwitch"))
                        control.bootstrapSwitch("state", value);
                    break;
                case ControlType.Val:
                    if (value || value === 0) {
                        if (control.hasClass("date") && value)
                            control.val(value.substring(0, 10));
                        else if (control.hasClass("datetime") && value)
                            control.val(DateTime.Parse(value).format("yyyy-MM-dd hh:mm"));
                        else if (control.data("dateformat") && value)
                            control.val(DateTime.Parse(value).format(control.data("dateformat")));
                        else {
                            control.val(formatValue(control, value));
                        }
                    }
                    break;
                case ControlType.Src:
                    control.prop("src", value);
                    break;
                case ControlType.Html:
                    if (!value && value !== 0)
                        control.empty();
                    else {
                        if (control.hasClass("date"))
                            control.html(value.substring(0, 10));
                        else if (control.hasClass("datetime"))
                            control.html(DateTime.Parse(value).format("yyyy-MM-dd hh:mm"));
                        else if (control.data("dateformat"))
                            control.html(DateTime.Parse(value).format(control.data("dateformat")));
                        else
                            control.html(formatValue(control, value));
                    }
                    break;
            }
            control.trigger("change");
        }
    };
    $.fn.GetValue = function (type) {
        var control = this;
        var controlType = control.GetControlType();
        switch (type) {
            case "boolean":
                return control.is(":checked");
            case "number":
                return parseFloat(control.val());
            case "string":
            default:
                switch (controlType) {
                    case ControlType.Val:
                        return control.val();
                    case ControlType.Src:
                        return control.prop("src");
                    case ControlType.Html:
                        return control.html();
                    default:
                        return control.val();
                }
        }
    };
    /**
     * 获取对象的属性列表
     * @param obj
     */
    function getProperties(obj) {
        var props = [];
        for (var p in obj) {
            if (obj.hasOwnProperty(p)) {
                if (typeof (obj[p]) != "function")
                    props.push({ Name: p, Value: obj[p], Type: typeof (obj[p]) });
            }
        }
        return props;
    }
    Sail.getProperties = getProperties;
    /**
     * 绑定对象到表单
     * @param model
     * @returns {}
     */
    $.fn.BindObject = function (model) {
        var $item = $(this);
        var props = getProperties(model);
        for (var index in props) {
            if (props.hasOwnProperty(index)) {
                var p = props[index];
                var control = $item.find("#" + p.Name + ",[name=" + p.Name + "]"); //找到能匹配id或name的对象
                if (control.length > 0) {
                    /**
                     * 检查有没有data-prop属性，如果有，就从目标值中获取prop指定的字段的值
                     * 常见的{a:{b:1}} 数据，会把1赋值给
                     * <select id='a' data-prop='b'> 这样的元素
                     */
                    var prop = control.data("prop");
                    var value = p.Value;
                    try {
                        if (prop)
                            value = p.Value[prop];
                    }
                    catch (e) {
                        value = p.Value;
                    }
                    if (value == null)
                        value = "";
                    control.SetValue(value);
                }
                else if (p.Type == "object") {
                    $item.find("[data-prop='" + p.Name + "']").each(function (index, obj) {
                        var id = $(obj).prop("id");
                        try {
                            if (p.Value)
                                this.SetValue($(obj), p.Value[id]);
                        }
                        catch (e) {
                        }
                    });
                }
            }
        }
        /**
         * 遍历所有.prop这个class的非输入html元素，根据name赋值
         * 例如 { a:{b:1} } 这样的数据 <span name='a.b'>  就能被赋值为1
         */
        $item.find(".prop:not(input):not(select):not(textarea)").each(function (i, o) {
            var $control = $(o);
            $control.empty();
            var prop = $control.attr("name");
            if (prop) {
                try {
                    var d = model;
                    //console.log(d);
                    var value = eval("d." + prop);
                    $control.SetValue(value);
                }
                catch (e) {
                    console.log(e);
                }
            }
        });
        $item.find("[data-hbind]").each(function (i, o) {
            var $control = $(o);
            $control.empty();
            var prop = $control.data("hbind");
            if (prop) {
                try {
                    $control.SetValue($.templates(prop).render([model]));
                }
                catch (e) {
                    console.log(e);
                }
            }
        });
        $item.find("[data-tmpl]:not(input):not(select):not(textarea)").each(function (i, o) {
            var $control = $(o);
            var tmpl = $control.data("tmpl");
            var hdata = $control.data("hdata");
            var dd = model;
            if (hdata)
                dd = model[hdata];
            if (tmpl) {
                if (tmpl == "inline") {
                    var tmplate = $control.data("template");
                    if (!tmplate) {
                        tmplate = $.templates($control.html());
                        if (Sail.debug) {
                            console.log($control);
                            console.log($control.html());
                        }
                        $control.data("template", tmplate);
                    }
                    try {
                        if (!$.isArray(dd))
                            dd = [dd];
                        if (Sail.debug) {
                            console.log(tmplate);
                            console.log(tmplate.render(dd));
                        }
                        $control.empty();
                        $control.html(tmplate.render(dd));
                    }
                    catch (e) {
                        console.log(e);
                    }
                }
                else {
                    try {
                        $control.empty();
                        $control.Render(tmpl, dd);
                    }
                    catch (e) {
                        console.log(e);
                    }
                }
            }
        });
        //获取百度编辑器的值
        $item.setToEditor(model);
        //触发绑定时间
        $item.trigger("after.Binder", [$item, model]);
    };
    var Binder = (function () {
        /**
         * 构造函数
         * @param {any} container 容器
         * @param {Object} objItem 默认对象 可为空
         */
        function Binder(container, objItem) {
            this._$item = $(container);
            this.CurrentItem = objItem;
        }
        Binder.Helper = function (ext) {
            Binder.Helpers = $.extend(Binder.Helpers, ext);
        };
        /**
         * 对象赋值到html
         * @param {Object} model
         */
        Binder.prototype.ToHtml = function (model) {
            this._$item.BindObject(model);
        };
        ;
        Binder.prototype.SetObject = function (obj) {
            this.CurrentItem = obj;
            this.ToHtml(this.CurrentItem);
        };
        return Binder;
    }());
    Sail.Binder = Binder;
})(Sail || (Sail = {}));
var JCookies;
(function (JCookies) {
    var config;
    var pluses = /\+/g;
    function encode(s) {
        return config.raw ? s : encodeURIComponent(s);
    }
    function decode(s) {
        return config.raw ? s : decodeURIComponent(s);
    }
    function stringifyCookieValue(value) {
        return encode(config.json ? JSON.stringify(value) : String(value));
    }
    function parseCookieValue(s) {
        if (s.indexOf('"') === 0) {
            s = s.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, '\\');
        }
        try {
            s = decodeURIComponent(s.replace(pluses, ' '));
        }
        catch (e) {
            return;
        }
        try {
            return config.json ? JSON.parse(s) : s;
        }
        catch (e) { }
    }
    function read(s, converter) {
        var value = config.raw ? s : parseCookieValue(s);
        return $.isFunction(converter) ? converter(value) : value;
    }
    $.cookie = function (key, value, options) {
        if (value !== undefined && !$.isFunction(value)) {
            options = $.extend({}, config.defaults, options);
            if (typeof options.expires === 'number') {
                var days = options.expires, t = options.expires = new Date();
                t.setDate(t.getDate() + days);
            }
            return (document.cookie = [encode(key), '=', stringifyCookieValue(value), options.expires ? '; expires=' + options.expires.toUTCString() : '', options.path ? '; path=' + options.path : '', options.domain ? '; domain=' + options.domain : '', options.secure ? '; secure' : ''].join(''));
        }
        var result = key ? undefined : {};
        var cookies = document.cookie ? document.cookie.split('; ') : [];
        for (var i = 0, l = cookies.length; i < l; i++) {
            var parts = cookies[i].split('=');
            var name = decode(parts.shift());
            var cookie = parts.join('=');
            if (key && key === name) {
                result = read(cookie, value);
                break;
            }
            if (!key && (cookie = read(cookie)) !== undefined) {
                result[name] = cookie;
            }
        }
        return result;
    };
    config = {
        defaults: {}
    };
    config = $.cookie;
    $.removeCookie = function (key, options) {
        if ($.cookie(key) !== undefined) {
            $.cookie(key, '', $.extend({}, options, { expires: -1 }));
            return true;
        }
        return false;
    };
})(JCookies || (JCookies = {}));
var Sail;
(function (Sail) {
    //弹窗模板
    Sail.modalTemplates = {
        dialog: "<div class='zqui-modal modal' role='dialog' data-backdrop='static' >" +
            '<div class="zqui-modal__mask"></div>' +
            "<div class='zqui-modal__dialog modal-dialog'>" +
            "<div class='zqui-modal__content modal-content'>" +
            "<div class='zqui-modal__bd modal-body'></div>" +
            "</div>" +
            "</div>" +
            "</div>",
        header: "<div class='zqui-modal__hd modal-header'>" +
            "<div class='zqui-modal__title modal-title'></div>" +
            "<a class='zqui-close' data-dismiss='modal' aria-hidden='true'>×</a>" +
            "</div>",
        footer: "<div class='zqui-modal__ft modal-footer'></div>",
        cancleButton: "<a  class='zqui-btn btn zqui-btn_secondary zqui-btn_default default btnCancel' data-dismiss='modal' aria-hidden='true'></a>",
        okButton: '<button class="zqui-btn btn zqui-btn_primary btn-primary btnOk"></button>'
    };
    /**
     * 弹窗插件
     */
    var Modal = (function () {
        /**
         * 初始化
         * @param {string} id
         * @param {any} settings
         */
        function Modal(id, settings) {
            var _this = this;
            settings = $.extend({
                title: '',
                modalHandle: "#btnAdd",
                tmplName: '#modalTmpl',
                addEvent: null,
                okEvent: null,
                init: null,
                hideEvent: null,
                cssClass: "",
                okTitle: "确定",
                cancelTitle: "取消"
            }, settings);
            var dialog = $(Sail.modalTemplates.dialog);
            //console.log(dialog);
            dialog.prop("id", id || guid());
            dialog.find(".modal-dialog").addClass(settings.cssClass);
            var div = dialog.find(".modal-content");
            var body = dialog.find(".modal-body");
            this.head = $(Sail.modalTemplates.header).insertBefore(body);
            this.head.find(".modal-title").html(settings.title);
            var foot = $(Sail.modalTemplates.footer).appendTo(div);
            if (settings.cancelTitle) {
                $(Sail.modalTemplates.cancleButton).html(settings.cancelTitle).appendTo(foot);
            }
            if (settings.okTitle) {
                this.$ok = $(Sail.modalTemplates.okButton).html(settings.okTitle).appendTo(foot);
                if (settings.okEvent)
                    this.$ok.on("click", function () {
                        //if (!_this.modal.CheckValidation())
                        //    return;
                        //_this.$ok.prop("disabled", true);
                        settings.okEvent(_this);
                    });
            }
            if (settings.hideEvent) {
                div.parent().parent().on("hidden.bs.modal", function () {
                    settings.hideEvent(_this);
                });
            }
            if (settings.modalHandle && settings.addEvent)
                $(settings.modalHandle).click(function () {
                    settings.addEvent(_this);
                    _this.Show();
                });
            div.parent().parent().appendTo("body");
            this.modal = dialog;
            var tmpl = $.LoadHtml(settings.tmplName);
            $("<div class='divView'>").appendTo(body);
            $("<div class='divEditor'>").html(tmpl).appendTo(body);
            this.modal.initForm();
            if (settings.init) {
                settings.init(this.modal);
            }
        }
        /**
         * 显示弹窗
         */
        Modal.prototype.Show = function () {
            this.resetOk();
            this.modal.modal();
        };
        /**
         * 隐藏弹窗
         */
        Modal.prototype.Hide = function () {
            this.modal.modal("hide");
        };
        /**
         * 修改弹窗标题
         * @param {string} title
         */
        Modal.prototype.Title = function (title) {
            this.head.find(".modal-title").html(title);
        };
        Modal.prototype.resetOk = function () {
            if (this.$ok)
                this.$ok.prop("disabled", false);
        };
        return Modal;
    }());
    Sail.Modal = Modal;
})(Sail || (Sail = {}));
/**
 * 提示浮动层
 * @param {type} text
 */
var MsgBox;
(function (MsgBox) {
    /**
     * 待移除的提示浮动层
     */
    var bootstrap_growl_remove = [];
    MsgBox.growl_default_options = {
        ele: "body",
        type: "info",
        allow_dismiss: true,
        position: {
            from: "top",
            align: "center"
        },
        offset: 20,
        spacing: 10,
        z_index: 99999,
        fade_in: 400,
        delay: 5000,
        pause_on_mouseover: true,
        template: {
            alertPrefix: "alert-",
            container: '<div class="col-sm-10 col-sm-10 col-md-3 alert">',
            dismiss: '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>'
        }
    };
    $.growl = function (content, options) {
        var message = content;
        options = $.extend(true, {}, MsgBox.growl_default_options, options);
        //options.template.icon = '<span class="">';
        var growlClass = "bootstrap-growl-" + options.position.from + "-" + options.position.align;
        var $growl = $(options.template.container);
        $growl.addClass(growlClass);
        $growl.addClass("bootstrap-growl");
        $growl.addClass(options.template.alertPrefix + options.type);
        $growl.append($(options.template.dismiss));
        //if (icon) {
        //    if (options.template.icon) {
        //            $growl.append($(options.template.icon).addClass(icon));
        //    } else {
        //        $growl.append(icon);
        //    }
        //}
        $growl.append(message);
        //if (options.template.message) {
        //    $growl.append($(options.template.message).html(message));
        //} else {
        //}
        var offsetAmount = options.offset;
        $("." + growlClass).remove();
        $("." + growlClass).each(function () {
            return offsetAmount = Math.max(offsetAmount, parseInt($(this).css(options.position.from)) + $(this).outerHeight() + options.spacing);
        });
        var css = {
            "position": options.ele === "body" ? "fixed" : "absolute",
            "margin": 0,
            "z-index": options.z_index,
            "display": "none"
        };
        css[options.position.from] = offsetAmount + "px";
        $growl.css(css);
        $(options.ele).append($growl);
        switch (options.position.align) {
            case "center":
                $growl.css({
                    "left": "50%",
                    "marginLeft": -($growl.outerWidth() / 2) + "px"
                });
                break;
            case "left":
                $growl.css("left", options.offset + "px");
                break;
            case "right":
                $growl.css("right", options.offset + "px");
                break;
        }
        $("body").trigger("grow.show");
        var fadeIn = $growl.fadeIn(options.fade_in, function (event) {
            $("body").trigger("grow.shown", event);
            if (options.delay > 0) {
                if (options.pause_on_mouseover == true) {
                    $growl.on('mouseover', function () {
                        clearTimeout(bootstrap_growl_remove[$growl.index()]);
                    }).on('mouseleave', function () {
                        bootstrap_growl_remove[$growl.index()] = setTimeout(function () { return $growl.remove(); }, options.delay);
                    });
                }
                bootstrap_growl_remove[$growl.index()] = setTimeout(function () { return $growl.remove(); }, options.delay);
            }
        });
        return $growl;
    };
    /**
     * 配置模板
     * alertPrefix: "alert-", //颜色样式前缀
       container: '<div class="col-sm-10 col-sm-10 col-md-3 alert">', //主体模板容器
       dismiss: '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>', //关闭按钮容器
     * @param options
     *
     */
    function SetConfig(options) {
        MsgBox.growl_default_options.template = $.extend(MsgBox.growl_default_options.template, options);
    }
    MsgBox.SetConfig = SetConfig;
    /**
     * 显示错误提示 (红色)
     * @param {type} text
     * @returns
     */
    function Error(text) {
        return $.growl(text, { type: 'danger' });
    }
    MsgBox.Error = Error;
    ;
    /**
     * 显示信息提示 (蓝色)
     * @param {type} text
     * @returns
     */
    function Info(text) {
        return $.growl(text, { type: 'info' });
    }
    MsgBox.Info = Info;
    ;
    /**
     * 显示成功提示 (绿色)
     * @param {type} text
     * @returns
     */
    function Success(text) {
        return $.growl(text, { type: 'success' });
    }
    MsgBox.Success = Success;
    ;
    /**
     * 根据返回结果决定显示成功或错误提示
     * @param {type} result 数据结果
     * @param {type} successText? 成功后显示的文字，如果不填默认为result.Msg
     * @param {type} act? 成功后执行的方法，会将result.Data传入作为变量
     */
    function Show(result, successText, act) {
        if (typeof (successText) == "function") {
            act = successText;
            successText = null;
        }
        if (result.IsSuccess)
            Success(successText || result.Msg);
        else
            Error(result.Msg);
        if (act && result.IsSuccess) {
            act(result.Data);
        }
    }
    MsgBox.Show = Show;
    ;
    /**
     * 操作成功才提示
     * @param {Sail.AjaxResult} result
     * @param {Function} act?
     */
    function Action(result, act, msg) {
        if (result.IsSuccess) {
            if (act)
                act(result.Data);
        }
        else
            Error(msg || result.Msg);
    }
    MsgBox.Action = Action;
})(MsgBox || (MsgBox = {}));
var ShowMessage = MsgBox.Show;
var ShowInfo = MsgBox.Info;
var ShowSuccess = MsgBox.Success;
var ShowError = MsgBox.Error;
var Sail;
(function (Sail) {
    var ApiHelper = (function () {
        function ApiHelper(controller) {
            this.Controller = controller;
        }
        ApiHelper.prototype.GetApi = function (actionName) {
            return $.SailConfig.ApiRoot + this.Controller + "/" + actionName;
        };
        ;
        ApiHelper.prototype.get = function (action, data, success) {
            if (typeof (data) == "function") {
                success = data;
                data = null;
            }
            $.get(this.GetApi(action), data, function (result) { success(result); });
        };
        ApiHelper.prototype.post = function (action, data, success) {
            if (typeof (data) == "function") {
                success = data;
                data = null;
            }
            $.post(this.GetApi(action), data, function (result) { success(result); });
        };
        ApiHelper.prototype.put = function (action, data, success) {
            if (typeof (data) == "function") {
                success = data;
                data = null;
            }
            $.ajax({
                type: "put",
                url: this.GetApi(action),
                data: data,
                success: function (result) { success(result); }
            });
        };
        ApiHelper.prototype.delete = function (action, data, success) {
            if (typeof (data) == "function") {
                success = data;
                data = null;
            }
            $.ajax({
                type: "delete",
                url: this.GetApi(action),
                data: data,
                success: function (result) { success(result); }
            });
        };
        return ApiHelper;
    }());
    Sail.ApiHelper = ApiHelper;
    var RazorPage = (function () {
        function RazorPage(title, api, idName, set) {
            var _this = this;
            this.ApiName = api;
            this.IdName = idName;
            this.Id = "";
            this.Page = undefined;
            this.modal = undefined;
            this.setting = $.extend({
                saveApi: "Save",
                isAutoHide: true,
                title: title
            }, set);
            this.$Editor = $("<div id=divEditor></div>");
            this.$Viewer = $("<div id=divViewer></div>");
            var content = $.SailConfig.PageContent;
            this.content = $(content);

            $("#toolbar")
                .ResetForm()
                .appendTo(this.content.find(".toolbar"));


            this.$form = $('#divDetail');
            this.$List = $("#divList");
            this.btnAdd = $("#btnAdd");
            var tool = this;
            this.content.on("click", ".btnReturn", function () {
                _this.Cancel();
            });
            if (this.$form.length > 0) {
                this.$form.on("click", "#btnSave", function () {
                    tool.Save(this);
                });
                var $formBody = this.$form.find(".form-body");
                this.$Editor.html($.LoadHtml("#modalTmpl")).prependTo($formBody);

                this.$Viewer.html($.LoadHtml("#viewTmpl")).prependTo($formBody);

                this.$form.initForm();
                if (Sail.debug)
                    console.log("init form");
            }
            this.$Viewer.on("click", ".btnToEdit", function () {
                _this.ToEdit();
            });
            this.$Editor.on("click", ".btnToView", function () {
                _this.ToView();
            });
            this.btnAdd.click(function () { _this.Add(); });
            if ($.fn.tooltip)
                this.content.find('[data-toggle="tooltip"]').tooltip();
            //this.$extend = this.$form.find(".expandForm");
            Sail.RazorPage.Default = this;
            $("body").trigger("pageInit.after", [this]);


            //操作按钮
            var tmpl = '<button class="zqui-btn zqui-btn_{{:color || \'primary\'}} view" name="{{:name}}">{{:text}}</button>';
            var buttons = $.templates(tmpl).render(this.setting.operation || []);
            $(buttons).insertBefore("#btnCancel");

            (this.setting.operation || []).forEach(function (v) {
                var selector = "[name=" + v.name + "]";
                tool.$form.on("click", selector, function (e) {
                    v.act.call(this, e);
                });
            });
        }

        //设置标题
        RazorPage.prototype.SetTitle = function (title) {
            var $title = this.content.find(".page-title");
            if ($title.length == 0) {
                this.content.find(".page-content").prepend("<h3 class='page-title'></h3><hr>");
                $title = this.content.find(".page-title");
            }
            $title.html(title);
            return this;
        };
        /**
         * 取消事件
         * 会触发after.Cancel事件，如需自定义请使用this.content.on("after.Cancel",function(){});
         * @returns
         */
        RazorPage.prototype.Cancel = function () {
            this.Id = "";
            if (this.modal)
                this.modal.Hide();
            else {
                this.$form.hide();
                this.$List.show();
            }
            this.$form.find(".has-error").removeClass("has-error");
            this.content.trigger("after.Cancel", [this.Page, this.modal ? this.modal.modal : this.$form]);
            window.location.hash = "";
            return this;
        };
        /**
         * 修改表单页面的标题
         * @param {type} title
         */
        RazorPage.prototype.SetFormTitle = function (title) {
            this.$form.find(".detailTitle span").eq(0).html(title);

        };
        ///获取api的url
        RazorPage.prototype.GetApi = function (name) {
            return $.SailConfig.ApiRoot + this.ApiName + "/" + name;
        };
        RazorPage.prototype.on = function (event, selector, data) {
            this.content.on(event, selector, data);
            return this;
        };
        ;
        /**
         * 返回对象的id值(根据idName)
         * @param {type} obj
         * @returns
         */
        RazorPage.prototype.GetDataId = function (obj) {
            if (obj[this.IdName] == undefined)
                console.log("获取主键值失败,可能是idName不对");
            return obj[this.IdName];
        };
        /**
         * 获取列表api的url
         * @returns
         */
        RazorPage.prototype.listApi = function () {
            return this.GetApi("getlist");
        };
        /**
         * 显示详情页，隐藏列表页
         * @param {string} act
         */
        RazorPage.prototype.showDetail = function (act) {
            this.$form.show().ResetForm();
            this.$List.hide();
            this.$form.find("#btnSave").show().prop("disabled", false);
            this.$Viewer.hide();
            this.$Editor.show();
            this.SetFormTitle(act + this.setting.title);
            //this.$extend.ResetForm();
            //this.Expand(false);
        };
        RazorPage.prototype.displayNav = function (isShow) {
            var $main = this.$form.find(".mainForm");
            $main.find(".tabbable  .nav:eq(0) ").Display(isShow);
        };
        //public Expand(isShow: boolean) {
        //    var $main = this.$form.find(".mainForm");
        //    //var $expand = this.$extend;
        //    if ($expand.length > 0) {
        //        if (!isShow)
        //            $main.removeClass("col-sm-8 dashed-border-right").addClass("col-sm-12");
        //        else
        //            $main.removeClass("col-sm-12").addClass("col-sm-8 dashed-border-right");
        //    }
        //    this.$extend.Display(isShow);
        //}
        //public isExpand() {
        //    return this.$extend.is(":visible");
        //}
        RazorPage.prototype.ToEdit = function () {
            //var isExpand = this.isExpand();
            this.showDetail("编辑");
            var data = this.$form.data("dataItem");
            this.$form.find(".btnToView").show();
            this.$Editor.ItemBinder().SetObject(data);
            this.Id = this.GetDataId(data);
            window.location.hash = 'edit__' + this.Id;
            this.content.trigger("after.Edit", [data, this.Page, null]);
        };
        RazorPage.prototype.ToView = function () {
            //var isExpand = this.isExpand();
            this.showDetail("查看");
            var data = this.$form.data("dataItem");

            $.when(this.content.trigger("before.View", [data, this.Page, null])).done(function () {

                this.$Viewer.ItemBinder().SetObject(data);
                this.$form.find("#btnSave").hide();
                this.$Editor.hide();
                this.$Viewer.show();
                this.Id = this.GetDataId(data);
                window.location.hash = 'view__' + this.Id;
                this.content.trigger("after.View", [data, this.Page, null]);
            });


            //   if (isExpand) this.Expand(true);;
        };
        /**
         * 新增事件
         * 会触发after.Add事件，如需自定义请使用this.content.on("after.Add",function(){});
         * @returns
         */
        RazorPage.prototype.Add = function () {
            if (Sail.debug)
                console.log("add");
            this.Id = "";
            if (this.modal) {
                this.modal.modal.ResetForm();
                this.modal.Title("新增" + this.setting.title);
                this.modal.Show();
            }
            else {
                this.showDetail("新增");
                this.displayNav(false);
                this.$form.find(".btnToView").hide();
            }
            window.location.hash = 'add';
            this.content.trigger("after.Add");
            this.$form.find("button.view").hide();
            return this;
        };
        /**
         * 编辑事件
         * 最后会触发after.Edit事件，如需自定义请使用this.content.on("after.Edit",function(data,page,obj){});
         * @param {type} data 要编辑的数据对象
         * @param {type} page 分页控件
         * @param {type} obj 触发事件的jquery对象
         */
        RazorPage.prototype.EditAct = function (data, page, obj) {
            this.Id = this.GetDataId(data);
            this.$form.data("dataItem", data);
            //console.log($("[data-tmpl]").html());
            this.$form.find("button.view").hide();

            this.content.trigger("before.Edit", [data, page, obj])

            var $target = this.modal ? this.modal.modal : this.$Editor;

            if (this.modal) {
                this.modal.Title("编辑" + this.setting.title);
                this.modal.Show();
            }
            else {
                this.showDetail("编辑");
                this.displayNav(false);
                this.$Editor.find(".btnToView").show();
            }

            $target.ResetForm();
            $target.ItemBinder().SetObject(data);
            window.location.hash = 'edit__' + this.Id;
            this.content.trigger("after.Edit", [data, page, obj]);
        };
        /**
        * 查看事件
        * 最后会触发after.View事件，如需自定义请使用this.content.on("after.View",function(data,page,obj){});
        * 暂时不提供对弹窗页面的ViewAct支持
        * @param {type} data
        * @param {type} page
        * @param {type} obj
        */
        RazorPage.prototype.ViewAct = function (data, page, obj) {
            this.Id = this.GetDataId(data);
            this.$form.data("dataItem", data);
            this.$form.find("button.view").show();

            if (!this.modal) {
                this.showDetail("查看");
                this.$form.find("#btnSave").hide();
                this.$Editor.hide();
                this.$Viewer.show();
                this.displayNav(true);
                var that = this;
                this.content.trigger("before.View", [data, this.Page, null])
                that.$Viewer.ItemBinder().SetObject(data);
            }
            window.location.hash = 'view__' + this.Id;
            this.content.trigger("after.View", [data, page, obj]);
        };

        /**
         * 保存事件
         * 保存前会触发PreSave函数，如果定义了则会在PreSave函数之中对数据进行修改
         * 保存成功会触发after.Save事件，如需自定义请使用this.content.on("after.Save",function(){});
         * @param {type} btn
         * @returns
         */
        RazorPage.prototype.Save = function (btn) {
            var data = {};
            var xform = this.modal ? this.modal.modal : this.$Editor;
            if (!xform.CheckValidation()) {
                MsgBox.Error("请确认所有必填项都已填写且所有内容都已正确填写");
                return null;
            }
            data = xform.GetJson();
            data = this.PreSave(xform, data);
            if (!data)
                return null;
            var tool = this;
            $(btn).prop("disabled", true);
            $.ajax({
                type: "put",
                url: this.GetApi(this.setting.saveApi + "?id=") + (this.Id || ""),
                data: { "": JSON.stringify(data) },
                success: function (result) {
                    $(btn).prop("disabled", false);
                    MsgBox.Show(result, "保存成功", function () {
                        tool.$form.data("dataItem", result.Data);
                        tool.Id = tool.GetDataId(result.Data);
                        //  var isExpand = tool.isExpand();
                        if (tool.Page)
                            tool.Page.Query();
                        if (tool.setting.isAutoHide) {
                            tool.Cancel();
                        }
                        //if (!tool.setting.isAutoHide || isExpand) {
                        //    tool.EditAct(result.Data, tool.Page, null);
                        //    if (isExpand) tool.Expand(isExpand);
                        //}
                        window.location.hash = "";
                        tool.content.trigger("after.Save", [result]);
                    });
                },
                error: function () {
                    $(btn).prop("disabled", false);
                    MsgBox.Error("提交失败,请检查网络连接");
                }
            });
            return this;
        };
        ;
        /**
         * 创建分页控件
         * @param {type} param
         */
        RazorPage.prototype.CreatePage = function (param) {
            var _this = this;
            var tool = this;
            param = $.extend({
                getPostKey: function () { return $("#toolbar").GetJson(); },
                handleName: tool.listApi()
            }, param);
            this.Page = $.Pagination(param);
            this.RegisterAct($(this.Page.settings.bodyContainer), this.Page);
            $("#toolbar .btnReset").click(function () {
                $("#toolbar").ResetForm();
                _this.Page.Query(1);
            });
            window.onhashchange = function (event) {
                if (window.location.hash == "") {
                    _this.Cancel();
                    _this.Page.Query();
                    console.log('this.Cancel', _this.Cancel(), _this.Page.Query())
                }
            };
            return this.Page;
        };
        RazorPage.prototype.RegisterAct = function (body, page) {
            var tool = this;
            body
                .on("click", ".btnView", function () {
                    tool.ViewAct($.view(this).data, page, this);
                })
                .on("click", ".btnEdit", function () {
                    tool.EditAct($.view(this).data, page, this);
                })
                .on("click", ".btnRemove", function () {
                    tool.DeleteAct($.view(this).data, page, this);
                })
                .on("click", ".btnAct", function () {
                    tool.BtnAct($.view(this).data, page, this);
                })
                .on("click", ".btnExpand", function () {
                    tool.ViewAct($.view(this).data, page, this);
                    //  tool.Expand(true);
                });
        };
        /**
         * 删除事件
         * 最后会触发after.Delete事件，如需自定义请使用this.content.on("after.Delete",function(data,page,obj){});
         * @param {type} data 要删除的对象
         * @param {type} page 分页控件
         * @param {type} obj 触发事件的jquery对象
         */
        RazorPage.prototype.DeleteAct = function (data, page, obj) {
            var tool = this;
            if (!confirm("确定要删除吗?"))
                return;
            $.Delete(tool.GetApi("delete"), { "": tool.GetDataId(data) }, function (result) {
                MsgBox.Show(result, "", function () {
                    if (page)
                        page.Query();
                    tool.content.trigger("after.Delete", [data, page, obj]);
                });
            });
        };
        /**
         * 按钮操作事件
         * @param {Object} data
         * @param {Pagination} page
         * @param {JQuery} obj
         */
        RazorPage.prototype.BtnAct = function (data, page, obj) {
            var tool = this;
            obj = $(obj);
            var act = obj.data("act");
            if (!confirm("确定要" + obj.data("originalTitle") + "吗"))
                return;
            $.post(tool.GetApi(act), { "": tool.GetDataId(data) }, function (result) {
                MsgBox.Show(result, "", function () {
                    if (page)
                        page.Query();
                    tool.content.trigger("after.Action", [data, page, obj, act]);
                });
            });
        };
        /**
         * 保存前对数据进行处理
         * @param {type} form
         * @param {type} modal
         */
        RazorPage.prototype.PreSave = function (form, model) {
            return model;
        };
        return RazorPage;
    }());
    Sail.RazorPage = RazorPage;
})(Sail || (Sail = {}));
var Sail;
(function (Sail) {
    Sail.pageSetting = {
        first: "第一页",
        last: "最末页",
        next: ">>",
        pre: "<<"
    };
    Sail.noDataTmpl = "<div class='nodata'></div>";
    function noData(desc) {
        var tag = desc.tagName();
        desc.empty();
        var $tr = null;
        if (tag === "TBODY") {
            var cols = desc.parents("table").find("tr th").length;
            $tr = $("<tr><td></td></tr>").appendTo(desc);
            desc = $tr.find("td").attr("colspan", cols);
        }
        desc.html(Sail.noDataTmpl);
    }
    Sail.noData = noData;
    function setPageSetting(set) {
        Sail.pageSetting = set;
    }
    Sail.setPageSetting = setPageSetting;
    /**
     * 生成页码
     * @param {any} pageInfo
     * @returns
     */
    function loadPages(pageInfo) {
        pageInfo.Pages = [];
        if (pageInfo.PageIndex > 1) {
            pageInfo.Pages.push({ index: 1, title: Sail.pageSetting.first });
            pageInfo.Pages.push({ index: pageInfo.PageIndex - 1, title: Sail.pageSetting.pre });
        }
        for (var i = pageInfo.StartIndex; i <= pageInfo.EndIndex; i++) {
            pageInfo.Pages.push({ index: i, title: i, isSelect: i == pageInfo.PageIndex });
        }
        if (pageInfo.PageIndex < pageInfo.PageCount) {
            pageInfo.Pages.push({ index: pageInfo.PageIndex + 1, title: Sail.pageSetting.next });
            pageInfo.Pages.push({ index: pageInfo.PageCount, title: Sail.pageSetting.last });
        }
        return pageInfo.Pages;
    }
    ;

    /**
    * 简化设置容器参数的方法,只要传入外层容器选择器即可
    * @param set
    * @param container
    * @param isTable
    */
    var SetContainer = function (container, set, isNotTable) {
        var data = !isNotTable
            ? {
                reQueryHandle: container + ' .btnQuery', //查询按钮
                headContainer: container + '  table thead',//头部容器
                bodyContainer: container + '  table tbody',
                footContainer: container + '  .page',//底部容器 
            }
            :
            {
                reQueryHandle: container + '  .btnQuery', //查询按钮
                headContainer: null,//头部容器
                bodyContainer: container + ' .pageBody',
                footContainer: container + ' .page',//底部容器 
            };
        return $.extend(data, set);
    }
    Sail.SetContainer = SetContainer;

    /*设置分页组件*/
    $.fn.SetPagination = function (set, isnotTable) {
        var $this = $(this);

        var data = {
            reQueryHandle: $this.find(".btnQuery"),
            footContainer: $this.find(".page"),
            headContainer: null,
        };
        if (!isnotTable) {
            data.headContainer = $this.find("table thead");
            data.bodyContainer = $this.find("table tbody");
        }
        else
            data.bodyContainer = $this.find(".pageBody");
        return new Pagination($.extend(data, set));
    };

    var Pagination = (function () {
        function Pagination(set) {
            var _this = this;
            set = jQuery.extend({
                isPaging: true,
                reQueryHandle: "#btnQuery",
                headContainer: "#pageList table thead",
                bodyContainer: "#pageList table tbody",
                footContainer: "#pageList #page",
                ajaxType: function () { return "get"; },
                queryed: null,
                pageSize: 10,
                bodyTmpl: "#listTmpl",
                footTmpl: 'tableFoot',
                titles: {},
                titleWidth: [],
                handleName: null,
                events: [],
                getPostKey: function () { return {}; } //获取提交数据的方法 
            }, set);
            this.settings = set;
            this.Container = $(set.bodyContainer);
            var result = this;
            if (set.events) {
                $.each(set.events, function (index, event) {
                    if (event.handle && event.act) {
                        _this.Container.on("click", event.handle, function () {
                            console.log($(this).view())
                            event.act($.view(this).data, this, result);
                        });
                    }
                });
            }

            if (set.init)
                set.init.call(this);
            
            this.currentIndex = 1;
            $(set.reQueryHandle).click(function () {
                _this.Query(1);
            });
            if (set.headContainer && set.titles)
                $(set.headContainer).MakeHead(set.titles, set.titleWidth);

            $(set.footContainer).on("click", ".onPage", function () {
                var page = $(this).data("page");
                console.log(page);
                console.log($(this));
                result.Query(page);
            });
        }
        ///读取数据
        Pagination.prototype.Query = function (pageIndex) {
            var _this = this;
            var tooltip = '[data-toggle="tooltip"]';
            if (pageIndex)
                this.currentIndex = pageIndex;
            pageIndex = this.currentIndex;
            var settings = this.settings;
            var postData = settings.getPostKey();
            if (settings.isPaging) {
                postData["pageSize"] = settings.pageSize;
                postData["pageIndex"] = pageIndex;
            }
            var url = settings.handleName;
            if (typeof url == "function")
                url = url();
            var type = settings.ajaxType();
            if (type !== "get")
                postData = { "": JSON.stringify(postData) };
            $.ajax({
                type: type,
                url: url,
                data: postData,
                dataType: "json",
                success: function (data) {
                    if (data && data.IsSuccess) {
                        var infoDatas = data.Data;
                        if (settings.isPaging) {
                            loadPages(data.Data.PageInfo);
                            infoDatas = data.Data.Data;
                        }
                        _this.Container = $(settings.bodyContainer);
                        _this.Container.Link(settings.bodyTmpl, infoDatas);
                        if (data.Data.Data.length === 0) {
                            noData(_this.Container);
                        }
                        _this.Container.TableChange();
                        if (settings.isPaging && settings.footTmpl && settings.footContainer)
                            $(settings.footContainer).Render(settings.footTmpl, data.Data.PageInfo);
                        if ($.fn.tooltip) {
                            $(_this.settings.headContainer).find(tooltip).tooltip();
                            $(_this.settings.footContainer).find(tooltip).tooltip();
                            _this.Container.find("td a").tooltip();
                        }
                        if (settings.queryed)
                            settings.queryed(data.Data, _this);
                    }
                    else
                        MsgBox.Error(data.Msg);
                }
            });
            return this;
        };
        return Pagination;
    }());
    Sail.Pagination = Pagination;
})(Sail || (Sail = {}));

var NumberHelper;
(function (NumberHelper) {
    Number.prototype.PadLeft = function (a, b) {
        return String(this).PadLeft(a, b);
    };
    Number.prototype.PadRight = function (a, b) {
        return String(this).PadRight(a, b);
    };
})(NumberHelper || (NumberHelper = {}));
var StringHelper;
(function (StringHelper) {
    /**
     * 获取待补齐的字符
     * @param {string} src
     * @param {number} length
     * @param {string} padStr
     * @returns
     */
    function PadHelper(src, length, padStr) {
        if (!padStr)
            padStr = " ";
        if (src.length < length) {
            var d = new String();
            for (var i = 1; i <= length - src.length; i++)
                d += padStr;
            return d;
        }
        return "";
    }
    ;
    String.prototype.SubStringByByte = function (n) {
        var str = this;
        if (!str || !n)
            return "";
        var a = 0;
        var i = 0;
        var temp = "";
        for (i = 0; i < str.length; i++) {
            a += str.charCodeAt(i) > 255 ? 2 : 1;
            if (a > n) {
                return temp + "...";
            }
            temp += str.charAt(i);
        }
        return str;
    };
    String.prototype.PadLeft = function (length, str) {
        return PadHelper(this, length, str) + this;
    };
    String.prototype.PadRight = function (length, str) {
        return this + PadHelper(this, length, str);
    };
    String.prototype.format = function () {
        var args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            args[_i - 0] = arguments[_i];
        }
        var a = arguments;
        return 0 === arguments.length ? this : this.replace(/\{(\d+)\}/g, function (b, c) { return a[parseInt(c)]; });
    };
    String.prototype.ToInt = function () {
        if (!this)
            return 0;
        var a = parseInt(this);
        return isNaN(a) ? 0 : a;
    };
    String.prototype.ToFloat = function () {
        if (!this)
            return 0;
        var a = parseFloat(this);
        return isNaN(a) ? 0 : a;
    };
    String.prototype.RemoveHtmlTag = function () {
        var str = this;
        str = str.replace(/<\/?[^>]*>/g, "");
        str = str.replace(/[ | ]*\n/g, "\n");
        str = str.replace(/&nbsp;/gi, "");
        return str;
    };
})(StringHelper || (StringHelper = {}));
var Sail;
(function (Sail) {
    /**
     * 数字以逗号分割形式显示
     * @param num
     */
    function formatNumber(num) {
        return String(num).split("").reverse().join("").replace(/(\d{3})(?=[^$])/g, "$1,").split("").reverse().join("");
    }
    Sail.formatNumber = formatNumber;
    function digitUppercase(n) {
        var fraction = ['角', '分'];
        var digit = [
            '零', '壹', '贰', '叁', '肆',
            '伍', '陆', '柒', '捌', '玖'
        ];
        var unit = [
            ['元', '万', '亿'],
            ['', '拾', '佰', '仟']
        ];
        var head = n < 0 ? '欠' : '';
        n = Math.abs(n);
        var s = '';
        for (var i = 0; i < fraction.length; i++) {
            s += (digit[Math.floor(n * 10 * Math.pow(10, i)) % 10] + fraction[i]).replace(/零./, '');
        }
        s = s || '整';
        n = Math.floor(n);
        for (var i = 0; i < unit[0].length && n > 0; i++) {
            var p = '';
            for (var j = 0; j < unit[1].length && n > 0; j++) {
                p = digit[n % 10] + unit[1][j] + p;
                n = Math.floor(n / 10);
            }
            s = p.replace(/(零.)*零$/, '').replace(/^$/, '零') + unit[0][i] + s;
        }
        return head + s.replace(/(零.)*零元/, '元')
            .replace(/(零.)+/g, '零')
            .replace(/^整$/, '零元整');
    }
    Sail.digitUppercase = digitUppercase;
    ;
    /**
     * 获取预览图片地址
     * @param str
     * @param preType
     */
    function ToPre(str, preType) {
        if (!preType)
            preType = "pre";
        if (!str)
            return '';
        var pos = str.lastIndexOf("/");
        if (pos >= 0)
            str = str.substr(0, pos) + "/" + preType + str.substr(pos);
        return str;
    }
    Sail.ToPre = ToPre;
    Sail.Binder.Helper({
        formatNumber: function (str) { return formatNumber(str); },
        Uppercase: function (str) { return digitUppercase(str); }
    });
    if ($.views) {
        $.views.helpers({
            subString: function (str, n) {
                return str.SubStringByByte(n);
            },
            subStringNoHtml: function (str, n) {
                return str.RemoveHtmlTag().SubStringByByte(n);
            },
            ToPre: function (str, preType) {
                return ToPre(str, preType);
            }
        });
        $.views.converters({
            ToPre: function (str) { return ToPre(str, null); },
            ToDate: function (value) { return value ? value.substring(0, 10) : ""; },
            ToFullDateTime: function (value) {
                return value ? DateTime.Parse(value).toLocaleString() : "";
            },
            ToDateHz: function (value) {
                return value ? DateTime.Parse(value).format("yyyy年MM月dd日") : "";
            },
            ToWeekDate: function (value) {
                return value ? DateTime.Parse(value).format("MM月dd日 (ddd)") : "";
            },
            ToDateTime: function (value) {
                return value ? DateTime.Parse(value).format("yyyy年MM月dd日 hh:mm") : "";
            },
            ToDateTime2: function (value) {
                return value ? DateTime.Parse(value).format("yyyy-MM-dd hh:mm:ss") : "";
            },
            intToStr: function (value) {
                return "" + value;
            },
            strToInt: function (value) {
                if (!value)
                    return 0;
                return parseInt(value);
            },
            strToFloat: function (value) {
                if (!value)
                    return 0;
                return parseFloat(value);
            },
            FloatToStr: function (value) {
                return "" + value;
            },
            FloatToStrSpace: function (value) {
                if (value === 0)
                    return "";
                return "" + value;
            }
        });
    }
})(Sail || (Sail = {}));
var validationEngine;
(function (validationEngine) {
    validationEngine.validationEngineLanguage = {
        allRules: {
            required: {
                regex: "none",
                alertText: "必填",
                alertTextCheckboxMultiple: "请选择一个选项",
                alertTextCheckboxe: "必须选中"
            },
            length: {
                regex: "none",
                alertText: "长度在 ",
                alertText2: " 到 ",
                alertText3: " 个字符之间"
            },
            confirm: {
                regex: "none",
                alertText: "输入内容不匹配"
            },
            url: {
                regex: /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i,
                alertText: "无效的网址"
            },
            idNumber: {
                regex: /^\d{17}([0-9]|X|x)$/,
                alertText: "身份证号码无效"
            },
            mobile: {
                regex: /^(1)[0-9]{10}$/,
                alertText: "无效手机号码"
            },
            email: {
                regex: /^[a-zA-Z0-9_.-]+@([a-zA-Z0-9-]+.)+[a-zA-Z0-9]{2,4}$/,
                alertText: "无效邮件地址"
            },
            integer: {
                regex: /^-?[0-9]+$/,
                alertText: "只允许录入整数"
            },
            pinteger: {
                regex: /^[0-9]+$/,
                alertText: "只允许录入整数"
            },
            pdecimal: {
                regex: /^\d*\.?\d+$/,
                alertText: "只能输入数字"
            },
            decimal: {
                regex: /^-?\d*\.?\d+$/,
                alertText: "只能输入数字"
            },
            letter: {
                regex: /^[a-zA-Z0-9_]+$/,
                alertText: "只能输入字母、数字和下划线(_)"
            },
            phone: {
                regex: /^[0-9_,、-]+$/,
                alertText: "只能输入数字、下划线(_)和减号(-)逗号(,)顿号(、)"
            }
        }
    };
    //我们自定义的设置，主要是error、formGroup
    validationEngine.customSet = {
        error: "has-error",
        formGroup: ".form-group-default",
        validationEventTriggers: "keyup blur",
        inlineValidation: true,
        returnIsValid: false,
        scroll: true,
        unbindEngine: false,
        ajaxSubmit: true,
        success: false,
        ignoreRequire: false
    };
    //不验证必填项
    $.IgnoreRequire = function () {
        validationEngine.customSet.ignoreRequire = true;
    };
    //必须验证必填项
    $.MustRequire = function () {
        validationEngine.customSet.ignoreRequire = false;
    };
    /**
     * 配置默认设置
     * @param set
     */
    function SetConfig(set) {
        validationEngine.customSet = $.extend(validationEngine.customSet, set);
    }
    validationEngine.SetConfig = SetConfig;
    validationEngine.settings = {};
    var isError = false;
    var showTriangle = true;
    function loadValidation(caller) {
        var rulesParsing = $(caller).attr("class");
        var rulesRegExp = /\[(.*)\]/;
        var getRules = rulesRegExp.exec(rulesParsing);
        var str = getRules[1];
        var pattern = /\[|,|\]/;
        var result = str.split(pattern);
        return validateCall(caller, result);
    }
    function validateCall(caller, rules) {
        var promptText = "";
        var callerName = $(caller).attr("name");
        isError = false;
        showTriangle = true;
        var callerType = $(caller).attr("type");
        for (var i = 0; i < rules.length; i++) {
            switch (rules[i]) {
                case "optional":
                    if (!$(caller).val()) {
                        return isError;
                    }
                    break;
                case "required":
                    if (validationEngine.settings.ignoreRequire != true)
                        required(caller);
                    break;
                case "custom":
                    customRegex(caller, rules, i);
                    break;
                case "length":
                    length(caller, rules, i);
                    break;
                case "confirm":
                    confirm(caller, rules, i);
                    break;
            }
        }
        radioHack();
        if (isError) {
            buildPrompt(caller, promptText);
        }
        else {
            var group = findTipsGroup(caller);
            group.removeClass(validationEngine.settings.error).prop("title", "");
        }
        function radioHack() {
            if ($("input[name='" + callerName + "']").size() > 1 && (callerType == "radio" || callerType == "checkbox")) {
                caller = $("input[name='" + callerName + "'][type!=hidden]:first");
                showTriangle = false;
            }
        }
        function required(caller) {
            callerType = $(caller).attr("type");
            switch ($(caller).prop("tagName").toLowerCase()) {
                case "textarea":
                    callerType = "textarea";
                    break;
                case "select":
                    callerType = "select";
                    break;
            }
            var types = ["hidden", "text", "password", "textarea", "select", "date", "number", "tel", "email", "url", "month", "week", "time", "datetime", "datetime-local", "search"];
            if (!callerType)
                callerType = "text";
            switch (callerType) {
                case "radio":
                case "checkbox":
                    callerName = $(caller).attr("name");
                    if ($("input[name='" + callerName + "']:checked").size() === 0) {
                        isError = true;
                        if ($("input[name='" + callerName + "']").size() === 1) {
                            promptText += validationEngine.validationEngineLanguage.allRules["required"].alertTextCheckboxe;
                        }
                        else {
                            promptText += validationEngine.validationEngineLanguage.allRules["required"].alertTextCheckboxMultiple;
                        }
                    }
                    break;
                default:
                    if (types.indexOf(callerType) >= 0) {
                        var value = $(caller).val();
                        if (typeof (value) == "object") {
                            if (!value || value.length <= 0)
                                isError = true;
                        }
                        else {
                            if (!value || !value.trim())
                                isError = true;
                        }
                        if (isError == true) {
                            promptText += validationEngine.validationEngineLanguage.allRules["required"].alertText;
                        }
                    }
            }
        }
        function customRegex(caller, rules, position) {
            var customRule = rules[position + 1];

            if (validationEngine.validationEngineLanguage.allRules[customRule]) {
                var pattern = eval(validationEngine.validationEngineLanguage.allRules[customRule].regex);
                if ($(caller).val() && !pattern.test($(caller).val())) {
                    isError = true;
                    var txt = validationEngine.validationEngineLanguage.allRules[customRule].alertText;
                    promptText += txt;
                }
            }
            else {
                var pattern = eval("/" + customRule + "/");
                if ($(caller).val() && !pattern.test($(caller).val())) {
                    isError = true;
                    var txt = ($(caller).data("errormsg") || "规则不匹配");
                    console.log($(caller).data("errormsg"))
                    promptText += txt;
                }
            }

        }
        function confirm(caller, rules, position) {
            var confirmField = rules[position + 1];
            if ($(caller).val() !== $("#" + confirmField).val()) {
                isError = true;
                promptText += validationEngine.validationEngineLanguage.allRules["confirm"].alertText;
            }
        }
        function length(caller, rules, position) {
            var startLength = eval(rules[position + 1]);
            var endLength = eval(rules[position + 2]);
            var feildLength = $(caller).val().length;
            if (feildLength < startLength || feildLength > endLength) {
                isError = true;
                promptText += validationEngine.validationEngineLanguage.allRules["length"].alertText
                    + startLength + validationEngine.validationEngineLanguage.allRules["length"].alertText2
                    + endLength
                    + validationEngine.validationEngineLanguage.allRules["length"].alertText3;
            }
        }
        return isError ? true : false;
    }
    /**
        * 找到控件组名称，或者向上数三层容器
        * @param {} caller
        * @returns {}
        */
    function findTipsGroup(caller) {
        var group = $(caller).closest(validationEngine.settings.formGroup);
        if (group.length === 0)
            group = $(caller).parent().parent();
        return group;
    }
    /**
         * 增加错误提示信息与错误颜色
         * @param {} caller
         * @param {} promptText
         * @param {} type
         * @param {} ajaxed
         * @returns {}
         */
    function buildPrompt(caller, promptText) {
        var group = findTipsGroup(caller);
        group.addClass(validationEngine.settings.error);
        group.prop("title", promptText);
        return $("<div>");
    }
    function submitValidation(caller) {
        var stopForm = false;
        var items = $(caller).find("[class*=validate]");
        items.each(function () {
            var validationPass = loadValidation(this);
            return validationPass ? stopForm = true : "";
        });
        return stopForm;
    }
    function setConfig(set) {
        if (!validationEngine.settings)
            SetConfig({});
        validationEngine.settings = jQuery.extend(validationEngine.customSet, set);
    }
    $.fn.validationEngine = function (set) {
        setConfig(set);
        if (validationEngine.settings.inlineValidation == true) {
            if (!validationEngine.settings.returnIsValid) {
                var target = "[class*=validate][type!=checkbox]";
                $(this).on(validationEngine.settings.validationEventTriggers, target, function () {
                    var s = loadValidation(this);
                });
                $(this).on("click", target, function () {
                    loadValidation(this);
                });
            }
        }
        if (validationEngine.settings.returnIsValid) {
            return !submitValidation(this);
        }
    };
})(validationEngine || (validationEngine = {}));
//# sourceMappingURL=jPlugin.js.map 
//# sourceMappingURL=jPlugin.js.map 
//# sourceMappingURL=jPlugin.js.map 
//# sourceMappingURL=jPlugin.js.map