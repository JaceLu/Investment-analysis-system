using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sail.Web;
using Sail.Common;
using Investment.Models;
using Investment.Code;

namespace Investment.Layouts
{
    public class AdminPage<T> : PageType<T, Admin> where T : IModel
    {
        public override Admin User => WebHelper.CurrentAdmin as Admin;
        public AdminPage(PageBase page) : base(page)
        {
            //this.Setting = FormSettings.Bootstrap;
        }

        public AdminPage(ViewContext page) : base(page)
        {
            //this.Setting = FormSettings.Bootstrap;
        }
        
        /// <summary>
        /// 是否有新增权限
        /// </summary>
        public bool CanOperation(string parentItemId, OperationWay type = OperationWay.新增)
        {
            string powerId = type.GetEnumDesc("powerId");
            return (this?.User?.IsSuperAdmin ?? false) || Extension.IsHasSubPower(User, parentItemId + powerId);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum OperationWay
    {
        [EnumDesc("powerId", "7137c81c5fe34158b392883abfe98f58")]
        新增,
        [EnumDesc("powerId", "67186728efda448ab2e1cd1ccb1c6f21")]
        编辑,
        [EnumDesc("powerId", "c4637e785e0e412e85445afa29289750")]
        删除,
        [EnumDesc("powerId", "388f6c720d504f33b697ca3021f1fb3f")]
        导出,
        [EnumDesc("powerId", "51db8534c2a24f27ac8d219c07535a56")]
        其他
    }
}
