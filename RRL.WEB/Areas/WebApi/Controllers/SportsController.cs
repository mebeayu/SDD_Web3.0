/*
 * STORY #18 lcl 20180515 Insert
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web.Http;

using RRL.Config;
using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using RRL.WEB.Models.Request;
using RRL.WEB.Models.Response;
using System.Text.RegularExpressions;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 兜兜运动WebApi
    /// </summary>
    public partial class SportsController : BaseApiController
    {
        /// <summary>
        /// 获取指定用户的运动竞猜数据
        /// </summary>
        /// <param name="request">包含token的请求</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public DataResult UserGuessData([FromBody]BaseTokenRequest request)
        {
            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            DataResult dataResult = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            List<dynamic> guessData = new SportsManager().GetUserGuessData(user.id, DateTime.Now.Date);
            if (guessData != null && guessData.Count > 0)
            {
                var userGuessData = guessData.First();
                dataResult.data = new
                {
                    money = userGuessData.money,
                    forecast_result = userGuessData.forecast_result
                };
            }

            return dataResult;
        }

        /// <summary>
        /// 用户竞猜投注操作
        /// </summary>
        /// <param name="request">包含token和投注信息的请求</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public DataResult UserGuessBetting([FromBody]GuessBettingRequest request)
        {
            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            if (request.money <= 0m)
            {
                return new DataResult() { status = -1, message = "竞猜出资数额必须大于0" };
            }
            if (string.IsNullOrWhiteSpace(request.forecast) || (request.forecast != "1" && request.forecast != "2"))
            {
                return new DataResult() { status = -2, message = "用户竞猜结果参数错误" };
            }

            // 执行竞猜出资的存储过程，以记录用户的出资数据
            List<dynamic> resultWithSP = new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_SportsGuessBetting",
                new
                {
                    uid = user.id,
                    money = request.money,
                    forecast = request.forecast
                });

            if (resultWithSP == null || resultWithSP.Count < 1)
            {
                // 存储过程执行失败
                return new DataResult() { status = -3, message = "存储过程执行失败" };
            }

            // 存储过程中业务逻辑处理成功后，应返回状态码为0，以与WebApi执行成功状态码一致
            var result = resultWithSP.First();
            // 存储过程执行成功
            return new DataResult() { status = result.status, message = result.message };
        }

        /// <summary>
        /// 用户步数数据维护
        /// </summary>
        /// <param name="request">包含token和用户总步数的请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult UserStepData([FromUri]SportStepRequest request)
        {
            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            if (request.step < 0)
            {
                return new DataResult() { status = -1, message = "用户步数不能是负数" };
            }

            // 执行用户步数维护的存储过程，以新增或更新当天的总步数
            List<dynamic> resultWithSP = new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_SportsStep",
                new
                {
                    uid = user.id,
                    step = request.step
                });

            if (resultWithSP == null || resultWithSP.Count < 1)
            {
                // 存储过程执行失败
                return new DataResult() { status = -2, message = "存储过程执行失败" };
            }

            // 存储过程中业务逻辑处理成功后，应返回状态码为0，以与WebApi执行成功状态码一致
            var result = resultWithSP.First();
            // 存储过程执行成功
            return new DataResult() { status = result.status, message = result.message };
        }

        /// <summary>
        /// 用户点赞和回赞操作
        /// </summary>
        /// <param name="request">包含token和用户点赞数据的请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult UserToLike([FromUri]UserToLikeRequest request)
        {
            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            string strSql = @"insert into rrl_run_like
                                     ([id] ,[from_uid] ,[to_uid] ,[like_date] ,[like_type] ,[addtime])
                              values (newid() ,@uid ,@to_uid ,getdate() ,@like_type ,getdate())";

            int result = new SqlDataBase().Execute(strSql, new { uid = user.id, to_uid = request.to_uid, like_type = request.type });
            if (result > 0)
            {
                // SQL语句执行成功
                return DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            }
            else
            {
                return new DataResult() { status = -1, message = "今天已经点赞了" };
            }
        }

        /// <summary>
        /// 获取指定用户的点赞和回赞的分页数据
        /// </summary>
        /// <param name="request">包含token、分页信息和查询条件的请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult LikePageDataByUser([FromUri]LikeQueryRequest request)
        {
            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            // 获取查询中的点赞日期条件
            DateTime dtLikeDate;
            if (!DateTime.TryParse(request.date, out dtLikeDate))
            {
                // 日期值转换不成功，则默认为当前日期
                dtLikeDate = DateTime.Now;
            }

            /*
             * 查询逻辑说明：
             *     1. to_uid是当前用户ID的。即，被点赞人或者被回赞人是当前用户的；
             *     2. 关于is_allow_replay（是否允许回赞）的标记控制。值为1表示可以回赞；0表示不能回赞。
             *        如果是被他人点赞的，并且未回赞的，允许当前用户进行回赞操作。
             *     3. 点赞和回赞的周期约定，以一天为一个有效周期。
             */
            string strSql = @"select rl.from_uid ,u.username ,u.head_pic ,rl.like_type
                                    ,case when rl.like_type = '0' and reply_rl.id is null then 1 else 0 end as is_allow_replay
                                from rrl_run_like(nolock) rl
                               inner join rrl_user(nolock) u
                                  on u.id = rl.from_uid
                                left join rrl_run_like(nolock) reply_rl
                                  on reply_rl.from_uid = rl.to_uid and reply_rl.to_uid = rl.from_uid and reply_rl.like_type = '1' and reply_rl.like_date = rl.like_date
                               where (rl.to_uid = @uid)
                                 and (rl.like_type = '0')
                                 and (rl.like_date = @likeDate)
                               order by rl.addtime desc";
            List<dynamic> resultList = new SqlDataBase().SelectPage(strSql, request.PageSize, request.PageIndex, new { uid = user.id, likeDate = dtLikeDate.Date });
            if (resultList != null && resultList.Count > 0)
            {
                // 如果查询到数据，则隐去手机号码中间4位数字
                resultList.ForEach(p => p.username = Regex.Replace(p.username, @"(\d{3})\d{4}(\d{4})", "$1****$2"));
            }

            DataResult dataResult = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            dataResult.data = resultList;

            return dataResult;
        }

        /// <summary>
        /// 获取指定用户的运动日记数据
        /// </summary>
        /// <param name="request">包含token和查询条件的请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult UserSportDiary([FromUri]SportDataQueryRequest request)
        {
            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            // 获取查询中的运动日期条件
            DateTime dtRunDate;
            if (!DateTime.TryParse(request.date, out dtRunDate))
            {
                // 日期值转换不成功，则默认为当前日期的前一天
                dtRunDate = DateTime.Now.AddDays(-1);
            }

            var userData = new SportsManager().GetUserSportDiary(user.id, dtRunDate.Date);

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            result.data = userData;

            return result;
        }

        /// <summary>
        /// 获取指定用户某一天的运动数据
        /// </summary>
        /// <param name="request">包含token和查询条件的请求</param>
        /// <returns>包括用户基础信息、总步数、步数名次、被点赞总次数</returns>
        [HttpGet]
        public DataResult UserSportData([FromUri]SportDataQueryRequest request)
        {
            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            // 获取查询中的日期条件
            DateTime dtDate;
            if (!DateTime.TryParse(request.date, out dtDate))
            {
                // 日期值转换不成功，则默认为当前日期
                dtDate = DateTime.Now;
            }

            string strSql = @"select u.username ,u.head_pic ,isnull(rs.step, 0) as step ,isnull(rs.ranking, 0) as ranking
                                    ,rl.like_count ,isnull(rs.award_money, 0) as awardMoney
                                from rrl_user(nolock) u
                                left join (
                                     select [uid] ,step ,row_number() over(order by step desc) as ranking ,award_money
                                       from rrl_run_step(nolock)
                                      where (run_date = @date)
                                ) as rs
                                  on rs.[uid] = u.[id]
                                left join (
                                     select @uid as [uid] ,count(0) as like_count
	                                   from rrl_run_like(nolock)
		                              where (like_date = @date)
		                                and (to_uid = @uid)
                                ) as rl
                                  on rl.[uid] = u.[id]
                               where (u.[id] = @uid)";

            var userData = new SqlDataBase().Select(strSql, new { uid = user.id, date = dtDate.Date });

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            if (userData != null && userData.Count > 0)
            {
                var data = userData.First();
                data.username = data.username.Remove(3, 4).Insert(3, "****");
                data.awardMoney = StepToRedpackage(data.step);
                result.data = data;
            }

            return result;
        }

        /// <summary>
        /// 获取指定用户某一天的战绩数据
        /// </summary>
        /// <param name="request">包含token和查询条件的请求</param>
        /// <returns>包括用户基础信息、总步数、步数名次、被点赞总次数</returns>
        [HttpGet]
        public DataResult UserEffortData([FromUri]SportDataQueryRequest request)
        {
            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            SportsManager SportsMgr = new SportsManager();

            // 获取查询中的日期条件
            DateTime dtDate;
            if (!DateTime.TryParse(request.date, out dtDate))
            {
                // 日期值转换不成功，则默认为当前日期的前一天
                dtDate = DateTime.Now.AddDays(-1);
            }

            dynamic effortData = new ExpandoObject();
            // 用户基础数据
            effortData.username = user.username;
            effortData.headPic = user.head_pic;

            var sportData = SportsMgr.GetUserSportDiary(user.id, dtDate.Date);
            if (sportData != null)
            {
                // 有运动数据
                effortData.isHasData = 1; // 有数据
                effortData.date = dtDate.ToString("MM月dd日");
                // 获取运动数据
                effortData.step = sportData.step; // 总步数
                effortData.ranking = sportData.ranking; //名次
                // 被点赞数据
                effortData.likeCount = 0; // 被点赞总数量
                effortData.replyLikeCount = 0; // 被回赞总数量
                var likeData = SportsMgr.GetUserLikeData(user.id, dtDate.Date);
                if (likeData != null && likeData.Count > 0)
                {
                    effortData.likeCount = likeData.Where(p => p.like_type == "0").Count();
                    effortData.replyLikeCount = likeData.Where(p => p.like_type == "1").Count();
                }
                // 竞猜和奖励数据
                effortData.sportMoney = sportData.money; // 运动奖励免费红包
                var userGuessData = new SqlDataBase().Select<dynamic>("select * from rrl_run_guess(nolock) where ([uid] = @uid) and (guess_date = @guess_date)",
                                                                      new { uid = user.id, guess_date = dtDate.Date });
                if (userGuessData != null && userGuessData.Count > 0)
                {
                    effortData.isJoinGuess = 1; // 参与了竞猜
                    var guessData = userGuessData.First();
                    effortData.guessResult = guessData.guess_result; // 竞猜结果("0"：未开奖；"1"：猜对；"2"：猜错)
                    effortData.guessMoeny = Math.Floor(guessData.money); // 出资数额
                }
                else
                {
                    effortData.isJoinGuess = 0; // 未参与竞猜
                }
                // 用户排名百分比
                effortData.rankingPercent = sportData.rankingPercent;
            }
            else
            {
                // 没有运动数据
                effortData.isHasData = 0; // 没有数据
            }

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            result.data = effortData;

            return result;
        }

        /// <summary>
        /// 获取指定日期的运动数据排名
        /// </summary>
        /// <param name="request">包含token和查询条件的请求</param>
        /// <returns>包括用户基础信息、总步数、步数名次、被点赞总次数</returns>
        [HttpGet]
        public DataResult UserSportDataRanking([FromUri]SportDataQueryRequest request)
        {
            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            // 获取查询中的日期条件
            DateTime dtDate;
            if (!DateTime.TryParse(request.date, out dtDate))
            {
                // 日期值转换不成功，则默认为当前日期
                dtDate = DateTime.Now;
            }

            string strSql = @"select top 100 rs.[uid] ,u.username ,u.head_pic ,rs.step ,row_number() over(order by rs.step desc) as ranking
                                    ,isnull(rl.like_count, 0) as like_count
	                                ,case when rs.[uid] <> @uid and u_rl.to_uid is null then 1 else 0 end as is_allow_like
                                from rrl_run_step(nolock) rs
                               inner join rrl_user(nolock) u
                                  on u.id = rs.[uid]
                                left join (
                                     select to_uid ,count(0) as like_count
	                                   from rrl_run_like(nolock)
		                              where (like_date = @date)
                                      group by to_uid
                                ) as rl
                                  on rl.to_uid = rs.[uid]
                                left join (
                                     select to_uid
	                                   from rrl_run_like(nolock)
		                              where (like_date = @date)
                                        and (from_uid = @uid)
                                ) as u_rl
                                  on u_rl.to_uid = rs.[uid]
                               where (rs.run_date = @date)";

            var rankingData = new SqlDataBase().Select(strSql, new { uid = user.id, date = dtDate.Date });

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            if (rankingData != null && rankingData.Count > 0)
            {
                rankingData.ForEach(p => p.username = p.username.Remove(3, 4).Insert(3, "****"));
                result.data = rankingData;
            }

            return result;
        }

        /// <summary>
        /// 用户分享运动信息奖励红包
        /// </summary>
        /// <param name="request">包含token的请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult SportShareRedpackage([FromUri]BaseTokenRequest request)
        {
            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            // 执行分享运动数据获得免费红包的存储过程
            List<dynamic> resultWithSP = new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_SportsShareRedpackage",
                new
                {
                    uid = user.id,
                    shareDate = DateTime.Now.Date
                });

            if (resultWithSP == null || resultWithSP.Count < 1)
            {
                // 存储过程执行失败
                return new DataResult() { status = -3, message = "存储过程执行失败" };
            }

            // 存储过程中业务逻辑处理成功后，应返回状态码为0，以与WebApi执行成功状态码一致
            var result = resultWithSP.First();
            // 存储过程执行成功
            return new DataResult() { status = result.status, message = result.message };
        }

        /// <summary>
        /// 按步数奖励的红包数
        /// </summary>
        /// <param name="step">步数</param>
        /// <returns></returns>
        private int StepToRedpackage(dynamic step)
        {
            ConfigManager configMgr = new ConfigManager();
            // 获取用户每天最多能领取的按步数奖励的红包数
            int intStepToRedpackageMaxDaily = 0;
            int.TryParse(configMgr.GetConfigValue("Sport_StepToRedpackage_Max_Daily"), out intStepToRedpackageMaxDaily);
            // 获取用户按步数奖励红包的转换比率（红包数 = 步数 * 该配置值）
            decimal dStepToRedpackageRate = 0m;
            decimal.TryParse(configMgr.GetConfigValue("Sport_StepToRedpackage_Rate"), out dStepToRedpackageRate);

            // 计算可领取的红包数
            int intRedpackage = 0;
            int.TryParse(Math.Floor(step * dStepToRedpackageRate).ToString(), out intRedpackage);

            return Math.Min(intRedpackage, intStepToRedpackageMaxDaily);
        }
    }
}