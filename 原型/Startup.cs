using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Sail.Common;
using Lamtip.Model;
using Sail.Web;
using System.Data;
using System.Diagnostics;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace Lamtip
{
    public class Startup
    {
        public IHostingEnvironment _env { set; get; }
        public Startup(IHostingEnvironment env) {
            _env = env;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterSailMvc(_env);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSail();
            app.UseStaticFiles();
            using (var db = new DataContext())
            {
                this.InitData(db);
               
            }
        }
        private void InitData(DataContext db)
        {
            Param.Config(Param.Get(db));
            if (!db.Any<User>())
            {
                var adminRole = new UserRole { Id = ModelBase.DefaultId, Powers = new List<Power>(), RoleName = "超级管理员" };
                db.Insert(adminRole);
                var admin = new User
                {
                    UserId = ModelBase.DefaultId,
                    UserName = "超级管理员",
                    LoginId = "admin",
                    Password = "admin".ToMd5().Encrypt(),
                    Role = adminRole,
#if DEBUG
                    IsAutoLogin=true
#endif
                };
                //db.Insert(admin);
            }
        }

        public static IEnumerable<DataRow> ReadExcel(string filePath, int sheetIndex = 0)
        {
            IWorkbook wk = null;
            var extension = Path.GetExtension(filePath);
            try
            {
                using (var fs = File.OpenRead(filePath))
                {
                    if (extension.Equals(".xls"))
                    {
                        //把xls文件中的数据写入wk中
                        wk = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        //把xlsx文件中的数据写入wk中
                        wk = new XSSFWorkbook(fs);
                    }
                }
                var sheet = wk.GetSheetAt(0);

                var dt = new DataTable(sheet.SheetName);
                // write header row
                var headerRow = sheet.GetRow(sheetIndex);
                foreach (var headerCell in headerRow)
                {
                    dt.Columns.Add(headerCell.ToString());
                }
                // write the rest
                var rowIndex = 0;
                foreach (IRow row in sheet)
                {
                    if (rowIndex++ == 0) continue;
                    var dataRow = dt.NewRow();
                    dataRow.ItemArray = Enumerable.Range(0, headerRow.Count()).Select(i => row.GetCell(i)?.ToString() ?? "").ToArray();
                    dt.Rows.Add(dataRow);
                }
                return dt.Rows.Cast<DataRow>();
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }
    }
}
