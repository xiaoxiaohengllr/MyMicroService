using System.Collections.Generic;

namespace MyMicroService.Infrastruct.Repository
{
    /// <summary>
    /// 连接字符实体
    /// </summary>
    public class ConnectionEntity
    {
        /// <summary>
        /// 连接字符
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public int DbType { get; set; }

        /// <summary>
        /// 读连接字符集合（读写分离使用）
        /// </summary>
        public List<string> ReadConnection { get; set; }
    }
}
