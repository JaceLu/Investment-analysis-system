using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sail.Web;
using Investment.Models;

namespace Investment.Views.Shared.Components.Layouts1
{
    public enum LayoutsType
    {
        Header,
        MainMenu,
        Footer
    }

    public class Layouts1ViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(LayoutBase model, LayoutsType type, string userIcon)
        {
            ViewData["UserIcon"] = userIcon ?? "/Content/images/default.jpg";
            ViewData["IsImport"] = false;

            return View(type.ToString(), model);
        }
    }
}
