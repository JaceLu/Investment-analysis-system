
namespace Kaycore {
    var CheckStatus = { 无效: "danger", 待审核: "primary", 已通过: "success", 已拒绝: "danger" };

    export function ToCheckStatusLable(val) {
        return ToLabel(CheckStatus, val);
    }

    $(function () {
        $.views.converters({
            ToCheckLable: function (str) {
                return ToCheckStatusLable(str);
            }
        });
    })
}