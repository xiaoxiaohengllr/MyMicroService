using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace MyMicroService.Infrastruct.Consul
{
    /// <summary>
    /// Consul扩展类
    /// </summary>
    public static class ConsulExtension
    {
        /// <summary>
        /// Consul注册扩展方法
        /// </summary>
        /// <param name="app">注册管道</param>
        /// <param name="consulID">consul唯一标识</param>
        /// <param name="consulName">consul名称</param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, string consulID, string consulName)
        {
            // 获取主机生命周期管理接口
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();

            var ip = configuration["IP"];
            var port = configuration["Port"];
            string consulUrl = configuration["ConsulUrl"];
            if (string.IsNullOrEmpty(consulUrl))
            {
                consulUrl = $"http://localhost:8500";
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = $"localhost";
            }
            if (string.IsNullOrEmpty(port))
            {
                port = $"5000";
            }

            var consulClient = new ConsulClient(x => x.Address = new Uri(consulUrl));//请求注册的 Consul 地址
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(5),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{ip}:{port}/api/ConsulHealthCheck",//健康检查地址
                Timeout = TimeSpan.FromSeconds(10)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = consulID,
                Name = consulName,
                Address = ip,
                Port = int.Parse(port),
                Tags = new[] { $"urlprefix-/{consulName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            });

            app.UseMiddleware<ConsulHealthCheckMiddleware>();//心跳检查中间件

            return app;
        }
    }
}
