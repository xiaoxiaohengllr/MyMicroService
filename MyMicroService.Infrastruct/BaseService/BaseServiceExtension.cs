using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MyMicroService.Infrastruct.Attributes;
using MyMicroService.Infrastruct.Enums;
using MyMicroService.Infrastruct.IRepository;
using MyMicroService.Infrastruct.Repository;
using System;
using System.Linq;

namespace MyMicroService.Infrastruct.BaseService
{
    /// <summary>
    /// 服务基类扩展
    /// </summary>
    public static class BaseServiceExtension
    {
        /// <summary>
        /// 添加服务基类注册
        /// </summary>
        /// <param name="services"></param>\
        /// <param name="callerType">调用者类型</param>
        public static void AddBaseService(this IServiceCollection services, Type callerType)
        {
            services.AddTransient(typeof(IRepositoryOperation<>), typeof(RepositoryOperationImpl<>));
            services.AddScoped(typeof(IBaseService<,>), typeof(BaseServiceImpl<,>));
            services.AddAutoMapper(config =>//自动注册对象映射
            {
                foreach (Type type in callerType.Assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(AutoMapperAttribute), false).Any()))
                {
                    AutoMapperAttribute autoMapperTypeAttribute = type.GetCustomAttributes(typeof(AutoMapperAttribute), false)[0] as AutoMapperAttribute;
                    if (autoMapperTypeAttribute.AutoMapperDirection == EnumAutoMapperDirection.Reverse)//反向
                    {
                        config.CreateMap(type, autoMapperTypeAttribute.AutoMapperType);
                    }
                    else if (autoMapperTypeAttribute.AutoMapperDirection == EnumAutoMapperDirection.Forward)//正向
                    {
                        config.CreateMap(autoMapperTypeAttribute.AutoMapperType, type);
                    }
                    else//双向
                    {
                        config.CreateMap(type, autoMapperTypeAttribute.AutoMapperType).ReverseMap();
                    }
                }
            });//注册AutoMapper
        }
    }
}
