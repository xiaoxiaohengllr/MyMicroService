
using Microsoft.EntityFrameworkCore;
using MyMicroService.Infrastruct.IRepository;
using MyMicroService.Service.WeChatAppletService.Models;

namespace MyMicroService.Service.WeChatAppletService.Context
{
    /// <summary>
    /// 任务调度上下文
    /// </summary>
    public partial class WeChatAppletContext : BaseDbContext<WeChatAppletContext>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WeChatAppletContext()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public WeChatAppletContext(DbContextOptions<WeChatAppletContext> options)
            : base(options)
        {
        }


        /// <summary>
        /// ApplicationMenu
        /// </summary>
        public virtual DbSet<ApplicationMenu> ApplicationMenu { get; set; }



        /// <summary>
        /// OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);

        }

        /// <summary>
        /// OnModelCreatingPartial
        /// </summary>
        /// <param name="modelBuilder"></param>
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

