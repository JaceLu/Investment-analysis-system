using Sail.Common;
using System;
using Lamtip;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Lamtip
{
    public class UrlParam : StringKeyPairs<object>
    {

        public UrlParam(Expression<Func<object>> expression, bool isToLower = true)
        {
            this.Merge(expression, isToLower);
        }

        public UrlParam(object item, bool isToLower = true)
        {
            this.Merge(item, isToLower);
        }

        public UrlParam(IDictionary<string, object> dict, bool isToLower = true)
        {
            this.Merge(dict, isToLower);
        }

        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { set; get; }


        public override string ToString()
        {
            return this.ToString("&");
        }

        
         


        public UrlParam Merge(Expression<Func<object>> expression, bool isToLower = false)
        {
            var item = expression.Compile().Invoke();
            var exp = expression.Body as NewExpression;
            exp.Members.ForEach((index, x) =>
            {
                var key = isToLower ? x.Name.ToLower() : x.Name;
                this[key] = item.GetValue(x.Name);
            });
            return this;
        }

        public UrlParam Merge(object item, bool isToLower = false)
        {
            if (item.IsNull()) return this;
            item.GetProperties().ForEach((index, x) =>
            {
                var key = isToLower ? x.Name.ToLower() : x.Name;
                this[key] = item.GetValue(x.Name);
            });
            return this;
        }

        public UrlParam Merge(IDictionary<string, object> dict, bool isToLower = false)
        {
            dict.ForEach((i, x) =>
            {
                var key = isToLower ? x.Key.ToLower() : x.Key;
                this[key] = x.Value;
            });
            return this;
        }
    }
}
