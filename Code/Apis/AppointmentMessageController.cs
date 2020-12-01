using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investment.Code;
using Investment.Models;
using Microsoft.AspNetCore.Mvc;
using Sail.Common;
using Sail.Web;

namespace Investment.Apis
{
    /// <summary>
    /// 预约留言控制器
    /// </summary>
    public class AppointmentMessageController : BaseController<AppointmentMessage>
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
        protected override Clip OrderBy => this.Order(x => x.OrderByNo.Asc());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetListInfo(int pageIndex, int pageSize, string key, int type,string dayStart, string dayEnd)
        {
            return HandleHelper.TryAction(db =>
            {
                return QueryInfo(db, pageIndex, pageSize, key, dayStart, dayEnd, type);
            });
        }

        private PageResult QueryInfo(DataContext db, int pageIndex, int pageSize, string key, string dayStart, string dayEnd, int type)
        {
            Clip where = this.MakeWhere(key);
            where &= this.Where(x => x.Language == I18N.CurrentWEBLanguage);
            if (type > -1) where &= this.Where(x => x.Type == (AppointmentType)type);
            var startDay = dayStart.ToDate().Date;
            var endDay = dayEnd.ToDate().Date;
            if (startDay > endDay)
            {
                throw new SailCommonException("开始不能晚于结束!");
            }

            where &= nameof(AppointmentMessage.StartTime).DateBetween(dayStart, dayEnd);
            return db.GetPageList<AppointmentMessage>(pageIndex, pageSize, where, OrderBy);
        }

        public FileContentResult Export(string key, string dayStart, string dayEnd, int type)
        {
            Func<int, int, DataContext, PageResult> act = (pageIndex, pageSize, db) =>
            {
                return QueryInfo(db, pageIndex, pageSize, key, dayStart, dayEnd, type);
            };
            var title = "开始时间";
            //switch (dateProp) {
            //    case "StartTime":
            //        title = "开始时间";
            //        break;
            //    case "EndTime":
            //        title = "结束时间";
            //        break;
            //}
            title += $"({dayStart}~{dayEnd})";
            var res = ExportHelper.Export<AppointmentMessage>($"{(AppointmentType)type}_{DateTime.Now.ToDateString()}" ,
                $"{(AppointmentType)type} {title}", act, model => new
            {
                姓名 = model.Name,
                电话 = model.Tel,
                场馆 = model.PavilionName,
                开始时间 = model.StartTime.ToDateString(),
                结束时间 = model.EndTime.ToDateString(),
                留言 = model.Message,
            });
            return File(res.content.ToBytes(Encoding.UTF8), "application/octet-stream", res.name);
        }
    }
}
