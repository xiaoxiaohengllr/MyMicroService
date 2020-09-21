using MyMicroService.Infrastruct.Attributes;
using MyMicroService.Infrastruct.Enums;
using MyMicroService.Service.TimingSchedulingService.Models;
using System.ComponentModel.DataAnnotations;

namespace MyMicroService.Service.TimingSchedulingService.APIModels
{
    /// <summary>
    /// MissionPlan_GetEntityAsync_Response返回对象
    /// </summary>
    [AutoMapper(typeof(MissionPlan), EnumAutoMapperDirection.Forward)]
    public class MissionPlan_GetEntityAsync_Response : MissionPlan_UpdateAsync_Request
    {
    }

    /// <summary>
    /// MissionPlan_GetListAsync_Response返回对象
    /// </summary>
    [AutoMapper(typeof(MissionPlan), EnumAutoMapperDirection.Forward)]
    public class MissionPlan_GetListAsync_Response : MissionPlan_GetEntityAsync_Response
    {
    }

    /// <summary>
    /// MissionPlan_GetPageListAsync_Response返回对象
    /// </summary>
    [AutoMapper(typeof(MissionPlan), EnumAutoMapperDirection.Forward)]
    public class MissionPlan_GetPageListAsync_Response : MissionPlan_GetListAsync_Response
    {
    }

    /// <summary>
    /// MissionPlan_InsertAsync_Request请求对象
    /// </summary>
    [AutoMapper(typeof(MissionPlan), EnumAutoMapperDirection.Reverse)]
    public class MissionPlan_InsertAsync_Request
    {
        /// <summary>
        /// 计划名称
        /// </summary>
        [Display(Name = "计划名称")]
        public string Name { get; set; }
        /// <summary>
        /// 计划分组
        /// </summary>
        [Display(Name = "计划分组")]
        public string Group { get; set; }
        /// <summary>
        /// cron表达式
        /// </summary>
        [Display(Name = "cron表达式")]
        public string Cron { get; set; }
        /// <summary>
        /// 计划描述
        /// </summary>
        [Display(Name = "计划描述")]
        public string Describe { get; set; }
        /// <summary>
        /// 计划状态
        /// </summary>
        [Display(Name = "计划状态")]
        public int State { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        [Display(Name = "操作类型")]
        public string OperationTypeID { get; set; }
        /// <summary>
        /// 操作数据
        /// </summary>
        [Display(Name = "操作数据")]
        public string OperationData { get; set; }
    }

    /// <summary>
    /// MissionPlan_UpdateAsync_Request请求对象
    /// </summary>
    [AutoMapper(typeof(MissionPlan), EnumAutoMapperDirection.Reverse)]
    public class MissionPlan_UpdateAsync_Request : MissionPlan_InsertAsync_Request
    {
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        public string ID { get; set; }
    }
}
