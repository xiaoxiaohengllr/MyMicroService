using MyMicroService.Infrastruct.ExceptionModels;
using MyMicroService.Service.TimingSchedulingService.Enums;
using MyMicroService.Service.TimingSchedulingService.IService;
using MyMicroService.Service.TimingSchedulingService.Models;
using MyMicroService.Service.TimingSchedulingService.Quartz;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMicroService.Service.TimingSchedulingService.Service
{
    /// <summary>
    /// 任务计划服务实现
    /// </summary>
    public class MissionPlanServiceImpl : TimingSchedulingBaseServiceImpl<MissionPlan>, IMissionPlanService
    {
        /// <summary>
        /// Quartz.Net调度工厂
        /// </summary>
        private readonly ISchedulerFactory _schedulerFactory;

        /// <summary>
        /// 调度任务工厂
        /// </summary>
        private readonly IJobFactory _jobFactory;

        /// <summary>
        /// 调度计划帮助类
        /// </summary>
        private readonly SchedulingPlanningHelp _schedulingPlanningHelp;

        /// <summary>
        /// 任务计划服务构造
        /// </summary>
        /// <param name="schedulerFactory">Quartz.Net调度工厂</param>
        /// <param name="jobFactory">调度任务工厂</param>
        /// <param name="schedulingPlanningHelp">任务计划服务构造</param>
        public MissionPlanServiceImpl(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, SchedulingPlanningHelp schedulingPlanningHelp)
        {
            this._schedulerFactory = schedulerFactory;
            this._jobFactory = jobFactory;
            this._schedulingPlanningHelp = schedulingPlanningHelp;
        }

        /// <summary>
        /// 添加调度计划
        /// </summary>
        /// <param name="entity">调度实体</param>
        /// <returns></returns>
        public override async Task<string> InsertAsync(MissionPlan entity)
        {
            Verification(entity);//验证
            entity.ID = Guid.NewGuid().ToString();//ID
            entity.CreatTime = DateTime.Now;//创建时间
            entity.ModifyTime = DateTime.Now;//修改时间
            string msg = await base.InsertAsync(entity);
            if (string.IsNullOrEmpty(msg))//添加成功，就添加到调度器中
            {
                await AddJob(entity);
            }
            return msg;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">调度实体</param>
        private void Verification(MissionPlan entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
                throw new UserOperationException("必须填写任务名称");
            if (string.IsNullOrEmpty(entity.Group))
                throw new UserOperationException("必须填写分组名称");
            if (string.IsNullOrEmpty(entity.Cron))
                throw new UserOperationException("必须填写Cron表达式");
            if (!CronExpression.IsValidExpression(entity.Cron))
                throw new UserOperationException("必须填写正确Cron表达式");
            if (entity.State != (int)EnumMissionPlanState.Start && entity.State != (int)EnumMissionPlanState.Close)
                throw new UserOperationException("无效任务状态");
            if (base.ServiceContext.Any<MissionPlan>(missionPlan => missionPlan.Name == entity.Name && missionPlan.Group == entity.Group && missionPlan.ID != entity.ID))
                throw new UserOperationException("该分组已存在此任务名称");
            OperationType operationType = base.ServiceContext.Find<OperationType>(entity.OperationTypeID);
            if (operationType == null)
                throw new UserOperationException("必须选择正确操作方式");
            string msg = this._schedulingPlanningHelp.GetType().GetMethod(operationType.DataValidationMethod).Invoke(this._schedulingPlanningHelp, new object[] { entity.OperationData }) as string;
            if (msg.Length != 0)
                throw new UserOperationException(msg);
        }

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="entity">任务实体</param>
        /// <returns></returns>
        public override async Task<string> UpdateAsync(MissionPlan entity)
        {
            MissionPlan updateMissionPlan = await base.GetEntityAsync(entity.ID);
            if (updateMissionPlan == null)
                throw new UserOperationException("未找到要修改对象");
            if (updateMissionPlan.State == (int)EnumMissionPlanState.Execution)
                throw new UserOperationException("无法修改进行中的任务");
            Verification(entity);//验证
            string name = updateMissionPlan.Name;
            string group = updateMissionPlan.Group;
            updateMissionPlan.Name = entity.Name;//任务名称
            updateMissionPlan.Group = entity.Group;//分组名称
            updateMissionPlan.Cron = entity.Cron;//Cron表达式
            updateMissionPlan.ModifyTime = DateTime.Now;//修改时间
            string msg = await base.UpdateAsync(updateMissionPlan);
            if (string.IsNullOrEmpty(msg))//添加成功，就更新调度器中的任务
            {
                await DeleteJob(name, group);
                await AddJob(updateMissionPlan);
            }
            return msg;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="keyValues">id</param>
        /// <returns></returns>
        public override async Task<string> DeleteAsync(params object[] keyValues)
        {
            MissionPlan deleteMissionPlan = await base.GetEntityAsync(keyValues);
            if (deleteMissionPlan == null)
                throw new UserOperationException("未找到要删除对象");
            string msg = await base.DeleteAsync(deleteMissionPlan);
            if (string.IsNullOrEmpty(msg))//添加成功，就删除调度器中的任务
            {
                await DeleteJob(deleteMissionPlan.Name, deleteMissionPlan.Group);
            }
            return msg;
        }

        /// <summary>
        /// 初始化任务计划
        /// </summary>
        /// <returns></returns>
        public virtual async Task InitializeMissionPlanToQuartz()
        {
            List<MissionPlan> missionPlanList = await base.GetListAsync();
            foreach (var missionPlan in missionPlanList)
            {
                await AddJob(missionPlan);
            }
        }

        #region Quartz
        /// <summary>
        /// 添加任务Quartz
        /// </summary>
        /// <param name="entity">任务实体</param>
        /// <returns></returns>
        private async Task AddJob(MissionPlan entity)
        {
            IScheduler _scheduler = await this._schedulerFactory.GetScheduler();//通过调度工厂获得调度器
            _scheduler.JobFactory = this._jobFactory;
            await _scheduler.Start();//开启调度器
            var trigger = TriggerBuilder.Create()
                        .WithCronSchedule(entity.Cron)
                        .Build();//创建一个触发器
            var jobDetail = JobBuilder.Create<TimingSchedulingJob>()
                            .UsingJobData("ID", entity.ID)
                            .UsingJobData("OperationTypeID", entity.OperationTypeID)
                            .UsingJobData("OperationData", entity.OperationData)
                            .WithIdentity(entity.Name, entity.Group)
                            .Build();
            await _scheduler.ScheduleJob(jobDetail, trigger);//将触发器和任务器绑定到调度器中
            if (entity.State == (int)EnumMissionPlanState.Close)//如果状态为关闭直接停止
            {
                await _scheduler.PauseJob(trigger.JobKey);
            }
        }

        /// <summary>
        /// 删除任务Quartz
        /// </summary>
        /// <param name="name">任务名称</param>
        /// <param name="group">任务分组</param>
        /// <returns></returns>
        private async Task DeleteJob(string name, string group)
        {
            IScheduler _scheduler = await this._schedulerFactory.GetScheduler();//通过调度工厂获得调度器
            JobKey jobKey = new JobKey(name, group);
            await _scheduler.DeleteJob(jobKey);
        }
        #endregion
    }
}
