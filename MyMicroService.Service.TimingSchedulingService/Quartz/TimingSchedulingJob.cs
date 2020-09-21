using MyMicroService.Infrastruct.ExceptionModels;
using MyMicroService.Infrastruct.ILog;
using MyMicroService.Service.TimingSchedulingService.IService;
using MyMicroService.Service.TimingSchedulingService.Models;
using MyMicroService.Service.TimingSchedulingService.Service;
using Quartz;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MyMicroService.Service.TimingSchedulingService.Quartz
{
    /// <summary>
    /// Job的实现类
    /// </summary>
    [PersistJobDataAfterExecution]//更新JobDetail的JobDataMap的存储副本，以便下一次执行这个任务接收更新的值而不是原始存储的值 
    public class TimingSchedulingJob : IJob
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogFactory _logFactory;

        /// <summary>
        /// 操作类型服务
        /// </summary>
        private readonly IOperationTypeService _operationTypeService;

        /// <summary>
        /// 调度计划帮助类
        /// </summary>
        private readonly SchedulingPlanningHelp _schedulingPlanningHelp;

        /// <summary>
        /// Job的实现构造
        /// </summary>
        /// <param name="logFactory"></param>
        /// <param name="operationTypeService"></param>
        /// <param name="schedulingPlanningHelp"></param>
        public TimingSchedulingJob(ILogFactory logFactory, IOperationTypeService operationTypeService, SchedulingPlanningHelp schedulingPlanningHelp)
        {
            this._logFactory = logFactory;
            this._operationTypeService = operationTypeService;
            this._schedulingPlanningHelp = schedulingPlanningHelp;
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            string id = context.JobDetail.JobDataMap.GetString("ID");
            string operationTypeID = context.JobDetail.JobDataMap.GetString("OperationTypeID");
            string operationData = context.JobDetail.JobDataMap.GetString("OperationData");

            await Task.Run(async () =>
            {
                OperationType operationType = await this._operationTypeService.GetEntityAsync(operationTypeID);
                if (operationType == null)
                {
                    await this._logFactory.LogError($"执行了调度计划{id},未找到操作类型");
                    throw new UserOperationException("未找到操作类型");
                }
                MethodInfo methodInfo = this._schedulingPlanningHelp.GetType().GetMethod(operationType.SchedulingExecutionMethod);
                if (methodInfo == null)
                {
                    await this._logFactory.LogError($"执行了调度计划{id},未找到需要执行的方法");
                    throw new UserOperationException("未找到需要执行的方法");
                }
                try
                {
                    methodInfo.Invoke(this._schedulingPlanningHelp, new object[] { operationData });
                    await this._logFactory.LogInformation($"执行调度计划{id}成功");
                }
                catch (Exception e)
                {
                    await this._logFactory.LogError($"执行调度计划{id}失败,原因：{e.Message}");
                    throw e;
                }
            });
        }
    }
}
