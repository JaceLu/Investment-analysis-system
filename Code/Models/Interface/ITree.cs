using Sail.Common;
using System;
using System.Collections.Generic;

namespace Investment.Models.Interface
{
    public interface ITree : IModel
    {
        /// <summary>
        /// 
        /// </summary>
        //string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        decimal OrderByNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}
