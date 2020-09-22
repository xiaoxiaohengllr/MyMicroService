using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace MyMicroService.Tool.CodeGenerationTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("参数不全");
            }
            string conn = args[0];//连接字符
            string entryName = args[1];//项目名称

            #region 清理之前文件
            if (Directory.Exists("IService"))
            {
                Directory.Delete("IService", true);
            }
            Directory.CreateDirectory("IService");
            if (Directory.Exists("Service"))
            {
                Directory.Delete("Service", true);
            }
            Directory.CreateDirectory("Service");
            #endregion

            DataTable surfaceDataTable = ExecuteTable(conn, $"select table_name from information_schema.tables where table_schema='{entryName}'");//获取数据下所有表

            //基类接口
            StreamWriter($"IService/I{entryName}BaseService.cs", @$"
                        using MyMicroService.Infrastruct.BaseService;
                        using MyMicroService.Service.{entryName}Service.Context;

                        namespace MyMicroService.Service.{entryName}Service.IService
                        {{
                            /// <summary>
                            /// {entryName}基类接口
                            /// </summary>
                            public interface I{entryName}BaseService<TEntity> : IBaseService<TEntity, {entryName}ServiceContext>
                                where TEntity : class
                            {{
                            }}
                        }}
             ");

            //基类实现
            StreamWriter($"Service/TimingSchedulingBaseServiceImpl.cs", @$"
                        using MyMicroService.Infrastruct.BaseService;
                        using MyMicroService.Service.{entryName}Service.Context;

                        namespace MyMicroService.Service.{entryName}Service.IService
                        {{
                            /// <summary>
                            /// {entryName}基类接口
                            /// </summary>
                            public interface I{entryName}BaseService<TEntity> : IBaseService<TEntity, {entryName}ServiceContext>
                                where TEntity : class
                            {{
                            }}
                        }}
             ");

            foreach (DataRow surfaceDataRow in surfaceDataTable.Rows)
            {
                string tableName = surfaceDataRow["table_name"].ToString();
                //表对应接口
                StreamWriter($"IService/I{tableName}Service.cs", @$"
                        using MyMicroService.Service.{entryName}Service.Models;

                        namespace MyMicroService.Service.{entryName}Service.IService
                        {{
                            /// <summary>
                            /// 操作类型接口
                            /// </summary>
                            public interface I{tableName}Service : I{entryName}BaseService<{tableName}>
                            {{
                            }}
                        }}
                 ");
            }
        }

        public static void StreamWriter(string fileNmae, string content)
        {
            FileStream fileStream = new FileStream(fileNmae, FileMode.CreateNew);
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine(content);
            }
        }

        public static DataTable ExecuteTable(string conn, string sql)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(conn))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
                DataSet dataSet = new DataSet();
                mySqlDataAdapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
        }
    }
}
