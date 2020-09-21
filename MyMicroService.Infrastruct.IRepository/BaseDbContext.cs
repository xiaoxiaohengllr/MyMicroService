using Microsoft.EntityFrameworkCore;

namespace MyMicroService.Infrastruct.IRepository
{
    /// <summary>
    /// 数据库上下文基类
    /// </summary>
    public class BaseDbContext<TDbContext> : DbContext
         where TDbContext : DbContext, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseDbContext()
        {

        }

        public BaseDbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {
        }
    }
}
