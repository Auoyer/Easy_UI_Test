using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace DataAccessLayer
{
    public class DBHelper
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        private static string dbProviderName = string.Empty;

        /// <summary>
        /// 数据库连接字符窜
        /// </summary>
        private static string dbConnectionString = string.Empty;

        private DBHelper()
        {
        }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static DBHelper()
        {
            dbProviderName = "MySql.Data.MySqlClient";
            dbConnectionString = ConfigurationManager.AppSettings["MySQLConnString"];
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection CreateConnection()
        {
            //根据数据库名获取工厂
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(dbProviderName);
            //创建连接
            DbConnection dbconn = dbfactory.CreateConnection();
            //设置连接字符窜
            dbconn.ConnectionString = dbConnectionString;
            //返回连接
            return dbconn;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="T">要获取实体</typeparam>
        /// <param name="pageIndex">要获取的页数</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="model">分页参数</param>
        /// <param name="totalCount">数据总数量</param>
        /// <returns></returns>
        public static List<T> GetPageList<T>(int pageIndex, int pageSize, PageModel model, out int totalCount)
        {
            totalCount = 0;
            List<T> result = new List<T>();
            var param = new DynamicParameters();

            using (var conn = CreateConnection())
            {
                conn.Open();
                param.Add("@Tables", model.Tables, dbType: DbType.String);
                param.Add("@PK", model.PKey, dbType: DbType.String);
                param.Add("@Sort", model.Sort, dbType: DbType.String);
                param.Add("@PageNumber", pageIndex, dbType: DbType.Int32);
                param.Add("@PageSize", pageSize, dbType: DbType.Int32);
                param.Add("@Fields", model.Fields, dbType: DbType.String);
                param.Add("@Filter", model.Filter, dbType: DbType.String);
                param.Add("@isCount", model.IsCount, dbType: DbType.Boolean);
                param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);

                result = conn.Query<T>("Proc_CommonPagingStoredProcedure", param, commandType: CommandType.StoredProcedure).ToList();
                totalCount = param.Get<int>("@Total");
            }

            return result;
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static SqlConnection CreateConnection(string keyName)
        {
            SqlConnection sqlconn = new SqlConnection(ConfigurationManager.AppSettings[keyName].ToString());
            return sqlconn;
        }

        /// <summary>
        /// 多数据库服务器事务提交
        /// </summary>
        /// <param name="sqlStrings">key为connName,value为Sql语句</param>
        /// <returns></returns>
        public static bool ExecuteMultiTran(List<string[]> sqlStrings)
        {
            bool reval = true;

            SqlCommand cmd = new SqlCommand();
            SqlTransaction tran;
            SqlConnection conn;
            //事务对象名，事务对象的集合
            Dictionary<string, SqlTransaction> tranResult = new Dictionary<string, SqlTransaction>();

            //conn对象名，对象
            Dictionary<string, SqlConnection> connResult = new Dictionary<string, SqlConnection>();

            //当前是否执行成功
            bool isSuccess = true;

            List<string> keys = new List<string>();

            //通过connName进行循环执行事务
            foreach (string[] sqls in sqlStrings)
            {
                string keyName = sqls[0];

                //如果keys中已经存在当前 keyname，说明改conn的已经执行完毕，跳到下一keyname执行
                if (!keys.Contains(keyName))
                {
                    keys.Add(keyName);

                    //提交当前conn的事务，如果失败，标记当前事务失败
                    try
                    {
                        conn = CreateConnection(keyName);
                        conn.Open();
                        cmd.Connection = conn;
                        tran = conn.BeginTransaction();
                        cmd.Transaction = tran;

                        //记录当前事务
                        tranResult.Add(keyName, tran);

                        //记录当前conn
                        connResult.Add(keyName, conn);

                        //读取当前conn的sql，执行
                        foreach (string[] sql in sqlStrings)
                        {
                            if (sql[0] == keyName)
                            {
                                cmd.CommandText = sql[1];
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                    }

                    if (!isSuccess)
                    {
                        break;
                    }
                }
            }

            //如果当前事务失败，把执行过的所有事务对象rollBack
            if (!isSuccess)
            {
                foreach (SqlTransaction sqlTran in tranResult.Values)
                {
                    sqlTran.Rollback();
                }
                reval = false;
            }
            else
            {
                foreach (SqlTransaction sqlTran in tranResult.Values)
                {
                    sqlTran.Commit();
                }
            }
            //关闭conn
            foreach (SqlConnection value in connResult.Values)
            {
                if (value.State != ConnectionState.Closed)
                {
                    value.Close();
                }
            }
            return reval;
        }

    }

    public class PageModel
    {
        public PageModel()
        {
            PageSize = 10;
            IsCount = true;
        }

        /// <summary>
        /// 表名,多表请使用 tableA a inner join tableB b On a.AID = b.AID
        /// </summary>
        public string Tables { get; set; }

        /// <summary>
        /// 主键，可以带表头 a.AID
        /// </summary>
        public string PKey { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 开始页码即要查询的页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 读取字段
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// Where条件
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 是否获得总记录数
        /// </summary>
        public bool IsCount { get; set; }
    }
}