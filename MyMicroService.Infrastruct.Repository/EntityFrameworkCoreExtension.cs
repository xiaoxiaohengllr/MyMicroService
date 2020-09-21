using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace MyMicroService.Infrastruct.Repository
{
    /// <summary>
    /// EF扩展
    /// </summary>
    public static class EntityFrameworkCoreExtension
    {
        /// <summary>
        /// Dapper执行sql查询集合
        /// </summary>
        /// <typeparam name="TEntity">查询的类型</typeparam>
        /// <param name="facade">数据库上下文访问对象</param>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> SqlQuery<TEntity>(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            return facade.GetDbConnection().Query<TEntity>(sql, parameters);
        }

        /// <summary>
        /// Dapper执行sql查询集合(异步)
        /// </summary>
        /// <typeparam name="TEntity">查询的类型</typeparam>
        /// <param name="facade">数据库上下文访问对象</param>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static async Task<IEnumerable<TEntity>> SqlQueryAsync<TEntity>(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            return await facade.GetDbConnection().QueryAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// 执行sql查询DataTable
        /// </summary>
        /// <param name="facade">数据库上下文访问对象</param>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static DataTable SqlQuery(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            DbConnection conn = null;
            try
            {
                var command = CreateCommand(facade, sql, out conn, parameters);
                var reader = command.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        /// <summary>
        /// Dapper执行sql查询单个
        /// </summary>
        /// <typeparam name="TEntity">查询的类型</typeparam>
        /// <param name="facade">数据库上下文访问对象</param>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static TEntity SQLQueryFind<TEntity>(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            return facade.GetDbConnection().QueryFirstOrDefault<TEntity>(sql, parameters);
        }

        /// <summary>
        /// Dapper执行sql查询单个(异步)
        /// </summary>
        /// <typeparam name="TEntity">查询的类型</typeparam>
        /// <param name="facade">数据库上下文访问对象</param>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static async Task<TEntity> SQLQueryFindAsync<TEntity>(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            return await facade.GetDbConnection().QueryFirstOrDefaultAsync<TEntity>(sql, parameters);
        }

        public static TEntity ScalarSqlQuery<TEntity>(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            DbConnection conn = null;
            try
            {
                var command = CreateCommand(facade, sql, out conn, parameters);
                var result = command.ExecuteScalar();
                conn.Close();
                return (TEntity)result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        /// <summary>
        /// 获取数据库命令
        /// </summary>
        /// <param name="facade">数据库上下文访问对象</param>
        /// <param name="sql">sql语句</param>
        /// <param name="connection">数据连接上下文</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection connection, params object[] parameters)
        {
            connection = facade.GetDbConnection();
            connection.Open();
            var cmd = connection.CreateCommand();
            if (facade.IsSqlServer())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
            }
            return cmd;
        }
    }
}
