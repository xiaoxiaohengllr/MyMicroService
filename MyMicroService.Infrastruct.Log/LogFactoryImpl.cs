using Exceptionless;
using Microsoft.Extensions.Logging;
using MyMicroService.Infrastruct.ILog;
using System;
using System.Threading.Tasks;

namespace MyMicroService.Infrastruct.Log
{
    /// <summary>
    /// 日志实现
    /// </summary>
    public class LogFactoryImpl : ILogFactory
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<LogFactoryImpl> _logger;

        /// <summary>
        /// 日志实现构造
        /// </summary>
        /// <param name="logger">日志对象</param>
        public LogFactoryImpl(ILogger<LogFactoryImpl> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public Task Log(LogLevel logLevel, string msg)
        {
            return Task.Run(() =>
            {
                if (logLevel == LogLevel.Debug)
                    ExceptionlessClient.Default.CreateLog(msg, Exceptionless.Logging.LogLevel.Debug).Submit();
                else if (logLevel == LogLevel.Information)
                    ExceptionlessClient.Default.CreateLog(msg, Exceptionless.Logging.LogLevel.Info).Submit();
                else if (logLevel == LogLevel.Warning)
                    ExceptionlessClient.Default.CreateLog(msg, Exceptionless.Logging.LogLevel.Warn).Submit();
                else if (logLevel == LogLevel.Error)
                    ExceptionlessClient.Default.CreateLog(msg, Exceptionless.Logging.LogLevel.Error).Submit();
                else if (logLevel == LogLevel.Critical)
                    ExceptionlessClient.Default.CreateLog(msg, Exceptionless.Logging.LogLevel.Fatal).Submit();
                else if (logLevel == LogLevel.None)
                    ExceptionlessClient.Default.CreateLog(msg, Exceptionless.Logging.LogLevel.Off).Submit();
                this._logger.Log(logLevel, msg);
            });
        }

        /// <summary>
        /// 严重错误日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public Task LogCritical(string msg)
        {
            return Log(LogLevel.Critical, msg);
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public Task LogDebug(string msg)
        {
            return Log(LogLevel.Debug, msg);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public Task LogError(string msg)
        {
            return Log(LogLevel.Error, msg);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <returns></returns>
        public Task LogError(Exception ex)
        {
            string msg = $"异常类型:{ex.GetType().FullName}异常消息:{ex.Message}堆栈信息{ex.StackTrace}";
            return Log(LogLevel.Error, msg);
        }

        /// <summary>
        /// 消息日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public Task LogInformation(string msg)
        {
            return Log(LogLevel.Information, msg);
        }

        /// <summary>
        /// 跟踪日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public Task LogTrace(string msg)
        {
            return Log(LogLevel.Trace, msg);
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public Task LogWarning(string msg)
        {
            return Log(LogLevel.Warning, msg);
        }
    }
}
