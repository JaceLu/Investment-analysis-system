using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lamtip.Model;
using Sail.Common;
using Sail.Web;

namespace Lamtip.Code
{
    public class UserPage<T> : PageType<T, User> where T : IModel
    {
        public override User User => WebHelper.CurrentUser as User;

        public UserPage( PageBase page) : base(page)
        {
            //this.Setting = FormSettings.Bootstrap;
        }

        public UserPage( ViewContext page) : base(page)
        {

            //this.Setting = FormSettings.Bootstrap;
        }
    }
}