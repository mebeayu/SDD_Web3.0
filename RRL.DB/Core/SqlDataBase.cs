using RRL.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.DB
{
    public class SqlDataBase : DataBase
    {
        public override string ConnectionString
        {
            get
            {
                return RRLConfig.DB_ConnectString;
            }
        }

        /// <summary>
        /// 尽少用
        /// </summary>
        /// <returns></returns>
        public override DbCommand CreateCommand()
        {
            return new System.Data.SqlClient.SqlCommand(); 
        }

        /// <summary>
        /// 常用
        /// </summary>
        /// <returns></returns>
        public override DbConnection CreateConnection()
        {
            DbConnection conn = new System.Data.SqlClient.SqlConnection(); 
            conn.ConnectionString = ConnectionString;
            return conn;
        }

        // lcl 20180509 Insert
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="pageSize">页大小（即，一页显示多少行）</param>
        /// <param name="pageIndex">页索引（即，显示第几页）</param>
        /// <param name="paramObject">SQL中使用的参数</param>
        /// <returns></returns>
        public   List<dynamic> SelectPage(string sql, int pageSize, int pageIndex, Object paramObject = null)
        {
            // 计算数据开始索引
            int intStartIndex = pageIndex * pageSize - pageSize;
            string strSql = $@"{sql} offset {intStartIndex} rows fetch next {pageSize} rows only";
            return Select<dynamic>(strSql, paramObject);
        }

        public   List<T> SelectPage<T>(string sql, int pageSize, int pageIndex, Object paramObject = null)
        {
            // 计算数据开始索引
            int intStartIndex = pageIndex * pageSize - pageSize;
            string strSql = $@"{sql} offset {intStartIndex} rows fetch next {pageSize} rows only";
            return Select<T>(strSql, paramObject);
        }
    }
}
