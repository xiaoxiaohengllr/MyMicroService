using System;

namespace MyMicroService.Infrastruct.ExceptionModels
{
    /// <summary>
    /// 用户操作异常
    /// </summary>
    public class UserOperationException : Exception
    {
        /// <summary>
        /// 用户操作异常
        /// </summary>
        /// <param name="ErrprMessage">异常消息</param>
        public UserOperationException(string ErrprMessage) : base(ErrprMessage)
        {

        }
    }
}
