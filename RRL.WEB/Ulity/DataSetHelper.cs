using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace RRL.WEB.Ulity
{
    /// <summary>
    /// DataSet辅助处理类
    /// </summary>
    public class DataSetHelper
    {
        /// <summary>
        /// DataTable转实体列表
        /// </summary>
        /// <param name="Table">DataTable</param>
        /// <returns>实体列表</returns>
        public static IList<T> ConvertDataTableToList<T>(DataTable Table)
        {
            if (Table == null)
                return new List<T>();
            List<T> list = new List<T>();
            T model = default(T);

            if (Table.Columns.Contains("RowNum"))
            {
                Table.Columns.Remove("RowNum");
            }

            foreach (DataRow Row in Table.Rows)
            {
                model = Activator.CreateInstance<T>();
                //var allProp= model.GetType().GetProperties();
               
                foreach (DataColumn Column in Row.Table.Columns)
                {
                    PropertyInfo pi = model.GetType().GetProperty(Column.ColumnName);
                    if (pi != null)
                    {
                        if (Row[Column.ColumnName] != DBNull.Value)
                            pi.SetValue(model, Row[Column.ColumnName]);
                        else
                            pi.SetValue(model, null);
                    }
                }
                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// DataRow转实体对象
        /// </summary>
        /// <param name="Row">DataRow</param>
        /// <returns>实体对象</returns>
        public static T ConvertDataRowToObj<T>(DataRow Row)
        {
            T model = default(T);
            model = Activator.CreateInstance<T>();

            foreach (DataColumn Column in Row.Table.Columns)
            {
                PropertyInfo pi = model.GetType().GetProperty(Column.ColumnName);
                if (Row[Column.ColumnName] != DBNull.Value)
                    pi.SetValue(model, Row[Column.ColumnName], null);
                else
                    pi.SetValue(model, null, null);
            }

            return model;
        }

        /// <summary>
        /// DataRows转Datatable
        /// </summary>
        /// <param name="Rows">Datarow数组</param>
        /// <returns></returns>
        public static DataTable ConvertRowsToTable(DataRow[] Rows)
        {
            if (Rows == null || Rows.Length == 0)
                return null;
            DataTable Table = Rows[0].Table.Clone(); 
            foreach (DataRow Row in Rows)
                Table.Rows.Add(Row.ItemArray);
            return Table;
        }

        /// <summary>
        /// 提取DataTable中一列为数组
        /// </summary>
        /// <param name="Table">表</param>
        /// <param name="ColumnName">列名</param>
        /// 一般用来转换数字和字符串
        /// <returns></returns>
        public static List<object> ConvertDataColumnToList(DataTable Table, string ColumnName)
        {
            List<object> List = new List<object>();
            if (Table.Columns.Count != 0)
            {
                foreach (DataRow Row in Table.Rows)
                {
                    List.Add(Row[ColumnName]);
                }
            }
            return List;
        }
    }
}