using RRL.DB;
using System;
using System.Data;

namespace RRL.Core
{
    internal class Preprocessor
    {
        public static DataSet QueryDataset(string tableName)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery($@"SELECT * FROM {tableName}");
            db.Close();
            return ds;
        }

        public static DataSet QueryPaginateDataset(string tableName, int page, int pageSize)
        {
            int pageId = Math.Max(page, 1);
            int offSet = (pageId - 1) * pageSize;
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(
                $@"SELECT TOP {
                    pageSize
                } * FROM(SELECT ROW_NUMBER()OVER(ORDER BY id DESC)AS RowNum ,* FROM {
                    tableName
                })AS TEMP WHERE TEMP.RowNum > {offSet}");
            db.Close();
            return ds;
        }

        public static string QueryPaginateString(string ulityString, int page, int pageSize)
        {
            int pageId = Math.Max(page, 1);
            int offSet = (pageId - 1) * pageSize; 
            return string.Format(ulityString, pageSize, offSet);
        }
      

        public static string ExQueryPaginateString(string ulityString, int page, int pageSize,string orderField,string orderType)
        {
            int pageId = Math.Max(page, 1);
            int offSet = (pageId - 1) * pageSize;
            return string.Format(ulityString, pageSize, offSet,orderField,orderType);
        }
    }
}