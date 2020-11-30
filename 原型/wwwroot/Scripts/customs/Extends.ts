/// <reference path="../tsd/jquery.d.ts" />
/// <reference path="../tsd/jsrender.d.ts" />
/// <reference path="../tsd/underscore.d.ts" />
/// <reference path="../tsd/sail.javascript.d.ts" />

interface JQuery {
    slimScroll(obj: any): JQuery;
    ValImage(value: string): JQuery;
    uploadify(value: any): JQuery;
    uploadify(value: any, an: any): JQuery;
    uploadifive(value: any): JQuery;
    uploadifive(value: any, an: any): JQuery;
    getTree(): any;
    MakeTab(): JQuery;
    //InputTable(set: ITableSetting): JQuery;
    //InputTableInsert(set: ITableSetting): JQuery;
    ButtonSelect();
    SetButtonSelect();
    iconUpload(tmpl: any);
}
/**
 * 返回可上传的文件类型
 * @returns
 */
function ReturnType() { return "*.docx;*.rar;*.mp3;*.wav;*.doc;*.txt;*.xls;*.zip;*.xlsx;*.jpg;*.png;*.pdf;*.jpeg;"; }

function IconType() { return "*.jpg;*.png;*.jpeg;*.gif;*.bmp"; }
 
$.views.helpers({
    ToDisabledLabel: function (isDisabled: boolean) {
        var cssClass = isDisabled ? "zqui-label_danger" : "zqui-label_success";
        var text = isDisabled ? "已停用" : "正常";
        var item = `<label class="zqui-label ${cssClass}">${text}</label>`;
        return item;
    }
});

$.views.converters({
    toBoolStr: function (flag) {
        return ToLabel({ 是: "success", 否: "danger" }, flag ? 0 : 1);
        //return flag ? '<label class="zqui-label zqui-label_success">是</label>' : '<label class="zqui-label zqui-label_danger">否</label>';
    },
    toInfoLabel: function (flag) {
        return ToLabel({ 正常: "success", 停用: "danger" }, flag);
        //return flag ? '<label class="zqui-label zqui-label_success">是</label>' : '<label class="zqui-label zqui-label_danger">否</label>';
    }
});


function ToLabel(data, val?, className?: string) {
    if (!className) className = "zqui-label zqui-label_";
    if (!val) val = 0;
    var color = [];
    var text = [];
    for (var prop in data) {
        text.push(prop);
        color.push(data[prop]);
    }
    if (val < 0 || val > color.length) return "无效";
    var result = `<label class="${className}${color[val]}">${text[val]}</label>`
    return result;

}

$.fn.selectByText = function (text) {
    var $ele = this;
    if ($ele.tagName() !== "SELECT")
        throw new Error("invalid element");
    $ele.find(`option:contains(${text})`).prop("selected", true);
    return $ele;
}


$.fn.iconUpload = function (apiName) {
    var $this = $(this);
    var $group = $this.closest(".zqui-form-group");
    var iconApi = new Sail.ApiHelper("Icon");
    $this.uploadifive({
        width: '5rem',
        height: '5rem',
        uploadScript: iconApi.GetApi("UploadFile"),
        fileType: "image/*",
        fileSizeLimit: '5MB',
        buttonText: "<div class='zqui-upload__text'></div>",
        multi: false,

        onUploadComplete(file, data) {
            var result = JSON.parse(data);
            var image = result.Data;
            $(".uploadifive-queue").hide();
            if (result.IsSuccess) {
                $group.find(".zqui-upload_img").find("img").prop("src", image.FilePath);
                let $ele = $group.find("input[type=hidden]");
                $ele.val(image.FilePath).data().file = image;
            }
        },
        inited() {
            $group.find(".zqui-upload_img").insertAfter($group.find(".zqui-upload__text"));
            $group.find(".zqui-upload__text").hide();
        }
    });

    /*清除按钮,暂时不要*/
    //$group.on("click", ".btnClear", () => {
    //    $this.data("image", {});
    //    $this.closest(".zqui-form-group").find("input[type=hidden]").val("");
    //    $this.closest(".zqui-form-group").find(".zqui-upload_img").hide().empty();
    //    $this.closest(".zqui-form-group").find(".uploadifive-button").show();
    //})
}

var UploadTmpls = {
    upImagesTmpl: $.templates(`<div class="zqui-upload zqui-upload_img" data-src={{:#data}} >
                    <img src="{{:#data}}"  >
                    <span class="zqui-upload__delete" > <i class="icon icon-trash btnRemove" > </i></span>
                </div>`),
    viewImgTmpl: $.templates(`<div class="zqui-upload zqui-upload_img" data-src={{:#data}} >
                    <img src="{{:#data}}">
                </div>`)
}

$.fn.batchUploadPic = function () {
    var tmpl = UploadTmpls.upImagesTmpl;
    var iconApi = new Sail.ApiHelper("Icon");
    var $this = $(this);
    var $item = $(this).closest(".zqui-upload__text");
    var $list = $(this).closest(".zqui-form-group").find(".imageList");

    function rebind(act: Function) {
        var images = $this.data("images") || [];
        act(images);
        $this.data("images", images);
        $list.html(tmpl.render(images));
        $this.val(JSON.stringify(images));
    }

    var eleData = $this.data();
    console.log(eleData);
    $item.uploadifive({
        width: '5rem',
        height: '5rem',
        buttonText: "<span>上传</span>",
        uploadScript: iconApi.GetApi("UploadFile"),
        fileType: eleData.type || "image/*",
        fileSizeLimit: eleData.size || '5MB',
        multi: true,
        onUploadComplete(file, data) {
            var result = JSON.parse(data);
            console.log("fuck:", result);
            var image = result.Data;
            $(".uploadifive-queue").hide();
            if (result.IsSuccess) {
                rebind(function (images) {
                    images.push(image.FilePath);
                });
            }
        },
    });

    $list.on("click", ".btnRemove", (e) => {
        if (!confirm("确定要删除此图片么?")) return false;
        var src = $(e.currentTarget).closest(".zqui-upload_img").data("src")
        rebind(images => _.remove(images, s => s == src));
    });
}

$.fn.setBatchImages = function (images) {
    var tmpl = UploadTmpls.upImagesTmpl;
    var $this = $(this);
    var $list = $(this).closest(".zqui-form-group").find(".imageList");
    $this.data("images", images);
    $this.val(JSON.stringify(images))
    $list.html(tmpl.render(images));
}

$.fn.SetButtonSelect = function () {
    var $list = $(this);
    $list.find(`button${Active.Selector}`).removeClass(Active.className);
    $list.each((i, o) => {
        var $group = $(o);
        var value = $group.find("input:hidden").val();
        $group.find(`button[value=${value}]`).addClass(Active.className);
    });
}


$.fn.disableButton = (isDisable) => {
    var that = $(this);
    that.closest(".zqui-form-group").find("button").toggleClass("disable", !!isDisable);
}


//ui2.0
function tableNoData($container) {
    var col = $container.find('th').length;
    var $tbody = $container.find('tbody');
    $tbody.empty();
    var html = '<tr class="zqui-table-nodata">\
                        <td colspan='+ col + '>暂无数据</td>\
                    </tr>';
    $tbody.append($(html));
}


function noData($container, content, tmplName) {
    tmplName = tmplName || 'Default';
    tmplName = 'NoData/' + tmplName;
    content = content || '暂无数据';
    $container.Render(tmplName, { Content: content });
}


$.fn.ButtonSelect = function () {
    var $list = $(this);
    $list.each(function (i, v) {
        var $group = $(this);
        $group.off("click");
        $group.on("click", "button", function () {
            var $preButton = $group.find(".active");
            var $btn = $(this);
            if (!$btn.hasClass("disabled")) {
                $group.find(`button${Active.Selector}`).removeClass(Active.className);
                $btn.addClass(Active.className);
                var value = $btn.val();
                var $input = $group.find("input");
                $input.val(value);
                $input.trigger("after.Click", [$input, $btn, $preButton]);
                $("body").trigger(`${$input.prop("name")}.button`, [$btn, $preButton, $input]);
            }
        });
    });
}

/**
 * RazorPage初始化
 * @param act
 */
function PageReady(act: Function) {
    $("body").on("pageInit.after", (sender, tool) => act(tool));

}

var $$ = PageReady;

$(function () {
    var modelId = $.Request("id");
    var action = $.Request("action");

    $("body").on("keyup", "#toolbar input[name=key]", function (event: any) {
        if (event.keyCode === 13)
            $("#toolbar #btnQuery").click();
    });

    $("body").on("after.ResetForm", function (sender, form) {
        $(".buttonSelect").ButtonSelect();
        $(form).find(".buttonSelect").SetButtonSelect();
    });

    $$(function (tool) {
        if (!tool)
            return;
        tool
            .on("after.Add", () => {
                $(".buttonSelect").SetButtonSelect();
                tool.$Editor.find(".selectModal").each(function () {
                    var s = $(this).data();
                    s.selecter.setData(null);
                });
            })
            .on("after.Edit", (sender, data) => {
                $(".buttonSelect").SetButtonSelect();
                tool.$Editor.find(".selectModal").each(function () {
                    var s = $(this).data();
                    s.selecter.setData(data[s.target]);
                    console.log(data[s.target]);
                });
            })
            .on("after.View", () => {

            })
            .on("after.Save", () => {
                if (["add", "view", "edit"].indexOf(action) >= 0) self.close();
            })
            .on("after.Cancel", () => {
                if (["add", "view", "edit"].indexOf(action) >= 0) self.close();
            });

        switch (action) {
            case "add":
                tool.Add();
                break;
            case "view":
            case "edit":
                if (modelId) {
                    $.get(tool.GetApi("Get"), { id: modelId })
                        .done(function (res) {
                            var act = {
                                view: "ViewAct",
                                edit: "EditAct"
                            }
                            MsgBox.Action(res, function () {
                                tool[act[action]](res.Data, tool.Page, tool);
                            })
                        });
                }
                break;
        }
    });


    $("body").on("click", ".icon-calendar", function () {
        $(this).parent().find("input.date:last").trigger("focus");
    });

    $(".navTab").on("click", "li", function (e) {
        var $li = $(this);
        var $target = $($li.data("target"));
        var $div = $li.closest(".navTab");
        $div.find("li.active").removeClass("active");
        $li.addClass("active");
        $div.find(".tabContent").hide();
        $target.show();
        $(".navTab").trigger("after.Switch", $li);
    });
})


/**
 * 弹窗查看图片
 * @param imgClass
 */
function ZoomInIMG(imgClass) {

    //获取图片url
    function getImageUrl(item) {
        return item.data("src");
    }

    //设置图片
    function setImage(src) {
        modalPic.modal.find("img").prop("src", src);
    }

    //图片位移
    function tryGoStep(step: number) {
        var data = modalPic.modal.data("data");
        var maxLength = $(imgClass).length;
        var index = data.index;
        if (index + step < 0) {
            ShowError("已经是第一张了");
            return false;
        }
        else if ((index + step) >= maxLength) {
            ShowError("已经是最后一张了");
            return false;
        }

        data.url = getImageUrl($(imgClass).eq(index + step));
        setImage(data.url);
        data.index += step;
    }

    //点击放大图片弹出层
    var modalPic = $.CreateModal(null, {
        title: "图片详情",
        okTitle: null,
        cancelTitle: "关闭",
        cssClass: "modal-lg",
        tmplName: "#PicTmpl",
        okEvent: (pic) => { },
        init: (div) => {
            var btnClass = 'zqui-btn zqui-btn_default zqui-m_l-xs';
            var $btnCancel = div.find(".btnCancel");
            div.find("img").css({ height: "100%", width: "100%" });

            $(`<a class='${btnClass}  btnLast'>上一张</a>`).insertBefore($btnCancel)
                .click(() => tryGoStep(-1));

            $(`<a class='${btnClass}  btnNext'>下一张</a>`).insertBefore($btnCancel)
                .click(() => tryGoStep(1));

            $(`<a class='${btnClass} zqui-m_r-xs btnNext'>查看原图</a>`).insertBefore($btnCancel)
                .click(() => {
                    var data = div.data("data");
                    window.open(data.url);
                });
        }
    });


    $("body").on("click", imgClass, (e) => {
        var $this = $(e.currentTarget);
        var data = {
            url: $this.data("src"),
            index: $this.index(imgClass)
        };
        setImage(data.url);
        modalPic.modal.data("data", data);
        modalPic.Show();
    });

}

$.views.converters({
    ToLabel: (str, label) => {
        return `<label class='zqui-label ${label} zqui-label_sm'>${str}</label>`;
    }
});


function btnUpload(tmpl, element, parent, buttonText) {

    if (!element) element = "#btnUpload";
    var data = $(element).data();
    if (!tmpl) {
        tmpl = getFileTmpl().UploadTmpl;
    }

    buttonText = buttonText || "上传附件";
    if (!parent) parent = $(element).closest(".zqui-form__element");
    var $fileList = $(parent).find(".fileList");
    var fileApi = new Sail.ApiHelper(data.api || "File");
    $(element).uploadifive({
        width: 100,
        height: 30,
        uploadScript: fileApi.GetApi("UploadFile"),
        fileType: data.type || "*.docx;*.rar;*.mp3;*.wav;*.doc;*.txt;*.xls;*.zip;*.xlsx;*.jpg;*.png;*.pdf;*.jpeg;",
        fileSizeLimit: data.size || "5MB",
        buttonText: `<span>${buttonText}</span>`,
        multi: false,
        onUploadComplete(file, data) {
            var result = JSON.parse(data);
            if (result.IsSuccess) {
                var files = $(element).data("files");
                files.push(result.Data);
                console.log(files);
                if (files.length >= 10) {
                    $(element).uploadifive("disable", true);
                };
                $fileList.html(tmpl.render(files));
                //$(`${parent} `).html(tmpl.render(files));
            } else {
                MsgBox.Error(result.Msg);
            }
        }
    });

    $(element).on("onUploadError", function (sender, errorMsg, errorType, $data, file) {
        if (errorType == "FILE_SIZE_LIMIT_EXCEEDED")
            MsgBox.Error("文件上传出错,文件大小不能超过" + $data.settings.fileSizeLimitText);
    });

    $(element).data("files", []);
    $(".uploadifive-queue").hide();

    var imageTextName = `.zqui-upload__name`;

    $($fileList).on("click", imageTextName, function () {
        $($fileList).find(imageTextName).removeClass(Active.className);
        $(this).addClass(Active.className);
        return false;
    }).on("blur", imageTextName, function () {
        var $this = $(this);
        var id = $this.data("fileid");
        var value = $this.val();
        if (!id) return;
        $this.prop("readonly", true);
        $.post(fileApi.GetApi("Save/") + id, { "": value }, result => {
            $this.prop("readonly", false);
            ShowMessage(result);
        });
    }).on("click", () => {
        $(parent).find(imageTextName).removeClass("active");
    });

    $($fileList).on("click", ".btnRemove", function () {
        var index = $($fileList).find(".btnRemove").index($(this));
        var images = $(element).data("files");
        if (!images) images = [];
        images.splice(index, 1);
        $fileList.html(tmpl.render(images));
    });
}

function getFileTmpl(api: string = "file") {
    return {
        UploadTmpl: $.templates('<div class="zqui-upload__item">\
                                    <div class="zqui-upload__bd">\
                                        {{>FileName}}\
                                    </div>\
                                    <div class="zqui-upload__ft">\
                                        <a class="zqui-text_danger btnRemove"><i class="icon icon-trash"></i></a>\
                                    </div>\
                                </div>'),
        ViewTmp: $.templates(`<div class="zqui-upload__item">\
                                    <div class="zqui-upload__bd">\
                                        <a class="zqui-upload__name">{{>DisPlayName}}</a>\
                                    </div>\
                                    <div class="zqui-upload__ft">\
                                        <a class="zqui-text_green" href="/api/${api}/DownLoad?id={{>FileDbId}}">下载</a>\
                                    </div>
                                </div>`),
        ImageViewTmpl: $.templates('<div class="zqui-upload__item">\
                                       <div class="zqui-upload__hd">\
                                           <a target="_blank" href="{{>FilePath}}"><img src="{{>FilePath}}"></a>\
                                       </div>\
                                       <div class="zqui-upload__bd">\
                                          <a class="zqui-upload__name">{{>DisPlayName}}</a>\
                                       </div>\
                                   </div>'),
        UploadImageTmpl: $.templates('<div class="zqui-upload__item">\
                                       <div class="zqui-upload__hd">\
                                           <a target="_blank" href="{{>FilePath}}"><img src="{{>FilePath}}"></a>\
                                       </div>\
                                    <div class="zqui-upload__bd">\
                                       <textarea data-fileId="{{>FileDbId}}" class="zqui-upload__name">{{>DisPlayName}}</textarea>\
                                    </div>\
                                    <div class="zqui-upload__ft">\
                                        <a class="zqui-text_danger btnRemove">删除</a>\
                                    </div>\
                                </div>'),
        IconTmpl: $.templates('<img src="{{>FilePath}}">')
    };
}



class ModelHelper {
    static rootId: string = "ffffffff-ffff-ffff-ffff-ffffffffffff";
}

class Active {
    static className: string = "active";
    static Selector: string = ".active";
}

function slimScroll(target: string) {
    //组织机构滚动条
    target = target || ".zqui-tray__cell.mainContent .treeview";
    $(window).resize(() => {
        var windowHeight = $(window).height() - 150;
        $(target).slimScroll({
            height: windowHeight,
            railVisible: true
        });
    }).trigger("resize");
}

class ChooseModal extends Sail.Modal {
    private page: Sail.Pagination;

    private find(selector: string): JQuery {
        return this.modal.find(selector);
    }

    public choose(data: any): boolean {
        return true;
    }

    constructor(config) {
        super("", config);
        const that = this;

        var cfg = $.extend({
            reQueryHandle: that.find(".btnQuery"),
            headContainer: that.find("thead"),
            bodyContainer: that.find("tbody"),
            footContainer: that.find(".page"),
            getPostKey: function () {
                return that
                    .find(".toolbar")
                    .GetJson();
            },
            events:[]
        }, config);

        cfg.events.push({
            handle: ".choose",
            act(data) {
                var res = that.choose(data);
                if (res)
                    that.Hide();
            }
        });

        this.page = new Sail.Pagination(cfg);

        this.modal.on("click", ".btnReset", function () {
            that
                .find(".toolbar")
                .ResetForm();
            that.page.Query(1);
        });
    }
}
