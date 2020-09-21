using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyMicroService.Infrastruct.ILog;
using MyMicroService.Infrastruct.Log;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace MyMicroService.Gateway.ApiGateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.         
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot(new ConfigurationBuilder().AddJsonFile("ocelot.json", true, true).Build()) //注册对ocelot的支持,引入ocelot配置文件
                    .AddConsul()//注册对Consul的支持
                    .AddPolly();//注册对Polly的支持
            services.AddControllers();

            services.AddLog(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseLog();

            app.UseHttpsRedirection();

            app.UseOcelot().Wait();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
