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
        /// ��������
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

            services.AddSwagger("MyMicroService.Service.TimingSchedulingService", "��ʱ���ȷ���", "MyMicroService.Service.TimingSchedulingService.xml");

            services.AddBaseService(this.GetType());//�������

            services.AddHttpWebHelper();//���HttpWeb������

            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();//ע��ISchedulerFactory��ʵ��

            services.AddLog();//�����־

            services.AddHttpClient();//ע��HttpClient���������ڽ��HttpClient�ڸ߲�����������������ͷţ�socket���ľ�
        }

        /// <summary>
        /// Autofacע��
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

            lifetimeScope.Resolve<IMissionPlanService>().InitializeMissionPlanToQuartz();//��ʼ������ƻ�

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
