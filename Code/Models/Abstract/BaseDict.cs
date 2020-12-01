using Sail.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investment.Models.Abstract
{
    public class BaseDict : SortableInfo, IModel, IDict
    {
        /// <summary>
        /// 名称
        /// </summary>
        [HColumn(Length = 200, IsNullable = false, IsUniqueness = true)]
        [Form(IsRequired = true)]
        public string Name { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        [HColumn]
        public InfoStatus Status { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        public string StatusStr => Status.ToString();


        /// <summary>
        /// 转成键值对
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, string> ToKvPair()
        {
            return new KeyValuePair<string, string>(this.Id, this.Name);
        }

        /// <summary>
        /// 获取所有字典
        /// </summary>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> GetDict<T>() where T : BaseDict
        {
            using var db = new DataContext();
            return db.GetList<T>(x => x.Status == InfoStatus.正常, x => x.OrderByNo.Asc() && x.CreateTime.Desc())
                        .Select(x => x.ToKvPair())
                        .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var o = obj as BaseDict;
            if (o == null)
                return false;
            return this.Id?.Equals(o.Id) ?? false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name?.ToString();
        }
    }
}
