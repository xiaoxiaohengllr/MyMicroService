using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

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
            if (Directory.Exists("Context"))
            {
                Directory.Delete("Context", true);
            }
            Directory.CreateDirectory("Context");
            if (Directory.Exists("Models"))
            {
                Directory.Delete("Models", true);
            }
            Directory.CreateDirectory("Models");
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
            StreamWriter($"Service/{entryName}BaseServiceImpl.cs", @$"
                        using MyMicroService.Infrastruct.BaseService;
                        using MyMicroService.Infrastruct.IRepository;
                        using MyMicroService.Service.{entryName}Service.Context;
                        using MyMicroService.Service.{entryName}Service.IService;

                        namespace MyMicroService.Service.{entryName}Service.Service
                        {{
                            /// <summary>
                            /// {entryName}基类实现
                            /// </summary>
                            /// <typeparam name=""TEntity""></typeparam>
                            public class {entryName}BaseServiceImpl<TEntity> : BaseServiceImpl<TEntity, {entryName}Context>, I{entryName}BaseService<TEntity>
                                 where TEntity : class
                            {{

                            }}
                        }}

             ");

            StringBuilder contextStr = new StringBuilder();
            foreach (DataRow surfaceDataRow in surfaceDataTable.Rows)
            {
                string tableName = surfaceDataRow["table_name"].ToString();

                contextStr.AppendLine(@$"
                /// <summary>
                /// {tableName}
                /// </summary>
                public virtual DbSet<{tableName}> {tableName} {{ get; set; }}
                ");

                //表对应接口
                StreamWriter($"IService/I{tableName}Service.cs", @$"
                        using MyMicroService.Service.{entryName}Service.Models;

                        namespace MyMicroService.Service.{entryName}Service.IService
                        {{
                            /// <summary>
                            /// {tableName}接口
                            /// </summary>
                            public interface I{tableName}Service : I{entryName}BaseService<{tableName}>
                            {{
                            }}
                        }}
                 ");

                //表对应实现
                StreamWriter($"Service/{tableName}ServiceImpl.cs", @$"
                        using MyMicroService.Service.{entryName}Service.IService;
                        using MyMicroService.Service.{entryName}Service.Models;

                        namespace MyMicroService.Service.{entryName}Service.Service
                        {{
                            /// <summary>
                            /// {tableName}基类实现
                            /// </summary>
                            public class {tableName}ServiceImpl : {entryName}BaseServiceImpl<{tableName}>, I{tableName}Service
                            {{
                            }}
                        }}

                 ");

                StringBuilder tabStr = new StringBuilder();

                DataTable fieldDataTable = ExecuteTable(conn, $"select * from INFORMATION_SCHEMA.Columns where table_name='{tableName}' and table_schema='{entryName}'");//获取表中字段
                foreach (DataRow fieldDataRow in fieldDataTable.Rows)
                {
                    tabStr.AppendLine(@$"
                    /// <summary>
                    /// {fieldDataRow["COLUMN_COMMENT"].ToString()}
                    /// </summary>
                    [Display(Name = ""{fieldDataRow["COLUMN_COMMENT"].ToString()}"",Order ={Convert.ToInt32(fieldDataRow["ORDINAL_POSITION"])})]");
                    if (fieldDataRow["COLUMN_KEY"] != DBNull.Value && fieldDataRow["COLUMN_KEY"].ToString() == "PRI")
                    {
                        tabStr.AppendLine(@$"[Key]");
                    }
                    tabStr.AppendLine(@$"public {MySqlDataTypeToType(fieldDataRow["DATA_TYPE"].ToString())} {fieldDataRow["COLUMN_NAME"].ToString()} {{ get; set; }}");
                }

                StreamWriter($"Models/{tableName}.cs", @$"
                        using System;
                        using System.ComponentModel.DataAnnotations;

                        namespace MyMicroService.Service.{entryName}Service.Models
                        {{
                            /// <summary>
                            /// {tableName}
                            /// </summary>
                            public partial class {tableName}
                            {{
                                {tabStr.ToString()}
                            }}
                        }}
                 ");
            }

            //Context
            StreamWriter($"Context/{entryName}Context.cs", @$"
            using Microsoft.EntityFrameworkCore;
            using MyMicroService.Infrastruct.IRepository;
            using MyMicroService.Service.{entryName}Service.Models;

            namespace MyMicroService.Service.{entryName}Service.Context
            {{
                /// <summary>
                /// 任务调度上下文
                /// </summary>
                public partial class {entryName}Context : BaseDbContext<{entryName}Context>
                {{
                    /// <summary>
                    /// 构造函数
                    /// </summary>
                    public {entryName}Context()
                    {{
                    }}

                    /// <summary>
                    /// 构造函数
                    /// </summary>
                    /// <param name=""options""></param>
                    public {entryName}Context(DbContextOptions<{entryName}Context> options)
                        : base(options)
                    {{
                    }}

                    {contextStr.ToString()}

                    /// <summary>
                    /// OnConfiguring
                    /// </summary>
                    /// <param name=""optionsBuilder""></param>
                    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                    {{
                    }}

                    /// <summary>
                    /// OnModelCreating
                    /// </summary>
                    /// <param name=""modelBuilder""></param>
                    protected override void OnModelCreating(ModelBuilder modelBuilder)
                    {{
                            OnModelCreatingPartial(modelBuilder);
                
                    }}

                    /// <summary>
                    /// OnModelCreatingPartial
                    /// </summary>
                    /// <param name=""modelBuilder""></param>
                    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
                }}
            }}
            ");
        }

        public static string MySqlDataTypeToType(string mySqlDataType)
        {
            if (mySqlDataType.ToLower() == "int".ToLower())
            {
                return "int";
            }
            else if (mySqlDataType.ToLower() == "text".ToLower())
            {
                return "string";
            }
            else if (mySqlDataType.ToLower() == "bigint".ToLower())
            {
                return "Int64";
            }
            else if (mySqlDataType.ToLower() == "binary".ToLower())
            {
                return "byte[]";
            }
            else if (mySqlDataType.ToLower() == "bit".ToLower())
            {
                return "bool";
            }
            else if (mySqlDataType.ToLower() == "char".ToLower())
            {
                return "string";
            }
            else if (mySqlDataType.ToLower() == "datetime".ToLower())
            {
                return "DateTime";
            }
            else if (mySqlDataType.ToLower() == "decimal".ToLower())
            {
                return "decimal";
            }
            else if (mySqlDataType.ToLower() == "float ".ToLower())
            {
                return "double";
            }
            else if (mySqlDataType.ToLower() == "image".ToLower())
            {
                return "byte[]";
            }
            else if (mySqlDataType.ToLower() == "money".ToLower())
            {
                return "decimal";
            }
            else if (mySqlDataType.ToLower() == "nchar".ToLower())
            {
                return "string";
            }
            else if (mySqlDataType.ToLower() == "ntext".ToLower())
            {
                return "string";
            }
            else if (mySqlDataType.ToLower() == "numeric".ToLower())
            {
                return "decimal";
            }
            else if (mySqlDataType.ToLower() == "nvarchar".ToLower())
            {
                return "string";
            }
            else if (mySqlDataType.ToLower() == "real".ToLower())
            {
                return "Single";
            }
            else if (mySqlDataType.ToLower() == "smalldatetime ".ToLower())
            {
                return "DateTime";
            }
            else if (mySqlDataType.ToLower() == "smallint".ToLower())
            {
                return "Int16";
            }
            else if (mySqlDataType.ToLower() == "smallmoney".ToLower())
            {
                return "decimal";
            }
            else if (mySqlDataType.ToLower() == "timestamp".ToLower())
            {
                return "DateTime";
            }
            else if (mySqlDataType.ToLower() == "tinyint".ToLower())
            {
                return "byte";
            }
            else if (mySqlDataType.ToLower() == "uniqueidentifier".ToLower())
            {
                return "Guid";
            }
            else if (mySqlDataType.ToLower() == "varbinary".ToLower())
            {
                return "byte[]";
            }
            else if (mySqlDataType.ToLower() == "varchar".ToLower())
            {
                return "string";
            }
            else if (mySqlDataType.ToLower() == "Variant ".ToLower())
            {
                return "object";
            }
            else if (mySqlDataType.ToLower() == "json".ToLower())
            {
                return "string";
            }
            return "string";
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
