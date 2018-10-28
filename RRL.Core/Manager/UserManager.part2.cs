// PROJECT #8 lcl 20180425 Insert

using System;
using System.Collections.Generic;
using System.Linq;

using RRL.Config;
using RRL.DB;

namespace RRL.Core.Manager
{
    public partial class UserManager
    {
        #region 签到

        /// <summary>
        /// 获取指定用户，最近一个周期领取的签到免费红包数据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="existDataInValidCycles">返回参数，有效周期(7天为一个周期)内是否存在红包领取数据. true:存在; false:不存在</param>
        /// <param name="isCanGetRedpackage">返回参数，当天是否可以领取红包. true:当天可以领取; false:当天不能领取</param>
        /// <returns></returns>
        public List<dynamic> GetUserSignRedpackageFreeList(int userId, out bool existDataInValidCycles, out bool isCanGetRedpackage)
        {
            string strSQl = $@"select * from rrl_user_receive_day_redpackage
                                where [uid] = @uid
                                  and rec_date >= (select max(rec_date) from rrl_user_receive_day_redpackage where ([uid] = @uid) and (day_index = 1))
                                  and rec_date <= (select max(rec_date) from rrl_user_receive_day_redpackage where ([uid] = @uid))
                                order by rec_date asc";
            var userRedpackageList = new SqlDataBase().Select<dynamic>(strSQl, new { uid = userId });
            var now = DateTime.Now;
            // 从当前日期向前推算出当前有效周期的第一天的日期值
            var firstDayInValidCycles = now.AddDays(-(RRLConfig.CNT_SIGN_CYCLES_REDPACKAGE_FREE - 1)).Date;
            dynamic rec_firstDate;

            // 初始化为不存在有效周期内数据(没有用户的红包领取数据，或者数据在一个有效周期之外的，都认为有效周期内没有领取过红包)
            existDataInValidCycles = false;
            if (userRedpackageList != null && userRedpackageList.Count > 0)
            {
                rec_firstDate = userRedpackageList.First().rec_date;
                if (rec_firstDate >= firstDayInValidCycles)
                {
                    // 有效周期内有数据(天数索引等于1的日期在有效周期内，说明至少有一笔数据存在)
                    existDataInValidCycles = true;
                }
                // 判断当天是否可以领取红包
                isCanGetRedpackage = !userRedpackageList.Any(p => p.rec_date == now.Date);
            }
            else
            {
                // 没有用户的红包领取数据时，用户可以领取红包
                isCanGetRedpackage = true;
            }

            return userRedpackageList;
        }

        /// <summary>
        /// 获取红包配置信息
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetRedpackageConfigList()
        {
            string strSql = $@"select title ,day_index ,Ceiling([money] / 10000) as [money] ,min_money ,max_money ,extra_money, '0' as is_signin
                                 from rrl_redpackage_day_config(nolock)
                                where (is_enable = '1')
                                order by day_index";
            return new SqlDataBase().Select<dynamic>(strSql);
        }

        #endregion

        #region 推荐人数
        // lcl 20180426 Insert
        /// <summary>
        /// 获取与推荐的用户人数相关的统计数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="isPartner">是否为合伙人(1：是合伙人)</param>
        /// <param name="startTime">起始时间（进行大于等于操作）</param>
        /// <param name="endTime">截止时间（进行小于等于操作）</param>
        /// <param name="recommandUserCount">返回推荐用户数合计</param>
        /// <param name="eachLevelRecommandUserCount">返回分级推荐人数统计数据</param>
        /// <param name="recommandUserCountRanking">返回推荐人数排名</param>
        public void GetPersonStatForRecommand(int uid, int isPartner, DateTime startTime, DateTime endTime,
                                              out int recommandUserCount, out dynamic[] eachLevelRecommandUserCount, out int recommandUserCountRanking)
        {
            // 获取数据库访问对象
            SqlDataBase db = new SqlDataBase();

            recommandUserCount = 0;
            eachLevelRecommandUserCount = new dynamic[0];
            List<dynamic> eachLevelList = new List<dynamic>() { 0, 0, 0 }; // 默认只设置前3个层级的数据，第4级为M级，根据实际数据决定是否加入到列表中
            recommandUserCountRanking = 0;
            int intRecommandUser_M_Class = 0;

            // 获取推荐人数分级统计数据
            var userCount = GetRecommandUserCountFor3level(uid, startTime, endTime);
            if (userCount != null && userCount.Count > 0)
            {
                var statisticsData = userCount.First();
                recommandUserCount = statisticsData.total_recommand_user_count;
                eachLevelList = new List<dynamic>()
                {
                    statisticsData.recommand_user_first_class, // 第1级分销
                    statisticsData.recommand_user_second_class, // 第2级分销
                    statisticsData.recommand_user_third_class, // 第3级分销
                };
            }

            if (isPartner == 1)
            {
                // 如果是合伙人才获取M级推荐人数
                var userMCount = GetRecommandUserCount(uid, startTime, endTime);
                if (userMCount != null && userMCount.Count > 0)
                {
                    int.TryParse(userMCount.First().userCount.ToString(), out intRecommandUser_M_Class);
                }
                // 追加到数组中
                eachLevelList.Add(intRecommandUser_M_Class);
            }

            // 将推荐人数各层级的统计数据转换为数组。注：数组中元素的总数只可能是3个或者4个。
            eachLevelRecommandUserCount = eachLevelList.ToArray();

            // 获取推荐人数排名名次数据
            var monthRanking = GetRecommandUserRanking(uid, startTime, endTime);
            if (monthRanking != null && monthRanking.Count > 0)
            {
                int.TryParse(monthRanking.First().rows.ToString(), out recommandUserCountRanking);
            }
        }

        // lcl 20180426 Insert
        /// <summary>
        /// 获取指定用户的按层级统计的推荐总人数据集合(支持无限层级递归)
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="startTime">起始时间（进行大于等于操作）</param>
        /// <param name="endTime">截止时间（进行小于等于操作）</param>
        /// <returns></returns>
        public List<dynamic> GetRecommandUserCount(int uid, DateTime startTime, DateTime endTime)
        {
            string strSQl = $@"with cte as
                               (
	                               select id ,spreader_uid ,username ,1 as lv
	                                 from dbo.rrl_user(nolock)
	                                where spreader_uid = @uid
	                                union all
	                               select c.id ,c.spreader_uid ,c.username ,1 + p.lv as lv
	                                 from cte P
	                                inner join dbo.rrl_user(nolock) c
		                               on p.id = c.spreader_uid
                                    where (c.addtime >= @startTime)
                                      and (c.addtime <= @endTime)
                               )
                               select isnull(sum(userCount),0) as userCount 
                                 from (
                                   select lv,count(*) as userCount
                                     from (select id ,spreader_uid ,username ,lv from cte ) s group by lv
                                 ) as t
                                where (lv > 3)";
            return new SqlDataBase().Select<dynamic>(strSQl, new { uid = uid, startTime = startTime, endTime = endTime });
        }

        // lcl 20180426 Insert
        /// <summary>
        /// 获取指定用户的推荐总人数排名名次
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="startTime">起始时间（进行大于等于操作）</param>
        /// <param name="endTime">截止时间（进行小于等于操作）</param>
        /// <returns></returns>
        public List<dynamic> GetRecommandUserRanking(int uid, DateTime startTime, DateTime endTime)
        {
            string strSQl = $@"select t2.rows
                                 from (
                                   select t1.uid ,row_number() over(order by t1.num desc) as [rows]
                                     from (
	                                   select t.uid ,sum(t.num) as num
	                                     from (
	                                   	   select sperader_uid as uid ,count(0) as num
	                                   	     from sperader_relation(nolock)
	                                   	    where (addtime >= @startTime) and (addtime <= @endTime) and (sperader_uid > 0)
	                                   	    group by sperader_uid
	                                   	   union all
	                                       select sperader_sperader_uid as uid ,count(0) as num
	                                   	     from sperader_relation(nolock)
	                                   	    where (addtime >= @startTime) and (addtime <= @endTime) and (sperader_sperader_uid > 0)
	                               	        group by sperader_sperader_uid
	                                   	   union all
	                                   	   select sperader_sperader_sperader_uid as uid ,count(0) as num
	                                   	     from sperader_relation(nolock)
	                                   	    where (addtime >= @startTime) and (addtime <= @endTime) and (sperader_sperader_sperader_uid > 0)
	                                   	    group by sperader_sperader_sperader_uid
	                                   ) as t
	                                   group by t.uid
                                     ) t1
                                    inner join rrl_user(nolock) u
                                       on t1.uid = u.id
                                 ) as t2
                                where (t2.uid = @uid)";

            return new SqlDataBase().Select<dynamic>(strSQl, new { uid = uid, startTime = startTime, endTime = endTime });
        }

        // lcl 20180427 Insert
        /// <summary>
        /// 获取推荐总人数排名列表，数据集已经进行从高到低的排序
        /// </summary>
        /// <param name="startTime">起始时间（进行大于等于操作）</param>
        /// <param name="endTime">截止时间（进行小于等于操作）</param>
        /// <returns></returns>
        public List<dynamic> GetRecommandUserRankingList(DateTime startTime, DateTime endTime)
        {
            string strSQl = $@"select top 50 t1.uid ,u.username ,t1.num ,row_number() over(order by t1.num desc) as [rows]
                                 from (
	                               select t.uid ,sum(t.num) as num
	                                 from (
	                               	   select sperader_uid as uid ,count(0) as num
	                               	     from sperader_relation(nolock)
	                               	    where (addtime >= @startTime) and (addtime <= @endTime) and (sperader_uid > 0)
	                               	    group by sperader_uid
	                               	   union all
	                                   select sperader_sperader_uid as uid ,count(0) as num
	                               	     from sperader_relation(nolock)
	                               	    where (addtime >= @startTime) and (addtime <= @endTime) and (sperader_sperader_uid > 0)
	                               	    group by sperader_sperader_uid
	                               	   union all
	                               	   select sperader_sperader_sperader_uid as uid ,count(0) as num
	                               	     from sperader_relation(nolock)
	                               	    where (addtime >= @startTime) and (addtime <= @endTime) and (sperader_sperader_sperader_uid > 0)
	                               	    group by sperader_sperader_sperader_uid
	                               ) as t
	                               group by t.uid
                                 ) t1
                                inner join rrl_user(nolock) u
                                   on t1.uid = u.id
                                where (u.id < 5826) or (u.id > 5826)";

            return new SqlDataBase().Select<dynamic>(strSQl, new { startTime = startTime, endTime = endTime });
        }

        // lcl 20180426 Insert
        /// <summary>
        /// 获取指定用户的按3个分销层级统计的推荐总人数据（按照旧逻辑依据sperader_relation表统计）
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="startTime">起始时间（进行大于等于操作）</param>
        /// <param name="endTime">截止时间（进行小于等于操作）</param>
        /// <returns></returns>
        public List<dynamic> GetRecommandUserCountFor3level(int uid, DateTime startTime, DateTime endTime)
        {
            string strSQl = $@"select (recommand_user_first_class + recommand_user_second_class + recommand_user_third_class) as total_recommand_user_count
                                      ,recommand_user_first_class ,recommand_user_second_class ,recommand_user_third_class
                                 from (
                                   select
                                     (select isnull(count(*) ,0)
                                        from sperader_relation(nolock)
                                       where (sperader_uid = @uid) and (addtime >= @startTime) and (addtime <= @endTime)
                                     ) as recommand_user_first_class,
                                     (select isnull(count(*) ,0)
	                               	    from sperader_relation(nolock)
	                               	   where (sperader_sperader_uid = @uid) and (addtime >= @startTime) and (addtime <= @endTime)
	                                 ) as recommand_user_second_class,
	                                 (select isnull(count(*) ,0)
	                               	    from sperader_relation(nolock)
		                               where (sperader_sperader_sperader_uid = @uid) and (addtime >= @startTime) and (addtime <= @endTime)
	                                 ) as recommand_user_third_class
                                 ) as t";
            return new SqlDataBase().Select<dynamic>(strSQl, new { uid = uid, startTime = startTime, endTime = endTime });
        }
        #endregion

        #region 推荐奖励
        // lcl 20180427 Insert
        /// <summary>
        /// 获取与推荐奖励相关的统计数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="startTime">起始时间（进行大于等于操作）</param>
        /// <param name="endTime">截止时间（进行小于等于操作）</param>
        /// <param name="recommandRewardsMoney">返回推荐奖励金额</param>
        /// <param name="recommandRewardsRanking">返回推荐奖励排名</param>
        public void GetRewardsStatForRecommand(int uid, DateTime startTime, DateTime endTime,
                                               out decimal recommandRewardsMoney, out int recommandRewardsRanking)
        {
            // 获取数据库访问对象
            SqlDataBase db = new SqlDataBase();

            // 获取推荐奖励金额
            recommandRewardsMoney = GetUserRewardsCount(uid, startTime, endTime);

            recommandRewardsRanking = 0;
            // 获取推荐奖励排行数据
            var rankingData = GetRecommandRewardsRanking(startTime, endTime);
            if (rankingData != null && rankingData.Count > 0)
            {
                // 获取当月排名数据
                var currentUserRanking = rankingData.Where(p => p.uid == uid);
                if (currentUserRanking.Count() > 0)
                {
                    // 如果当前用户存在当月数据，则获取名次
                    int.TryParse(currentUserRanking.First().rows.ToString(), out recommandRewardsRanking);
                }
            }
        }

        // lcl 20180427 Insert
        /// <summary>
        /// 获取推荐奖励排行的数据，数据集已经进行从高到低的排序
        /// </summary>
        /// <param name="startTime">起始时间（进行大于等于操作）</param>
        /// <param name="endTime">截止时间（进行小于等于操作）</param>
        /// <returns></returns>
        public List<dynamic> GetRecommandRewardsRanking(DateTime startTime, DateTime endTime)
        {
            string strSQl = $@"select t.[uid] ,u.username ,t.[money] ,row_number() over(order by t.[money] desc) as [rows]
                                 from (
	                               select [uid] ,sum([money]) as [money]
	                                 from rrl_user_money_record(nolock)
	                                where ([type] = 3)
	                                  and (addtime >= @startTime) and (addtime <= @endTime)
	                                group by [uid]
                                 ) as t
                                inner join rrl_user(nolock) u
                                   on t.uid = u.id
                                where (u.id < 5826) or (u.id > 5826)";

            return new SqlDataBase().Select<dynamic>(strSQl, new { startTime = startTime, endTime = endTime });
        }

        // lcl 20180427 Insert
        /// <summary>
        /// 获取指定用户在某时间范围内的奖励金额总数
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="startTime">起始时间（进行大于等于操作）</param>
        /// <param name="endTime">截止时间（进行小于等于操作）</param>
        /// <returns></returns>
        public decimal GetUserRewardsCount(int uid, DateTime startTime, DateTime endTime)
        {
            decimal mMoney = 0m;

            string strSQl = $@"select isnull(sum([money]),0) as [money]
	                             from rrl_user_money_record(nolock)
	                            where ([type] = 3)
	                              and (addtime >= @startTime) and (addtime <= @endTime)
                                 and ([uid] = @uid)";

            var resultList = new SqlDataBase().Select<dynamic>(strSQl, new { uid = uid, startTime = startTime, endTime = endTime });
            if (resultList != null && resultList.Count > 0)
            {
                mMoney = resultList.First().money;
            }

            return mMoney;
        }
        #endregion

        #region 小红包

        // lcl 20180504 Insert
        /// <summary>
        /// 获取每日签到小红包配置数据
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetMoneyPayConfigList()
        {
            string strSql = $@"select title ,day_index ,[money] ,min_money ,max_money ,extra_money ,'0' as is_signin
                                 from rrl_money_pay_day_config(nolock)
                                where (is_enable = '1')
                                order by day_index";

            var result = new SqlDataBase().Select<dynamic>(strSql);

            return result;
        }

        // lcl 20180504 Insert
        /// <summary>
        /// 获取指定用户，最近一个周期领取的签到小红包数据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="existDataInValidCycles">返回参数，有效周期内是否存在小红包领取数据. true:存在; false:不存在</param>
        /// <param name="isCanGetMoneyPay">返回参数，当天是否可以领取小红包. true:当天可以领取; false:当天不能领取</param>
        /// <returns></returns>
        public List<dynamic> GetUserSignMoneyPayList(int userId, out bool existDataInValidCycles, out bool isCanGetMoneyPay)
        {
            string strSQl = $@"select * from rrl_user_receive_day_money_pay
                                where [uid] = @uid
                                  and rec_date >= (select max(rec_date) from rrl_user_receive_day_money_pay where ([uid] = @uid) and (day_index = 1))
                                  and rec_date <= (select max(rec_date) from rrl_user_receive_day_money_pay where ([uid] = @uid))
                                order by rec_date asc";
            var userMoneyPayList = new SqlDataBase().Select<dynamic>(strSQl, new { uid = userId });
            var now = DateTime.Now;
            // 从当前日期向前推算出当前有效周期的第一天的日期值
            var firstDayInValidCycles = now.AddDays(-(RRLConfig.CNT_SIGN_CYCLES_MONEY_PAY - 1)).Date;
            dynamic rec_firstDate;

            // 初始化为不存在有效周期内数据(没有用户的小红包领取数据，或者数据在一个有效周期之外的，都认为有效周期内没有领取过小红包)
            existDataInValidCycles = false;
            if (userMoneyPayList != null && userMoneyPayList.Count > 0)
            {
                rec_firstDate = userMoneyPayList.First().rec_date;
                if (rec_firstDate >= firstDayInValidCycles)
                {
                    // 有效周期内有数据(天数索引等于1的日期在有效周期内，说明至少有一笔数据存在)
                    existDataInValidCycles = true;
                }
                // 判断当天是否可以领取小红包
                isCanGetMoneyPay = !userMoneyPayList.Any(p => p.rec_date == now.Date);
            }
            else
            {
                // 没有用户的小红包领取数据时，用户可以领取小红包
                isCanGetMoneyPay = true;
            }

            return userMoneyPayList;
        }

        #endregion

        #region 代理人数据统计

        // lcl 2018-07-17 Insert
        /// <summary>
        /// 获取分别针对每一个被推荐人的，其在某一日期范围内的按天显示的数据
        /// </summary>
        /// <param name="spreaderUid">推荐人ID</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<dynamic> GetProxyDataByUserDaily(int spreaderUid, DateTime startTime, DateTime endTime)
        {
            // 获取数据库访问对象
            SqlDataBase db = new SqlDataBase();

            string strSql = $@"select * from (
	                               select s.[uid]
                                         ,convert(varchar(10), s.stat_date) as stat_date ,u.username ,s.buy_coupons_cash ,s.h_money ,s.receive_redpackage_money
		                                 ,s.shopping_bean ,s.bean_profit ,s.shopping_cash ,s.cash_profit ,s.bean_profit + s.cash_profit as profit_total
	                                 from rrl_agent_data_auto_stat(nolock) s
	                                inner join rrl_user(nolock) u
	                               	   on s.[uid] = u.id
	                                where (s.spreader_uid = @uid)
	                                  and (s.stat_date >= @startTime)
	                                  and (s.stat_date <= @endTime)
                               ) as t
                                order by t.profit_total desc";

            return db.Select<dynamic>(strSql, new { uid = spreaderUid, startTime = startTime, endTime = endTime });
        }

        // lcl 2018-07-17 Insert
        /// <summary>
        /// 获取分别针对每一个被推荐人的，统计其在某一日期范围内的汇总数据
        /// </summary>
        /// <param name="spreaderUid">推荐人ID</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<dynamic> GetProxyStatisticsByUser(int spreaderUid, DateTime startTime, DateTime endTime)
        {
            // 获取数据库访问对象
            SqlDataBase db = new SqlDataBase();

            string strSql = $@"select s.[uid] ,u.username ,s.buy_coupons_cash ,s.h_money ,s.receive_redpackage_money
                                     ,s.shopping_bean ,s.bean_profit ,s.shopping_cash ,s.cash_profit ,s.profit_total
                                 from (
	                               select [uid]
	                                     ,sum(buy_coupons_cash) as buy_coupons_cash ,sum(h_money) as h_money ,sum(receive_redpackage_money) as receive_redpackage_money
		                                 ,sum(shopping_bean) as shopping_bean ,sum(bean_profit) as bean_profit ,sum(shopping_cash) as shopping_cash
		                                 ,sum(cash_profit) as cash_profit ,sum(bean_profit + cash_profit) as profit_total
	                                 from rrl_agent_data_auto_stat(nolock) 
	                                where (spreader_uid = @uid)
	                                  and (stat_date >= @startTime)
	                                  and (stat_date <= @endTime)
	                                group by [uid]
                               ) as s
                                inner join rrl_user(nolock) u
                                   on s.[uid] = u.id
                                order by s.profit_total desc";

            return db.Select<dynamic>(strSql, new { uid = spreaderUid, startTime = startTime, endTime = endTime });
        }

        // lcl 2018-07-17 Insert
        /// <summary>
        /// 获取按天统计所有被推荐人，在某一日期范围内的汇总数据
        /// </summary>
        /// <param name="spreaderUid">推荐人ID</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<dynamic> GetProxyStatisticsByDaily(int spreaderUid, DateTime startTime, DateTime endTime)
        {
            // 获取数据库访问对象
            SqlDataBase db = new SqlDataBase();

            string strSql = $@"select convert(varchar(10), s.stat_date) as stat_date
                                     ,s.buy_coupons_cash ,s.h_money ,s.receive_redpackage_money
                                     ,s.shopping_bean ,s.bean_profit ,s.shopping_cash ,s.cash_profit ,s.profit_total
	                                 ,isnull(u.userCount,0) as userCount
                                 from (
	                               select stat_date
	                                     ,sum(buy_coupons_cash) as buy_coupons_cash ,sum(h_money) as h_money ,sum(receive_redpackage_money) as receive_redpackage_money
		                                 ,sum(shopping_bean) as shopping_bean ,sum(bean_profit) as bean_profit ,sum(shopping_cash) as shopping_cash
		                                 ,sum(cash_profit) as cash_profit ,sum(bean_profit + cash_profit) as profit_total
	                                 from rrl_agent_data_auto_stat(nolock) 
	                                where (spreader_uid = @uid)
	                                  and (stat_date >= @startTime)
	                                  and (stat_date <= @endTime)
	                                group by stat_date
                               ) as s
                                 left join (
                                   select convert(date ,addtime) as addtime ,count(0) as userCount
	                                 from rrl_user(nolock)
	                                where (spreader_uid = @uid)
	                                  and (addtime >= @startTime)
	                                  and (addtime <= @endTime)
	                                group by convert(date ,addtime)
                               ) as u
                                   on s.stat_date = u.addtime
                                order by s.stat_date";

            return db.Select<dynamic>(strSql, new { uid = spreaderUid, startTime = startTime, endTime = endTime });
        }

        // lcl 2018-07-17 Insert
        /// <summary>
        /// 获取某一日期范围内的汇总数据
        /// </summary>
        /// <param name="spreaderUid">推荐人ID</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<dynamic> GetProxyStatistics(int spreaderUid, DateTime startTime, DateTime endTime)
        {
            // 获取数据库访问对象
            SqlDataBase db = new SqlDataBase();

            string strSql = $@"select s.buy_coupons_cash ,s.h_money ,s.receive_redpackage_money
                                     ,s.shopping_bean ,s.bean_profit ,s.shopping_cash ,s.cash_profit ,s.profit_total
	                                 ,u.userCount
                                 from (
	                               select sum(buy_coupons_cash) as buy_coupons_cash ,sum(h_money) as h_money ,sum(receive_redpackage_money) as receive_redpackage_money
		                                 ,sum(shopping_bean) as shopping_bean ,sum(bean_profit) as bean_profit ,sum(shopping_cash) as shopping_cash
		                                 ,sum(cash_profit) as cash_profit ,sum(bean_profit + cash_profit) as profit_total
	                                 from rrl_agent_data_auto_stat(nolock) 
	                                where (spreader_uid = @uid)
	                                  and (stat_date >= @startTime)
	                                  and (stat_date <= @endTime)
                               ) as s ,(
                                   select count(0) as userCount
	                                 from rrl_user(nolock)
	                                where (spreader_uid = @uid)
	                                  and (addtime >= @startTime)
	                                  and (addtime <= @endTime)
                               ) as u";

            return db.Select<dynamic>(strSql, new { uid = spreaderUid, startTime = startTime, endTime = endTime });
        }

        // lcl 2018-07-17 Insert
        /// <summary>
        /// 历史盈利数据
        /// </summary>
        /// <param name="spreaderUid">推荐人ID</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<dynamic> GetHistoryProfit(int spreaderUid, DateTime startTime, DateTime endTime)
        {
            // 获取数据库访问对象
            SqlDataBase db = new SqlDataBase();

            string strSql = $@"select monthNum ,sum(bean_profit + cash_profit) as profit_total
                                 from (select datepart(mm, stat_date) monthNum ,bean_profit ,cash_profit
                                         from rrl_agent_data_auto_stat(nolock) 
                                        where (spreader_uid = @uid)
                                          and (stat_date >= @startTime)
                                          and (stat_date <= @endTime)
                                 ) as t
                                group by t.monthNum
                                order by t.monthNum";

            return db.Select<dynamic>(strSql, new { uid = spreaderUid, startTime = startTime, endTime = endTime });
        }

        #endregion
    }
}