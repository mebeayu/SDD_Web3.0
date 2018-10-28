using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using RRL.WEB.Models.Request;
using RRL.WEB.Models.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using RRL.Core.Manager;
using RRL.Config;
using System.Dynamic;
using RRL.Core.Push;
using RRL.WEB.Ulity;
using System.Text;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 游戏API
    /// </summary>
    public partial class GameController : BaseApiController
    {
        private TradeManager tm = new TradeManager();
        private RdSession rdsession = new RdSession(System.Web.HttpContext.Current);
        // 默认的兑换金豆所需的游戏总局数
        private const int CNT_CHANGE_GOLDBEAN_GAME_TIMES = 35;

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="request">qingqiu</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetUserInfo")]
        public DataResult AddFavouriteGoodsByGet([FromBody]BaseTokenRequest request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
                    var db = new RRLDB();
                    var ds = db.ExeQuery($@"SELECT
	username,
	real_name,
	head_pic,
	h_money,
	h_money_free,
    h_money_free_frz,
    isnull(h_money_pay,0) as h_money_pay,
    level_category
FROM
	rrl_user
WHERE
    id = {User.id}");
                    db.Close();
                    result.data = new
                    {
                        username = ds.Tables[0].Rows[0]["username"].ToString(),
                        real_name = ds.Tables[0].Rows[0]["real_name"].ToString(),
                        h_money = Convert.ToDecimal(ds.Tables[0].Rows[0]["h_money"]),
                        h_money_free = Convert.ToDecimal(ds.Tables[0].Rows[0]["h_money_free"]),
                        h_money_pay = Convert.ToDecimal(ds.Tables[0].Rows[0]["h_money_pay"]),
                        head_pic = $@"/SWebApi/Public/picture/{ds.Tables[0].Rows[0]["head_pic"].ToString()}.jpg",
                        small_redpackage_money = Convert.ToDecimal(ds.Tables[0].Rows[0]["h_money_pay"]),
                        level_category = ds.Tables[0].Rows[0]["level_category"].ToString()
                    };
                    return result;
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 开奖
        /// </summary>
        /// <param name="request">开奖请求</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Lottery")]
        public DataResult Lottery([FromBody]LotteryRequest request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                var User = new UserAuth();
                res = User.Load(Token.id);
                if ("1".Equals(User.is_locked_login) || "1".Equals(User.is_locked_trade))
                {
                    return new DataResult() { status = 99, message = "您的帐号被锁定,请用注册手机号码联系客服!" };
                }
                if (res == MessageCode.SUCCESS)
                {
                    var free = request.free;
                    var zs = request.zs;
                    var freetotal = free.total;
                    var zstotal = zs.total;
                    var little = request.little;
                    var littletotal = little.total;
                    object obj;
                    var lo = lotty(user: User, free: free, zs: zs, little: little, obj: out obj);
                    var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
                    if (!lo)
                    {
                        result = DataResult.InitFromMessageCode(MessageCode.ERROR_UNKONWN);

                    }
                    result.data = obj;
                    return result;
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPost]
        [ActionName("SetState")]
        public DataResult SetState([FromBody]SetStateRequest request)
        {
            int res;
            var token = TokenObject.InitTokenObjFromString(request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                var user = new UserAuth();
                res = user.Load(token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
                    var data = false;
                    var additional_data = false;
                    var db = new RRLDB();
                    /*var ds = db.ExeQuery($@"Select Count(*) From [rrl_user_play_state] Where uid = {user.id}");
                    if (ds == null)
                    {
                        db.Close();
                        return DataResult.InitFromMessageCode(MessageCode.ERROR_CODE);
                    }
                    var count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    var starttime = request.starttime;
                    var endtime = request.endtime;
                    if (count == 0)
                    {
                        db.ExeCMD($@"INSERT INTO [rrl_user_play_state] ([uid], [duration], [rdcode], [starttime], [endtime]) VALUES ({user.id},'{request.duration}','{request.rdcode}',DATEADD(s, {starttime}, '1970-01-01 00:00:00'),DATEADD(s, {endtime}, '1970-01-01 00:00:00'))");
                    }
                    else
                    {
                        db.ExeCMD($@"UPDATE TOP(1) [rrl_user_play_state] SET [duration]='{request.duration}', [endtime]=DATEADD(s, {endtime}, '1970-01-01 00:00:00') WHERE uid = {user.id}");
                    }
                    */
                    #region 随机红包队列，绑定data

                    var now = DateTime.Now;
                    var timestr = now.GetDateTimeFormats('T')[0];
                    var dateStr = now.GetDateTimeFormats('d')[0];
                    var ds = db.ExeQuery($@"select count(*)
                                              from spreader_queue
                                             where (start_at < '{timestr}')
                                               and (end_at > '{timestr}')");
                    var tcount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    if (tcount > 0)
                    {
                        ds = db.ExeQuery($@"select money ,start_at ,end_at ,id ,type
                                              from spreader_queue
                                             where (start_at < '{timestr}')
                                               and (end_at > '{timestr}')");
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            List<string> idList = new List<string>();
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                var money = Convert.ToInt32(dr["money"]);
                                if (money > 0)
                                {
                                    int intMoneyType = Convert.ToInt32(dr["type"].ToString()); // 资金类型(1：红包(免费)；2：小红包)
                                    string id = dr["id"].ToString(); // 表示时段的数据唯一ID
                                    string order_id = now.ToString("yyyyMMdd") + intMoneyType.ToString() + id;
                                    // 已领取红包的唯一主键ID = 用户ID + 年月日 + 资金类型 + 时段数据唯一ID
                                    string rrl_random_redpackage = user.id.ToString() + order_id;
                                    // 追加到ID列表
                                    idList.Add(rrl_random_redpackage);
                                }
                            }
                            if (idList.Count > 0)
                            {
                                string strIdList = $@"'{string.Join("','", idList)}'";
                                var dsRedpackage = db.ExeQuery($@"select count(*) from rrl_random_redpackage where (id in ({strIdList}))");
                                var rpCount = Convert.ToInt32(dsRedpackage.Tables[0].Rows[0][0]);
                                if (rpCount < idList.Count)
                                {
                                    // 如果查询获得的ID总数比命中了时段的ID总数少，则说明还有未领取过的红包，则提示领取
                                    data = true;
                                }
                            }
                        }
                    }

                    #endregion

                    #region 5人一组红包，绑定addtional_data

                    if (!user.has_received_h_money_five_group_spreade)
                    {
                        //additional_data = true;
                    }

                    #endregion

                    #region 针对个人发放红包
                    var nowTime = DateTime.Now;
                    string SelectPerSql = $@"select COUNT(*) as cnt from rrl_give_personal_money_config 
                        where uid = {user.id}
                        AND isReceived = 0
                        AND add_time <= '{nowTime}'
                        AND effective_time >= '{nowTime}'";
                    var PerDt = db.ExeQuery(SelectPerSql);
                    var PerCount = Convert.ToInt32(PerDt.Tables[0].Rows[0][0].ToString());
                    if (PerCount > 0)
                    {
                        additional_data = true;
                    }
                    #endregion

                    db.Close();
                    result.data = data;
                    result.additional_data = additional_data;
                    return result;
                }
            }
            return DataResult.InitFromMessageCode(MessageCode.ERROR_CODE);
        }

        /// <summary>
        /// 获取用户往期开奖数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetLogs")]
        public DataResult GetLogs([FromBody]BaseTokenRequest request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth user = new UserAuth();
                res = user.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
                    var db = new RRLDB();
                    var ds = db.ExeQuery($@"Select Top(68) [num] From [roulette_logs] Where [uid] = {user.id} Order By [id] DESC");
                    db.Close();
                    result.data = ds.Tables[0];
                    return result;
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 首次进入游戏红包奖励
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("FirstHongFreeMoneyRequest")]
        public DataResult FirstHongFreeMoneyRequest([FromBody]BaseTokenRequest request)
        {
            int res;
            var tokenObj = TokenObject.InitTokenObjFromString(request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, tokenObj.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                var user = new UserAuth();
                res = user.Load(tokenObj.id);
                if (res == MessageCode.SUCCESS)
                {
                    var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
                    if (!user.has_received_free_money_default)
                    {
                        var db = new RRLDB();
                        var ds = db.ExeQuery($@"Select [Value] From rrl_config Where [Item] = 'first_h_money_free'");
                        var first = Convert.ToDouble(ds.Tables[0].Rows[0][0]);
                        //小红包
                        db.ExeCMD($@"Update rrl_user Set h_money_pay = h_money_pay + {first},has_received_free_money_default = 1 Where id = {user.id}");
                        db.ExeCMD($@"INSERT INTO [rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({user.id}, {first},{14} ,N'首次进入游戏赠送红包')");
                        db.Close();
                        result.add_data = $@"恭喜您成功领取新人红包{first}金豆！";
                        result.data = ds.Tables[0];
                        //首次进入游戏领取红包金额
                        result.additional_data = first;
                    }
                    return result;
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 每日红包奖励
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("DailyHongFreeMoneyRequest")]
        public DataResult DailyHongFreeMoneyRequest([FromBody]BaseTokenRequest request)
        {
            int res;
            var tokenObj = TokenObject.InitTokenObjFromString(request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, tokenObj.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                var user = new UserAuth();
                res = user.Load(tokenObj.id);
                if (res == MessageCode.SUCCESS)
                {
                    var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
                    result.add_data = "您已领取过每日红包，请勿重复领取。";
                    if (!user.has_received_daily_free_h_money)
                    {
                        var now = DateTime.Now;
                        var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 30);
                        var ts = now - today;
                        var db = new RRLDB();
                        var ds = db.ExeQuery("Select [Value] From rrl_config Where [Item] = 'daily_h_money_free'");
                        var daily = Convert.ToDouble(ds.Tables[0].Rows[0][0]);
                        if (daily > 0 && user.h_money_free <= 0 && ts.TotalSeconds > 0 && !user.has_received_daily_free_h_money)
                        {
                            db.ExeCMD($@"Update rrl_user Set h_money_free = h_money_free + {daily},has_received_daily_free_h_money = 1 Where id = {user.id}");
                            db.ExeCMD($@"INSERT INTO [rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({user.id}, {daily},{14} ,N'每日赠送免费游戏红包')");
                        }
                        db.Close();
                        result.data = ds.Tables[0];
                        result.add_data = $@"您已成功领取价值{daily}金豆的每日红包，明天还有哦！";
                        //每日红包金额
                        result.additional_data = daily;
                    }

                    return result;
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 获取最近10条红包记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetAddLogs")]
        public DataResult GetAddLogs()
        {
            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT
	TOP (10) rrl_user.username AS real_name,
	isnull(rrl_user_money_record.money,0) AS addcount,
   isnull(rrl_user_money_record.freeRedPackage,0) as addfreecount
FROM
	rrl_user_money_record
LEFT JOIN rrl_user ON rrl_user.id = rrl_user_money_record.uid
WHERE
	type = 14
ORDER BY
	rrl_user_money_record.id DESC");
            db.Close();
            result.data = ds.Tables[0];
            return result;
        }

        /// <summary>
        /// 购买卡券(获取卡券待支付订单列表)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("BuyQuan")]
        public DataResult BuyQuan([FromBody]KaQuanRequest request)
        {
            var db = new RRLDB();
            try
            {
                int res;
                var data = "";
                var tokenObj = TokenObject.InitTokenObjFromString(request.token);
                if (!string.Equals(TokenObject.ShortTimeToken, tokenObj.Prefix))
                {
                    res = MessageCode.ERROR_TOKEN_VALIDATE;
                }
                else
                {
                    var user = new UserAuth();
                    res = user.Load(tokenObj.id);
                    if (res == MessageCode.SUCCESS)
                    {
                        var ds = db.ExeQuery($"Select * From rrl_goods_coupons Where id = {request.id}");
                        KaQuanModel model = null;
                        if (ds == null || ds.Tables[0].Rows.Count == 0)
                        {
                            return DataResult.InitFromMessageCode(MessageCode.ERROR_CODE);
                        }
                        else
                        {
                            model = new KaQuanModel(ds.Tables[0].Rows[0]);
                            #region 买券送免费红包,只能买一个zetee
                            if ("2".Equals(model.type))
                            {
                                request.num = "1";
                                SqlDataBase sqldb = new SqlDataBase();
                                var configList = sqldb.Select<Core.Models.Goods_Coupons_Config>("select * from rrl_goods_coupons_config order by startTime asc", null);
                                if (configList == null || configList.Count == 0)
                                {
                                    return new DataResult() { message = "活动还未准备好", status = 99 };
                                }
                                else
                                {
                                    #region 判断能否购买
                                    List<TimeSpan> TimeSpanList = new List<TimeSpan>();
                                    bool is_find = false;
                                    var nowTOD = DateTime.Now.TimeOfDay;
                                    Core.Models.Goods_Coupons_Config closeGCC = null;
                                    foreach (var item in configList)
                                    {
                                        var startTOD = item.startTime.TimeOfDay;
                                        var endTOD = item.endTime.TimeOfDay;
                                        if (startTOD <= nowTOD && nowTOD <= endTOD)
                                        {
                                            is_find = true;
                                            closeGCC = item;
                                            break;
                                        }
                                    }
                                    if (!is_find)
                                    {
                                        return new DataResult() { message = "该时间段不允许购买", status = 99 };
                                    }
                                    #endregion

                                    var now = DateTime.Now;
                                    string startDateStr = now.ToString("yyyy-MM-dd") + " " + closeGCC.startTime.TimeOfDay.ToString();
                                    string endDateStr = now.ToString("yyyy-MM-dd") + " " + closeGCC.endTime.TimeOfDay.ToString();
                                    var cnt = sqldb.ExecuteScalar<int>($@"select count(*) as cnt from rrl_coupons where uid={user.id} and is_paid=1 and addtime>='{startDateStr}' and addtime<='{endDateStr}'", null);
                                    if (cnt > 0)
                                    {
                                        return new DataResult() { message = string.Format("{0}至{1},只允许购买一次", configList[0].startTime.TimeOfDay.ToString(), configList[0].endTime.TimeOfDay.ToString()), status = 99 };
                                    }
                                }

                            }
                            #endregion
                        }
                        var valstr = $@"'{model.goodsid}','{model.money}','{model.countmoney}','{model.goldbean}','{model.starttime}','{model.endtime}','{user.id}','{model.goldcoin}','{model.redpacket}','{model.leastconsume}','{model.goodsname}','{model.id}','{model.type}'";
                        var num = Convert.ToInt32(request.num);
                        var valList = new List<string>();
                        for (var i = 0; i < num; i++)
                        {
                            valList.Add($@"({valstr})");
                        }
                        var sql = $@"INSERT INTO  [dbo].[rrl_coupons] ([goodsid], [moeny], [countmoney], [goldbean], [starttime], [endtime], [uid],[goldcoin],[redpacket],[leastconsume],[goodsname],[goods_coupons_id],[type]) output inserted.id VALUES {string.Join(",", valList.ToArray())};";
                        ds = db.ExeQuery(sql);
                        var idlist = (from DataRow row in ds.Tables[0].Rows select row[0].ToString()).ToList();
                        data = string.Join(",", idlist);


                    }
                }
                var result = DataResult.InitFromMessageCode(res);
                result.data = data;
                return result;
            }
            finally
            {
                db.Close();
            }
        }

        /// <summary>
        /// 申请卡券支付（APP）
        /// </summary>
        /// <param name="coupon_list">预支付卡券列表</param>
        /// <param name="token">短效令牌</param>
        /// <param name="trans_type">交易类型1：微信，2：支付宝</param>
        /// <param name="ip">客户ip</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ApplyCouponPay")]
        public DataResult ApplyCouponPay(string coupon_list, string token, int trans_type, string ip = null)
        {
            if (ip == null)
            {
                ip = "0.0.0.0";
            }
            int res;
            double money = 0;
            DataResult result;
            string data = "";
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    data = tm.ApplyAppCouponPay(coupon_list, ip, trans_type, out res, out money);
                }
            }
            result = DataResult.InitFromMessageCode(res);
            result.data = data;
            result.additional_data = money;
            return result;
        }

        /// <summary>
        /// 获取卡券列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("KaQuanList")]
        public ResultView<List<KaQuanModel>> GetKaQuanList()
        {
            var db = new RRLDB();
            var ds = db.ExeQuery($@"Select * From rrl_goods_coupons Where deletemark IS NULL and (type='1' or type is null) order by displayindex asc");
            db.Close();
            var result = ResultView<List<KaQuanModel>>.InitFromMessageCode(MessageCode.ERROR_CODE);
            if (ds != null)
            {
                result = ResultView<List<KaQuanModel>>.InitFromMessageCode(MessageCode.SUCCESS);
                var list = (from DataRow row in ds.Tables[0].Rows select new KaQuanModel(row)).ToList();
                result.data = list;
            }
            return result;
        }

        #region 获取红包卡券列表
        /// <summary>
        /// 获取卡券列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetRedpacketCouponsList")]
        public ResultView<List<KaQuanModel>> GetRedpacketCouponsList(int? payRedpacket)
        {
            if (payRedpacket == null)
                return new ResultView<List<KaQuanModel>>();
            string RedpacketCouponsPerRate = System.Configuration.ConfigurationManager.AppSettings["RedpacketCouponsPerRate"].ToString();
            decimal RedpacketCouponsPerRate_dec = 0.2m;
            if (!decimal.TryParse(RedpacketCouponsPerRate, out RedpacketCouponsPerRate_dec))
            {
                RedpacketCouponsPerRate_dec = 0.2m;
            }
            //1000-5000之间
            var db = new RRLDB();
            decimal money = (decimal)((payRedpacket / 100) * RedpacketCouponsPerRate_dec);
            var ds = db.ExeQuery($@"Select * From rrl_goods_coupons Where deletemark IS NULL and  type='2' and money={money} ");
            db.Close();
            var result = ResultView<List<KaQuanModel>>.InitFromMessageCode(MessageCode.ERROR_CODE);
            if (ds != null)
            {
                result = ResultView<List<KaQuanModel>>.InitFromMessageCode(MessageCode.SUCCESS);
                var list = (from DataRow row in ds.Tables[0].Rows select new KaQuanModel(row)).ToList();
                result.data = list;
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 针对个人发放红包
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("RefreshPer")]
        public DataResult RefreshPer(BaseTokenRequest request)
        {
            int res;
            decimal FreeTotalMoney = 0; //免费红包
            decimal PayTotalMoney = 0; //小红包
            decimal BeansTotalMoney = 0; //豆
            DataResult result;
            TokenObject token = TokenObject.InitTokenObjFromString(request.token);
            double hcount = 0;
            if (!string.Equals(TokenObject.ShortTimeToken, token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth user = new UserAuth();
                res = user.Load(token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var db = new RRLDB();
                    var nowTime = DateTime.Now;
                    string SelectPerSql = $@"select * from rrl_give_personal_money_config 
                        where uid = {user.id}
                        AND isReceived = 0
                        AND add_time <= '{nowTime}'
                        AND effective_time >= '{nowTime}'";
                    var PerDt = db.ExeQuery(SelectPerSql);
                    var PerCount = PerDt.Tables[0].Rows.Count;
                    foreach (DataRow item in PerDt.Tables[0].Rows)
                    {
                        var type = Convert.ToInt32(item["type"]); //红包类型
                        decimal tempMoney = Convert.ToDecimal(item["amount"]);
                        var rrl_give_personal_money_config_id = Convert.ToInt32(item["id"]);
                        var eff = db.ExeCMD($@"Update rrl_give_personal_money_config Set isReceived = 1 Where id = {rrl_give_personal_money_config_id} and isReceived=0");
                        if (eff == 1)  //没领过的领取
                        {
                            //免费红包
                            if (type == 1)
                            {
                                //写入日志表
                                db.ExeCMD(
                                    $@"INSERT INTO [rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({
                                            user.id
                                        }, {tempMoney},{18} ,N'针对个人发放红包(免费红包)')");

                                //更新用户表数据
                                db.ExeCMD($@"Update rrl_user Set h_money_free = h_money_free + {tempMoney} Where id = {user.id}");
                                FreeTotalMoney += tempMoney;
                            }
                            //小红包
                            else if (type == 2)
                            {
                                //写入日志表
                                db.ExeCMD(
                                    $@"INSERT INTO [rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({
                                            user.id
                                        }, {tempMoney},{18} ,N'针对个人发放V红包(小红包)')");
                                //更新用户表数据
                                db.ExeCMD($@"Update rrl_user Set h_money_pay = h_money_pay + {tempMoney} Where id = {user.id}");
                                PayTotalMoney += tempMoney;
                            }
                            //金豆
                            else if (type == 3)
                            {
                                //写入日志表
                                db.ExeCMD(
                                    $@"INSERT INTO [rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({
                                            user.id
                                        }, {tempMoney},{18} ,N'针对个人发放金豆(金豆)')");
                                //更新用户表数据
                                db.ExeCMD($@"Update rrl_user Set h_money = h_money + {tempMoney} Where id = {user.id}");
                                BeansTotalMoney += tempMoney;
                            }

                        }

                        hcount++;
                    }
                    db.Close();
                }
            }
            result = DataResult.InitFromMessageCode(res);
            if (res == MessageCode.SUCCESS)
            {
                //已领取红包数量
                result.add_data = hcount;
                //返回数据
                result.data = new
                {
                    FreeMoney = new
                    {
                        Money = FreeTotalMoney,
                        Msg = $"{(int)FreeTotalMoney} 红包"
                    },
                    PayMoney = new
                    {
                        Money = PayTotalMoney,
                        Msg = $"{(int)PayTotalMoney} V红包"
                    },
                    BeansMoney = new
                    {
                        Money = BeansTotalMoney,
                        Msg = $"{(int)BeansTotalMoney} 金豆"
                    },
                };
            }
            return result;
        }

        /// <summary>
        /// 刷新分享次数
        /// </summary>
        /// <param name="request">基础令牌请求</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Refresh")]
        public DataResult RefreshTimeSpreader(BaseTokenRequest request)
        {
            int res;
            DataResult result;
            TokenObject token = TokenObject.InitTokenObjFromString(request.token);
            string data = "领取失败，请重新尝试！";
            double hong = 0;
            double hcount = 0;
            if (!string.Equals(TokenObject.ShortTimeToken, token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth user = new UserAuth();
                res = user.Load(token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var now = DateTime.Now;
                    var timestr = now.GetDateTimeFormats('T')[0];
                    var dateStr = now.GetDateTimeFormats('d')[0];
                    var db = new RRLDB();
                    var ds = db.ExeQuery($@"SELECT
	COUNT(*)
FROM
	spreader_queue
WHERE
	start_at < '{timestr}'
AND end_at > '{timestr}'");
                    var count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    ds = db.ExeQuery($@"SELECT
	COUNT(*)
FROM
	spreader_queue
WHERE
	start_at > '{timestr}'
AND money > 0");
                    hcount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    if (count > 0)
                    {
                        ds = db.ExeQuery($@"SELECT
	TOP (1) money,start_at,end_at,count
FROM
	spreader_queue
WHERE
	start_at < '{timestr}'
AND end_at > '{timestr}'");
                        var money = Convert.ToInt32(ds.Tables[0].Rows[0]["money"]);

                        var startAt = Convert.ToString(ds.Tables[0].Rows[0]["start_at"]);
                        var endAt = Convert.ToString(ds.Tables[0].Rows[0]["end_at"]);
                        //倍率
                        var rate = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                        var start = Convert.ToDateTime($@"{dateStr} {startAt}");
                        var end = Convert.ToDateTime($@"{dateStr} {endAt}");
                        var a = start.CompareTo(user.last_spreader_h_money_time);
                        var b = end.CompareTo(user.last_spreader_h_money_time);
                        if (user.spreader_count >= 5)
                        {
                            //rate = rate + 1;
                        }

                        if (!(a < 0 && b > 0) && money > 0 && rate > 0)
                        {
                            db.ExeCMD($@"Update rrl_user Set h_money_free = h_money_free + {money * rate},has_received_daily_free_h_money = 1,last_spreader_h_money_time = getdate() Where id = {user.id}");
                            db.ExeCMD($@"INSERT INTO [dbo].[rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({user.id}, {money * rate},{14} ,N'每日随机时段分享赠送免费游戏红包')");
                            data = $@"你已成功领取价值为{money * rate},金豆的时段分享奖励红包，关注省兜兜，还有更多惊喜哦";
                            hong = money * rate;
                        }
                        else
                        {
                            data = $@"您已在该时段领取过分享红包，请勿重复领取";
                        }
                    }

                    db.Close();
                }
            }
            result = DataResult.InitFromMessageCode(res);
            if (res == MessageCode.SUCCESS)
            {
                //消息
                result.data = data;
                //红包金额
                result.additional_data = hong / 100;
                //剩余红包数量
                result.add_data = hcount;
            }
            return result;
        }

        /// <summary>
        /// 领取时段随机红包
        /// </summary>
        /// <param name="request">基础令牌请求</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("RefreshRandom")]
        public DataResult RefreshTimeRandom(BaseTokenRequest request)
        {//XoW68IidGJXRlX9YF4Aepm5HvXIEGANGVUODQpSlXIEGANGw6tCy2MLDoenAPTJIAHAOD8v18UZbOoPJjkHsKAMetdTYDENGHAO
            int res;
            DataResult result;
            TokenObject token = TokenObject.InitTokenObjFromString(request.token);
            string data = "领取失败，请重新尝试！";
            double hong = 0;
            string hcount = "";
            int intMoneyType = 0;
            string strMoneyType = "红包";

            if (!string.Equals(TokenObject.ShortTimeToken, token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth user = new UserAuth();
                res = user.Load(token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var now = DateTime.Now;
                    var timestr = now.GetDateTimeFormats('T')[0];
                    var dateStr = now.GetDateTimeFormats('d')[0];
                    var db = new RRLDB();
                    DataSet ds = null;
                    int Min_Game_Play_Count = 20;//最小游戏局数
                    int PlayCountToday = 0;//今天玩的游戏局数
                    //ds = db.ExeQuery($@"select [Value] from rrl_config where Item='Min_Game_Play_Count'");
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    Min_Game_Play_Count = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    //}
                    string today = DateTime.Now.ToString("yyyy-MM-dd");
                    ds = db.ExeQuery($@"select count from game_user_daily_count where uid={user.id} and date='{today}'  and active=1");
                    if (ds != null && ds.Tables[0].Rows.Count > 0) PlayCountToday = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                    ds = db.ExeQuery($@"select count(*)
                                              from spreader_queue
                                             where (start_at < '{timestr}')
                                               and (end_at > '{timestr}') and (money > 0)");
                    var count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    ds = db.ExeQuery($@"select start_at
                                           from spreader_queue
                                          where (start_at > '{timestr}')
                                            and (money > 0)
                                          order by start_at asc ");
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        hcount = ds.Tables[0].Rows[0][0].ToString();
                        hcount += "";
                    }

                    if (count > 0)
                    {
                        ds = db.ExeQuery($@"select money ,start_at ,end_at ,id ,type,min_play_count 
                                              from spreader_queue
                                             where (start_at < '{timestr}')
                                               and (end_at > '{timestr}')");

                        if (ds != null && ds.Tables.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                var money = Convert.ToInt32(dr["money"]);
                                //最少次数限制
                                Min_Game_Play_Count = Convert.ToInt32(dr["min_play_count"]);
                                if(PlayCountToday< Min_Game_Play_Count)//今天玩的局数小于了红包要求的局数
                                {
                                    db.Close();
                                    result = DataResult.InitFromMessageCode(MessageCode.ERROR_UNKONWN);
                                    result.message = "今天玩的局数小于了红包要求的局数";
                                    return result;
                                }


                                intMoneyType = Convert.ToInt32(dr["type"].ToString()); // 资金类型(1：红包(免费)；2：小红包)
                                string id = dr["id"].ToString(); // 表示时段的数据唯一ID
                                string order_id = now.ToString("yyyyMMdd") + intMoneyType.ToString() + id;
                                // 已领取红包的唯一主键ID = 用户ID + 年月日 + 资金类型 + 时段数据唯一ID
                                string rrl_random_redpackage = user.id.ToString() + order_id;

                                if (money > 0)
                                {
                                    int intMoneyRecordType = 14;
                                    if (intMoneyType == 1)
                                    {
                                        strMoneyType = "红包";
                                    }
                                    if (intMoneyType == 2)
                                    {
                                        strMoneyType = "V红包";
                                        intMoneyRecordType = 140;
                                    }

                                    int effect = db.ExeCMD($@"insert into rrl_random_redpackage(id) values('{rrl_random_redpackage}')");
                                    if (effect > 0)
                                    {
                                        // 用于根据领取红包次数，控制兑换金豆的游戏局数
                                        db.ExeCMD($@"insert into rrl_random_redpackage_times (id) values('{rrl_random_redpackage}')");

                                        db.ExeCMD($@"INSERT INTO  [rrl_user_money_record] ([uid],[order_id], [money],[type], [remark]) 
                                        values( {user.id} ,0 , {money},{intMoneyRecordType} ,N'每日随机时段赠送{strMoneyType}')");
                                        if (intMoneyType == 1)
                                        {
                                            // 更新免费红包数额
                                            db.ExeCMD($@"update rrl_user set h_money_free = isnull(h_money_free,0) + {money},has_received_daily_free_h_money = 1, last_random_h_money_time = getdate() where id = {user.id}");
                                        }
                                        else if (intMoneyType == 2)
                                        {
                                            double h_money_pay = 0;
                                            ds = db.ExeQuery($@"select h_money_pay from rrl_user where id={user.id}");
                                            if (ds != null && ds.Tables[0].Rows.Count > 0) h_money_pay = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
                                            int NeedPlayCount = Convert.ToInt32((h_money_pay / 1.5));//所需局数转换条件为V红包金额/1.5
                                            if (PlayCountToday> NeedPlayCount)
                                            {
                                                //达到条件，把V红包自动转金豆
                                                db.ExeCMD($@"update rrl_user set h_money=h_money+h_money_pay where id={user.id}");
                                            }
                                            // 更新小红包数额
                                            db.ExeCMD($@"update rrl_user set h_money_pay = {money},has_received_daily_free_h_money = 1, last_random_h_money_time = getdate() where id = {user.id}");
                                            //更新游戏局数
                                            db.ExeCMD($@"update game_user_daily_count set count = 0 where uid = {user.id} and date='{today}'");
                                        }

                                        data = $@"你已成功领取分时段小红包 {money} ，关注省兜兜，还有更多惊喜哦";
                                        hong = money;
                                    }
                                    else
                                    {
                                        db.ExeCMD($@"Update rrl_user set  has_received_daily_free_h_money = 1,last_random_h_money_time = getdate() Where id = {user.id}");
                                        data = $@"您已在该时段领取过分享红包，请勿重复领取";
                                    }
                                }
                            }
                        }
                    }

                    db.Close();
                }
            }
            result = DataResult.InitFromMessageCode(res);
            if (res == MessageCode.SUCCESS)
            {
                //消息
                result.data = hong;
                //红包金额
                result.additional_data = (int)hong + strMoneyType;
                //剩余红包数量
                result.add_data = intMoneyType;
            }
            return result;
        }



        [ActionName("RedPackageBeans")]
        public DataResult RedPackageBeans(RedPackageToBeansRequest request)
        {
            //return new DataResult { status = 99, message = "该功能已经禁用!" };
            var db = new RRLDB();
            try
            {
                int res;
                DataResult result;
                TokenObject token = TokenObject.InitTokenObjFromString(request.token);
                if (!string.Equals(TokenObject.ShortTimeToken, token.Prefix))
                {
                    res = MessageCode.ERROR_TOKEN_VALIDATE;
                    result = DataResult.InitFromMessageCode(res);
                }
                else
                {
                    UserAuth user = new UserAuth();
                    res = user.Load(token.id);
                    if (res == MessageCode.SUCCESS)
                    {

                        var now = DateTime.Now;
                        var timestr = now.ToString("HH:mm:ss");

                        //var ds = db.ExeQuery($@"SELECT top 1  money,cast(isnull(exchange_money,money*1.2) as int)  FROM   spreader_queue  where start_at<'{timestr}' and end_at>'{timestr}' order by money  desc");
                        //int spreader_redpackage = 2000000;
                        int total_hb = 0;
                        //if (!int.TryParse(new ConfigManager().GetConfigValue("GameCenter_RedpackageToBean_Min"), out total_hb))
                        //{
                        //    return new DataResult { status = 99, message = "未配置最小转换额度，不能转换成金豆!" };
                        //}
                        //decimal freeRedPackage_to_beans_rate = 1.2m;
                        //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        //{
                        //    spreader_redpackage = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        //    total_hb = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                        //}else
                        //{
                        //    return new DataResult { status = 99, message = "该时间段未配置可转换金额!" };
                        //}
                        string strMoneyType = string.Empty;
                        if (!string.IsNullOrEmpty(request.moneyType))
                        {
                            strMoneyType = request.moneyType.ToUpper();
                        }
                        switch (strMoneyType)
                        {
                            case "FREEREDPACKET": // 免费红包
                                if (user.h_money_free <= 0.1d)
                                {
                                    return new DataResult { status = 99, message = "红包数量不足,明天继续努力!" };
                                }
                                break;
                            case "REDPACKET": // V红包
                                if (user.h_money_pay <= 0.1m)
                                {
                                    return new DataResult { status = 99, message = "V红包数量不足,明天继续努力!" };
                                }
                                break;
                        }

                        GameManager GameMgr = new GameManager();
                        int GameCenter_can_change_goldbeans_times = GetCanChangeToGoldBeanTimes(user, request.moneyType);
                        if (GameCenter_can_change_goldbeans_times < 0)
                        {
                            return new DataResult { status = 99, message = "未配置最小转换成金豆的次数,请联系客服!" };
                        }

                        int count = GameMgr.GetGamePlayTimesByMoneyType(user.id, request.moneyType);
                        if (count < GameCenter_can_change_goldbeans_times)
                        {
                            return new DataResult { status = 99, message = $@"今天玩游戏次数未到{GameCenter_can_change_goldbeans_times}把,继续努力哦!" };
                        }

                        int effect = 0;
                        int intMoneyType = 0;
                        switch (strMoneyType)
                        {
                            case "FREEREDPACKET": // 免费红包
                                intMoneyType = 1;
                                effect = db.ExeCMD($@"update rrl_user set h_money=isnull(h_money,0)+(isnull(h_money_free,0)/1),h_money_free=0  where id={user.id} and h_money_free>={total_hb} ");
                                if (effect > 0)
                                {
                                    db.ExeCMD($@"INSERT INTO  [rrl_user_money_record] ([uid], [money],[type], [remark])
                                     select {user.id}, {user.h_money_free}/1,200,N'红包转到金豆的收入' from rrl_user where id={user.id} and h_money_free=0");
                                }
                                break;
                            case "REDPACKET": // V红包
                                intMoneyType = 2;
                                effect = db.ExeCMD($@"update rrl_user set h_money=isnull(h_money,0)+(isnull(h_money_pay,0)/1),h_money_pay=0  where id={user.id} and h_money_pay>={total_hb} ");
                                if (effect > 0)
                                {
                                    db.ExeCMD($@"INSERT INTO  [rrl_user_money_record] ([uid], [money],[type], [remark])
                                     select {user.id}, {user.h_money_pay}/1,200,N'V红包转到金豆的收入' from rrl_user where id={user.id} and h_money_pay=0");
                                }
                                break;
                        }

                        if (effect > 0)
                        {
                            SqlDataBase sqlDB = new SqlDataBase();
                            // 构造领取次数记录表中ID前缀
                            string strIdPrefix = $@"{user.id}{DateTime.Now.ToString("yyyyMMdd")}{intMoneyType}";
                            string strSql = $@"delete from rrl_random_redpackage_times where (id like '{strIdPrefix}%')";
                            sqlDB.Execute(strSql);

                            if (intMoneyType > 0)
                            {
                                // 清空用户当天指定资金类型玩的游戏的总局数
                                strSql = $@"update game_user_play_times set play_times = 0 where (uid = @uid) and (money_type = @money_type)";
                                sqlDB.Execute(strSql, new { uid = user.id, money_type = request.moneyType });
                            }

                            return new DataResult { status = 0, message = "ok" };
                        }
                        else
                        {
                            return new DataResult { status = 99, message = "红包不足" + total_hb + ", 不能转换成金豆!" };
                        }

                    }
                    result = DataResult.InitFromMessageCode(res);
                }
                return result;
            }
            finally
            {
                db.Close();
            }
        }


        [ActionName("RedPackageToCoupons")]
        public DataResult RedPackageToCoupons(BaseTokenRequest request)
        {
            var db = new RRLDB();
            try
            {
                int res;
                DataResult result;
                TokenObject token = TokenObject.InitTokenObjFromString(request.token);
                if (!string.Equals(TokenObject.ShortTimeToken, token.Prefix))
                {
                    res = MessageCode.ERROR_TOKEN_VALIDATE;
                    result = DataResult.InitFromMessageCode(res);
                }
                else
                {
                    UserAuth user = new UserAuth();
                    res = user.Load(token.id);
                    if (res == MessageCode.SUCCESS)
                    {

                        var now = DateTime.Now;
                        var timestr = now.ToString("yyyy-MM-dd HH:mm:ss");

                        int intExpireDays = 0;
                        if (!int.TryParse(new ConfigManager().GetConfigValue("RedpackageToVoucher_ExpiredDays"), out intExpireDays))
                        {
                            intExpireDays = 15;
                        }
                        var enddatestr = now.AddDays(intExpireDays).ToString("yyyy-MM-dd HH:mm:ss");

                        var ds = db.ExeQuery($@"SELECT h_money_free  FROM  rrl_user   where id={user.id}");
                        var h_money_free = 0m;
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            h_money_free = Convert.ToDecimal(ds.Tables[0].Rows[0][0]);
                        }
                        decimal total_hb = 100000;
                        if (h_money_free < total_hb)
                        {

                            return new DataResult { status = 99, message = "免费红包不足" + total_hb + ", 不能转换成购物券!" };
                        }
                        else
                        {
                            int Coupons_count = (int)(h_money_free / 100000.0m);
                            decimal sx_h_money_free = h_money_free - (Coupons_count * 100000.0m);
                            sx_h_money_free = sx_h_money_free > 0 ? sx_h_money_free : 0;
                            var effect = db.ExeCMD($@"update rrl_user set  h_money_free={sx_h_money_free}  where id={user.id} ");

                            var valstr = $@"0,10,10,0,'{timestr}','{enddatestr}', {user.id} ,0,0,200,'代金券，所有商品，购物满200元能使用', 19 ,3,1";
                            var num = Coupons_count;
                            var valList = new List<string>();
                            for (var i = 0; i < num; i++)
                            {
                                valList.Add($@"({valstr})");
                            }
                            var sql = $@"INSERT INTO  [dbo].[rrl_coupons] ([goodsid], [moeny], [countmoney], [goldbean], [starttime], [endtime], [uid],[goldcoin],[redpacket],[leastconsume],[goodsname],[goods_coupons_id],[type],[is_paid]) output inserted.id VALUES {string.Join(",", valList.ToArray())};";
                            var ds2 = db.ExeQuery(sql);
                        }





                    }
                    result = DataResult.InitFromMessageCode(res);
                }
                return result;
            }
            finally
            {
                db.Close();
            }
        }

        /// <summary>
        /// 领取5人一组红包
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        public DataResult FiveSpreaderGroup([FromBody]BaseTokenRequest request)
        {
            int res;
            DataResult result;
            TokenObject token = TokenObject.InitTokenObjFromString(request.token);
            string data = "红包领取失败，请重新尝试！";
            double hong = 0;
            double spreader_count = 0;
            if (!string.Equals(TokenObject.ShortTimeToken, token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth user = new UserAuth();
                res = user.Load(token.id);
                if (res == MessageCode.SUCCESS)
                {
                    if (/*!user.has_received_h_money_five_group_spreade*/false)
                    {
                        var mod = user.spreader_count % 5;
                        hong = 1000;
                        if (mod == 0)
                        {
                            hong = 6000;
                        }
                        if (mod == 0)
                        {
                            mod = 5;
                        }
                        spreader_count = mod;
                        var db = new RRLDB();
                        db.ExeCMD($@"Update rrl_user Set h_money_free = h_money_free + {hong},has_received_h_money_five_group_spreade = 1 Where id = {user.id}");
                        db.ExeCMD($@"INSERT INTO  [rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({user.id}, {hong},{14} ,N'五人一组推荐红包')");
                        db.Close();
                        data = $@"您成功推荐本组第{ mod }个人，可领取价值为{hong}金豆的随机红包，分享更多，领取更多！";
                    }
                }
            }
            result = DataResult.InitFromMessageCode(res);
            if (res == MessageCode.SUCCESS)
            {
                //消息
                result.data = data;
                //红包金额
                result.additional_data = hong / 100;
                //分享到第几个人
                result.add_data = spreader_count;
            }
            return result;
        }

        // STORY #6 lcl 20180418 Insert
        /// <summary>
        /// 被推荐用户通过省兜兜APP登录后，奖励推荐人小红包(新游戏中心分享得小红包功能)
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SpreaderShareHongMoneyPay")]
        public DataResult SpreaderShareHongMoneyPay([FromBody]SpreaderShareRedpackageParameter param)
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace(param.DeviceInfo))
            {
                // 参数无效时，给出参数错误的提示
                return new DataResult() { status = -1, message = "未获取到参数" };
            }

            string strToken = string.Empty;
            string strDeviceID = string.Empty;

            // 解密获得原始参数字符串。原始参数字符串格式：token=token字符串&deviceId=设备标识
            string strOriginalString = DES.DecryptDES(param.DeviceInfo, RRLConfig.DES_SECRET_KEY);
            if (strOriginalString.Equals(param.DeviceInfo))
            {
                // 如果解密后的字符串等于需要解密的密文字符串，则说明解密失败
                return new DataResult() { status = -2, message = "参数转换失败" };
            }

            // 从解密后的明文参数字符串中解析出用户token和设备标识
            IDictionary<string, string> dicParameter = GetUrlParameter(strOriginalString);
            if (dicParameter.ContainsKey("token"))
            {
                // 获取token
                strToken = dicParameter["token"];
            }
            if (dicParameter.ContainsKey("deviceId"))
            {
                // 获取设备ID
                strDeviceID = dicParameter["deviceId"];
            }
            if (string.IsNullOrWhiteSpace(strToken) || string.IsNullOrWhiteSpace(strDeviceID))
            {
                // token或者设备ID中的任意一个值无效，都视为参数无效
                return new DataResult() { status = -3, message = "获取参数值失败" };
            }
            // 将token字符串转换为token对象
            var tokenObj = TokenObject.InitTokenObjFromString(strToken);
            if (tokenObj.id < 1)
            {
                // token转换失败
                return new DataResult() { status = -4, message = "Token无效" };
            }
            // 根据token中的用户ID，获取用户信息
            UserAuth User = new UserAuth();
            if (User.Load(tokenObj.id) != MessageCode.SUCCESS)
            {
                // 用户信息获取失败
                return new DataResult() { status = -5, message = "用户信息获取失败" };
            }
            //极光推送
            if (!string.IsNullOrWhiteSpace(param.RegistrationId))
            {
                PushbindingManager pushBiz = new PushbindingManager();
                pushBiz.PushBinding(new { id = Guid.NewGuid().ToString(), tag = "", registration_id = param.RegistrationId, alias = User.username, uid = User.id, platform = "", DateTime.Now });
            }
            // 获取数据库访问对象
            SqlDataBase db = new SqlDataBase();
            // 执行奖励小红包的存储过程
            List<dynamic> resultWithSP = db.ExecuteStoredProcedure<dynamic>(@"sp_V3_SpreaderShareRedpackage",
                new
                {
                    target_UserName = User.username,
                    device_ID = strDeviceID
                });

            if (resultWithSP == null || resultWithSP.Count < 1)
            {
                // 存储过程执行失败
                return new DataResult() { status = -6, message = "存储过程执行失败" };
            }

            var result = resultWithSP.First();
            // 存储过程执行成功
            return new DataResult() { status = result.result, message = result.message };
        }

        // PROJECT #8 lcl 20180425 Insert
        /// <summary>
        /// 获取自营和第三方的游戏基础数据
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GameBaseData")]
        public DataResult GameBaseData([FromBody]GameDataRequest request)
        {
            // 返回游戏数据集合
            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);

            // 获取全部有效的游戏数据
            var allGameList = new GameManager().GetGameList();
            if (allGameList != null && allGameList.Count > 0)
            {
                result.data = allGameList.Where(p => p.type.StartsWith(request.Type))
                                         .OrderBy(p => p.sortcode)
                                         /*.Select(p => new
                                         {
                                             name = p.name,
                                             action_js = p.action_js,
                                             logo_url = p.logo_url,
                                             background_color = p.background_color,
                                             simple_remark = p.simple_remark 
                                         })*/;
            }

            return result;
        }

        // lcl 20180504 Insert
        /// <summary>
        /// 获取签到领小红包数据
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpPost]
        public DataResult MoneyPaySignInData([FromBody]BaseTokenRequest request)
        {
            // 声明返回的结果对象
            dynamic resultData = new ExpandoObject();

            // 获取用户数据管理对象
            UserManager userMgr = new UserManager();
            // 默认设置否登录标记为“未登录”状态
            resultData.isLogin = false;
            // 默认设置连续签到日期数为0
            resultData.continuousSignDays = 0;
            // 初始化token对象
            TokenObject tokenObj = new TokenObject();

            tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth User = new UserAuth();
            int res = User.Load(tokenObj.id);
            if (res == MessageCode.SUCCESS)
            {
                // 如果token中的用户ID有效，则认为已登录
                resultData.isLogin = true;
            }
            else
            {
                resultData.isLogin = false;
            }

            // 获取每日签到小红包配置数据
            var moneyPayDayConfigList = userMgr.GetMoneyPayConfigList();
            resultData.moneyPayDayConfig = moneyPayDayConfigList.Select(p => new { title = p.title, day_index = p.day_index, money = p.money, is_signin = p.is_signin });

            // 初始化时间差值
            resultData.countdown = 0;

            if (!resultData.isLogin)
            {
                // 未登录。也让用户能看到可以领取的状态
                resultData.isCanGetMoneyPay = true;
            }
            else
            {
                // 已登录。
                bool blnExistDataInValidCycles;
                bool blnIsCanGetMoneyPay;
                var userMoneyPayList = userMgr.GetUserSignMoneyPayList(tokenObj.id, out blnExistDataInValidCycles, out blnIsCanGetMoneyPay);
                if (blnExistDataInValidCycles)
                {
                    // 有效周期内存在数据时，将签到配置数据集合中对应的签到标记设置为“已签到”
                    moneyPayDayConfigList.Take(userMoneyPayList.Count).ToList().ForEach(p => p.is_signin = "1");
                    // 设置连续签到日期总天数
                    resultData.continuousSignDays = userMoneyPayList.Count;
                }
                // 设置当天是否可以领取红包
                resultData.isCanGetMoneyPay = blnIsCanGetMoneyPay;
            }

            // 倒计时
            DateTime dtEndDatetime = DateTime.Now.Date.AddDays(1);
            TimeSpan ts = dtEndDatetime - DateTime.Now;
            // 获取时间差所表示的总的秒数
            resultData.countdown = Math.Floor(ts.TotalSeconds);

            DataResult dataResult = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            dataResult.data = resultData;

            return dataResult;
        }

        // lcl 20180506 Insert
        /// <summary>
        /// 每日小红包签到
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpPost]
        public DataResult SignInMoneyPay([FromBody]BaseTokenRequest request)
        {
            // 获取数据库访问对象
            SqlDataBase db = new SqlDataBase();
            // 获取用户数据管理对象
            UserManager userMgr = new UserManager();

            if (string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 初始化token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            if (tokenObj.id < 1)
            {
                // Token无效不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_VALIDATE);
            }

            // 限制当天零点至零点五分不能领取
            DateTime dtNow = DateTime.Now;
            if (dtNow >= dtNow.Date && dtNow <= dtNow.Date.AddMinutes(5))
            {
                // 系统自动处理数据的时间段内，不能签到
                return new DataResult()
                {
                    status = 1001,
                    message = "由于数据处理，零点至零点五分不能领取"
                };
            }

            bool blnExistDataInValidCycles;
            bool blnIsCanGetMoneyPay;
            var userRedpackageList = userMgr.GetUserSignMoneyPayList(tokenObj.id, out blnExistDataInValidCycles, out blnIsCanGetMoneyPay);
            if (blnIsCanGetMoneyPay)
            {
                // 执行签到领小红包的存储过程
                List<dynamic> result = db.ExecuteStoredProcedure<dynamic>(@"sp_V3_ReceiveMoneyPay",
                    new
                    {
                        uid = tokenObj.id,
                        isExistDataInValidCycles = blnExistDataInValidCycles
                    });
            }

            // 返回执行成功的数据
            return DataResult.InitFromMessageCode(MessageCode.SUCCESS);
        }

        // lcl 20180506 Insert
        /// <summary>
        /// 获取签到领取的小红包的倒计时数据
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult MoneyPayCountdown()
        {
            DataResult dataResult = DataResult.InitFromMessageCode(MessageCode.SUCCESS);

            DateTime dtEndDatetime = DateTime.Now.Date.AddDays(1);
            TimeSpan ts = dtEndDatetime - DateTime.Now;
            // 获取时间差所表示的总的秒数
            dataResult.data = Math.Floor(ts.TotalSeconds);

            return dataResult;
        }

        // lcl 20180608 Insert
        [HttpGet]
        public DataResult GamePlayTimesByUser([FromUri]BaseTokenRequest request, string game_type = "")
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObject = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int intResult = user.Load(tokenObject.id);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            // 获取游戏数据管理对象
            GameManager GameMgr = new GameManager();
            ConfigManager configMgr = new ConfigManager();

            DataResult result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            if (string.IsNullOrWhiteSpace(game_type))
            {
                result.data = GameMgr.GetGamePlayTimesByUser(user.id);
            }
            else
            {
                result.data = GameMgr.GetGamePlayTimesByType(user.id, game_type);
            }

            if (new UserManager().IfNewUser(user))
            {
                // 新用户
                result.add_data = configMgr.GetConfigValue("GameCenter_can_change_goldbeans_times_NewUser");
            }
            else
            {
                // 老用户
                result.add_data = configMgr.GetConfigValue("GameCenter_can_change_goldbeans_times");
            }

            return result;
        }

        // lcl 20180705 Insert
        [HttpGet]
        public DataResult GamePlayTimesByMoneyType([FromUri]GamePlayTimesRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObject = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int intResult = user.Load(tokenObject.id);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            // 获取游戏数据管理对象
            GameManager GameMgr = new GameManager();
            ConfigManager configMgr = new ConfigManager();
            UserManager userMgr = new UserManager();

            DataResult result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            result.data = GameMgr.GetGamePlayTimesByMoneyType(user.id, request.moneyType);

            int GameCenter_can_change_goldbeans_times = GetCanChangeToGoldBeanTimes(user, request.moneyType);
            if (GameCenter_can_change_goldbeans_times < 0)
            {
                GameCenter_can_change_goldbeans_times = CNT_CHANGE_GOLDBEAN_GAME_TIMES;
            }

            result.add_data = GameCenter_can_change_goldbeans_times;

            return result;
        }

        // lcl 20180613 Insert
        /// <summary>
        /// 被推荐人在微信公众号中登录系统，赠送推荐人红包
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public DataResult SpreaderShareRedpackageForWeChat([FromUri]WeChatBaseRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObject = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int intResult = user.Load(tokenObject.id);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return DataResult.InitFromMessageCode(intResult);
            }

            string strOpenId = request.openId;
            if (string.IsNullOrWhiteSpace(strOpenId))
            {
                // 如果从前端参数中获取不到OpenId，则尝试从Redis中获取
                if (rdsession.Exist("openid"))
                {
                    strOpenId = rdsession["openid"];
                }
            }

            // 执行奖励小红包的存储过程
            List<dynamic> resultWithSP = new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_SpreaderShareRedpackage",
                new
                {
                    target_UserName = user.username,
                    device_ID = strOpenId
                });

            if (resultWithSP == null || resultWithSP.Count < 1)
            {
                // 存储过程执行失败
                return new DataResult() { status = -6, message = "存储过程执行失败" };
            }

            var result = resultWithSP.First();
            // 存储过程执行成功
            return new DataResult() { status = result.result, message = result.message };
        }

        // lcl 20180712 Insert
        /// <summary>
        /// 每日游戏自动红包
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpPost]
        public DataResult GameAutoRedpackageDaily([FromBody]GameAutoRedpackageRequest request)
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

            // 执行领取自动发红包的存储过程
            List<dynamic> resultWithSP = new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_GameAutoRedpackageDaily",
                new
                {
                    uid = user.id,
                    source = request.source
                });

            if (resultWithSP == null || resultWithSP.Count < 1)
            {
                // 存储过程执行失败
                return new DataResult() { status = -1, message = "存储过程执行失败" };
            }

            // 存储过程执行成功
            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            result.data = resultWithSP.First();

            return result;
        }

        // lcl 20180716 Insert
        /// <summary>
        /// 每日做任务领取的红包
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("DoTaskDaily")]
        public DataResult TaskRedpackageDaily([FromBody]TaskRedpackageRequest request)
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

            // 执行领取自动发红包的存储过程
            List<dynamic> resultWithSP = new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_TaskRedpackageDaily",
                new
                {
                    uid = user.id,
                    task_type = request.taskType
                });

            if (resultWithSP == null || resultWithSP.Count < 1)
            {
                // 存储过程执行失败
                return new DataResult() { status = -1, message = "存储过程执行失败" };
            }

            // 存储过程执行成功
            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            result.data = resultWithSP.First();

            return result;
        }

        private int saveLoseRecord(Game_Record gameWinRecord, String type, String year)
        {
            SqlDataBase db = new SqlDataBase();
            string sql = "insert into game_record_lose_" + type + "_" + year + " (uid,goldenBeans,redPacket,time_stamp,result_type,play_id,result_message,goldenCoin,freeRedPacket) VALUES(@uid,@goldenBeans,@redPacket,@time_stamp,@result_type,@play_id,@result_message,@goldenCoin,@freeRedPacket)";
            int rs = db.Execute(sql, new { uid = gameWinRecord.uid, goldenBeans = gameWinRecord.goldenBeans, redPacket = gameWinRecord.redPacket, time_stamp = gameWinRecord.timeStamp, result_type = gameWinRecord.resultType, play_id = gameWinRecord.playId, result_message = gameWinRecord.resultMessage, goldenCoin = gameWinRecord.goldenCoin, freeRedPacket = gameWinRecord.freeRedPacket });
            return rs;
        }

        private int saveWinRecord(Game_Record gameWinRecord, string type, String year)
        {
            SqlDataBase db = new SqlDataBase();
            string sql = "insert into game_record_win_" + type + "_" + year + " (uid,goldenBeans,redPacket,time_stamp,result_type,play_id,result_message,goldenCoin,freeRedPacket) VALUES(@uid,@goldenBeans,@redPacket,@time_stamp,@result_type,@play_id,@result_message,@goldenCoin,@freeRedPacket)";
            int rs = db.Execute(sql, new { uid = gameWinRecord.uid, goldenBeans = gameWinRecord.goldenBeans, redPacket = gameWinRecord.redPacket, time_stamp = gameWinRecord.timeStamp, result_type = gameWinRecord.resultType, play_id = gameWinRecord.playId, result_message = gameWinRecord.resultMessage, goldenCoin = gameWinRecord.goldenCoin, freeRedPacket = gameWinRecord.freeRedPacket });
            return rs;
        }

        public int updateUserGoldenBeans(int uid, decimal goldenBeans, decimal redPacket, decimal freeRedPacket)
        {
            SqlDataBase db = new SqlDataBase();

            // lcl 2018-07-13 Insert
            if (freeRedPacket > 0)
            {
                // 免费红包大于0，则表示是奖励了，直接转金豆
                goldenBeans = goldenBeans + freeRedPacket;
                // 免费红包设置为0，不参与免费红包账户的累加
                freeRedPacket = 0;
            }

            string sql = "UPDATE rrl_user SET h_money = h_money + @goldenBeans ,h_money_free = h_money_free + @freeRedPacket,h_money_pay = h_money_pay + @redPacket WHERE id =@id and h_money+@goldenBeans>=0 and h_money_free+@freeRedPacket >=0 and isnull(h_money_pay,0) + @redPacket >= 0  and isnull(is_locked_login,'0')='0' and isnull(is_locked_trade,'0')='0'";
            int rs = db.Execute(sql, new { id = uid, goldenBeans = goldenBeans, redPacket = redPacket, freeRedPacket = freeRedPacket });
            return rs;
        }

        /// <summary>
        /// 开奖
        /// </summary>
        /// <param name="user"></param>
        /// <param name="free"></param>
        /// <param name="zs"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool lotty(UserAuth user, LotteryModel free, LotteryModel zs, LotteryModel little, out object obj)
        {
            obj = new
            {
                addcount = 0,
                result = 0,
                addfreecount = 0
            };

            if (free.da > 0 || free.xiao > 0 || free.dan > 0 || free.shuang > 0 || free.hong > 0)
            {
                if (zs.da > 0 || zs.xiao > 0 || zs.dan > 0 || zs.shuang > 0 || zs.hong > 0)
                {
                    return false;
                }
                if (little.da > 0 || little.xiao > 0 || little.dan > 0 || little.shuang > 0 || little.hong > 0)
                {
                    return false;
                }
            }
            /*if (free.da > 101 || free.xiao > 101 || free.dan > 101 || free.shuang > 101 || free.hong > 101)
            {
                return false;
            }*/

            string lose_result_type_json = "[" + Newtonsoft.Json.JsonConvert.SerializeObject(free) + "," + Newtonsoft.Json.JsonConvert.SerializeObject(little) + "," + Newtonsoft.Json.JsonConvert.SerializeObject(zs) + "]";

            //var less = user.h_money - zs.total;
            //var lessfree = user.h_money_free - free.total;
            //var lesslittle=(double) user.h_money_pay - little.total;
            string play_id = Guid.NewGuid().ToString("N");
            string game_type = "0001";
            string year = DateTime.Now.ToString("yyyy");
            #region 游戏出资逻辑
            var r = new Random();
            var num = r.Next(1, 27);

            double freeaddcount = 0;
            double zsaddcount = 0;
            double littleaddcount = 0;

            if (user.h_money_free < free.total)
            {
                return false;
            }
            if (user.h_money < zs.total)
            {
                return false;
            }
            if ((double)user.h_money_pay < little.total)
            {
                return false;
            }

            if (num < 25)
            {
                if (num < 13)
                {
                    zsaddcount += zs.xiao * 2;
                    freeaddcount += free.xiao * 2;
                    littleaddcount += little.xiao * 2;
                }
                else
                {
                    zsaddcount += zs.da * 2;
                    freeaddcount += free.da * 2;
                    littleaddcount += little.da * 2;
                }
                if (num % 2 == 0)
                {
                    zsaddcount += zs.shuang * 2;
                    freeaddcount += free.shuang * 2;
                    littleaddcount += little.shuang * 2;
                }
                else
                {
                    zsaddcount += zs.dan * 2;
                    freeaddcount += free.dan * 2;
                    littleaddcount += little.dan * 2;
                }
            }
            else
            {
                zsaddcount += zs.hong * 12;
                freeaddcount += free.hong * 12;
                littleaddcount += little.hong * 12;
            }


            #endregion

            //扣钱再说
            if ((decimal)zs.total > 0 || (decimal)little.total > 0 || (decimal)free.total > 0)
            {
                int effuser_lose = updateUserGoldenBeans(user.id, -(decimal)zs.total, -(decimal)little.total, -(decimal)free.total);
                if (effuser_lose <= 0)
                {
                    return false;
                }
                //扣的记录写到lose 表
                int efflose = saveLoseRecord(new Game_Record()
                {
                    freeRedPacket = -(decimal)free.total,
                    goldenBeans = -(decimal)zs.total,
                    goldenCoin = 0m,
                    playId = play_id,
                    redPacket = -(decimal)little.total,
                    resultType = lose_result_type_json,
                    timeStamp = DateTime.Now,
                    resultMessage = "开奖:" + num,
                    uid = user.id
                }, game_type, year);
            }

            //开奖了,如果奖励了就加帐号
            if ((zsaddcount + freeaddcount + littleaddcount) > 0)
            {
                int effuser_add = updateUserGoldenBeans(user.id, (decimal)zsaddcount + (decimal)littleaddcount, 0m, (decimal)freeaddcount);
                //
                int effwin = saveWinRecord(new Game_Record()
                {
                    freeRedPacket = (decimal)freeaddcount,
                    goldenBeans = (decimal)zsaddcount + (decimal)littleaddcount,
                    goldenCoin = 0m,
                    playId = play_id,
                    redPacket = 0,
                    resultType = lose_result_type_json,
                    timeStamp = DateTime.Now,
                    resultMessage = "开奖:" + num,
                    uid = user.id
                }, game_type, year);
            }
            SqlDataBase db = new SqlDataBase();
            int logs_effect = db.Execute($@"INSERT INTO  [roulette_logs] ([uid], [num]) VALUES ({user.id}, {num})", null);
            obj = new
            {

                addcount = zsaddcount + littleaddcount,
                result = num,
                addfreecount = freeaddcount
            };
            return true;
        }

        /// <summary>
        /// 从一个类似url参数字符串中解析出参数集合
        /// </summary>
        /// <param name="urlParameterString">表示url参数的字符串</param>
        /// <returns>参数键值对集合</returns>
        private IDictionary<string, string> GetUrlParameter(string urlParameterString)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            int intDelimiterIndex = 0; // 分隔符位置索引
            string[] arrParameter = urlParameterString.Split('&');

            foreach (var item in arrParameter)
            {
                if (item.Length < 3)
                {
                    // 少于3个字符视为无效。格式：参数名=参数值，最少需要3个字符
                    continue;
                }

                intDelimiterIndex = item.IndexOf("=");
                if (intDelimiterIndex > 0)
                {
                    // =分隔符所在位置的索引，最小需要从索引1开始
                    dic.Add(item.Substring(0, intDelimiterIndex), item.Substring(intDelimiterIndex + 1));
                }
            }

            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="moneyType">资金类型</param>
        /// <returns></returns>
        private int GetCanChangeToGoldBeanTimes(UserAuth user, string moneyType)
        {
            if (string.IsNullOrWhiteSpace(moneyType))
            {
                // 无法确定类型
                return CNT_CHANGE_GOLDBEAN_GAME_TIMES;
            }

            SqlDataBase sqlDB = new SqlDataBase();
            ConfigManager configMgr = new ConfigManager();
            int intDefaultTimes = 0; // 默认的兑换金豆的游戏局数
            int intTimesIncrement = 0; // 每多领一个红包，兑换金豆的游戏局数的递增量
            int intMoneyType = 0;
            string strMoneyType = moneyType.ToUpper();
            switch (strMoneyType)
            {
                case "FREEREDPACKET": // 免费红包
                    int.TryParse(configMgr.GetConfigValue("GameCenter_can_change_goldbeans_times"), out intDefaultTimes);
                    int.TryParse(configMgr.GetConfigValue("Game_Times_ChangeGoldBean_Rule_MoneyFree"), out intTimesIncrement);
                    intMoneyType = 1;
                    break;
                case "REDPACKET": // V红包
                    int.TryParse(configMgr.GetConfigValue("GameCenter_can_change_goldbeans_times_MoneyPay"), out intDefaultTimes);
                    int.TryParse(configMgr.GetConfigValue("Game_Times_ChangeGoldBean_Rule_MoneyPay"), out intTimesIncrement);
                    intMoneyType = 2;
                    break;
            }

            if (intDefaultTimes < 1)
            {
                intDefaultTimes = CNT_CHANGE_GOLDBEAN_GAME_TIMES;
            }

            //if ((strMoneyType == "FREEREDPACKET" && user.h_money_free < 0.1d) || (strMoneyType == "REDPACKET" && user.h_money_pay == 0))
            //{
            //    // 如果用户对应的账户余额为0，则返回默认局数
            //    return intDefaultTimes;
            //}

            // 获取用户当天领取的随机红包次数
            // 构造领取记录表中ID前缀
            string strIdPrefix = $@"{user.id}{DateTime.Now.ToString("yyyyMMdd")}{intMoneyType}";
            string strSql = $@"select count(0) from rrl_random_redpackage_times(nolock) where (id like '{strIdPrefix}%')";
            var randomRedpackageCount = sqlDB.ExecuteScalar<int>(strSql);
            // 没有领取过的和领过1次的，都不递增
            int intIncrementCount = randomRedpackageCount == 0 ? 0 : randomRedpackageCount - 1;

            int intTimesTotal = intDefaultTimes + intTimesIncrement * intIncrementCount;
            return intTimesTotal;
        }
    }

    /// <summary>
    /// 代金券购买请求
    /// </summary>
    public class KaQuanRequest
    {
        /// <summary>
        /// 购买卡券id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public string num { get; set; }
        /// <summary>
        /// 短效令牌
        /// </summary>
        public string token { get; set; }
    }

    /// <summary>
    /// 代金券模型rrl_goods_coupons
    /// </summary>
    public class KaQuanModel
    {
        /// <summary>
        /// 卡券标识
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        public int goodsid { get; set; }
        /// <summary>
        /// 购买代金券金额
        /// </summary>
        public decimal money { get; set; }
        /// <summary>
        /// 代金券金额
        /// </summary>
        public decimal countmoney { get; set; }
        /// <summary>
        /// 赠送金豆数额
        /// </summary>
        public int goldbean { get; set; }
        /// <summary>
        /// 开始使用时间
        /// </summary>
        public DateTime starttime { get; set; }
        /// <summary>
        /// 结束使用时间
        /// </summary>
        public DateTime endtime { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string goodsname { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime addtime { get; set; }
        /// <summary>
        /// 赠送金币
        /// </summary>
        public int goldcoin { get; set; }
        /// <summary>
        /// 最少消费可抵用
        /// </summary>
        public decimal leastconsume { get; set; }

        /// <summary>
        /// 券类型[1=买券送豆,2=买券送红包]
        /// </summary>
        public string type { get; set; }

        public decimal redpacket { get; set; }

        //public string name { get; set; }



        public KaQuanModel(DataRow row)
        {
            id = Convert.ToInt32(row["id"]);
            goodsid = Convert.ToInt32(row["goodsid"]);
            money = Convert.ToDecimal(row["money"]);
            countmoney = Convert.ToDecimal(row["countmoney"]);
            goldbean = Convert.ToInt32(row["goldbean"]);
            starttime = Convert.ToDateTime(row["starttime"]);
            endtime = Convert.ToDateTime(row["endtime"]);
            goodsname = Convert.ToString(row["goodsname"]);
            addtime = Convert.ToDateTime(row["addtime"]);
            goldcoin = Convert.ToInt32(row["goldcoin"]);
            if (decimal.TryParse(row["leastconsume"].ToString(), out decimal leastconsume_s))
            {
                leastconsume = leastconsume_s;
            }
            else
            {
                leastconsume = 0;
            }

            type = row["type"].ToString();

            if (decimal.TryParse(row["redpacket"].ToString(), out decimal redpacket_s))
            {
                redpacket = redpacket_s;
            }
            else
            {
                redpacket = 0;
            }

        }
    }

    /// <summary>
    /// 红包开奖请求参数
    /// </summary>
    public class LotteryRequest
    {
        /// <summary>
        /// 免费红包
        /// </summary>
        public LotteryModel free { get; set; }

        /// <summary>
        /// 赠送红包
        /// </summary>
        public LotteryModel zs { get; set; }


        public LotteryModel little { get; set; }
        /// <summary>
        /// 用户身份令牌
        /// </summary>
        public string token { get; set; }
    }

    /// <summary>
    /// 红包开奖模型
    /// </summary>
    public class LotteryModel
    {
        /// <summary>
        /// 大
        /// </summary>
        public double da { get; set; }

        /// <summary>
        /// 小
        /// </summary>
        public double xiao { get; set; }

        /// <summary>
        /// 单
        /// </summary>
        public double dan { get; set; }

        /// <summary>
        /// 双
        /// </summary>
        public double shuang { get; set; }

        /// <summary>
        /// 红
        /// </summary>
        public double hong { get; set; }

        /// <summary>
        /// 合计出资
        /// </summary>
        public double total => da + xiao + dan + shuang + hong;
    }

    public class SetStateRequest
    {
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string duration { get; set; }
        public string rdcode { get; set; }
        public string token { get; set; }
    }


    public class Game_Record
    {
        //主键id
        public int id { get; set; }
        //用户id
        public int uid { get; set; }
        //金豆数量
        public decimal goldenBeans { get; set; }
        //小红包数量
        public decimal redPacket { get; set; }
        //免费红包数量
        public decimal freeRedPacket { get; set; }
        //操作时间
        public DateTime timeStamp { get; set; }
        //操作结果
        public string resultType { get; set; }
        //GUID
        public string playId { get; set; }
        //游戏结果
        public string resultMessage { get; set; }
        //金币数量
        public decimal goldenCoin { get; set; }
    }

    // STORY #6 lcl 20180418 Insert
    /// <summary>
    /// 被推荐用户通过省兜兜APP登录后，奖励推荐人小红包的API参数
    /// </summary>
    public class SpreaderShareRedpackageParameter
    {
        /// <summary>
        /// 包含用户信息和设备信息的加密字符串
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 极光推送注册号
        /// </summary>
        public string RegistrationId { get; set; }
    }

    // PROJECT #8 lcl 20180425 Insert
    /// <summary>
    /// 获取游戏数据的API参数
    /// </summary>
    public class GameDataRequest
    {
        /// <summary>
        /// 游戏类型(比如:1开头表示自营游戏；2开头表示第三方游戏)
        /// </summary>
        public string Type { get; set; }
    }
}