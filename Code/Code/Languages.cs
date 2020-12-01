using Sail.Common;
using Sail.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Investment.Code
{
    public enum Languages
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumDesc("Code", "cn")]
        CHINESE_CN,

        /// <summary>
        /// 
        /// </summary>
        [EnumDesc("Code", "en")]
        ENGLISH,
    }

    public class I18N
    {
        class I18NMessage
        {
            public string Key { get; set; }

            public Dictionary<Languages, string> Languages { set; get; } = new Dictionary<Languages, string>();
            public I18NMessage()
            {

            }

            public I18NMessage(string key)
            {
                this.Key = key;

            }

            public I18NMessage(string key, Dictionary<Languages, string> langs) : this(key)
            {
                if (langs.IsNotNull()) this.Languages.AddRangeUnique(langs);
            }
        }
      
        /// <summary>
        /// 语言包
        /// </summary>
        public static Dictionary<string, Dictionary<Languages, string>> CurrentPacket { private set; get; } = new Dictionary<string, Dictionary<Languages, string>>();
        /// <summary>
        /// 是否中文幻境
        /// </summary>
        public static bool IsCN { get { return CurrentLanguage == Languages.CHINESE_CN; } }
        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetText(string key)
        {
            var isGetted = CurrentPacket.TryGetValue(key, out var msg);
            if (msg.IsNull()) return "Illegal key";

            return msg[CurrentLanguage];
        }

        /// <summary>
        /// 网站当前语言
        /// </summary>
        public static Languages CurrentLanguage
        {
            get
            {
                var obj = WebHelper.Session.Session["CurrentLang"];
                if (obj.IsNull()) return Languages.CHINESE_CN;
                return (Languages)obj;
            }
            set
            {
                WebHelper.Session.Session["CurrentLang"] = value;
            }
        }
        public static bool WEBPAGEIsCN { get { return CurrentLanguage == Languages.CHINESE_CN; } }

        /// <summary>
        /// 管理后台当前语言
        /// </summary>
        public static Languages CurrentWEBLanguage
        {
            get
            {
                var obj = WebHelper.Session.Session["WEBCurrentLang"];
                if (obj.IsNull()) return Languages.CHINESE_CN;
                return (Languages)obj;
            }
            set
            {
                WebHelper.Session.Session["WEBCurrentLang"] = value;
            }
        }
        public static bool WEBIsCN { get { return CurrentWEBLanguage == Languages.CHINESE_CN; } }
        public static string DateLan { get {
                return IsCN ? "zh-cn" : "en";
        } }

        public static void SetCurrentLang(Languages lang)
        {
            WebHelper.Session.Session["CurrentLang"] = lang;
        }
        public static void SetCurrentWEBLang(Languages lang)
        {
            WebHelper.Session.Session["WEBCurrentLang"] = lang;
        }
        public static void Init()
        {
            ConvertToMessage("~/config/lang.xlsx".FullFileName().ReadExcel());
        }

        /// <summary>
        /// convert dataRow to dictionary
        /// </summary>
        /// <param name="dataRows"></param>
        /// <returns></returns>
        private static bool ConvertToMessage(IEnumerable<DataRow> dataRows)
        {
            if (dataRows.IsNullOrEmpty())
            {
                return false;
            }

            var headers = GetLanguagesesByColumns(dataRows.First().Table.Columns);

            var index = 0;
            dataRows.Select(x => x.ItemArray.Cast<string>().ToList()).ForEach(x =>
            {
                if (!x.IsNullOrEmpty())
                {
                    var key = x[0];
                    CurrentPacket.AddUnique(KeyValuePair.Create(key, ConvertRawToLanguageMap(headers, x)));
                }
                index++;
            });
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static List<Languages> GetLanguagesesByColumns(DataColumnCollection column)
        {
            var languageEnums = Enum
                .GetValues(typeof(Languages))
                .Cast<Languages>()
                .ToDictionaryWith(x => KeyValuePair.Create(x.GetEnumDesc("Code") ?? "", x));
            return column.GetHeader<string>().Where(x => languageEnums.ContainsKey(x)).Select(x => languageEnums[x])
                .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languages"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Dictionary<Languages, string> ConvertRawToLanguageMap(List<Languages> languages,
            List<string> row)
        {
            var result = new Dictionary<Languages, string>();
            for (int i = 1; i < row.Count; i++)
            {
                result.Add(languages[i - 1], row[i]);
            }
            return result;
        }

    }


}
