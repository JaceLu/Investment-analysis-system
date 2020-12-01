using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Investment.Code;
using Investment.Models.Abstract;
using Sail.Common;

namespace Investment.Models
{
    /// <summary>
    /// 字典数据
    /// </summary>
    public class Dict : BaseDict, IMultiLanguages
    {
        /// <summary>
        /// 字典类型
        /// </summary>
        [HColumn(IsNotNeedUniqueness = true)]
        public DictType Type { set; get; }
        /// <summary>
        /// 语言
        /// </summary>
        [HColumn(IsNotNeedUniqueness = true)]
        public Languages Language { get; set; }

        /// <summary>
        /// 获取所有字典
        /// </summary>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> GetDicts(DictType DictType)
        {
            using var db = new DataContext();
            return db
            .GetList<Dict>(x => x.Status == InfoStatus.正常 && x.Type == DictType&&x.Language==I18N.CurrentWEBLanguage, x => x.OrderByNo.Asc())
            .Select(x => x.ToKvPair())
            .ToList();
        }

        /// <summary>
        /// 获取所有字典
        /// </summary>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetDictsbyLan(DictType DictType)
        {
            using var db = new DataContext();
            return db
            .GetList<Dict>(x => x.Status == InfoStatus.正常 && x.Type == DictType && x.Language == I18N.CurrentLanguage, x =>x.OrderByNo.Asc()&& x.CreateTime.Desc())
            .Select(x => x.ToKvPair())
            .ToList().Prepend(new KeyValuePair<string, string>("", I18N.GetText("Select"))).ToList();
        }
        /// <summary>
        /// 获取所有字典
        /// </summary>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> GetAllDict(DictType DictType)
        {
            using var db = new DataContext();
            return db
            .GetList<Dict>(x => x.Type == DictType && x.Language == I18N.CurrentWEBLanguage, x => x.CreateTime.Desc())
            .Select(x => x.ToKvPair())
            .ToList();
        }
    }

    public enum DictType
    {
        展馆
    }
}
