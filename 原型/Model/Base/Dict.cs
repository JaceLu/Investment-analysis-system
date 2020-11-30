using System.Linq;
using Sail.Common;
using System.Collections.Generic;

namespace Lamtip.Model
{

    /// <summary>
    /// 字典数据
    /// </summary>
    public class Dict : BaseDict
    {
        /// <summary>
        /// 转视图
        /// </summary>
        /// <returns></returns>
        public DictView ToView() => new DictView { Id = Id, Name = Name };
         
    }
}