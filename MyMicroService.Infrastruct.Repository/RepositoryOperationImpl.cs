using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyMicroService.Infrastruct.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMicroService.Infrastruct.Repository
{
    /// <summary>
    /// 仓储操作实现类
    /// </summary>
    /// <typeparam name="TDbContext">数据上下文</typeparam>
    public class RepositoryOperationImpl<TDbContext> : DbContextFactoryImpl<TDbContext>, IRepositoryOperation<TDbContext>
        where TDbContext : BaseDbContext<TDbContext>, new()
    {
        /// <summary>
        /// 数据上下文(读)
        /// </summary>
        /// <returns></returns>
        public sealed override DbContext GetReadDbContext()
        {
            return base.GetReadDbContext();
        }

        /// <summary>
        /// 数据上下文(写)
        /// </summary>
        /// <returns></returns>
        public sealed override DbContext GetWriteDbContext()
        {
            return base.GetWriteDbContext();
        }

        /// <summary>
        ///  数据上下文
        /// </summary>
        /// <returns></returns>
        public virtual DbContext GetDbContext()
        {
            return GetWriteDbContext();
        }

        #region 事务
        /// <summary>
        /// 事务对象
        /// </summary>
        private IDbContextTransaction _dbContextTransaction;

        /// <summary>
        /// 是否存在事务
        /// </summary>
        /// <returns></returns>
        public virtual bool IsExistTransaction()
        {
            return _dbContextTransaction != null;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        public virtual IDbContextTransaction BeginTransaction()
        {
            IDbContextTransaction dbContextTransaction = GetDbContext().Database.BeginTransaction();
            this._dbContextTransaction = dbContextTransaction;
            return dbContextTransaction;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public virtual void CommitTransaction()
        {
            this._dbContextTransaction.Commit();
            this._dbContextTransaction.Dispose();
            this._dbContextTransaction = null;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public virtual void RollbackTransaction()
        {
            this._dbContextTransaction.Rollback();
            this._dbContextTransaction.Dispose();
            this._dbContextTransaction = null;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return this.GetReadDbContext().Set<TEntity>().AsQueryable();
        }

        /// <summary>
        /// 获取是否存在
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual bool Any<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return this.GetReadDbContext().Set<TEntity>().Where(where).Any();
        }

        /// <summary>
        /// 获取是否存在（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return await this.GetReadDbContext().Set<TEntity>().Where(where).AnyAsync();
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual int Count<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return this.GetReadDbContext().Set<TEntity>().Where(where).Count();
        }

        /// <summary>
        /// 获取行数（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual async Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return await this.GetReadDbContext().Set<TEntity>().Where(where).CountAsync();
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual TEntity Find<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return this.GetReadDbContext().Set<TEntity>().Where(where).FirstOrDefault();
        }

        /// <summary>
        /// 获取单个（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return await this.GetReadDbContext().Set<TEntity>().Where(where).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        public virtual TEntity Find<TEntity>(params object[] keyValues) where TEntity : class
        {
            return this.GetReadDbContext().Set<TEntity>().Find(keyValues);
        }

        /// <summary>
        /// 获取单个（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
        {
            return await this.GetReadDbContext().Set<TEntity>().FindAsync(keyValues);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return this.GetReadDbContext().Set<TEntity>().Where(where).ToList();
        }

        /// <summary>
        /// 获取集合（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return await this.GetReadDbContext().Set<TEntity>().Where(where).ToListAsync();
        }

        /// <summary>
        /// 获取分页集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual PageList<TEntity> GetPageList<TEntity>(int page, int rows, Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            PageList<TEntity> pageList = new PageList<TEntity>();
            pageList.Data = this.GetReadDbContext().Set<TEntity>().Where(where).Skip((page - 1) * rows).Take(rows).ToList();
            pageList.Total = this.GetReadDbContext().Set<TEntity>().Where(where).Count();
            return pageList;
        }

        /// <summary>
        /// 获取分页集合（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual async Task<PageList<TEntity>> GetPageListAsync<TEntity>(int page, int rows, Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            PageList<TEntity> pageList = new PageList<TEntity>();
            pageList.Data = await this.GetReadDbContext().Set<TEntity>().Where(where).Skip((page - 1) * rows).Take(rows).ToListAsync();
            pageList.Total = await this.GetReadDbContext().Set<TEntity>().Where(where).CountAsync();
            return pageList;
        }
        #endregion

        #region 增删改
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        public virtual bool Insert<TEntity>(TEntity entitie) where TEntity : class
        {
            if (entitie == null)
                throw new ArgumentNullException(nameof(entitie));
            this.GetWriteDbContext().Add(entitie);
            return this.GetWriteDbContext().SaveChanges() > 0;
        }

        /// <summary>
        /// 添加（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        public virtual async Task<bool> InsertAsync<TEntity>(TEntity entitie) where TEntity : class
        {
            if (entitie == null)
                throw new ArgumentNullException(nameof(entitie));
            await this.GetWriteDbContext().AddAsync(entitie);
            return await this.GetWriteDbContext().SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual bool Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            this.GetWriteDbContext().Add(entities);
            return this.GetWriteDbContext().SaveChanges() > 0;
        }

        /// <summary>
        /// 添加（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<bool> InsertAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            await this.GetWriteDbContext().AddAsync(entities);
            return await this.GetWriteDbContext().SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        public virtual bool Update<TEntity>(TEntity entitie) where TEntity : class
        {
            if (entitie == null)
                throw new ArgumentNullException(nameof(entitie));
            this.GetWriteDbContext().Update(entitie);
            return this.GetWriteDbContext().SaveChanges() > 0;
        }

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync<TEntity>(TEntity entitie) where TEntity : class
        {
            if (entitie == null)
                throw new ArgumentNullException(nameof(entitie));
            this.GetWriteDbContext().Update(entitie);
            return await this.GetWriteDbContext().SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual bool Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            this.GetWriteDbContext().Update(entities);
            return this.GetWriteDbContext().SaveChanges() > 0;
        }

        /// <summary>
        /// 修改（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            this.GetWriteDbContext().Update(entities);
            return await this.GetWriteDbContext().SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        public virtual bool Delete<TEntity>(TEntity entitie) where TEntity : class
        {
            if (entitie == null)
                throw new ArgumentNullException(nameof(entitie));
            this.GetWriteDbContext().Remove(entitie);
            return this.GetWriteDbContext().SaveChanges() > 0;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitie"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync<TEntity>(TEntity entitie) where TEntity : class
        {
            if (entitie == null)
                throw new ArgumentNullException(nameof(entitie));
            this.GetWriteDbContext().Remove(entitie);
            return await this.GetWriteDbContext().SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual bool Delete<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return this.Delete(this.GetList<TEntity>(where));
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return await this.DeleteAsync(await this.GetListAsync<TEntity>(where));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual bool Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            this.GetWriteDbContext().Remove(entities);
            return this.GetWriteDbContext().SaveChanges() > 0;
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            this.GetWriteDbContext().Remove(entities);
            return await this.GetWriteDbContext().SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public virtual bool Delete<TEntity>(params object[] keyValues) where TEntity : class
        {
            return this.Delete(Find<TEntity>(keyValues));
        }

        /// <summary>
        /// 删除（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync<TEntity>(params object[] keyValues) where TEntity : class
        {
            return await this.DeleteAsync(await FindAsync<TEntity>(keyValues));
        }
        #endregion

        #region 执行sql
        /// <summary>
        /// sql查询集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected IEnumerable<TEntity> SQLQuery<TEntity>(string sql, params object[] parameters)
        {
            return this.GetReadDbContext().Database.SqlQuery<TEntity>(sql, parameters);
        }

        /// <summary>
        /// sql查询集合（异步）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected async Task<IEnumerable<TEntity>> SQLQueryAsync<TEntity>(string sql, params object[] parameters)
        {
            return await this.GetReadDbContext().Database.SqlQueryAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// sql查询Table
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected DataTable SQLQuery(string sql, params object[] parameters)
        {
            return this.GetReadDbContext().Database.SqlQuery(sql, parameters);
        }

        /// <summary>
        /// sql查询单个
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected TEntity SQLQueryFind<TEntity>(string sql, params object[] parameters)
        {
            return this.GetReadDbContext().Database.SQLQueryFind<TEntity>(sql, parameters);
        }

        /// <summary>
        /// sql查询单个
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected async Task<TEntity> SQLQueryFindAsync<TEntity>(string sql, params object[] parameters)
        {
            return await this.GetReadDbContext().Database.SQLQueryFindAsync<TEntity>(sql, parameters);
        }
        #endregion
    }
}
