
using MyMicroService.Infrastruct.BaseService;
using MyMicroService.Service.WeChatAppletService.Context;

namespace MyMicroService.Service.WeChatAppletService.IService
{
    /// <summary>
    /// WeChatApplet基类接口
    /// </summary>
    public interface IWeChatAppletBaseService<TEntity> : IBaseService<TEntity, WeChatAppletContext>
                                where TEntity : class
    {
    }
}

