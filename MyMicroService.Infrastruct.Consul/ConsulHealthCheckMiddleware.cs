using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MyMicroService.Infrastruct.Consul
{
    /// <summary>
    /// 心跳检查中间件
    /// </summary>
    public class ConsulHealthCheckMiddleware
    {
        /// <summary>
        ///下个要执行的中间件
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 构造心跳检查中间件
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logFactory"></param>
        public ConsulHealthCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/api/ConsulHealthCheck")
            {
                return;
            }
            await this._next(context);
        }
    }
}