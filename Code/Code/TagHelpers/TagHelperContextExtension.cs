using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investment.Code.TagHelpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class TagHelperContextExtension
    {
        /// <summary>
        /// 是否含指定有属性
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasAttr(this TagHelperContext context, string name)
        {
            return context.AllAttributes.ContainsName(name);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class TagHelperOutputExtension
    {
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="output"></param>
        /// <param name="attributes"></param>
        public static void SetAttributes(this TagHelperOutput output, IDictionary<string, string> attributes)
        {
            foreach (var attr in attributes)
            {
                output.Attributes.SetAttribute(attr.Key, attr.Value);
            }
        }
    }
}
