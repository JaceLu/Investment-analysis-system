using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investment.Models.Abstract
{
    public interface IDict
    {
        string Id { set; get; }
        string Name { set; get; }

        KeyValuePair<string, string> ToKvPair();
    }
}
