using MyMicroService.Infrastruct.IRepository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMicroService.Infrastruct.BaseService
{
    /// <summary>
    /// 服务接基类口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IBaseService<TEntity, TDbContext>
         where TEntity : class
         where TDbContext : BaseDbContext<TDbContext>, new()
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where = null);

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> where = null);

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        Task<TEntity> GetEntityAsync(params object[] keyValues);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        Task<PageList<TEntity>> GetPageListAsync(int page, int rows, Expression<Func<TEntity, bool>> where = null);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">添加的实体</param>
        /// <returns></returns>
        Task<string> InsertAsync(TEntity entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">修改的实体</param>
        /// <returns></returns>
        Task<string> UpdateAsync(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        Task<string> DeleteAsync(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        Task<string> DeleteAsync(params object[] keyValues);
    }
}
