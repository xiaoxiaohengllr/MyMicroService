using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyMicroService.Infrastruct.BaseService;
using MyMicroService.Infrastruct.Consul;
using MyMicroService.Infrastruct.Filters;
using MyMicroService.Infrastruct.HttpWeb;
using MyMicroService.Infrastruct.ILog;
using MyMicroService.Infrastruct.Log;
using MyMicroService.Infrastruct.Swagger;
using MyMicroService.Service.TimingSchedulingService.Interceptor;
using MyMicroService.Service.TimingSchedulingService.IService;
using MyMicroService.Service.TimingSchedulingService.Quartz;
using MyMicroService.Service.TimingSchedulingService.Service;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Linq;

namespace MyMicroService.Service.TimingSchedulingService
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 容器服务
        /// </summary>
        public static ILifetimeScope lifetimeScope { get; set; }

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddControllersAsServices();

            services.AddSwagger("MyMicroService.Service.TimingSchedulingService", "定时调度服务", "MyMicroService.Service.TimingSchedulingService.xml");

            services.AddBaseService(this.GetType());//服务基类

            services.AddHttpWebHelper();//添加HttpWeb帮助类

            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();//注册ISchedulerFactory的实例

            services.AddLog();//添加日志

            services.AddHttpClient();//注入HttpClient工厂，用于解决HttpClient在高并发情况连接来不及释放，socket被耗尽
        }

        /// <summary>
        /// Autofac注册
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<JobFactoryImpl>().As<IJobFactory>().InstancePerLifetimeScope();
            builder.RegisterType<TimingSchedulingJob>().InstancePerLifetimeScope();
            builder.RegisterType<LogInterceptor>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWorkIInterceptor>().InstancePerLifetimeScope();
            builder.RegisterType<SchedulingPlanningHelp>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("ServiceImpl"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .InterceptedBy(new[] { typeof(LogInterceptor), typeof(UnitOfWorkIInterceptor) })
                .EnableInterfaceInterceptors();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            lifetimeScope = app.ApplicationServices.GetAutofacRoot();

            app.UseSwagger();

            app.UseConsul(Guid.NewGuid().ToString(), "TimingSchedulingService");

            app.UseLog();

            lifetimeScope.Resolve<IMissionPlanService>().InitializeMissionPlanToQuartz();//初始化任务计划

            app.UseRouting();

            app.UseAuthorization();

            //获取客户端IP地址使用
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
