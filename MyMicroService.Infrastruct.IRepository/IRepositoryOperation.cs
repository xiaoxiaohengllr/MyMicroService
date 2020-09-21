using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMicroService.Infrastruct.IRepository
{
    /// <summary>
    /// 仓储操作接口
    /// </summary>
    /// <typeparam name="TDbContext">数据上下文</typeparam>
    public interface IRepositoryOperation<TDbContext> : IDbContextFactory<TDbContext>
         where TDbContext : BaseDbContext<TDbContext>, new()
    {
        #region 事务
        /// <summary>
        /// 是否存在事务
        /// </summary>
        /// <returns></returns>
        public bool IsExistTransaction();

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public void CommitTransaction();


        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns></returns>
        public void RollbackTransaction();
        #endregion

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>() where TEntity : class;

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        int Count<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 获取行数（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 获取是否存在
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        bool Any<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 获取是否存在（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 获取集合（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 获取分页集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        PageList<TEntity> GetPageList<TEntity>(int page, int rows, Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 获取分页集合（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<PageList<TEntity>> GetPageListAsync<TEntity>(int page, int rows, Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        TEntity Find<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 获取单个（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;

        /// <summary>
        /// 获取单个（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;
        #endregion

        #region 增删改
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        bool Insert<TEntity>(TEntity entitie) where TEntity : class;

        /// <summary>
        /// 添加（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        Task<bool> InsertAsync<TEntity>(TEntity entitie) where TEntity : class;

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 添加（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> InsertAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        bool Update<TEntity>(TEntity entitie) where TEntity : class;

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync<TEntity>(TEntity entitie) where TEntity : class;

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        bool Delete<TEntity>(TEntity entitie) where TEntity : class;

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync<TEntity>(TEntity entitie) where TEntity : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Delete<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        bool Delete<TEntity>(params object[] keyValues) where TEntity : class;

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync<TEntity>(params object[] keyValues) where TEntity : class;
        #endregion
    }
}
