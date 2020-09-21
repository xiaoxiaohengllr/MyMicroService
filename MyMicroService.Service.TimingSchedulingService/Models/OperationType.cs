using System.ComponentModel.DataAnnotations;

namespace MyMicroService.Service.TimingSchedulingService.Models
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public partial class OperationType
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        public string ID { get; set; }
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
}
