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
    ButtonSelect(): any;
    SetButtonSelect(): any;
    iconUpload(tmpl: any): any;
}
/**
 * 返回可上传的文件类型
 * @returns
 */
function ReturnType() { return "*.docx;*.rar;*.mp3;*.wav;*.doc;*.txt;*.xls;*.zip;*.xlsx;*.jpg;*.png;*.pdf;*.jpeg;"; }

/** 
返回可上传的图片类型
*/
function IconType() { return "*.jpg;*.png;*.jpeg;*.gif;*.bmp"; }

module App {
    const debug: boolean = true;

}




$.views.helpers({
    ToDisabledLabel: function (isDisabled: boolean) {
        return ToLabel({ 正常: "success", 停用: "danger" }, isDisabled ? 0 : 1);
    }
});


$.views.converters({
    ToLabel: (str: string, label: string) => {
        return `<label class='zqui-label ${label} zqui-label_sm'>${str}</label>`;
    },
    toBoolStr: function (flag: boolean) {
        return ToLabel({ 是: "success", 否: "danger" }, flag ? 0 : 1);
    },
    toInfoLabel: function (flag: boolean) {
        return ToLabel({ 正常: "success", 停用: "danger" }, flag ? 0 : 1);
    },
    toJson(v) {
        return JSON.stringify(v);
    }
});

function ToLabel(data: any, val?: any, className?: string) {
    if (!className) className = "zqui-label zqui-label_";
    if (!val) val = 0;
    var color: any = [];
    var text: any = [];
    for (var prop in data) {
        text.push(prop);
        color.push(data[prop]);
    }
    if (val < 0 || val > color.length) return "无效";
    var result = `<label class="${className}${color[val]}">${text[val]}</label>`
    return result;

}

$.fn.selectByText = function (text: string) {
    var $ele = this;
    if ($ele.tagName() !== "SELECT") throw new Error("invalid element");
    $ele.find(`option:contains(${text})`).prop("selected", true);
    return $ele;
}

$.fn.iconUpload = function (apiName: string) {
    var $this = $(this);
    var $group = $this.closest(".zqui-form-group");
    var iconApi = new Sail.ApiHelper("Icon");
    $this.uploadifive({
        width: '5rem',
        height: '5rem',
        uploadScript: iconApi.GetApi("UploadFile"),
        fileType: "image/*",
        fileSizeLimit: '10MB',
        buttonText: "<div class='zqui-upload__text'></div>",
        multi: false,
        onProgress(file: any, e: any) {
            if (e.lengthComputable) {
                var percent = Math.round((e.loaded / e.total) * 100);
            }
            $this.closest('.uploadifive-queue').find(".progress").html(`[${file.name}] 已上传 ${percent}% ...`).show();
        },
        onUploadComplete(file: any, data: any) {
            var result = JSON.parse(data);
            var image = result.Data;
            $(".uploadifive-queue").find(".uploadifive-queue-item").remove();
            if (result.IsSuccess) {
                $group.find(".zqui-upload_img").find("img").prop("src", image.FilePath);
                $group.find("input[type=hidden]").val(image.FilePath);
            }
        },
        inited() {
            $group.find(".zqui-upload_img").insertAfter($group.find(".zqui-upload__text"));
            $group.find(".zqui-upload__text").hide();
        }
    });
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
    // console.log(eleData);
    $item.uploadifive({
        width: '5rem',
        height: '5rem',
        buttonText: "<span>上传</span>",
        uploadScript: iconApi.GetApi("UploadFile"),
        fileType: eleData.type || "image/*",
        fileSizeLimit: eleData.size || '100MB',
        multi: true,
        onUploadComplete(file: any, data: any) {
            var result = JSON.parse(data);
            // console.log("fuck:", result);
            var image = result.Data;
            $(".uploadifive-queue").hide();
            if (result.IsSuccess) {
                rebind(function (images: any[]) {
                    images.push(image.FilePath);
                });
            }
        },
    });

    $list.on("click", ".btnRemove", (e: any) => {
        if (!confirm("确定要删除此图片么?")) return false;
        var src = $(e.currentTarget).closest(".zqui-upload_img").data("src")
        rebind((images: any) => _.remove(images, (s: any) => s == src));
    });
}

$.fn.setBatchImages = function (images: any) {
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
    return $list;
}


$.fn.disableButton = function (isDisable: boolean) {
    var that = $(this);
    that.closest(".zqui-form-group").find("button").toggleClass("disable", !!isDisable);
}


//ui2.0
function tableNoData($container: any) {
    var col = $container.find('th').length;
    var $tbody = $container.find('tbody');
    $tbody.empty();
    var html = '<tr class="zqui-table-nodata">\
                        <td colspan='+ col + '>暂无数据</td>\
                    </tr>';
    $tbody.append($(html));
}


function noData($container: any, content: any, tmplName: any) {
    tmplName = tmplName || 'Default';
    tmplName = 'NoData/' + tmplName;
    content = content || '暂无数据';
    $container.Render(tmplName, { Content: content });
}

/*
 * 按钮形式的单选
 */
$.fn.ButtonSelect = function () {
    var $list = $(this);
    $list.each(function (i, v) {
        var $group = $(v);
        $group.off("click");
        $group.on("click", "button", function (e) {
            var $preButton = $group.find(".active");
            var $btn = $(e.target);
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
    return $list;
}


/**
 * RazorPage初始化
 * @param act
 */
var $$ = function (act: Function) {
    $("body").on("pageInit.after", (sender, tool) => act(tool));
}
var PageReady = $$;

/*页面初始化*/
$(function () {
    var modelId = $.Request("id");
    var action = $.Request("action");

    $("body")
        .on("keyup", "#toolbar input[type=text]", function (event: any) {
            if (event.keyCode === 13) $("#toolbar #btnQuery").click();
        })
        .on("after.ResetForm", function (sender, form) {
            $(form).find(".buttonSelect").ButtonSelect().SetButtonSelect();
        });

    $("body").on("after.InitForm", function (s, form) {
        // console.log(form);
        $(form).find(".singleFileUpload,.multiFilesUpload").each(function () {
            $(this).btnUpload();
        });
    })

    $("body").on("after.GetJson", function (s: any, model) {
        $(s.target).find(".multiFilesUpload").each(function () {
            model[$(this).attr("id")] = $(this).data("files") == null ? [] : $(this).data("files");
        });
    });

    $$(function (tool: any) {

        if (!tool) return;
        tool
            .on("after.Add", () => {
                $(".buttonSelect").SetButtonSelect();
                tool.$Editor.find(".selectModal").each(function (index: number, e: any) {
                    var s = $(e).data();
                    s.selecter.setData(null);
                });
                tool.$Editor.find(".singleFileUpload,.multiFilesUpload").each(function (i, e) {
                    $(e).closest(".zqui-form").find(".fileList").html("")
                    $(e).data("files", []);
                });
            })
            .on("after.Edit", (sender: any, data: any) => {
                $(".buttonSelect").SetButtonSelect();
                tool.$Editor.find(".selectModal").each(function (index: number, e: any) {
                    var s = $(e).data();
                    s.selecter.setData(data[s.target]);
                });
                tool.$Editor.find(".singleFileUpload,.multiFilesUpload").each(function (i, e) {
                    var value = data[$(e).attr("id")];
                    if (value) {
                        value.multi = $(e).data("multi")
                        $(e).setUpload(data[$(e).attr("id")]);
                    }
                    $(this).closest('.zqui-box').find(".zqui-progress").html("").hide();
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
                            var act: any = {
                                view: "ViewAct",
                                edit: "EditAct"
                            }
                            MsgBox.Action(res, function () {
                                var fAction = act[action];
                                if (fAction) {
                                    var a = tool[fAction];
                                    if (typeof (a) == 'function') {
                                        console.log(res.Data)
                                        a.apply(tool, [res.Data, tool.Page, tool]);
                                    }
                                }
                            })
                        });
                }
                break;
        }
    });

    //日历点击事件
    $("body").on("click", ".icon-calendar", function (e) {
        $(e.target).parent().find("input.date:last").trigger("focus");
    });

    //选项卡点击事件
    registerNav("body");

    //注册页面上静态的modal
    $(".modal[role='dialog']").each(function () {
        $(this).on("click", ".btnCancel", () => {
            $(this).removeClass("active").trigger("hidden.bs.modal", [$(this)]);
        });
    });


})

/*弹窗相关*/
$.fn.Show = function () {
    $(this).ResetForm();
    $(this).find(".btnOk").prop("disabled", false);
    $(this).addClass("active");
}

$.fn.Hide = function () {
    $(this).removeClass("active");
}

$.fn.OkEvent = function (act) {
    $(this).on("click", ".btnOk", () => act($(this)));
}

$.fn.CancelEvent = function (act) {
    $(this).on("hidden.bs.modal", () => act($(this)));
}


$.fn.SetTitle = function (title) {
    $(this).find(".modal-title").html(title);
}
/*弹窗相关结束*/


function registerNav(div) {
    $(div).on("click", ".navTab li", function (e) {
        var $li = $(this);
        var $div = $li.closest(".navTab");
        $div.find("li.active").removeClass("active");
        $li.addClass("active");
        $div.find(".tabContent").hide().eq($li.index()).show();
        $(".navTab").trigger("after.Switch", [$li, $li.index(), $div]);
    });
}



/**
 * 弹窗查看图片
 * @param imgClass
 */
function ZoomInIMG(imgClass: string | JQuery | Element) {

    //获取图片url
    function getImageUrl(item: JQuery) {
        return item.data("src");
    }

    //设置图片
    function setImage(src: any) {
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
        okEvent: (pic: any) => { },
        init: (div: any) => {
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
        var $this = $(e.target);
        var data = {
            url: $this.data("src"),
            index: $this.index(imgClass)
        };
        setImage(data.url);
        modalPic.modal.data("data", data);
        modalPic.Show();
    });

}

$.fn.btnUpload = function (buttonText?: any) {
    btnUpload(null, this, null, buttonText);
    return $(this);
}
$.fn.setUpload = function (data, parent?: any) {
    var element = this;
    var setting = $(element).data();
    var files = data;
    if (!$.isArray(data)) files = [data];

    if (!parent) parent = $(element).closest(".zqui-form");
    var $fileList = $(parent).find(".fileList");
    $(element).data("files", files);
    var tmpl: any;
    if (setting.uptmpl) tmpl = getFileTmpl()[data.uptmpl];
    else tmpl = data.multi ? getFileTmpl().UploadTmpl : getFileTmpl().OneTmpl;
    // console.log(files);
    $fileList.html(tmpl.render(files));
    return $(this);
}



function btnUpload(tmpl: any, element: any, parent?: any, buttonText?: any) {
    if (!element) element = "#btnUpload";
    var data = $(element).data();
    if (!tmpl) {
        if (data.uptmpl) tmpl = getFileTmpl()[data.uptmpl];
        else tmpl = data.multi ? getFileTmpl().UploadTmpl : getFileTmpl().OneTmpl;
    }
    buttonText = buttonText || $(element).data("text");
    buttonText = buttonText || "上传附件";
    if (!parent) parent = $(element).closest(".zqui-form");
    var $fileList = $(parent).find(".fileList");
    var filelimit = data.filelimit || 8;

    var fileApi = new Sail.ApiHelper(data.api || "File");
    $(element).uploadifive({
        width: 100,
        height: 30,
        uploadScript: fileApi.GetApi("UploadFile"),
        fileType: data.type || "*.docx;*.rar;*.mp3;*.wav;*.doc;*.txt;*.xls;*.zip;*.xlsx;*.jpg;*.png;*.pdf;*.jpeg;",
        fileSizeLimit: data.size || "1024MB",
        buttonText: `<span>${buttonText}</span><span class='progress'></span>`,
        multi: false,
        overrideEvents: ["onProgress"],
        onProgress(file: any, e: any) {
            if (e.lengthComputable) {
                var percent = Math.round((e.loaded / e.total) * 100);
            }
            $(element).closest('.zqui-box').find(".zqui-progress").html(`[${file.name}] 已上传 ${percent}% ...`).show();
            $(".uploadifive-queue").hide();
        },
        onUploadComplete(file: any, data: any) {
            var setting = $(element).data();
            $(".uploadifive-queue").hide();
            $(element).closest('.zqui-box').find(".zqui-progress").html("").hide();
            var result = JSON.parse(data);
            MsgBox.Action(result, function () {
                var valuefield = setting.valuefield;
                if (setting.multi) {
                    var files = $(element).data("files");
                    files.push(result.Data);
                    $fileList.html(tmpl.render(files));
                    $(element).val($.map(files, function (x) { return x[valuefield] }));
                }
                else {
                    files = [result.Data];
                    $(element).val(result.Data[valuefield]);
                    $fileList.html(tmpl.render(files));
                }
            })
        }
    });

    $(element).on("onUploadError", function (sender, errorMsg, errorType, $data, file) {
        if (errorType == "FILE_SIZE_LIMIT_EXCEEDED")
            MsgBox.Error("文件上传出错,文件大小不能超过" + $data.settings.fileSizeLimitText);
    });

    $(element).data("files", []);
    //  

    var imageTextName = `.zqui-upload__name`;

    $($fileList).on("click", imageTextName, function (e) {
        $($fileList).find(imageTextName).removeClass(Active.className);
        $(e.target).addClass(Active.className);
        return false;
    }).on("blur", imageTextName, function (e) {
        var $this = $(e.target);
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

    $($fileList).on("click", ".btnRemove", function (e) {
        var index = $($fileList).find(".btnRemove").index($(this));
        var images = $(element).data("files");
        if (!images) images = [];
        images.splice(index, 1);
        $fileList.html(tmpl.render(images));
    });
}

function getFileTmpl(api: string = "file") {
    return {
        OneTmpl: $.templates('<div class="zqui-upload__item">\
            <div class="zqui-upload__bd">\
                {{>FileName}}\
            </div>\
            </div>'),
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
                                        <a class="zqui-upload__name">{{:FileName}}</a>\
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

        IconTmpl: $.templates('<img src="{{>FilePath}}">'),
        viewTmplLive: $.templates(`<div class="zqui-upload__item">\
                        <div class="zqui-upload__bd">\
                            <a class="zqui-upload__name">{{:FileName}}</a>\
                        </div>\
                        <a class="zqui-text_green" target="_blank" href="{{>FilePath}}">预览</a>
                        <div class="zqui-upload__ft">\
                            <a class="zqui-text_green" href="/api/${api}/DownLoad?id={{>FileDbId}}">下载</a>\
                        </div>
                    </div>`)
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


