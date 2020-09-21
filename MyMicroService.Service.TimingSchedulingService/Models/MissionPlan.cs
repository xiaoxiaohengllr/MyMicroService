using System;
using System.ComponentModel.DataAnnotations;

namespace MyMicroService.Service.TimingSchedulingService.Models
{
    /// <summary>
    /// 任务计划
    /// </summary>
    public partial class MissionPlan
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        public string ID { get; set; }
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
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime CreatTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        public DateTime ModifyTime { get; set; }
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
}
