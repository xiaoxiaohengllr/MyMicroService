using MyMicroService.Infrastruct.Attributes;
using MyMicroService.Infrastruct.Enums;
using MyMicroService.Service.TimingSchedulingService.Models;
using System.ComponentModel.DataAnnotations;

namespace MyMicroService.Service.TimingSchedulingService.APIModels
{
    /// <summary>
    /// OperationType_GetEntityAsync_Response返回对象
    /// </summary>
    [AutoMapper(typeof(OperationType), EnumAutoMapperDirection.Forward)]
    public class OperationType_GetEntityAsync_Response : OperationType_UpdateAsync_Request
    {
    }

    /// <summary>
    /// OperationType_GetListAsync_Response返回对象
    /// </summary>
    [AutoMapper(typeof(OperationType), EnumAutoMapperDirection.Forward)]
    public class OperationType_GetListAsync_Response : OperationType_GetEntityAsync_Response
    {
    }

    /// <summary>
    /// OperationType_GetPageListAsync_Response返回对象
    /// </summary>
    [AutoMapper(typeof(OperationType), EnumAutoMapperDirection.Forward)]
    public class OperationType_GetPageListAsync_Response : OperationType_GetListAsync_Response
    {
    }

    /// <summary>
    /// OperationType_InsertAsync_Request请求对象
    /// </summary>
    [AutoMapper(typeof(OperationType), EnumAutoMapperDirection.Reverse)]
    public class OperationType_InsertAsync_Request
    {
        /// <summary>
        /// 操作类型名称
        /// </summary>
        [Display(Name = "操作类型名称")]
        public string Name { get; set; }
        /// <summary>
        /// 数据验证方法
        /// </summary>
        [Display(Name = "数据验证方法")]
        public string DataValidationMethod { get; set; }
        /// <summary>
        /// 调度执行方法
        /// </summary>
        [Display(Name = "调度执行方法")]
        public string SchedulingExecutionMethod { get; set; }
    }

    /// <summary>
    /// OperationType_UpdateAsync_Request返回对象
    /// </summary>
    [AutoMapper(typeof(OperationType), EnumAutoMapperDirection.Reverse)]
    public class OperationType_UpdateAsync_Request : OperationType_InsertAsync_Request
    {
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        public string ID { get; set; }
    }
}
