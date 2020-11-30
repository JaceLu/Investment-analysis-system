/// <reference path="jquery.d.ts" />
/// <reference path="jsrender.d.ts" />
/// <reference path="underscore.d.ts" />

declare function escape(s: string): string;
declare function unescape(s: string): string;
interface ILoginSet {
    inputId: string;
    inputPwd: string;
    ckRember: string;
    btnLogin: string;
    cookiesName: string;
    path: string;
    controller: string;
}
interface ITHead {
    name: string;
    class?: string;
    width?: number;
    toString(): string;
}
interface JQuery {
    /**
     * 导出Excel
     * @param option
     * @returns {}
     */
    Table2Excel(option: any): JQuery;
    /**
     * 日期段
     * @returns {}
     */
    DateSwitch(): JQuery;
    /**
     *
     * @param args
     * @returns {}
     */
    datetimepicker(...args: any[]): JQuery;
    /**
     *
     * @param args
     * @returns {}
     */
    bootstrapSwitch(...args: any[]): JQuery;
    /**
     *
     * @returns {}
     */
    tooltip(): JQuery;
    /**
     *
     * @param args
     * @returns {}
     */
    modal(...args: any[]): JQuery;
    /**
     *
     * @param param
     * @returns {}
     */
    validationEngine(...args: any[]): boolean;
    /**
     *
     * @param args
     * @returns {}
     */
    ajaxAddOptions(...args: any[]): JQuery;
    /**
     *
     * @param args
     * @returns {}
     */
    AddOptions(...args: any[]): JQuery;
    /**
     * 表单初始化
     */
    initForm(): JQuery;
    /**
     * 重置表单
     */
    ResetForm(): JQuery;
    /**
     * 根据标题生成表头
     * @param {type} heads 标题数组
     */
    MakeHead(heads: Array<ITHead>, widths?: Array<number>): JQuery;
    /**
     * 初始化编辑器
     */
    initEditor(): JQuery;
    /**
     *重置编辑器
     */
    resetEditor(): JQuery;
    /**
     * 设置对象值到编辑器
     * @param {Object} model
     */
    setToEditor(model: Object): JQuery;
    /**
     * 从编辑器取值到对象
     * @param {Object} model
     */
    editorToModel(model: Object): JQuery;
    /**
     * 定义日期插件
     */
    SetDate(): JQuery;
    /**
     * 定义时间插件
     */
    SetDateTime(): JQuery;
    /**
     * 获取html标签名称
     */
    tagName(): string;
    /**
     * 获取json对象
     */
    GetJson(): Object;
    /**
     * 关闭自定义的弹窗
     */
    closeModal(): JQuery;
    /**
     * 兼容bootstrap的自定义弹窗(或许在微信端用)
     * @param {string} o? 如果为'hide'则隐藏弹窗，否则显示弹窗
     */
    myModal(o?: string): JQuery;
    /**
     * 获得双向绑定工具
     * @param {type} o?
     */
    ItemBinder(o?: Object): Sail.Binder;
    /**
     * 表单验证
     */
    CheckValidation(): boolean;
    /**
     * 回车触发按钮
     * @param {any} btn
     */
    EnterToClick(btn: any): JQuery;
    /**
     * 给表格中空白td增加一个&nbsp;
     */
    TableChange(): JQuery;
    /**
     * 读取远程模板文件
     */
    loadRemoteTmpl(): JQuery;
    /**
     * 根据模板和数据输出html
     * @param {string} tmpl
     * @param {any} data
     */
    Render(tmpl: string, data: any): JQuery;
    /**
     * 根据模板和数据绑定html
     * @param {string} tmpl
     * @param {any} data
     */
    Link(tmpl: string, data: any): JQuery;
    /**
     * 表单对象的val转换为int 如果转换失败则为0
     */
    valToInt(): number;
    /**
     * 把表单对象的val转换为float 如果转换失败则为0
     */
    valToFloat(): number;
    /**
     * 设置元素显示或隐藏
     * @param {type} isshow
     */
    Display(isshow: boolean): JQuery;
    /**
     * 给table增加合计行 根据thead的第一行th数量生成对应的td数量
     * @param {type} cols 需合计的列
     * @param {type} title 合计行的名称，默认为:合计
     * @param {type} skipStep 每隔n行计算一次，默认为每行都计算
     */
    AddSum(cols: Array<number>, title: string, skipStep: number): JQuery;
}
declare function WdatePicker(param: any): any;
declare module Sail {
    class ExportExcel {
        private element;
        private template;
        private tableRows;
        private settings;
        private uri;
        private defaults;
        constructor(element: any, options: any);
        init(): void;
        private ctx;
        format(s: any, c: any): any;
        base64(s: string): string;
        getFileName(settings: any): string;
        tableToExcel(table: any, name: string): boolean;
    }
}

interface ISailConfig {
    Root: string;
    ApiRoot: string;
    PageContent: string;
}
interface JQueryStatic {
    SailConfig: ISailConfig;
    setSailConfig(set: ISailConfig): void;
    templatesRoot: string;
    view(obj: any): any;
    growl(text: string, obj: any): JQuery;
    /**
     * 获取url参数
     * @param {type} name
     */
    Request(name: string): string;
    /**
     * 把url的参数序列化成对象
     */
    decodeParam(): Object;
    /**
     * 把对象编码成字符串
     * @param {Object} obj
     */
    encodeObject(obj: Object): string;
    /**
     * 把编码后的字符串还原成对象
     * @param {string} str
     */
    decodeObject(str: string): Object;
    /**
     * 双向绑定工具
     * @param container 待绑定的容器
     * @param {Object} objItem 要绑定的默认数据
     */
    ItemBinder(container: any, objItem: Object): Sail.Binder;
    /**
     * ajax delete方法
     * @param {type} url 地址
     * @param {type} data? 要提交的数据
     * @param {type} act? 成功后执行的方法
     */
    Delete(url: string, data?: any, act?: Function): JQueryXHR;
    /**
     *  ajax put方法
     * @param {type} url 地址
     * @param {type} data? 要提交的数据
     * @param {type} act? 成功后执行的方法
     */
    Put(url: string, data?: any, act?: Function): JQueryXHR;
    /**
     * 读取html文本
     * @param {string} tname
     */
    LoadHtml(tname: string): string;
    /**
     * 读取模板
     * @param {string} tname
     */
    Loadtmpl(tname: string): JsRender.Template;
    /**
     * 创建分页控件
     * @param {type} set
     */
    Pagination(set: Sail.IPaginationSetting): Sail.Pagination;
    /**
     * 读取后台序列化并转换成json的对象
     * @param {type} obj
     */
    LoadParam(obj: JQuery): JSON;
    /**
     * 创建弹窗
     * @param {string} id
     * @param {Sail.ModalSetting} set
     */
    CreateModal(id: string, set: Sail.IModalSetting): Sail.Modal;
}
declare function guid(): string;
declare module Sail {
    interface AjaxResult {
        IsSuccess: Boolean;
        Data: any;
        Msg: string;
    }
}

interface JQuery {
    dateRegister(): JQuery;
}
declare module Sail {
    class DateSwitcher {
        template: {
            DateSwitch: (str: any) => string;
            DateButton: string;
            TimeBox: string;
            Thead: string;
            TbodyRow: string;
            dateRangeButton: string;
        };
        Week: string[];
        Months: string[];
        viewDate(y: number, m: number): any[];
        render(container: any, data: any): void;
        refresh(container: any, y: any, m: any): void;
        findLocation($table: any, date: any): JQuery;
        recoverState(set: any): void;
        initBox(set: any): void;
        open(set: any): void;
        close(set: any): void;
        choose(set: any): void;
        changeDate(set: any): void;
        clear(set: any): void;
        today(set: any): void;
        shortCutButton(set: any): void;
        bind(set: any): void;
        createTop(topTitle: any): JQuery;
        createTable(): JQuery;
        createBottom(isHasTime: any): JQuery;
        dateBox(set: any): JQuery;
        dateRangeBox(set: any): JQuery;
        static datePicker(set: any): DateSwitcher;
        static rangePicker(set: any): DateSwitcher;
        static register(form: any): void;
    }
}

interface Object {
    [key: string]: any;
}
interface Date {
    /**
     * 获取日期属于周几
     */
    dayofWeek(): string;
    /**
     * 格式化日期字符串
     * @param {string} format 格式化字符串 同c#的格式
     *  默认格式是yyyy-MM-dd
     */
    format(format?: string): string;
    /**
     * 获取月头第一天日期
     */
    MonthFirst(): Date;
    /**
     * 获取约莫最后一天
     */
    MonthEnd(): Date;
    /**
     * 获取本周第一天(周一)
     */
    WeekFirst(): Date;
    /**
     * 获取本周最后一天(周日)
     */
    WeekEnd(): Date;
    /**
     * 增加n秒
     * @param sec
     */
    AddSeconds(seconds: number): Date;
    /**
     * 增加n分钟
     * @param {number} minutes
     */
    AddMinutes(minutes: number): Date;
    /**
     * 增加n个小时(可以为负数)
     * @param {number} hour
     */
    AddHours(hour: number): Date;
    /**
     * 增加n天
     * @param {number} days
     */
    AddDays(days: number): Date;
    /**
     * 增加n月
     * @param {number} month
     */
    AddMonths(month: number): Date;
    /**
     * 增加n年
     * @param {number} years
     */
    AddYears(years: number): Date;
}
declare module DateTime {
    /**
     * 把字符串解析成日期时间格式
     * @param {string} str
     * @returns {Date} 时间
     */
    function Parse(str: string): Date;
    /**
     * 判断指定年是否为闰年
     * @param {number} year
     * @returns
     */
    function IsLeapYear(year: number): Boolean;
    /**
     * 获取某年或某月的总天数
     * @param {number} year 年份
     * @param {number} month? 月份
     * @return {number} 天数
     */
    function DaysCount(year: number, month?: number): number;
    /**
     * 返回某年每个月的天数
     * @param {number} year
     */
    function MonthDaysList(year: number): Array<Number>;
    /**
     * 获取一个日期所在月的天数
     * @param date
     */
    function MonthDays(date: Date): number;
}

interface JQuery {
    /**
    * 获取控件类型
    * @param control
    * @returns
    */
    GetControlType(): Sail.ControlType;
    SetValue(value: any): JQuery;
    GetValue(type: string): JQuery;
    BindObject(model: any): JQuery;
}
declare module Sail {
    enum ControlType {
        Val = 0,
        Checked = 1,
        Src = 2,
        Html = 3,
    }
    /**
     * 获取对象的属性列表
     * @param obj
     */
    function getProperties(obj: Object): any[];
    class Binder {
        private _$item;
        CurrentItem: Object;
        static Helpers: any;
        static Helper(ext: any): void;
        /**
         * 构造函数
         * @param {any} container 容器
         * @param {Object} objItem 默认对象 可为空
         */
        constructor(container: any, objItem?: Object);
        /**
         * 对象赋值到html
         * @param {Object} model
         */
        private ToHtml(model);
        SetObject(obj: Object): void;
    }
}

interface JQueryCookieOptions {
    /**
     * Define lifetime of the cookie. Value can be a Number which will be interpreted as days from time of creation or a Date object. If omitted, the cookie becomes a session cookie.
     */
    expires?: any;
    /**
     * Define the path where the cookie is valid. By default the path of the cookie is the path of the page where the cookie was created (standard browser behavior). If you want to make it available for instance across the entire domain use path: '/'. Default: path of page where the cookie was created.
     */
    path?: string;
    /**
     * Define the domain where the cookie is valid. Default: domain of page where the cookie was created.
     */
    domain?: string;
    /**
     * If true, the cookie transmission requires a secure protocol (https). Default: false.
     */
    secure?: boolean;
}
interface JQueryCookieStatic {
    /**
     * By default the cookie value is encoded/decoded when writing/reading, using encodeURIComponent/decodeURIComponent. Bypass this by setting raw to true:
     */
    raw?: boolean;
    /**
     * Turn on automatic storage of JSON objects passed as the cookie value. Assumes JSON.stringify and JSON.parse
     */
    json?: boolean;
    /**
     * Cookie attributes can be set globally by setting properties of the $.cookie.defaults object or individually for each call to $.cookie() by passing a plain object to the options argument. Per-call options override the default options.
     */
    defaults?: JQueryCookieOptions;
    /**
     * Gets an object of cookies as key-value pairs
     */
    (): {
        [key: string]: string;
    };
    /**
     * Gets a cookie by name
     * @param name The name of the cookie to get
     */
    (name: string): any;
    /**
     * Sets a cookie
     * @param name The name of the cookie to set
     * @param value The value to set the cookie to
     */
    (name: string, value: string): void;
    /**
     * Gets a cookie by name after applying a conversion function to the value
     * @param name The name of the cookie to get
     * @param converter A conversion function to change the cookie's value to a different representation on the fly
     */
    (name: string, converter: (value: string) => any): any;
    /**
     * Sets a cookie with some options
     * @param name The name of the cookie to set
     * @param value The value to set the cookie to
     * @param options An object of options that change how the cookie is set
     */
    (name: string, value: string, options: JQueryCookieOptions): void;
    /**
     * Sets a cookie using .toString(), or if $.cookie.json is set to true using JSON.stringify()
     * @param name The name of the cookie to set
     * @param value The value to set the cookie to
     */
    (name: string, value: any): void;
    /**
     * Sets a cookie using .toString(), or if $.cookie.json is set to true using JSON.stringify()
     * @param name The name of the cookie to set
     * @param value The value to set the cookie to
     * @param options An object of options that change how the cookie is set
     */
    (name: string, value: any, options: JQueryCookieOptions): void;
}
interface JQueryStatic {
    /**
     * A simple, lightweight jQuery plugin for reading, writing and deleting cookies.
     */
    cookie(key: string, value?: any, options?: any): any;
    /**
     * Deletes a cookie
     * @param name Name of cookie to delete
     */
    removeCookie(name: string): boolean;
    /**
     * Deletes a cookie
     * @param name Name of cookie to delete
     * @param options The same attributes (path, domain) as what the cookie was written with
     */
    removeCookie(name: string, options: JQueryCookieOptions): boolean;
}
declare module JCookies {
}

declare module Sail {
    interface IModalSetting {
        title: string;
        modalHandle?: string;
        tmplName?: string;
        addEvent?: any;
        okEvent?: any;
        init?: any;
        hideEvent?: any;
        cssClass?: string;
        okTitle?: string;
        cancelTitle?: string;
    }
    var modalTemplates: {
        dialog: string;
        header: string;
        footer: string;
        cancleButton: string;
        okButton: string;
    };
    /**
     * 弹窗插件
     */
    class Modal {
        modal: JQuery;
        /**
         * 显示弹窗
         */
        Show(): void;
        /**
         * 隐藏弹窗
         */
        Hide(): void;
        /**
         * 修改弹窗标题
         * @param {string} title
         */
        Title(title: string): void;
        resetOk(): void;
        private head;
        private $ok;
        /**
         * 初始化
         * @param {string} id
         * @param {any} settings
         */
        constructor(id: string, settings: IModalSetting);
    }
}

/**
 * 提示浮动层
 * @param {type} text
 */
declare module MsgBox {
    var growl_default_options: {
        ele: string;
        type: string;
        allow_dismiss: boolean;
        position: {
            from: string;
            align: string;
        };
        offset: number;
        spacing: number;
        z_index: number;
        fade_in: number;
        delay: number;
        pause_on_mouseover: boolean;
        template: {
            alertPrefix: string;
            container: string;
            dismiss: string;
        };
    };
    /**
     * 配置模板
     * alertPrefix: "alert-", //颜色样式前缀
       container: '<div class="col-sm-10 col-sm-10 col-md-3 alert">', //主体模板容器
       dismiss: '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>', //关闭按钮容器
     * @param options
     *
     */
    function SetConfig(options: any): void;
    /**
     * 显示错误提示 (红色)
     * @param {type} text
     * @returns
     */
    function Error(text: string): JQuery;
    /**
     * 显示信息提示 (蓝色)
     * @param {type} text
     * @returns
     */
    function Info(text: string): JQuery;
    /**
     * 显示成功提示 (绿色)
     * @param {type} text
     * @returns
     */
    function Success(text: string): JQuery;
    /**
     * 根据返回结果决定显示成功或错误提示
     * @param {type} result 数据结果
     * @param {type} successText? 成功后显示的文字，如果不填默认为result.Msg
     * @param {type} act? 成功后执行的方法，会将result.Data传入作为变量
     */
    function Show(result: Sail.AjaxResult, successText?: any, act?: Function): void;
    /**
     * 操作成功才提示
     * @param {Sail.AjaxResult} result
     * @param {Function} act?
     */
    function Action(result: Sail.AjaxResult, act?: Function, msg?: string): void;
}
declare var ShowMessage: typeof MsgBox.Show;
declare var ShowInfo: typeof MsgBox.Info;
declare var ShowSuccess: typeof MsgBox.Success;
declare var ShowError: typeof MsgBox.Error;

declare namespace Sail {
    interface ApiSet {
        action: string;
        data?: any;
        success: Function;
    }
    class ApiHelper {
        private Controller;
        constructor(controller: string);
        GetApi(actionName: string): string;
        get(action: string, data?: any, success?: Function): void;
        post(action: string, data?: any, success?: Function): void;
        put(action: string, data?: any, success?: any): void;
        delete(action: string, data?: any, success?: Function): void;
    }
    interface IRazorPageSet {
        saveApi: string;
        isAutoHide: boolean;
        title: string;
    }
    class RazorPage {
        private ApiName;
        private IdName;
        private Id;
        $List: JQuery;
        btnAdd: JQuery;
        Page: Sail.Pagination;
        modal: Sail.Modal;
        $form: JQuery;
        content: JQuery;
        $Editor: JQuery;
        $Viewer: JQuery;
        setting: IRazorPageSet;
        static Default: RazorPage;
        constructor(title: string, api: string, idName: string, set: IRazorPageSet);
        SetTitle(title: string): this;
        /**
         * 取消事件
         * 会触发after.Cancel事件，如需自定义请使用this.content.on("after.Cancel",function(){});
         * @returns
         */
        Cancel(): this;
        /**
         * 修改表单页面的标题
         * @param {type} title
         */
        SetFormTitle(title: string): void;
        GetApi(name: string): string;
        /**
         * 返回对象的id值(根据idName)
         * @param {type} obj
         * @returns
         */
        GetDataId(obj: Object): any;
        /**
         * 获取列表api的url
         * @returns
         */
        listApi(): string;
        /**
         * 显示详情页，隐藏列表页
         * @param {string} act
         */
        private showDetail(act);
        displayNav(isShow: boolean): void;
        ToEdit(): void;
        ToView(): void;
        /**
         * 新增事件
         * 会触发after.Add事件，如需自定义请使用this.content.on("after.Add",function(){});
         * @returns
         */
        Add(): this;
        /**
         * 编辑事件
         * 最后会触发after.Edit事件，如需自定义请使用this.content.on("after.Edit",function(data,page,obj){});
         * @param {type} data 要编辑的数据对象
         * @param {type} page 分页控件
         * @param {type} obj 触发事件的jquery对象
         */
        EditAct(data: Object, page: Pagination, obj: JQuery): void;
        /**
        * 查看事件
        * 最后会触发after.View事件，如需自定义请使用this.content.on("after.View",function(data,page,obj){});
        * 暂时不提供对弹窗页面的ViewAct支持
        * @param {type} data
        * @param {type} page
        * @param {type} obj
        */
        ViewAct(data: Object, page: Pagination, obj: JQuery): void;
        /**
         * 保存事件
         * 保存前会触发PreSave函数，如果定义了则会在PreSave函数之中对数据进行修改
         * 保存成功会触发after.Save事件，如需自定义请使用this.content.on("after.Save",function(){});
         * @param {type} btn
         * @returns
         */
        Save(btn: JQuery): any;
        /**
         * 创建分页控件
         * @param {type} param
         */
        CreatePage(param: Sail.IPaginationSetting): Pagination;
        RegisterAct(body: JQuery, page: any): void;
        /**
         * 删除事件
         * 最后会触发after.Delete事件，如需自定义请使用this.content.on("after.Delete",function(data,page,obj){});
         * @param {type} data 要删除的对象
         * @param {type} page 分页控件
         * @param {type} obj 触发事件的jquery对象
         */
        DeleteAct(data: Object, page: Pagination, obj: JQuery): void;
        /**
         * 按钮操作事件
         * @param {Object} data
         * @param {Pagination} page
         * @param {JQuery} obj
         */
        BtnAct(data: Object, page: Pagination, obj: JQuery): void;
        /**
         * 保存前对数据进行处理
         * @param {type} form
         * @param {type} modal
         */
        PreSave(form: JQuery, model: Object): Object;
    }
}

declare module Sail {
    interface IEvent {
        handle: string;
        act: Function;
    }
    interface IPaginationSetting {
        isPaging?: boolean;
        bodyContainer?: any;
        getPostKey?: Function;
        pageSize?: number;
        handleName: any;
        headContainer?: any;
        footContainer?: any;
        ajaxType?: Function;
        bodyTmpl?: any;
        footTmpl?: any;
        queryed?: any;
        titles?: Array<ITHead>;
        titleWidth?: Array<number>;
        events?: Array<IEvent>;
        reQueryHandle?: string;
    }
    interface IPageSetting {
        first: string;
        last: string;
        next: string;
        pre: string;
    }
    var pageSetting: IPageSetting;
    var noDataTmpl: string;
    function noData(desc: JQuery): void;
    function setPageSetting(set: IPageSetting): void;
    class Pagination {
        constructor(set: IPaginationSetting);
        currentIndex: number;
        Container: JQuery;
        settings: IPaginationSetting;
        Query(pageIndex?: number): this;
    }
}

interface Number {
    /**
     * 从左补齐字符串宽度
     * @param {number} length 目标字符串宽度
     * @param {string} padStr 用来补齐的字符，默认为空格
     */
    PadLeft(length: number, padStr: string): string;
    /**
     * 从右补齐字符串宽度
     * @param {number} length 目标字符串宽度
     * @param {string} padStr 用来补齐的字符，默认为空格
     */
    PadRight(length: number, padStr: string): string;
}
declare module NumberHelper {
}
interface String {
    /**
     * 从左补齐字符串宽度
     * @param {number} length 目标字符串宽度
     * @param {string} padStr 用来补齐的字符，默认为空格
     */
    PadLeft(length: number, padStr: string): string;
    /**
     * 从右补齐字符串宽度
     * @param {number} length 目标字符串宽度
     * @param {string} padStr 用来补齐的字符，默认为空格
     */
    PadRight(length: number, padStr: string): string;
    /**
     * 格式化字符串，用法同c#
     * @param {type} ...args
     */
    format(...args: any[]): string;
    /**
     * 字符串转整数
     */
    ToInt(): Number;
    /**
     * 字符串转浮点
     */
    ToFloat(): Number;
    /**
     * 移除字符串中的html标签
     */
    RemoveHtmlTag(): string;
    /**
     * 根据字符数截断文字(汉字算2个字符),如果原字符串超长自动追加……
     * @param {type} len
     */
    SubStringByByte(len: number): string;
}
declare module StringHelper {
}

declare namespace Sail {
    /**
     * 数字以逗号分割形式显示
     * @param num
     */
    function formatNumber(num: any): string;
    function digitUppercase(n: number): string;
    /**
     * 获取预览图片地址
     * @param str
     * @param preType
     */
    function ToPre(str: string, preType: string): string;
}

interface JQueryStatic {
    IgnoreRequire(value?: any): any;
    MustRequire(value?: any): any;
}
interface JQuery {
    size(): any;
}
interface IValidationSet {
    error?: string;
    formGroup?: string;
    validationEventTriggers?: string;
    inlineValidation?: boolean;
    returnIsValid?: boolean;
    scroll?: boolean;
    unbindEngine?: boolean;
    ajaxSubmit?: boolean;
    success?: boolean;
    ignoreRequire?: boolean;
}
declare module validationEngine {
    var validationEngineLanguage: {
        allRules: {
            required: {
                regex: string;
                alertText: string;
                alertTextCheckboxMultiple: string;
                alertTextCheckboxe: string;
            };
            length: {
                regex: string;
                alertText: string;
                alertText2: string;
                alertText3: string;
            };
            confirm: {
                regex: string;
                alertText: string;
            };
            url: {
                regex: RegExp;
                alertText: string;
            };
            idNumber: {
                regex: RegExp;
                alertText: string;
            };
            mobile: {
                regex: RegExp;
                alertText: string;
            };
            email: {
                regex: RegExp;
                alertText: string;
            };
            integer: {
                regex: RegExp;
                alertText: string;
            };
            pinteger: {
                regex: RegExp;
                alertText: string;
            };
            pdecimal: {
                regex: RegExp;
                alertText: string;
            };
            decimal: {
                regex: RegExp;
                alertText: string;
            };
        };
    };
    var customSet: IValidationSet;
    /**
     * 配置默认设置
     * @param set
     */
    function SetConfig(set: any): void;
    var settings: IValidationSet;
}
