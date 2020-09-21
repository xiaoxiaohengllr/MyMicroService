using MyMicroService.Infrastruct.Enums;
using System;

namespace MyMicroService.Infrastruct.Attributes
{
    /// <summary>
    /// 对象映射信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class AutoMapperAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="autoMapperType">映射类型</param>
        /// <param name="autoMapperDirection">映射方向</param>
        public AutoMapperAttribute(Type autoMapperType, EnumAutoMapperDirection autoMapperDirection)
        {
            this.AutoMapperType = autoMapperType;
            this.AutoMapperDirection = autoMapperDirection;
        }

        /// <summary>
        /// 映射类型
        /// </summary>
        public Type AutoMapperType { get; set; }

        /// <summary>
        /// 映射方向
        /// </summary>
        public EnumAutoMapperDirection AutoMapperDirection { get; set; }
    }
}
