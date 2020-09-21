using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyMicroService.Infrastruct.ExceptionModels;
using MyMicroService.Infrastruct.ResponseModels;
using MyMicroService.Infrastruct.ILog;

namespace MyMicroService.Infrastruct.Filters
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogFactory _logFactory;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GlobalExceptionFilter(ILogFactory logFactory)
        {
            this._logFactory = logFactory;
        }

        /// <summary>
        ///全局错误处理
        /// </summary>
        /// <param name="context">错误上下文</param>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(UserOperationException))//用户操作异常直接提示用户
            {
                ResponseMessage responseMessage = new ResponseMessage();
                responseMessage.ReturnMessage(false, context.Exception.Message);
                context.Result = new JsonResult(responseMessage);
            }
            else
            {
                this._logFactory.LogError(context.Exception);
                ResponseMessage responseMessage = new ResponseMessage();
                responseMessage.ReturnMessage(false, "发生未知错误,请于管理员联系");
                context.Result = new JsonResult(responseMessage);
            }
            context.ExceptionHandled = true;
        }

    }
}
