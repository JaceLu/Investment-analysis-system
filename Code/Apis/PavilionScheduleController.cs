using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Investment.Code;
using Investment.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Sail.Common;
using Sail.Web;

namespace Investment.Apis
{
    /// <summary>
    /// 展馆日程控制器
    /// </summary>
    public class PavilionScheduleController : BaseController<PavilionSchedule>
    {

        /// <summary>
        /// 筛选查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override Clip BaseWhere(string key)
        {
            return this.Where(x => x.ExhibitionName.Like(key));
        }

        /// <summary>
        ///排序
        /// </summary>
        protected override Clip OrderBy => this.Order(x => x.StartTime.Desc());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="pavilionId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetListInfo(int pageIndex, int pageSize, string key, string pavilionId)
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = this.MakeWhere(key);
                where &= this.Where(x => x.Language == I18N.CurrentWEBLanguage);
                if (pavilionId.IsNotNull())
                {
                    where &= this.Where(x => x.Pavilion.Like(pavilionId));
                }
                return db.GetPageList<PavilionSchedule>(pageIndex, pageSize, where, OrderBy);
            });
        }

        /// <summary>
        /// 前台获取数据（根据语言）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetPavilionSchedule(int year, int month)
        {
            return HandleHelper.TryAction(db =>
            {
                int day = 1;
                Clip where = this.Where(x => x.Language == I18N.CurrentLanguage);
                DateTime StartDate = new DateTime(year, month, day);
                var EndDate = StartDate.AddMonths(1).AddMinutes(-1);
                where &= nameof(PavilionSchedule.StartTime).DateBetween(StartDate, EndDate);
                var OrderBy = Clip.OrderBy<PavilionSchedule>(x => x.StartTime.Desc());
                return db.GetList<PavilionSchedule>(where, OrderBy);
            });
        }

        /// <summary>
        /// 前台获取数据（根据语言）(浮窗)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetFloatingWindow()
        {
            return HandleHelper.TryAction(db =>
            {
                Clip where = this.Where(x => x.Language == I18N.CurrentLanguage);
                var OrderBy = Clip.OrderBy<PavilionSchedule>(x => x.StartTime.Asc());
                return db.GetList<PavilionSchedule>(where, OrderBy);
            });
        }

        /// <summary>
        /// 前台获取数据（根据语言）(展会日程详情)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetSchedule(string id)
        {
            return HandleHelper.TryAction(db =>
            {
                return db.GetModel<PavilionSchedule>(x => x.Id == id && x.Language == I18N.CurrentLanguage);
            });
        }

        /// <summary>
        /// 前台获取数据（根据语言）(展会日程主页展示信息)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetScheduleList()
        {
            return HandleHelper.TryAction(db =>
            {
                var today = DateTime.Now; 
                Clip where = this.Where(x => x.Language == I18N.CurrentLanguage);
                where &= this.Where(x => x.StartTime > today);
                var OrderBy = Clip.OrderBy<PavilionSchedule>(x => x.StartTime.Asc());
                return db.GetList<PavilionSchedule>(where, OrderBy).Take(3);
            });
        }

        //[HttpPut]
        //public override AjaxResult Save(string id, [FromForm] string value)
        //{
        //    return HandleHelper.TryAction(db =>
        //    {
        //        var model = LoadModelByJson<PavilionSchedule>(db,value, id);
                
        //        db.Save(model);
        //        return model;
        //    });
        //}
    }
}

