using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Manager
{
    public partial class GameManager
    {
        ///// <summary>
        ///// 获取游戏类型描述
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public string GetGameTypeMsg(int id)
        //{
        //    string Msg = ""; //结果类型(1=大,单，2=大,双，3=小,单，4=小,双，5=红)
        //    switch (id)
        //    {
        //        case 1:
        //            Msg = "大,单";
        //            break;
        //        case 2:
        //            Msg = "大,双";
        //            break;
        //        case 3:
        //            Msg = "小,单";
        //            break;
        //        case 4:
        //            Msg = "小,双";
        //            break;
        //        case 5:
        //            Msg = "红";
        //            break;
        //        default:
        //            Msg = "未知";
        //            break;
        //    }
        //    return Msg;
        //}

        ///// <summary>
        ///// 获取游戏类型
        ///// </summary>
        ///// <param name="num"></param>
        ///// <returns></returns>
        //public int GetGameType(int num)
        //{
        //    int result_type = 0;
        //    if (num < 25)
        //    {
        //        if (num < 13)
        //        {
        //            //小
        //            if (num % 2 == 0)
        //            {
        //                //双
        //                result_type = 4;
        //            }
        //            else
        //            {
        //                //单
        //                result_type = 3;
        //            }
        //        }
        //        else
        //        {
        //            //大
        //            if (num % 2 == 0)
        //            {
        //                //双
        //                result_type = 2;
        //            }
        //            else
        //            {
        //                //单
        //                result_type = 1;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        result_type = 5; //红
        //    }
        //    return result_type;
        //}

        //public List<SqlParameter> GetSqlParaList(int num, int uid, string play_id, double goldenBeans, double redPacket, double goldenCoin)
        //{
        //    var time_stamp = DateTime.Now;
        //    int result_type = GetGameType(num);  //结果类型(1=大,单，2=大,双，3=小,单，4=小,双，5=红)
        //    string ResultTypeMsg = GetGameTypeMsg(result_type);
        //    var paramList = new List<SqlParameter>()
        //        {
        //            new SqlParameter("@uid", uid){ DbType= System.Data.DbType.Int32 },
        //            new SqlParameter("@goldenBeans", goldenBeans){ DbType= System.Data.DbType.Decimal } ,
        //            new SqlParameter("@redPacket", redPacket){ DbType= System.Data.DbType.Decimal },
        //            new SqlParameter("@time_stamp", time_stamp){ DbType= System.Data.DbType.DateTime },
        //            new SqlParameter("@result_type", result_type){ DbType= System.Data.DbType.Int32 },
        //            new SqlParameter("@result_message", ResultTypeMsg){ DbType= System.Data.DbType.AnsiString },
        //            new SqlParameter("@play_id", play_id){ DbType= System.Data.DbType.AnsiString },
        //            new SqlParameter("@goldenCoin", goldenCoin){ DbType= System.Data.DbType.Decimal }
        //        };
        //    return paramList;
        //}

        ///// <summary>
        ///// 获取写入数据SQL
        ///// </summary>
        ///// <param name="num"></param>
        ///// <param name="uid"></param>
        ///// <param name="free"></param>
        ///// <param name="zs"></param>
        ///// <returns></returns>
        //public List<Game_Record_SQL_Param> GetGameSqlList(int num, int uid, LotteryModel free, LotteryModel zs)
        //{
        //    var Year = DateTime.Now.ToString("yyyy"); //获取年份

        //    string win_insert_sql = $@"INSERT INTO game_record_win_0001_{Year} ([uid], [goldenBeans], [redPacket], [time_stamp], [result_type], [result_message], [play_id], [goldenCoin]) VALUES 
        //            (@uid, @goldenBeans, @redPacket, @time_stamp, @result_type, @result_message, @play_id, @goldenCoin)";
        //    string lose_insert_sql = $@"INSERT INTO game_record_lose_0001_{Year} ([uid], [goldenBeans], [redPacket], [time_stamp], [result_type], [result_message], [play_id], [goldenCoin]) VALUES 
        //            (@uid, @goldenBeans, @redPacket, @time_stamp, @result_type, @result_message, @play_id, @goldenCoin)";
        //    List<Game_Record_SQL_Param> SqlList = new List<Game_Record_SQL_Param>();

        //    string play_id = Guid.NewGuid().ToString();

        //    int times = 2; //倍数(大，小，单，双)
        //    int times_hong = 12; //倍数(红)

        //    if (free.da > 0 || zs.da > 0)
        //    {
        //        if (num >= 13 && num < 25)
        //        {
        //            //win
        //            SqlList.Add(new Game_Record_SQL_Param()
        //            {
        //                sql = win_insert_sql,
        //                paramList = GetSqlParaList(num, uid, play_id, zs.da * times, free.da * times, 0.00)
        //            });
        //        }
        //        else
        //        {
        //            //lose
        //            SqlList.Add(new Game_Record_SQL_Param()
        //            {
        //                sql = lose_insert_sql,
        //                paramList = GetSqlParaList(num, uid, play_id, 0 - zs.da, 0 - free.da, 0.00)
        //            });
        //        }
        //    }
        //    if (free.xiao > 0 || zs.xiao > 0)
        //    {
        //        if (num > 0 && num < 13)
        //        {
        //            //win
        //            SqlList.Add(new Game_Record_SQL_Param()
        //            {
        //                sql = win_insert_sql,
        //                paramList = GetSqlParaList(num, uid, play_id, zs.xiao * times, free.xiao * times, 0.00)
        //            });
        //        }
        //        else
        //        {
        //            //lose
        //            SqlList.Add(new Game_Record_SQL_Param()
        //            {
        //                sql = lose_insert_sql,
        //                paramList = GetSqlParaList(num, uid, play_id, 0 - zs.xiao, 0 - free.xiao, 0.00)
        //            });
        //        }
        //    }
        //    if (free.dan > 0 || zs.dan > 0)
        //    {
        //        if (num % 2 != 0)
        //        {
        //            //win
        //            SqlList.Add(new Game_Record_SQL_Param()
        //            {
        //                sql = win_insert_sql,
        //                paramList = GetSqlParaList(num, uid, play_id, zs.dan * times, free.dan * times, 0.00)
        //            });
        //        }
        //        else
        //        {
        //            //lose
        //            SqlList.Add(new Game_Record_SQL_Param()
        //            {
        //                sql = lose_insert_sql,
        //                paramList = GetSqlParaList(num, uid, play_id, 0 - zs.dan, 0 - free.dan, 0.00)
        //            });
        //        }
        //    }
        //    if (free.shuang > 0 || zs.shuang > 0)
        //    {
        //        if (num % 2 == 0)
        //        {
        //            //win
        //            SqlList.Add(new Game_Record_SQL_Param()
        //            {
        //                sql = win_insert_sql,
        //                paramList = GetSqlParaList(num, uid, play_id, zs.shuang * times, free.shuang * times, 0.00)
        //            });
        //        }
        //        else
        //        {
        //            //lose
        //            SqlList.Add(new Game_Record_SQL_Param()
        //            {
        //                sql = lose_insert_sql,
        //                paramList = GetSqlParaList(num, uid, play_id, 0 - zs.shuang, 0 - free.shuang, 0.00)
        //            });
        //        }
        //    }
        //    if (free.hong > 0 || zs.hong > 0)
        //    {
        //        if (num >= 25)
        //        {
        //            //win
        //            SqlList.Add(new Game_Record_SQL_Param()
        //            {
        //                sql = win_insert_sql,
        //                paramList = GetSqlParaList(num, uid, play_id, zs.hong * times_hong, free.hong * times_hong, 0.00)
        //            });
        //        }
        //        else
        //        {
        //            //lose
        //            SqlList.Add(new Game_Record_SQL_Param()
        //            {
        //                sql = lose_insert_sql,
        //                paramList = GetSqlParaList(num, uid, play_id, 0 - zs.hong, 0 - free.hong, 0.00)
        //            });
        //        }
        //    }
        //    return SqlList;
        //}
    }
}
