using Sail.Common;
using System;
using System.Collections.Generic;

namespace Lamtip.Code
{
    /// <summary>
    /// Model属性变动记录
    /// </summary>
    public abstract class ModelHistoryBase : ModelBase, IModel
    {
        /// <summary>
        /// 
        /// </summary>
        [HColumn]
        public List<PropHistory> Details { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [HColumn]
        public string Creater { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [HColumn]
        public DateTime CreateTime { set; get; }
    }
}
