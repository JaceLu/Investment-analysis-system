using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sail.Common;
using Investment.Models.Interface;

namespace Investment.Models
{
    /// <summary>
    /// 树的帮助类
    /// </summary>
    public static class TreeHelper
    {
        /// <summary>
        /// 根据父节点读取子节点
        /// </summary>
        /// <typeparam name="Tree"></typeparam>
        /// <param name="db"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static IList<Tree> GetNodes<Tree>(this DataContext db, string parentId) where Tree : ITree
        {
            if (parentId.IsNull()) parentId = "";
            var where = Clip.Where<Tree>(x => x.ParentId == parentId);
            var orderBy = Clip.OrderBy<Tree>(x => x.OrderByNo.Asc() && x.CreateTime.Desc());
            return db.GetList<Tree>(where, orderBy);
        }
    }
}
