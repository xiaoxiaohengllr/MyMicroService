using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net;

namespace MyMicroService.Gateway.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
