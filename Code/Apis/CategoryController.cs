using System.Collections.Generic;
using Sail.Common;
using Sail.Web;
using Investment.Models;
using Microsoft.AspNetCore.Mvc;
using Investment.Code;
using System.Linq;
using Org.BouncyCastle.Math.EC.Rfc7748;
using NPOI.SS.Formula.Atp;

namespace Investment.Apis
{
    /// <summary>
    /// 类型管理控制器
    /// </summary>
    public class CategoryController : BaseController<Category>
    {
        /// <summary>
        /// 筛选查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip BaseWhere(string key)
        {
            return this.Where(x => x.Name.Like(key));
        }

        /// <summary>
        ///排序
        /// </summary>
        protected override Clip OrderBy => this.Order(x => x.OrderByNo.Asc()&&x.CreateTime.Asc());
        /// <summary>
        ///  
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="PageCategory"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetListInfo(int pageIndex, int pageSize, string key,int PageCategory)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = this.MakeWhere(key);
                where &= this.Where(x => x.Language == I18N.CurrentWEBLanguage&&x.ParentId=="");
                if (PageCategory>-1) where &= this.Where(x => x.PageCategory == (PageCategory)PageCategory);
                return db.GetPageList<Category>(pageIndex, pageSize, where, OrderBy);
            });
        }

        [HttpGet]
        public AjaxResult GetCatalog(string parentId,int category)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = Where(x => x.Language == I18N.CurrentWEBLanguage);
                where &= Where(x => x.ParentId == parentId);
                if(category.IsNotNull()) where &= Where(x => x.PageCategory == (PageCategory)category);
                var list = db.GetList<Category>(where, OrderBy);
                list.ForEach(x =>
                {
                    Clip itemWhere = Where(t => t.ParentId == x.Id);
                    x.SubItem = db.GetList<Category>(itemWhere, OrderBy).ToList();
                });
                return list;
            });
        }

        [HttpGet]
        public AjaxResult QueryData(string id)
        {
            return HandleHelper.TryAction(db =>
            {
                return db.GetModelById<Category>(id);
            });
        }

        [HttpPost]
        public AjaxResult DeleteCategory(string id)
        {
            return HandleHelper.TryAction(db =>
            {
                var news = db.GetList<News>(t => t.Category.Id == id);
                if (news.Any()) throw new SailCommonException("该类型有关联新闻数据，不可删除");
                db.DeleteById<Category>(id);
            });
        }
        /// <summary>
        /// 创建二级菜单
        /// </summary>
        [HttpPost]
        public AjaxResult CreateChild(string id,[FromForm]string value)
        {
            return HandleHelper.TryAction(db=> {
                var model = db.LoadModelByJson<Category>(value,id);
                if (model.IsNew()) { 
                    var parentCate = db.GetModelById<Category>(model.ParentId);
                    model.PageCategory = parentCate.PageCategory;
                    model.ParentName = parentCate.Name;
                    model.Language = I18N.CurrentWEBLanguage;
                }
                db.Save(model);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="model"></param>
        protected override void BeforeDelete(DataContext db, Category model)
        {
            switch (model.PageCategory)
            {
                case PageCategory.News:
                    if(db.GetCount<News>(x => x.Language == I18N.CurrentWEBLanguage && x.Category.ParentId == model.Id) > 0) 
                        throw new SailCommonException("该类型子节点有关联新闻数据，不可删除");
                    break;
                case PageCategory.Navigation:
                    if (db.GetCount<News>(x => x.Language == I18N.CurrentWEBLanguage && x.Category.Id == model.Id) > 0)
                        throw new SailCommonException("该类型有关联展馆介绍数据，不可删除");
                    break;
                case PageCategory.Schedule:
                    if (db.GetCount<PavilionSchedule>(x => x.Language == I18N.CurrentWEBLanguage) > 0) 
                        throw new SailCommonException("该类型有关联展馆日程数据，不可删除");
                    break;
            }
        }
    }
}
