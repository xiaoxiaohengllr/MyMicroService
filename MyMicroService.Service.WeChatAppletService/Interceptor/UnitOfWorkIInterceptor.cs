using Castle.DynamicProxy;
using MyMicroService.Infrastruct.IRepository;
using MyMicroService.Service.WeChatAppletService.Attributes;
using MyMicroService.Service.WeChatAppletService.Context;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyMicroService.Service.WeChatAppletService.Interceptor
{
    /// <summary>
    /// 工作单元控制器
    /// </summary>
    public class UnitOfWorkIInterceptor : IInterceptor
    {
        /// <summary>
        /// 数据上下文
        /// </summary>
        private readonly IRepositoryOperation<WeChatAppletContext> _repositoryOperation;

        /// <summary>
        /// 工作单元控制器构造
        /// </summary>
        /// <param name="repositoryOperation">数据上下文工厂</param>
        public UnitOfWorkIInterceptor(IRepositoryOperation<WeChatAppletContext> repositoryOperation)
        {
            this._repositoryOperation = repositoryOperation;
        }

        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="invocation">被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }
            UnitOfWorkTransactionAttribute workTransaction = methodInfo.GetCustomAttributes<UnitOfWorkTransactionAttribute>(true).FirstOrDefault();
            //如果标记了 [UnitOfWorkTransaction]，并且不在事务嵌套中
            if (workTransaction != null && !this._repositoryOperation.IsExistTransaction())
            {
                this._repositoryOperation.BeginTransaction();//开始事务
                try
                {
                    if (methodInfo.ReturnType != null && methodInfo.ReturnType == typeof(Task<>))//是否是异步方法有返回值
                    {
                        var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
                        var mi = methodInfo.MakeGenericMethod(resultType);
                        invocation.ReturnValue = mi.Invoke(this, new[] { invocation.ReturnValue });
                        this._repositoryOperation.CommitTransaction();//提交事务
                    }
                    else if (methodInfo.ReturnType != null && methodInfo.ReturnType == typeof(Task))//是否是异步方法没有返回值的
                    {
                        Func<Task> returnValue = async () => await (Task)invocation.ReturnValue;
                        invocation.ReturnValue = returnValue;
                        this._repositoryOperation.CommitTransaction();//提交事务
                    }
                    else
                    {
                        invocation.Proceed();//不是异步方法直接执行
                        this._repositoryOperation.CommitTransaction();//提交事务
                    }
                }
                catch (Exception ex)
                {
                    this._repositoryOperation.RollbackTransaction();//回滚事务
                    throw ex;
                }
            }
            else
            {
                invocation.Proceed();//没有标记直接执行
            }

        }
    }
}
