using System.ComponentModel;

namespace MyMicroService.Service.TimingSchedulingService.Enums
{
    /// <summary>
    /// 调度计划状态枚举
    /// </summary>
    public enum EnumMissionPlanState
    {
        /// <summary>
        /// 已启动
        /// </summary>
        [Description("运行中")]
        Start = 1,

        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Close = 2,

        /// <summary>
        /// 执行中
        /// </summary>
        [Description("执行中")]
        Execution = 3,
    }
}
