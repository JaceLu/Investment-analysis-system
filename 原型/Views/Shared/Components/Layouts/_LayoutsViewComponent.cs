using Microsoft.AspNetCore.Mvc;
using Lamtip.Model;
using Sail.Web;
using System.Threading.Tasks;

namespace Lamtip
{
    public enum LayoutsType
    {
        MainMenu,
        Header,
        Footer
    }
    public class LayoutsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(LayoutBase model, LayoutsType type, string userIcon)
        {
            ViewData["SiteName"] = Param.Default.SiteName;
            ViewData["UserIcon"] = userIcon ?? "/Content/images/default.png";
            ViewData["IsImport"] = false;
            return View(type.ToString(), model);
        }


    }
}