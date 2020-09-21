using System.Collections.Generic;

namespace MyMicroService.Infrastruct.IRepository
{
    /// <summary>
    /// 分页返回实体
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PageList<TEntity> where TEntity : class
    {
        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<TEntity> Data { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int Total { get; set; }
    }
}