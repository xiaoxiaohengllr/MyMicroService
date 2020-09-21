using Autofac;
using Quartz;
using Quartz.Spi;
using System;

namespace MyMicroService.Service.TimingSchedulingService.Quartz
{
    /// <summary>
    /// JobFactoryImpl
    /// </summary>
    public class JobFactoryImpl : IJobFactory
    {
        /// <summary>
        /// IServiceProvider
        /// </summary>
        private readonly ILifetimeScope _lifetimeScope;

        /// <summary>
        /// 构造
        /// </summary>
        public JobFactoryImpl()
        {
            _lifetimeScope = Startup.lifetimeScope;
        }

        /// <summary>
        /// NewJob
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _lifetimeScope.Resolve<TimingSchedulingJob>();
        }

        /// <summary>
        /// ReturnJob
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
