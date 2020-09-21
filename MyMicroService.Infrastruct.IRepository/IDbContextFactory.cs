using Microsoft.EntityFrameworkCore;

namespace MyMicroService.Infrastruct.IRepository
{
    /// <summary>
    /// 数据上下文工厂接口
    /// </summary>
    /// <typeparam name="TDbContext">数据上下文</typeparam>
    public interface IDbContextFactory<TDbContext>
        where TDbContext : BaseDbContext<TDbContext>, new()
    {
        /// <summary>
        /// 获取数据上下文(读)
        /// </summary>
        /// <returns></returns>
        DbContext GetReadDbContext();

        /// <summary>
        /// 获取数据上下文(写)
        /// </summary>
        /// <returns></returns>
        DbContext GetWriteDbContext();
    }
}
