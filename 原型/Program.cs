using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Sail.Common;

namespace Lamtip
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }


        public static IWebHost BuildWebHost(string[] args)
        {
            var webBuilder = WebHost.CreateDefaultBuilder(args).UseStartup<Startup>()
                .ConfigureLogging(builder => builder.AddFile())
                ;
            var port = webBuilder.GetSetting("port");
            if (!port.IsNull()) webBuilder.UseUrls($"http://localhost:{port}");
            return webBuilder
                .UseIISIntegration()
                .Build();
        }
    }
}
