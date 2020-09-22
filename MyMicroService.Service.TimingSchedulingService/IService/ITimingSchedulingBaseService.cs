using MyMicroService.Infrastruct.BaseService;
using MyMicroService.Service.TimingSchedulingService.Context;

namespace MyMicroService.Service.TimingSchedulingService.IService
{
    /// <summary>
    /// 任务调度基类接口
    /// </summary>
    public interface ITimingSchedulingBaseService<TEntity> : IBaseService<TEntity, TimingSchedulingContext>
        where TEntity : class
    {
    }
}
