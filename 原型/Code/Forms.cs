using Sail.Common;
using Sail.Web;
using System.Collections.Generic;
using System.Linq;

namespace Damlux
{
    public static class Forms
    {
        public static FormSettings Cell
        {
            get
            {
                var form = new FormSettings();
                new Dictionary<FormElements, string>() {
                    {FormElements.Group,"cell"},
                    {FormElements.Label,"cell__hd"},
                    {FormElements.Element,"cell__bd"},
                    {FormElements.Controller,"input"},
                    {FormElements.View,""},
                    {FormElements.Tips,"tips"},
                    {FormElements.Required,"required"},
                    {FormElements.RequiredTag,"span"},
                    {FormElements.Textarea,"input"}
                }.ForEach(x => form.Class[x.Key.ToString()] = x.Value);
                return form;
            }
        }
    }
}
