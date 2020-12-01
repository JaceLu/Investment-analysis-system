using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Sail.Common;
using Sail.Web;
using Investment.Code;
using Investment.Models;

namespace Investment
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IWebHostEnvironment Env { set; get; }
        private IConfiguration Configuration { get; }




        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {

            Configuration = configuration;
            Env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddWebOptimizer(pp => pp.RegisterMainJs().RegisterMainCss());


            //services.RegisterSailMvc(_env);

            //services.AddControllers().AddJsonOptions(delegate (JsonOptions x)
            //{
            //    x.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            //    x.JsonSerializerOptions.PropertyNamingPolicy = null;
            //});


            services.AddWebOptimizer(pp => pp.RegisterMainCss().RegisterMainJs());

            services.RegisterSailMvc(Env);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {

            app.UseWebOptimizer();
            app.UseSail();
            //app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
            });

            using var db = new DataContext();
            this.InitData(db);

        }

        private void InitData(DataContext db)
        {
            I18N.Init();
            var config = db.GetModel<Param>(Clip.Where<Param>(x => x.Id != ModelBase.NewId)) ?? new Param();
            Param.Config(config);
            if (!db.Any<Admin>())
            {
                var role = new List<Role>();
                var superAdminRole = new Role
                {
                    Id = ModelBase.DefaultId,
                    RoleName = "��������Ա",
                    Memo = ""
                };
                role.Add(superAdminRole);
                db.Insert(superAdminRole);

                var superAdmin = new Admin
                {
                    UserId = ModelBase.DefaultId,
                    UserName = "��������Ա",
                    LoginId = "admin",
                    Password = "admin".ToMd5().Encrypt(),
                    Role = superAdminRole,
#if DEBUG
                    IsAutoLogin = true
#endif
                };
                db.Insert(superAdmin);
            }
        }
    }
}
