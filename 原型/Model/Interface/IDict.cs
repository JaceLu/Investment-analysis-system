using System.Collections.Generic;

namespace Lamtip.Model
{
    public interface IDict
    {
        string Id { set; get; }
        string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        KeyValuePair<string, string> ToKvPair();
    }
}