using Castle.DynamicProxy;
using MyMicroService.Infrastruct.ILog;
using System.Linq;

namespace MyMicroService.Service.TimingSchedulingService.Interceptor
{
    /// <summary>
    /// 日志拦截器
    /// </summary>
    public class LogInterceptor : IInterceptor
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogFactory _logFactory;

        /// <summary>
        /// 日志拦截器构造
        /// </summary>
        /// <param name="logFactory">日志</param>
        public LogInterceptor(ILogFactory logFactory)
        {
            this._logFactory = logFactory;
        }

        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="invocation">被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            this._logFactory.LogInformation($"方法执行前:拦截{invocation.InvocationTarget.GetType()}类下的方法{invocation.Method.Name}的参数是{string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())}");

            //在被拦截的方法执行完毕后 继续执行
            invocation.Proceed();
        }
    }
}
