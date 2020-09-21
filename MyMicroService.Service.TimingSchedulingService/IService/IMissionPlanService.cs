using MyMicroService.Service.TimingSchedulingService.Models;
using System.Threading.Tasks;

namespace MyMicroService.Service.TimingSchedulingService.IService
{
    /// <summary>
    /// 任务计划服务接口
    /// </summary>
    public interface IMissionPlanService : ITimingSchedulingBaseService<MissionPlan>
    {
        /// <summary>
        /// 初始化任务计划
        /// </summary>
        /// <returns></returns>
        Task InitializeMissionPlanToQuartz();
    }
}
