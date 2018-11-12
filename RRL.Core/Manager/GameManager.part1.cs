using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RRL.DB;
using System.Dynamic;
using RRL.Config;
using System.Text.RegularExpressions;
using RRL.Core.Models;
using System.Data;

namespace RRL.Core.Manager
{
    public partial class GameManager
    {
        static public bool IsCompletePlayCountToday(int uid ,out double h_money_pay)
        {
            h_money_pay = 0;
            var db = new RRLDB();
            DataSet ds = null;

            int PlayCountToday = 0;//今天玩的游戏局数

            string today = DateTime.Now.ToString("yyyy-MM-dd");
            ds = db.ExeQuery($@"select count from game_user_daily_count where uid={uid} and date='{today}'  and active=1");
            if (ds != null && ds.Tables[0].Rows.Count > 0) PlayCountToday = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            
            ds = db.ExeQuery($@"select h_money_pay,need_play_conut from rrl_user where id={uid}");
            if (ds != null && ds.Tables[0].Rows.Count > 0) h_money_pay = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
            int NeedPlayCount = Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString());
            db.Close();
            if (PlayCountToday >= NeedPlayCount) return true;
            else return false;
        }
        /// <summary>
        /// 获取所有有效的游戏数据
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetGameList()
        {
            return new SqlDataBase().Select<dynamic>($@"select * from rrl_game_list(nolock) where is_enable='1'");
        }

        /// <summary>
        /// 获取用户所有玩过的游戏的总次数
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        public int GetGamePlayTimesByUser(int uid)
        {
            var param = new { uid = uid };
            string strSql = $@"select isnull(sum(play_times),0)
                                 from game_user_play_times(nolock)
                                where ([uid] = @uid)";
            return new SqlDataBase().Select<int>(strSql, param).FirstOrDefault();
        }

        /// <summary>
        /// 获取用户所有玩过的游戏的总次数
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="gameType">游戏类型</param>
        /// <returns></returns>
        public int GetGamePlayTimesByType(int uid, string gameType)
        {
            string strSql = $@"select isnull(sum(play_times),0) as cnt
                                 from game_user_play_times(nolock)
                                where ([uid] = @uid)
                                  and (game_type = @game_type)";

            var cnt= new SqlDataBase().Select<int?>(strSql, new { uid = uid, game_type = gameType }).FirstOrDefault();
            if (cnt == null)
                return 0;
            return cnt.Value;
        }

        /// <summary>
        /// 根据资金类型获取用户玩过的游戏的总次数
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="moneyType">资金类型</param>
        /// <returns></returns>
        public int GetGamePlayTimesByMoneyType(int uid, string moneyType)
        {
            string strSql = $@"select isnull(sum(play_times),0) as cnt
                                 from game_user_play_times(nolock)
                                where ([uid] = @uid)
                                  and (money_type = @money_type)";

            var cnt = new SqlDataBase().Select<int?>(strSql, new { uid = uid, money_type = moneyType }).FirstOrDefault();
            if (cnt == null)
                return 0;
            return cnt.Value;
        }

        #region 红包裂变

        /// <summary>
        /// 获取用户红包裂变任务数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        public dynamic GetUserRedpackageFissionTaskData(int uid)
        {
            dynamic result = new ExpandoObject();
            result.taskStatus = -1; // 任务状态（1：没有领取过任务；2：已过期；3：未过期未完成；4：未过期已完成）
            result.isCanReceivedTask = 0; // 是否可以领取任务（1：可以；0：不可以）
            result.taskid = string.Empty; // 最近一次领取的任务的ID
            result.receivedMoney = 0m; // 已获得的红包总额
            result.receivedCount = 0; // 已分享并帮拆的红包总个数
            result.countdown = 0; // 用于显示倒计时的总秒数
            result.userList = null; // 帮拆用户列表

            SqlDataBase sqlDB = new SqlDataBase();
            // 任务数据
            string strSql = $@"select c.uid ,c.task_id ,t.task_receive_time
                                     ,isnull(t.receive_redpackage_total,0) as receive_redpackage_total
                                 from redpackage_fission_current_task(nolock) c
                                inner join redpackage_fission_task(nolock) t
                                   on c.task_id = t.id
                                where(c.uid = @uid)";
            var taskData = sqlDB.Single<dynamic>(strSql, new { uid = uid });
            if (taskData == null)
            {
                // 没有领取过任务
                result.taskStatus = 1;
                result.isCanReceivedTask = 1; // 没有领取过任务的，可以领取任务
                return result;
            }

            // 获取最近一次领取的任务的ID
            result.taskid = taskData.task_id;
            // 获取已获得的红包总额
            result.receivedMoney = taskData.receive_redpackage_total;
            // 帮拆红包的记录
            strSql = $@"select t.invitation_uid as uid
                              ,isnull(u.username, '') as username
                              ,isnull(u.head_pic, 0) as head_pic
                          from redpackage_fission_task_invitation(nolock) t
                          left join rrl_user(nolock) u
                            on t.invitation_uid = u.id
                         where (t.task_id = @task_id)
                           and (t.is_login = 1)
                         order by t.redpackage_number asc";

            var invitationData = sqlDB.Select<dynamic>(strSql, new { task_id = taskData.task_id });
            if (invitationData == null)
            {
                // 如果获取帮拆数据异常，则帮拆的红包总个数为0
                result.receivedCount = 0;
            }
            else
            {
                // 获取实际的帮拆的红包总个数
                result.receivedCount = invitationData.Count;
                if (invitationData.Count > 0)
                {
                    invitationData.ForEach(p => p.username = Regex.Replace(p.username, @"(\d{3})\d{4}(\d{4})", "$1****$2"));
                    result.userList = invitationData;
                }
            }

            TimeSpan timeSpanOfOneDay = new TimeSpan(24, 0, 0);
            DateTime dtNow = DateTime.Now;
            var timeDiff = timeSpanOfOneDay.TotalSeconds - (dtNow - taskData.task_receive_time).TotalSeconds;
            if (timeDiff < 0)
            {
                // 任务已过期
                result.taskStatus = 2;
                result.isCanReceivedTask = 1; // 前一个任务已过期，可以领取任务
            }
            else
            {
                // 任务未过期
                // 获取时间差所表示的总的秒数
                result.countdown = Math.Floor(timeDiff);
                // 帮拆次数未达到指定次数，表示该任务未完成
                if (result.receivedCount < RRLConfig.ReapackageCountForFinishFissionTask)
                {
                    // 未过期未完成
                    result.taskStatus = 3;
                }
                else
                {
                    // 未过期已完成
                    result.taskStatus = 4;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取用户领取红包的历史数据
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetRedpackageFissionHistory()
        {
            string strSql = $@"select t.uid ,u.username ,u.head_pic ,t.receive_redpackage_total
                                 from redpackage_fission_task(nolock)t
                                inner join rrl_user(nolock) u
                                   on u.id = t.[uid]
                                where (t.receive_redpackage_total > 0)
                                order by t.task_receive_time desc; ";

            return new SqlDataBase().Select<dynamic>(strSql);
        }

        /// <summary>
        /// 检查是否可以帮拆红包
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public dynamic CheckIfOpenFissionRedpackage(string taskId)
        {
            dynamic result = new ExpandoObject();
            result.status = 0;
            result.message = string.Empty;

            // 查询任务数据
            string strSql = $@"select count(0)
                                 from redpackage_fission_current_task(nolock) c
                                inner join redpackage_fission_task(nolock) t
                                   on c.task_id = t.id
                                where (datediff(ss, t.task_receive_time, getdate()) <= 24 * 60 * 60)
                                  and (c.task_id = @task_id)";
            var execResult = new SqlDataBase().ExecuteScalar<int>(strSql, new { task_id = taskId });
            if (execResult < 1)
            {
                result.status = 99;
                result.message = "该任务不存在，或任务已过期";
                return result;
            }

            strSql = $@"select count(0)
                          from redpackage_fission_task_invitation(nolock)
                         where (task_id = @task_id)
                           and (is_login = 1)";
            execResult = new SqlDataBase().ExecuteScalar<int>(strSql, new { task_id = taskId });
            if (execResult >=10)
            {
                result.status = 99;
                result.message = "该任务已经完成了帮拆红包的数量，请参与下一次的任务";
                return result;
            }

            return result;
        }

        /// <summary>
        /// 检查是否可以帮拆红包
        /// </summary>
        /// <param name="taskId">红包裂变任务ID</param>
        /// <param name="receiveTaskUid">红包裂变任务领取人的ID</param>
        /// <param name="openRedpackageUid">帮拆红包的用户ID</param>
        /// <returns></returns>
        public dynamic OpenFissionRedpackage(string taskId, int receiveTaskUid, int openRedpackageUid)
        {
            dynamic result = new ExpandoObject();

            // 执行领取任务的存储过程
            List<dynamic> resultWithSP = new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_RedpackageFissionOpen",
                new
                {
                    task_id = taskId,
                    receive_task_uid = receiveTaskUid,
                    open_redpackage_uid = openRedpackageUid
                });

            if (resultWithSP == null || resultWithSP.Count < 1)
            {
                // 存储过程执行失败
                result.status = -99;
                result.message = "存储过程执行失败";

                return result;
            }

            var resultData = resultWithSP.First();
            result.status = resultData.status;
            result.message = resultData.message;

            return result;
        }

        #endregion

        #region 分时段充值送红包

        public dynamic GetTimeIntervalRedpaketData(UserAuth user)
        {
            dynamic result = new ExpandoObject();
            SqlDataBase sqlDB = new SqlDataBase();

            result.isHasRedpacket = 0; // 是否有红包可以领取
            result.payMoney = 0m; // 需要充值支付的金额（RMB）
            result.redpacket = 0m; // 可以领取的红包数
            result.couponsMoney = 0m; // 购物券额度
            result.isExistUnpaid = 0; // 在满足领取条件前提下，是否存在未支付的领取记录
            result.receiveId = string.Empty; // 红包的领取记录ID
            result.type = 2;

            var now = DateTime.Now;
            var timestr = now.GetDateTimeFormats('T')[0];
            var balance = Convert.ToDecimal(user.h_money + user.h_money_free);

            string strSql = $@"select count(*)
                                 from time_interval_redpaket_config(nolock)
                                where (start_time < '{timestr}')
                                  and (end_time > '{timestr}')
                                  and ({balance} >= start_money)
                                  and ({balance} <= end_money)";
            var recordCount = sqlDB.ExecuteScalar<int>(strSql);
            if (recordCount <1)
            {
                // 没有分时段充值红包
                return result;
            }

            // 获取配置数据
            strSql = $@"select id ,receive_redpacket ,pay_money ,type
                          from time_interval_redpaket_config(nolock)
                         where (start_time < '{timestr}')
                           and (end_time > '{timestr}')";
            var configData = sqlDB.Single<dynamic>(strSql);
            if (configData == null || configData.pay_money <= 0)
            {
                // 未获取到配置数据
                return result;
            }

            // 构造用于标识当前用户当日在指定时段领取红包的领取记录ID
            // 领取记录ID = 用户ID + 年月日 + 资金类型 + 时段数据唯一ID。各部分之间使用"-"符号分隔
            string strReceiveId = $@"{user.id}-{now.ToString("yyyyMMdd")}-{configData.type}-{configData.id}";
            strSql = $@"select is_paid from time_interval_redpaket_receive(nolock) where (receive_id = '{strReceiveId}')";
            var receiveData = sqlDB.Single<dynamic>(strSql);
            if (receiveData != null && receiveData.is_paid == 1)
            {
                // 已经领取过的
                return result;
            }

            // 未领取过的，或者有领取记录但是未支付的，提示领取
            result.isHasRedpacket = 1;
            result.payMoney = configData.pay_money;
            result.redpacket = configData.receive_redpacket;
            result.couponsMoney = configData.pay_money;
            result.receiveId = strReceiveId;
            result.type = configData.type;
            if (receiveData != null && receiveData.is_paid == 0)
            {
                // 针对领取过的，但未支付的
                result.isExistUnpaid = 1;
            }

            return result;
        }

        #endregion
    }
}
