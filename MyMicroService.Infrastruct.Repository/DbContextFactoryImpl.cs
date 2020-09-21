using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyMicroService.Infrastruct.IRepository;
using System;
using System.IO;

namespace MyMicroService.Infrastruct.Repository
{
    /// <summary>
    /// 数据上下文工厂实现
    /// </summary>
    /// <typeparam name="TDbContext">数据上下文</typeparam>
    public class DbContextFactoryImpl<TDbContext> : IDbContextFactory<TDbContext>
        where TDbContext : BaseDbContext<TDbContext>, new()
    {
        /// <summary>
        /// 数据上下文(写)
        /// </summary>
        private DbContext ReadDbContext { get; set; }

        /// <summary>
        /// 数据上下文(读)
        /// </summary>
        private DbContext WriteDbContext { get; set; }

        /// <summary>
        /// 获取数据上下文(读)
        /// </summary>
        /// <returns></returns>
        public virtual DbContext GetReadDbContext()
        {
            if (this.ReadDbContext == null)
            {
                this.ReadDbContext = CreateDbContext(EnumDbContextOperationType.Read);
            }
            return this.ReadDbContext;
        }

        /// <summary>
        /// 获取数据上下文(写)
        /// </summary>
        /// <returns></returns>
        public virtual DbContext GetWriteDbContext()
        {
            if (this.WriteDbContext == null)
            {
                this.WriteDbContext = CreateDbContext(EnumDbContextOperationType.Write);
            }
            return this.WriteDbContext;
        }

        /// <summary>
        /// 创建数据库上下文
        /// </summary>
        /// <typeparam name="EnumDbContextOperationType">数据上下文操作类型枚举</typeparam>
        /// <returns></returns>
        private DbContext CreateDbContext(EnumDbContextOperationType enumDbContextOperationType)
        {
            var configuration = BuildConfiguration();
            var builder = new DbContextOptionsBuilder<TDbContext>();
            ConnectionEntity connectionEntity = configuration.GetSection(typeof(TDbContext).Name).Get<ConnectionEntity>();
            if (connectionEntity == null)
            {
                throw new Exception("未设置连接配置");
            }
            string connection = string.Empty;//连接字符串
            if (enumDbContextOperationType == EnumDbContextOperationType.Write)
            {
                connection = connectionEntity.Connection;
            }
            else
            {
                if (connectionEntity.ReadConnection == null || connectionEntity.ReadConnection.Count == 0)
                {
                    connection = connectionEntity.Connection;
                }
                else
                {
                    connection = connectionEntity.ReadConnection[int.MaxValue % connectionEntity.ReadConnection.Count];
                }
            }
            switch (connectionEntity.DbType)
            {
                case (int)EnumDbType.SqlServer:
                    builder.UseSqlServer(connection);
                    break;
                case (int)EnumDbType.MySql:
                    builder.UseMySql(connection);
                    break;
                default:
                    throw new Exception("未有数据库类型");
            }
            return typeof(TDbContext).Assembly.CreateInstance(typeof(TDbContext).FullName, true, System.Reflection.BindingFlags.Default, null, new object[] { builder.Options }, null, null) as TDbContext;
            //return new TDbContext<TDbContext>(builder.Options);
        }

        /// <summary>
        /// 生成配置
        /// </summary>
        /// <returns></returns>
        private IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);
            return builder.Build();
        }
    }
}
