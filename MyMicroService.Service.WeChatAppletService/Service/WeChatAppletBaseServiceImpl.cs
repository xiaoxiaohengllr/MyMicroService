
using Autofac;
using MyMicroService.Infrastruct.BaseService;
using MyMicroService.Infrastruct.IRepository;
using MyMicroService.Service.WeChatAppletService.Context;
using MyMicroService.Service.WeChatAppletService.IService;

namespace MyMicroService.Service.WeChatAppletService.Service
{
    /// <summary>
    /// WeChatApplet基类实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class WeChatAppletBaseServiceImpl<TEntity> : BaseServiceImpl<TEntity, WeChatAppletContext>, IWeChatAppletBaseService<TEntity>
                                 where TEntity : class
    {
        /// <summary>
        /// WeChatApplet基类实现构造
        /// </summary>
        public WeChatAppletBaseServiceImpl()
        {
            base.ServiceContext = Startup.lifetimeScope.Resolve(typeof(IRepositoryOperation<WeChatAppletContext>)) as IRepositoryOperation<WeChatAppletContext>;
        }
    }
}


