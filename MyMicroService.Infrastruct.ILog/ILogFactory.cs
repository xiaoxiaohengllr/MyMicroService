using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MyMicroService.Infrastruct.ILog
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogFactory
    {

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        Task Log(LogLevel logLevel, string msg);

        /// <summary>
        /// 严重错误日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        Task LogCritical(string msg);

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        Task LogDebug(string msg);

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        Task LogError(string msg);

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <returns></returns>
        Task LogError(Exception ex);

        /// <summary>
        /// 消息日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        Task LogInformation(string msg);

        /// <summary>
        /// 跟踪日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        Task LogTrace(string msg);

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        Task LogWarning(string msg);
    }
}
