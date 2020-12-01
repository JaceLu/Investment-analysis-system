using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sail.Common;

namespace Investment
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseConfiguration(config);
                //var port = config.GetValue<int>("port");
                //if (port == 0) port = 5000;
                //webBuilder.UseUrls($"http://0.0.0.0:{port}");

                webBuilder.ConfigureLogging(b => b.AddFile()).UseStartup<Startup>();
            });

        }

    }
}
