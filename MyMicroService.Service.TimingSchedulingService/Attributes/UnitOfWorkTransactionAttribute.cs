using System;

namespace MyMicroService.Service.TimingSchedulingService.Attributes
{
    /// <summary>
    /// 工作单元
    /// 仅用来做特性标记 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class UnitOfWorkTransactionAttribute : Attribute
    {

    }
}
