using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sail.Common;
using Sail.Web;
using Investment.Models;
using Microsoft.AspNetCore.Mvc;
using Investment.Code;
using Org.BouncyCastle.Math.EC.Rfc7748;
using NPOI.SS.Formula.Atp;
using NPOI.SS.Formula.Functions;

namespace Investment.Apis
{
    /// <summary>
    /// 新闻资讯
    /// </summary>
    public class NewsController : BaseController<News>
    {
        /// <summary>
        /// 条件搜索
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip BaseWhere(string key)
        {
            return this.Where(x => x.Title.Like(key));
        }

        /// <summary>
        /// 排序
        /// </summary>
        protected override Clip OrderBy => Clip.OrderBy<News>(x => x.PublishTime.Desc() && x.CreateTime.Desc());

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="categoryId"></param>
        /// <param name="type"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetNewsList(int pageIndex, int pageSize, string key, string categoryId, int type, string startDay, string endDay)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip OrderBy = null;
                var where = this.MakeWhere(key);
                where &= Where(x => x.Language == I18N.CurrentWEBLanguage);
                where &= Where(x => x.Category.Id == categoryId);
                where &= nameof(News.PublishTime).DateBetween(startDay, endDay);
                if (type == 1)
                {
                    OrderBy = Clip.OrderBy<News>(x => x.OrderByNo.Asc() && x.CreateTime.Desc());
                }
                else
                {
                    OrderBy = Clip.OrderBy<News>(x => x.PublishTime.Desc() && x.CreateTime.Desc());
                }
                return this.GetPageList(db, pageIndex, pageSize, where, OrderBy);
            });
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        //[HttpGet]
        //public AjaxResult Update()
        //{
        //    return HandleHelper.TryAction(db =>
        //    {
        //        db.RunTran(p => { 
        //            var category = db.GetList<Category>(x => x.Language ==Languages.CHINESE_CN&&x.ParentId=="");
        //            category.ForEach(t => {
        //                var k = t.CopyTo<Category>();
        //                k.Id = null;
        //                k.Code = null;
        //                k.Language = Languages.ENGLISH;
        //                db.Save(k);
        //                switch (t.PageCategory) {
        //                    case PageCategory.Navigation:
        //                        var navs = db.GetList<News>(c => c.Category.Id==t.Id);
        //                        navs.ForEach(x => {
        //                            x.Id = null;
        //                            x.Language = Languages.ENGLISH;
        //                            x.Category = k;
        //                            db.Save(x);
        //                        });
        //                        break;
        //                    case PageCategory.News:
        //                        var categorys = db.GetList<Category>(x => x.Language == Languages.CHINESE_CN&&x.ParentId==t.Id);
        //                        categorys.ForEach(l => { 
        //                            var item = l.CopyTo<Category>();
        //                            item.Id = null;
        //                            item.Code = null;
        //                            item.ParentId = db.GetModel<Category>(c => c.Name == k.Name).Id;
        //                            item.Language = Languages.ENGLISH;
        //                            db.Save(item);
        //                            var navs = db.GetList<News>(c => c.Category.Id == l.Id);
        //                            navs.ForEach(x => {
        //                                x.Id = null;
        //                                x.Category = item;
        //                                x.Language = Languages.ENGLISH;
        //                                db.Save(x);
        //                            });
        //                        });
                           
        //                        break;
        //                }
        //            });
        //            var dict = db.GetList<Dict>(y=>y.Type== DictType.展馆 &&y.Language == Languages.CHINESE_CN);
        //            dict.ForEach(t=> {
        //                t.Id = null;
        //                t.Language = Languages.ENGLISH;
        //                db.Save(t);
        //            });
        //            var schedule = db.GetList<PavilionSchedule>();
        //            schedule.ForEach(t => {
        //                t.Id = null;
        //                t.Pavilion = db.GetModel<Dict>(c=>c.Name==t.PavilionName && c.Language == Languages.ENGLISH);
        //                t.Language = Languages.ENGLISH;
        //                db.Save(t);
        //            });
        //        });
        //    });
        //}
        
        /// <summary>
        /// 前台获取数据（根据语言）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetService(string category)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = this.Where(x => x.Category.Id == category && x.ImageUrl != "");
                var OrderBy = Clip.OrderBy<News>(x => x.OrderByNo.Asc() && x.CreateTime.Desc());
                return db.GetList<News>(where, OrderBy).Take(3);
            });
        }

        /// <summary>
        /// 前台获取数据（根据语言）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetNews(string category)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = this.Where(x => x.Category.ParentId == category);
                var OrderBy = Clip.OrderBy<News>(x => x.PublishTime.Desc());
                var newsList = db.GetList<News>(where, OrderBy);
                where &= Where(x => x.ImageUrl != "");
                var newsPictureList = db.GetList<News>(where, OrderBy).Take(3);
                return new {
                    newsList,
                    newsPictureList
                };
            });
        }

        /// <summary>
        /// PC网站获取数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="category"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetNewsData(int pageIndex, string category, int pageSize)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = this.Where(t=>t.Language==I18N.CurrentLanguage);
                if (category.IsNotNull())
                {
                    where &= Where(x => x.Category.Id == category);
                }
                return this.GetPageList(db, pageIndex, pageSize, where, this.OrderBy);
            });
        }
    }
}
