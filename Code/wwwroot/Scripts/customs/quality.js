var quality = {
    handleOkEvent: function (modal, tool) {
        if (modal != null) modal.Hide();
       
        if (tool.inViewing()) {
            if (self.location.search == "") {
                $("#btnCancel").click();
            } else {
                self.location = document.referrer;//去另一个页面刷新
            }
        }
        tool.Page.Query();
    },
    bindEvent: function (crudBtns, tool) {
        tool.on('after.View', function (sender, data) {
            tool.viewData = data;
        });
        for (var i in crudBtns) {
            var v = crudBtns[i];
            (function (btnOptions, tool) {
                $('body').on('click', btnOptions.handle, function () {
                    var listData = $(this).view().data;
                    btnOptions.act(listData || tool.viewData, !listData);
                });
            })(v, tool);
        }
        //$('body').on('click', '.btnAct', function () {
        //    tool.BtnAct($.view(this).data, tool.Page, this);
        //})
    }
}