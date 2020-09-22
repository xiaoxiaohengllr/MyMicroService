using Autofac;
using MyMicroService.Infrastruct.BaseService;
using MyMicroService.Infrastruct.IRepository;
using MyMicroService.Service.TimingSchedulingService.Context;
using MyMicroService.Service.TimingSchedulingService.IService;

namespace MyMicroService.Service.TimingSchedulingService.Service
{
    /// <summary>
    /// 任务调度基类实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class TimingSchedulingBaseServiceImpl<TEntity> : BaseServiceImpl<TEntity, TimingSchedulingContext>, ITimingSchedulingBaseService<TEntity>
         where TEntity : class
    {
        /// <summary>
        /// 任务调度基类实现构造
        /// </summary>
        public TimingSchedulingBaseServiceImpl()
        {
            base.ServiceContext = Startup.lifetimeScope.Resolve(typeof(IRepositoryOperation<TimingSchedulingContext>)) as IRepositoryOperation<TimingSchedulingContext>;
        }
    }
}
