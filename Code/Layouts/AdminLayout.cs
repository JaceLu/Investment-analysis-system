using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Investment.Models;
using Sail.Common;
using Sail.Web;

namespace Investment.Layouts
{
    /// <summary>
    /// 用户模板页
    /// </summary>
    public class AdminLayout : LayoutBase<Admin>
    {
        public bool IsNeedLogin { private set; get; }

        public override Admin CurrentUser { get => WebHelper.CurrentAdmin as Admin; set => WebHelper.CurrentAdmin = value; }

        protected override void BeforeCheckLogin(DataContext db)
        {
#if DEBUG
            if (this.CurrentUser.IsNull())
            {
                this.CurrentUser = db.GetModel<Admin>(Clip.Where<Admin>(x => x.IsAutoLogin == true));
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public override string LoginUrl => "/Admin/Login";

        /// <summary>
        /// 
        /// </summary>
        public override string IndexUrl => "/Admin/";

        private UserMenu _menu;
        /// <summary>
        /// 
        /// </summary>
        public override IList<HMenuItem> Menus => _menu?.Menus;


        /// <summary>
        /// 加载菜单
        /// </summary>
        public override void LoadMenu()
        {
            _menu = new UserMenu();
            _menu.LoadMenu(this.CurrentUser);
        }
    }
}
