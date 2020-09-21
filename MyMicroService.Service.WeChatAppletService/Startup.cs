using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyMicroService.Infrastruct.Consul;
using MyMicroService.Infrastruct.Filters;
using MyMicroService.Infrastruct.Log;
using MyMicroService.Infrastruct.Swagger;
using System;

namespace MyMicroService.Service.WeChatAppletService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container. 
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddControllersAsServices();

            services.AddLog();//�����־

            services.AddSwagger("MyMicroService.Service.WeChatAppletService", "��ʱ���ȷ���", "MyMicroService.Service.WeChatAppletService.xml");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseConsul(Guid.NewGuid().ToString(), "WeChatAppletService");

            app.UseLog();

            app.UseRouting();

            app.UseAuthorization();

            //��ȡ�ͻ���IP��ַʹ��
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
