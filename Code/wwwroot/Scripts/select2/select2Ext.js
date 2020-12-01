

$(function () {
    $("body")
        .on("after.ResetForm", function (sender, form) {
            ///$(form).find("input[type=hidden].select2").select2("data", null);
        })
        .on("after.InitForm", function (sender, form) {
            $(form).find("select.select2").each(function () {
                $(this).select2();
            });
        });

    $$(function (tool) {
        function getSelect2Config(targetEl) {
            var dataAttr = targetEl.data();
            var config = {
                width: '400px',
                placeholder: targetEl.attr("placeholder"),//'请选择仓库',
                multiple: false
            };
            return {
                config,
                dataAttr
            };
        }
        tool
            .on("after.Edit", function (sender, data) {
                tool.$Editor.find("[data-disableEdit=true]").each(function () {
                    $(this).attr("disabled", true);
                });
               
                tool.$Editor.find("select.select2").each(function (i, v) {

                    var targetEl = $(v);
                    var result = getSelect2Config(targetEl);
                    var dataAttr = result.dataAttr;
                    var value = data[dataAttr.value]; // typeof v.key === 'function' ? v.key.call(null, data) : data[v.key];
                    if (dataAttr.field) value = value[dataAttr.field];
                    targetEl.val(value).trigger("change").select2(result.config);
                });
            })
            .on("after.Add", function (s, data) {
                tool.$Editor.find("[data-disableEdit=true]").each(function () {
                    $(this).attr("disabled", false);
                });

                tool.$Editor.find("select.select2").each(function (i, v) {
                    var targetEl = $(v);
                    var result = getSelect2Config(targetEl);
                    targetEl.val("").trigger("change").select2(result.config);
                });
            });
    });
});

