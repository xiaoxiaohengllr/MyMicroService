using MyMicroService.Infrastruct.ExceptionModels;
using MyMicroService.Infrastruct.IRepository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMicroService.Infrastruct.BaseService
{
    /// <summary>
    /// 服务基类实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public class BaseServiceImpl<TEntity, TDbContext> : IBaseService<TEntity, TDbContext>
         where TEntity : class
         where TDbContext : BaseDbContext<TDbContext>, new()
    {
        /// <summary>
        /// 服务仓储
        /// </summary>
        public virtual IRepositoryOperation<TDbContext> ServiceContext { get; set; }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if (where == null)
                where = tEntity => 1 == 1;
            return await this.ServiceContext.GetListAsync<TEntity>(where);
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if (where == null)
                where = tEntity => 1 == 1;
            return await this.ServiceContext.FindAsync<TEntity>(where);
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetEntityAsync(params object[] keyValues)
        {
            return await this.ServiceContext.FindAsync<TEntity>(keyValues);
        }


        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual async Task<PageList<TEntity>> GetPageListAsync(int page, int rows, Expression<Func<TEntity, bool>> where = null)
        {
            if (where == null)
                where = tEntity => 1 == 1;
            return await this.ServiceContext.GetPageListAsync<TEntity>(page, rows, where);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">添加的实体</param>
        /// <returns></returns>
        public async virtual Task<string> InsertAsync(TEntity entity)
        {
            bool success = await this.ServiceContext.InsertAsync(entity);
            if (!success)
            {
                throw new UserOperationException("添加失败");
            }
            return "";
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">修改的实体</param>
        /// <returns></returns>
        public async virtual Task<string> UpdateAsync(TEntity entity)
        {
            bool success = await this.ServiceContext.UpdateAsync(entity);
            if (!success)
            {
                throw new UserOperationException("修改失败");
            }
            return "";
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public async virtual Task<string> DeleteAsync(Expression<Func<TEntity, bool>> where)
        {
            bool success = await this.ServiceContext.DeleteAsync(where);
            if (!success)
            {
                throw new UserOperationException("删除失败");
            }
            return "";
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="where">要删除的实体</param>
        /// <returns></returns>
        public async virtual Task<string> DeleteAsync(TEntity entity)
        {
            bool success = await this.ServiceContext.DeleteAsync(entity);
            if (!success)
            {
                throw new UserOperationException("删除失败");
            }
            return "";
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        public async virtual Task<string> DeleteAsync(params object[] keyValues)
        {
            bool success = await this.ServiceContext.DeleteAsync(keyValues);
            if (!success)
            {
                throw new UserOperationException("删除失败");
            }
            return "";
        }
    }
}
