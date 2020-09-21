using Microsoft.Extensions.DependencyInjection;

namespace MyMicroService.Infrastruct.HttpWeb
{
    /// <summary>
    /// HttpWeb扩展
    /// </summary>
    public static class HttpWebExtension
    {
        /// <summary>
        /// 注册http请求帮助
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpWebHelper(this IServiceCollection services)
        {
            services.AddTransient<IHttpWebHelper, HttpWebHelperImpl>();
        }
    }
}
