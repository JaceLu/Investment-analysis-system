

function PageLoad(title, pageTmpl) {

    this.Title = title;
    this.page = undefined;
    this.modal = undefined;
    var tool = this;

    //加载主模板页面
    if (!pageTmpl) pageTmpl = "WeGenera";

    this.content = $($.LoadHtml(pageTmpl)).appendTo("body");
    this.content.find(".title").eq(0).html(title);

    this.$form = $('#content');
    if (this.$form.length > 0) {
        this.$form.html($("#modalTmpl").html());
    }

    this.$form.initForm();
};


(function ($) {

    $.fn.ClickOrTouch = function (target, act) {
        var clickOrTouch = (('ontouchend' in window)) ? 'touchend' : 'click';
        $(this).on(clickOrTouch, target, act);
        return $(this);
    } 
  
})(jQuery);



(function (doc, win) {
    var docEl = doc.documentElement,
      resizeEvt = 'orientationchange' in window ? 'orientationchange' : 'resize',
      recalc = function () {
          var clientWidth = docEl.clientWidth;
          if (!clientWidth) return;
          docEl.style.fontSize = 20 * (clientWidth / 320) + 'px';
      };

    if (!doc.addEventListener) return;
    win.addEventListener(resizeEvt, recalc, false);
    doc.addEventListener('DOMContentLoaded', recalc, false);
})(document, window);


var moreTmpl = '<div class="zqui-pull-bottom-pocket zqui-block zqui-visibility last showMore">\
                    <div class="zqui-pull">\
                        <div class="zqui-pull-caption zqui-pull-caption-nomore more">点击显示更多</div>\
                    </div>\
                </div>';
var noneTmpl = '<div class="zqui-pull-bottom-pocket zqui-block zqui-visibility">\
                    <div class="zqui-pull">\
                        <div class="zqui-pull-loading zqui-icon zqui-spinner zqui-hidden"></div>\
                        <div class="zqui-pull-caption zqui-pull-caption-nomore">没有更多数据了</div>\
                    </div>\
                </div>';

function Nodata(pager) {
    pager.empty();
    //$(pager).noData();
    $(noneTmpl).appendTo(pager);
}

Sail.modalTemplates = {
    dialog: "<div class='modal zqui-modal' >" +
        "<div class='modal-dialog zqui-modal-dialog' >" +
        "<div class='modal-content zqui-modal-content'>" +
        "<div class='modal-body zqui-modal-body'></div>" +
        "</div>" +
        "</div>" +
        "<div class='zqui-modal-mask'></div></div>",
    header: "<div class='modal-header zqui-modal-header'>" +
        "<h4 class='modal-title zqui-modal-title'></h4>" +
        "</div>",
    footer: "<div class='modal-footer zqui-modal-footer'></div>",
    cancleButton: "<a  class='zqui-modal-btn btnCancel' data-dismiss='modal' aria-hidden='true'></a>",
    okButton: '<button class="zqui-modal-btn text-primary btnOk"></button>'
};

$.fn.modal = $.fn.myModal;

jQuery.fn.extend({
    LoadMore: function (set) {
        set = jQuery.extend({
            noData: "暂无数据",
            queryed: null, //查询完成后的回调
            pageSize: 10,//每页行数   
            Type: 1,
            handleName: null,//handle  
            events: [],//事件集合
            getPostKey: function () { return {}; }//获取提交数据的方法 
        }, set);

        var result = {
            pager: $("#pager"),
            $list: $(this),
            currentData: null,
            currentIndex: 1,
            settings: set,
            Query: function (pageIndex) {
                result.pager.html("");
                if (pageIndex) result.currentIndex = pageIndex;
                pageIndex = result.currentIndex;

                if (pageIndex == 1) result.$list.html("");

                var settings = result.settings;
                var postData = settings.getPostKey();
                postData["pageSize"] = settings.pageSize;
                postData["pageIndex"] = pageIndex;
                var url = settings.handleName;
                if (typeof url == "function") url = url();
                $.ajax({
                    url: url,
                    data: postData,
                    success: function (data) {
                        
                        if (data.Data == null ||data.Data.Count == 0) {
                            Nodata(result.pager);
                            if (settings.queryed)
                                settings.queryed.call(null, data.Data);
                            return;
                        }
                        
                        result.$list.append($.Loadtmpl("#listTmpl").render(data.Data.Data)); 
                        var pageCount = data.Data.PageInfo.PageCount;
                        result.pager.html(pageCount > result.currentIndex ? moreTmpl : noneTmpl);
                        if (settings.queryed)
                            settings.queryed.call(null, data.Data);
                    }
                });
                return result;
            }
        };
        result.pager.on("click", ".showMore", function () {
            result.Query(result.currentIndex + 1);
        });
        return result;
    },

    BindList: function (listTmpl, data, msgtext, noTmpl) {  //绑定列表并在没数据的时候显示暂无数据
        $(this).parent().find(".noResult").remove();
        if (!noTmpl) noTmpl = "NoResult";
        $(this).Link(listTmpl, data);

        if (!data || data.length == 0) {

            var noresult = $("<div class='noResult'></div>");
            console.log(noTmpl);
            noresult.Render(noTmpl);

            if (msgtext) noresult.find(".massage").text(msgtext);
            noresult.insertAfter(this);
        }
    },
});


function scanCode(act) {

    wx.error(function (res) {
        alert("微信初始化错误，请重新打开页面")
    });
    wx.scanQRCode({
        needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
        scanType: ["qrCode", "barCode"], // 可以指定扫二维码还是一维码，默认二者都有
        success: function (res) {

            var cardCode = "";
            if (res.resultStr.indexOf(",") > 0) {
                cardCode = res.resultStr.split(",")[1];
            } else {
                cardCode = res.resultStr;
            }
            act(cardCode);
        }
    });
}
