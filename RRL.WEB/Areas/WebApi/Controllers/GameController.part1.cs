/*
 *  lcl 20180524 Insert
 */

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;

using RRL.Config;
using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using RRL.WEB.Models.Request;
using RRL.WEB.Models.Response;
using RRL.WEB.Ulity;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 与签到功能和游戏相关的WebApi
    /// </summary>
    public partial class GameController
    {
        /// <summary>
        /// 初始化签到数据
        /// </summary>
        /// <param name="request">包含token的请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult InitSignInData([FromUri]BaseTokenRequest request)
        {
            // 定义作为DataResult.data属性值的动态对象
            dynamic baseViewData = new ExpandoObject();

            UserManager UserMgr = new UserManager();
            // 默认设置否登录标记为“未登录”状态
            bool blnIsLogin = false;
            // 默认设置连续签到日期数为0
            baseViewData.continuousSignDays = 0;
            // 初始化“是否显示游戏相关内容”的标记
            var isAtUSA = new ShopManager().isAtUSA("");
            baseViewData.is_show_game = isAtUSA ? "0" : "1";

            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult == MessageCode.SUCCESS)
            {
                // 如果token中的用户ID有效，则认为已登录
                blnIsLogin = true;
            }
            else
            {
                // 如果token验证失败，则认为是未登录
                blnIsLogin = false;
            }

            // 获取红包配置信息
            var redpackageDayConfigList = UserMgr.GetRedpackageConfigList();
            baseViewData.redpackageDayConfig = redpackageDayConfigList;

            // 获取与红包兑换相关的配置数据
            ConfigManager configMgr = new ConfigManager();
            // 分享得小红包
            baseViewData.shareRedpackage = configMgr.GetConfigValue("GameCenter_Share_Redpackage");
            // 分享得金币
            baseViewData.shareGoldCoins = configMgr.GetConfigValue("GameCenter_Share_GoldCoins");

            // 初始化时间差值
            baseViewData.countdown = 0;

            if (!blnIsLogin)
            {
                // 未登录。也让用户能看到可以领取的状态
                baseViewData.isCanGetRedpackage = "1";
            }
            else
            {
                // 已登录。
                bool existDataInValidCycles;
                bool isCanGetRedpackage;
                var userRedpackageList = UserMgr.GetUserSignRedpackageFreeList(user.id, out existDataInValidCycles, out isCanGetRedpackage);
                if (existDataInValidCycles)
                {
                    // 有效周期内存在数据时，将签到配置数据集合中对应的签到标记设置为“已签到”
                    redpackageDayConfigList.Take(userRedpackageList.Count).ToList().ForEach(p => p.is_signin = "1");
                    // 设置连续签到日期总天数
                    baseViewData.continuousSignDays = userRedpackageList.Count;

                    #region 倒计时
                    DateTime dtEndDatetime = userRedpackageList.First().rec_date.AddDays(RRLConfig.CNT_SIGN_CYCLES_REDPACKAGE_FREE);
                    DateTime dtNow = DateTime.Now;

                    if (dtEndDatetime > dtNow)
                    {
                        TimeSpan ts = dtEndDatetime - dtNow;
                        // 获取时间差所表示的总的秒数
                        baseViewData.countdown = Math.Floor(ts.TotalSeconds);
                    }
                    #endregion
                }
                // 设置当天是否可以领取红包
                baseViewData.isCanGetRedpackage = isCanGetRedpackage ? "1" : "0";
            }

            var dataResult = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            dataResult.data = baseViewData;

            return dataResult;
        }

        /// <summary>
        /// 签到操作
        /// </summary>
        /// <param name="request">包含token的请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult SignInRedpackage([FromUri]BaseTokenRequest request)
        {
            return new DataResult()
            {
                status = 99,
                message = "签到功能已下架"
            };

            UserAuth user = null;
            int intResult = CheckToken(request, out user);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            // 限制当天零点至零点五分不能领取
            DateTime dtNow = DateTime.Now;
            if (dtNow <= dtNow.Date.AddMinutes(5))
            {
                // 在受限的时间范围内，不做处理
                return new DataResult()
                {
                    status = 99,
                    message = "由于数据处理中，零点至零点五分不能领取"
                };
            }

            bool existDataInValidCycles;
            bool isCanGetRedpackage;
            var userRedpackageList = new UserManager().GetUserSignRedpackageFreeList(user.id, out existDataInValidCycles, out isCanGetRedpackage);
            if (isCanGetRedpackage)
            {
                // 执行签到领红包的存储过程
                List<dynamic> result = new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_ReceiveRedpackage",
                    new
                    {
                        uid = user.id,
                        isExistDataInValidCycles = existDataInValidCycles
                    });
            }

            // 返回执行成功的数据
            return DataResult.InitFromMessageCode(MessageCode.SUCCESS);
        }

        #region 红包裂变

        // lcl 2018-07-23 Insert
        /// <summary>
        /// 红包裂变分享到微信，获得每日首次分享红包
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult RedpackageFissionShareDaily([FromUri]BaseTokenRequest request)
        {
            // lcl 2018-08-28 取消每日首次分享得红包
            return DataResult.InitFromMessageCode(MessageCode.SUCCESS);

            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int res = user.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            // 执行每日首次分享领红包的存储过程
            List<dynamic> resultWithSP = new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_RedpackageFissionShareDaily",
                new
                {
                    uid = user.id
                });

            if (resultWithSP == null || resultWithSP.Count < 1)
            {
                // 存储过程执行失败
                return new DataResult() { status = -99, message = "存储过程执行失败" };
            }

            var result = resultWithSP.First();
            // 存储过程执行成功
            return new DataResult() { status = result.status, message = result.message };
        }

        // lcl 2018-07-23 Insert
        /// <summary>
        /// 用户红包裂变任务数据
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult UserRedpackageFissionTaskData([FromUri]BaseTokenRequest request)
        {
            return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);

            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int res = user.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            result.data = new GameManager().GetUserRedpackageFissionTaskData(user.id);

            return result;
        }

        // lcl 2018-08-10 Insert
        /// <summary>
        /// 领取新的红包裂变任务
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult ReceiveRedpackageFissionTask([FromUri]BaseTokenRequest request)
        {
            return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);

            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int res = user.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            // 执行领取任务的存储过程
            List<dynamic> resultWithSP = new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_RedpackageFissionReceiveTask",
                new
                {
                    uid = user.id
                });

            if (resultWithSP == null || resultWithSP.Count < 1)
            {
                // 存储过程执行失败
                return new DataResult() { status = -99, message = "存储过程执行失败" };
            }

            var result = resultWithSP.First();
            // 存储过程执行成功
            return new DataResult() { status = result.status, message = result.message, data = result.task_id };
        }

        // lcl 2018-08-10 Insert
        /// <summary>
        /// 获取用户领取红包的历史数据
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult RedpackageFissionHistory([FromUri]BaseTokenRequest request)
        {
            return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);

            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int res = user.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            var dataList = new GameManager().GetRedpackageFissionHistory();
            if (dataList.Count > 0)
            {
                dataList.ForEach(p => p.username = Regex.Replace(p.username, @"(\d{3})\d{4}(\d{4})", "$1****$2"));
                result.data = dataList;
            }

            return result;
        }

        // lcl 2018-08-13 Insert
        /// <summary>
        /// 检查是否可以帮拆红包
        /// </summary>
        /// <param name="request">基础token请求(该token是任务发起人的)</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult CheckIfOpenFissionRedpackage([FromUri]RedpackageFissionRequest request)
        {
            return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);

            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int res = user.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            var result = new GameManager().CheckIfOpenFissionRedpackage(request.taskId);

            return new DataResult() { status = result.status, message = result.message};
        }

        #endregion

        /// <summary>
        /// 爆双喜游戏状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GameStatusForBalloon2()
        {
            string strResult = MyHttpHelper.HttpGet("http://119.23.139.194:4399/sta_str");
            return strResult;
        }

        /// <summary>
        /// 爆多喜游戏状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GameStatusForBalloonMore()
        {
            string strResult = MyHttpHelper.HttpGet("http://119.23.139.194:4400/sta_str");
            return strResult;
        }

        #region 分时段充值送红包

        // lcl 2018-08-10 Insert
        /// <summary>
        /// 是否可以领取时段充值红包
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult IfHasTimeIntervalRedpaket([FromUri]BaseTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int res = user.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            var receiveData = new GameManager().GetTimeIntervalRedpaketData(user);
            result.data = new
            {
                isHasRedpacket = receiveData.isHasRedpacket,
                payMoney = receiveData.payMoney,
                redpacket = receiveData.redpacket,
                couponsMoney = receiveData.couponsMoney,
                type = receiveData.type
            };

            return result;
        }

        // lcl 2018-08-31 Insert
        /// <summary>
        /// 当前时间是否有红包
        /// </summary>
        /// <returns>1：有红包；0：没有红包</returns>
        [HttpGet]
        public int IfHasRedpaketAtCurrentTime()
        {
            var now = DateTime.Now;
            var timestr = now.GetDateTimeFormats('T')[0];

            string strSql = $@"select count(*)
                                 from time_interval_redpaket_config(nolock)
                                where (start_time < '{timestr}')
                                  and (end_time > '{timestr}')";
            var recordCount = new SqlDataBase().ExecuteScalar<int>(strSql);
            if (recordCount < 1)
            {
                // 当前时间没有分时段充值红包
                return 0;
            }

            // 当前时间有分时段充值红包
            return 1;
        }

        // lcl 2018-08-31 Insert
        /// <summary>
        /// 获取当前时间下的余额区间条件
        /// </summary>
        /// <returns>1：有红包；0：没有红包</returns>
        [HttpGet]
        public object MoneyAreaAtCurrentTime()
        {
            var now = DateTime.Now;
            var timestr = now.GetDateTimeFormats('T')[0];

            string strSql = $@"select start_money ,end_money
                                 from time_interval_redpaket_config(nolock)
                                where (start_time < '{timestr}')
                                  and (end_time > '{timestr}')";
            var configData = new SqlDataBase().Single<dynamic>(strSql);
            if (configData == null)
            {
                // 未获取到配置数据
                return new { startMoney = -1, endMoney = -1 };
            }

            // 返回
            return new { startMoney = configData.start_money, endMoney = configData.end_money };
        }

        // lcl 2018-08-31 Insert
        /// <summary>
        /// 当前用户是否已经领取分时段充值红包
        /// </summary>
        /// <returns>1：已领取；0：未领取</returns>
        [HttpGet]
        public DataResult IfReceivedTimeIntervalRedpaket([FromUri]BaseTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int res = user.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            SqlDataBase sqlDB = new SqlDataBase();
            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);

            var now = DateTime.Now;
            var timestr = now.GetDateTimeFormats('T')[0];

            // 获取配置数据
            string strSql = $@"select id ,receive_redpacket ,pay_money ,type
                                 from time_interval_redpaket_config(nolock)
                                where (start_time < '{timestr}')
                                  and (end_time > '{timestr}')";
            var configData = sqlDB.Single<dynamic>(strSql);
            if (configData == null || configData.pay_money <= 0)
            {
                // 未获取到配置数据
                return new DataResult() { status = 99, message = "未获取到分段充值红包配置数据" };
            }

            // 构造用于标识当前用户当日在指定时段领取红包的领取记录ID
            // 领取记录ID = 用户ID + 年月日 + 资金类型 + 时段数据唯一ID。各部分之间使用"-"符号分隔
            string strReceiveId = $@"{user.id}-{now.ToString("yyyyMMdd")}-{configData.type}-{configData.id}";
            strSql = $@"select is_paid from time_interval_redpaket_receive(nolock) where (receive_id = '{strReceiveId}')";
            var receiveData = sqlDB.Single<dynamic>(strSql);
            if (receiveData != null && receiveData.is_paid == 1)
            {
                // 已经领取过的
                return new DataResult() { status = 0, data = new { isCanReceive = 0, payMoney = 0, redpacket = 0, couponsMoney = 0 } };
            }

            // 未领取
            return new DataResult() { status = 0, data = new { isCanReceive = 1, payMoney = configData.pay_money, redpacket = configData.receive_redpacket, couponsMoney = configData.pay_money } };
        }

        #endregion
    }
}