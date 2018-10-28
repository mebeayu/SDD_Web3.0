using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using RRL.Config;

namespace RRL.Core.Manager
{
    public partial class UserManager
    {
        /// <summary>
        /// 获取推荐人列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="res"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public object getSperaderListByUid(int uid, out int res, out string errorMessage, int pageIndex = 1, int pageSize = 40)
        {
            res = 0;
            errorMessage = "";
            SqlDataBase db = new SqlDataBase();
            var list = db.ExecuteStoredProcedure<dynamic>("sp_V3_getSperaderListByUid", new { uid = uid, pageIndex = pageIndex, pageSize = pageSize });
            var list_count = db.ExecuteStoredProcedure<dynamic>("sp_V3_getSperaderCountByUid", new { uid = uid });
            dynamic lv_one_total_spreader_count = list_count.First().lv_one_total_spreader_count;

            return new { id = uid, lv_one_total_spreader_count = lv_one_total_spreader_count, list = list };
        }


        /// <summary>
        /// 获取未确认订单的总体奖励
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="yearmonth">yyyy-MM 格式的月份</param>
        /// <returns></returns>
        public decimal getUnConfirmOrderSum_Reward(int uid, string yearmonth)
        {
            SqlDataBase db = new SqlDataBase();
            var list = db.ExecuteStoredProcedure<decimal>("sp_V3_getUnConfirmOrderSum_Reward", new { uid = uid, yearmonth = yearmonth });
            if (list != null && list.Count > 0)
            {
                return list.First();
            }
            return 0m;
        }

        /// <summary>
        /// 更新微信openid
        /// </summary>
        /// <param name="username">用户手机号码</param>
        /// <param name="channel_open_id">wx的openid</param>
        public int UpdateOpenIdByUserName(string username, string channel_open_id)
        {
            SqlDataBase db = new SqlDataBase();
            var sql = "update rrl_user set wx_open_id=@wx_open_id where username=@username";
            int eff = db.Execute(sql, new { wx_open_id = channel_open_id, username = username });
            return eff;
        }

        // lcl 20180627 Modify (修改为根据用户等级类型获取门槛数据)
        /// <summary>
        /// 查询门槛配置
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetUserHoldMoneyConfig(string userCategory = "A")
        {
            SqlDataBase db = new SqlDataBase();
            var sql = "select * from rrl_user_hold_money_config(nolock) where (user_category = @user_category) order by pay_coupons_total_money_start asc";
            var eff = db.Select<dynamic>(sql, new { user_category = userCategory });
            return eff;
        }

        /*
           lcl 20180627 Modify (修改门槛规则)
           支付门槛规则：
             1）判断用户等级类型，根据rrl_user表中的level_category字段；
             2）如果是新用户的等级类型，则判断是否新用户（从注册日期算起，多少日内算新用户，日数据从配置中获取）；
             3）如果是新用户每日仅限使用多少金豆抵扣，金豆数额从配置中获取；
             4）其他情况执行无限门槛流程 
         */
        /// <summary>
        /// 查询当前用户的门槛
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public decimal GetUserHoldMoney(UserAuth user)
        {
            // lcl 2018-07-24 Delete 删除新用户0门槛的规则
            //if (user.level_category.ToUpper() == "A" && IfNewUser(user))
            //{
            //    // 获取新用户当天下单总共已经抵扣的金豆数
            //    decimal dBeanTotalDaily = new TradeManager().GetOrderGoldBeansTodayByUser(user.id);
            //    // 获取配置的金豆限制值
            //    decimal dCfgMaxBeanNumDaily = 0m;
            //    decimal.TryParse(new ConfigManager().GetConfigValue("NewUser_MaxBeanNum_Daily"), out dCfgMaxBeanNumDaily);
            //    // 判断该用户当天下单总共抵扣的金豆数是否超过设定值
            //    if (dBeanTotalDaily < dCfgMaxBeanNumDaily)
            //    {
            //        // 如果是新用户，并且未超过限额，则允许用户无门槛支付(返回值0，仅针对新用户无门槛的情况)
            //        return 0m;
            //    }
            //}

            // 获取当前用户对应的等级类型的门槛配置数据
            var list = GetUserHoldMoneyConfig(user.level_category);
            foreach (var item in list)
            {
                if (item.pay_coupons_total_money_start <= user.pay_coupons_total_money && item.pay_coupons_total_money_end > user.pay_coupons_total_money)
                {
                    return item.platform_hold_money;
                }
            }
            return 800m * RRLConfig.RMB_To_GoldBean_Rate;
        }

        // lcl 20180628 Insert
        /// <summary>
        /// 是否为新用户 
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>true：新用户；false：老用户</returns>
        public bool IfNewUser(UserAuth user)
        {
            int intDays = 0;
            int.TryParse(new ConfigManager().GetConfigValue("NewUser_Confirm_Days"), out intDays);
            if ((DateTime.Now - user.addtime).TotalDays <= intDays)
            {
                // 用户注册时间在指定的天数范围内的，则认为是新用户
                return true;
            }

            return false;
        }
    }
}