using Investment.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sail.Common;
using Sail.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Layouts
{
    public class UserMenu
    {
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<HMenuItem> Menus { get; private set; } = new List<HMenuItem>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="formatUrl"></param>
        /// <returns></returns>
        public List<HMenuItem> LoadAdminMenu(string fileName = "menu", Func<string, string> formatUrl = null)
        {
            var user = WebHelper.CurrentAdmin as Admin;
            Menus = LoadMenu(fileName, formatUrl);
            HMenuItem.Filter(Menus, "", user);
            return Menus.OrderBy(x => x.ItemName).ToList();
        }

        public List<HMenuItem> LoadMenu(IUser user, string fileName = "menu", Func<List<HMenuItem>, List<HMenuItem>> func = null)
        {
            Menus = func == null ? LoadMenu(fileName) : func.Invoke(LoadMenu(fileName));
            if (user.IsNotNull()) HMenuItem.Filter(Menus, "", user);
            return Menus.OrderBy(x => x.ItemName).ToList();
        }

        private static List<HMenuItem> LoadMenu(string filename, Func<string, string> formatUrl = null)
        {
            if (formatUrl.IsNull()) formatUrl = url => $"{url}";
            var menutext = FileHelper.ReadTextFile($"~/config/{filename}.json".FullFileName());
            var list = menutext.JsonToObj<List<HMenuItem>>().ToList();
            list.ForEach(x =>
            {
                x.Url = formatUrl(x.Url);
                x.SubItems.ForEach(c =>
                {
                    c.Url = formatUrl(c.Url);
                    c.SubItems.ForEach(t =>
                    {
                        t.Url = formatUrl(t.Url);
                    });
                });
            });
            return list;
        }
    }
}
