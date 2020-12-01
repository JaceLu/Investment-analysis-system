var Kaycore;
(function (Kaycore) {
    var CheckStatus = { 无效: "danger", 待审核: "primary", 已通过: "success", 已拒绝: "danger" };
    function ToCheckStatusLable(val) {
        return ToLabel(CheckStatus, val);
    }
    Kaycore.ToCheckStatusLable = ToCheckStatusLable;
    $(function () {
        $.views.converters({
            ToCheckLable: function (str) {
                return ToCheckStatusLable(str);
            }
        });
    });
})(Kaycore || (Kaycore = {}));
//# sourceMappingURL=Status.js.map