using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investment.Code.TagHelpers
{
    public static class TagHelperExtentions
    {
        /// <summary>
        /// 合并属性
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void MergeAttribute(this TagHelperAttributeList list, string name, string value)
        {
            list.TryGetAttribute(name, out var attr);
            list.SetAttribute(name, attr == null ? value : $"{attr.Value} {value}");
        }

        /// <summary>
        /// 合并属性
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void MergeAttribute(this TagHelperAttributeList list, string name, TagHelperAttribute value)
        {
            list.MergeAttribute(name, value.Value.ToString());
        }

        /// <summary>
        /// 移除属性
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        public static void RemoveAttribute(this TagHelperAttributeList list, string name)
        {
            var index = list.IndexOfName(name);
            if (index > -1)
                list.RemoveAt(index);
        }

    }
}
