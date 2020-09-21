using Microsoft.EntityFrameworkCore;
using MyMicroService.Infrastruct.IRepository;
using MyMicroService.Service.TimingSchedulingService.Models;

namespace MyMicroService.Service.TimingSchedulingService.Context
{
    /// <summary>
    /// 任务调度上下文
    /// </summary>
    public partial class TimingSchedulingServiceContext : BaseDbContext<TimingSchedulingServiceContext>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TimingSchedulingServiceContext()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public TimingSchedulingServiceContext(DbContextOptions<TimingSchedulingServiceContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 任务计划
        /// </summary>
        public virtual DbSet<MissionPlan> MissionPlan { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public virtual DbSet<OperationType> OperationType { get; set; }

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
