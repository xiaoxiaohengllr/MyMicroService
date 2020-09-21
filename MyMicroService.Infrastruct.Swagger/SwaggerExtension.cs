using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System.IO;

namespace MyMicroService.Infrastruct.Swagger
{
    /// <summary>
    /// Swagger扩展类
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// Api版本信息
        /// </summary>
        private static IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// 添加Swagger配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="title">标题</param>
        /// <param name="description">描述</param>
        /// <param name="paths">XML文档路径</param>
        public static void AddSwagger(this IServiceCollection services, string title, string description, params string[] paths)
        {
            #region Swagger
            services.AddApiVersioning(options =>
            {
                // 可选，为true时API返回支持的版本信息
                options.ReportApiVersions = true;
                // 不提供版本时，默认为1.0
                options.AssumeDefaultVersionWhenUnspecified = true;
                // 请求中未指定版本时默认为1.0
                options.DefaultApiVersion = new ApiVersion(1, 0);

            }).AddVersionedApiExplorer(option =>
            {
                // 版本名的格式：v+版本号
                option.GroupNameFormat = "'v'V";
                option.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddSwaggerGen(c =>
            {
                // 多版本控制
                foreach (var item in _provider.ApiVersionDescriptions)
                {
                    // 添加文档信息
                    c.SwaggerDoc(item.GroupName, new OpenApiInfo
                    {
                        Title = "SCCW_Administration API",
                        Version = item.ApiVersion.ToString(),
                        Description = "SCCW_Administration"
                    });
                    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                    foreach (var path in paths)
                    {
                        //Core.Admin.webapi.xml是我的项目生成XML文档的后缀名,具体的以你项目为主
                        var xmlPath = Path.Combine(basePath, path);
                        if (File.Exists(xmlPath))
                            c.IncludeXmlComments(xmlPath);
                    }

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
                }
            });
            _provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            #endregion
        }


        /// <summary>
        /// Swagge中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
        {
            SwaggerBuilderExtensions.UseSwagger(app);
            app.UseSwaggerUI(c =>
            {
                foreach (var item in _provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", "MyMicroService API V" + item.ApiVersion);
                }
            });
            return app;
        }
    }
}
