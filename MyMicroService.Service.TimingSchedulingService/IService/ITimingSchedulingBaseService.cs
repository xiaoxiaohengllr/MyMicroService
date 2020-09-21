using MyMicroService.Infrastruct.BaseService;
using MyMicroService.Service.TimingSchedulingService.Context;

namespace MyMicroService.Service.TimingSchedulingService.IService
{
    /// <summary>
    /// 任务调度类接口
    /// </summary>
    public interface ITimingSchedulingBaseService<TEntity> : IBaseService<TEntity, TimingSchedulingServiceContext>
        where TEntity : class
    {
    }
}
