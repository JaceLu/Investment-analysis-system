using Lamtip.Model;
using Sail.Common;
using Sail.Web;
using System.Collections.Generic;
using System;

namespace Lamtip.Code
{
    /// <summary>
    /// 用户模板页
    /// </summary>
    public class UserLayout : LayoutBase<User>
    {
        public bool IsNeedLogin { private set; get; }

        public override User CurrentUser { get => WebHelper.CurrentUser as User; set => WebHelper.CurrentUser = value; }


        protected override void beforeCheckLogin(DataContext db)
        {
#if DEBUG
            if (this.CurrentUser.IsNull())
            {
                this.CurrentUser = db.GetModel<User>(Clip.Where<User>(x => x.IsAutoLogin == true));
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public override string LoginUrl => "/Login";

        /// <summary>
        /// 
        /// </summary>
        public override string IndexUrl => "/";

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

        /// <summary>
        /// 登陆后跳转路径
        /// </summary>
        public static string Referer { set; get; } = "/index";
    }
}