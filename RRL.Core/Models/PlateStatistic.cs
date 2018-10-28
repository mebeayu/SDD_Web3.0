using RRL.DB;
using System;
using System.Data;
using System.Globalization;

namespace RRL.Core.Models
{
    public class PlateStatistic
    {
        private static DataSet StatisticalData = null;
        private static DateTime Stamp = DateTime.MinValue;

        public static DataSet GetData()
        {
            if (DateTime.Compare(DateTime.Now, Stamp) > 0 || StatisticalData == null)
            {
                InitStatistic();
            }
            return StatisticalData;
        }

        private static void InitStatistic()
        {
            var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(-1).ToString(CultureInfo.InvariantCulture);
            var end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).AddDays(-1).ToString(CultureInfo.InvariantCulture);

            var sql = $@"SELECT ISNULL((SELECT SUM(money) FROM rrl_trans_record WHERE addtime > '{start}' AND addtime < '{end}'),0) AS total_trans,ISNULL((SELECT SUM(money) FROM rrl_user_money_record WHERE type = 2 AND	addtime > '{start}' AND addtime < '{end}'),0) AS total_return,( SELECT SUM(plate_to_return_weight) FROM rrl_user WHERE plate_to_return_weight <> 0) AS total_weight";
            try
            {
                RRLDB db = new RRLDB();
                StatisticalData = db.ExeQuery(sql);
                db.Close();
                if (StatisticalData.Tables[0].Rows.Count != 0)
                {
                    Stamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                }
            }
            catch
            {
                StatisticalData = null;
            }
        }
    }
}