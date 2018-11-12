using System;
using RRL.WEB.Ulity;
using System.Web.Mvc;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using RRL.Core.Manager;
using System.Linq;
using System.Collections.Generic;
using RRL.WEB.Models.Response;
using Newtonsoft.Json;
using RRL.Config;

namespace RRL.WEB.Controllers
{
    public class GameController : Controller
    {
        private RdSession rdsession = new RdSession(System.Web.HttpContext.Current);

        const int CNT_SIGN_CYCLES = 7; // 签到周期

        public ActionResult GamePop()
        {
            return View();
        }

        // GET: Game
        public ActionResult Roulette_V2()
        {
            if (Request.QueryString["token"] != null)
            {
                rdsession["token"] = Request.QueryString["token"];
                //SessionHelper.Add("token", Request.QueryString["token"]);
                //Response.Redirect("/Game/Roulette");
            }
            if (Request.Headers["token"] != null)
            {
                rdsession["token"] = Request.Headers["token"];
                //SessionHelper.Add("token", Request.Headers["token"]);
            }

            var token = rdsession["token"];

            ViewBag.token = token;
            ViewBag.h_money_free = 0;
            ViewBag.daily_h_money_free = 0;
            ViewBag.has_received_free_money_default = true;
            ViewBag.has_received_daily_free_h_money = false;
            ViewBag.first_h_money_free = 0;
            //ViewBag.h_money_free_frz_expire = DateTime.Now;


            try
            {
                var tokenObj = TokenObject.InitTokenObjFromString(token);
                if (!string.Equals(TokenObject.ShortTimeToken, tokenObj.Prefix))
                {
                    rdsession["token"] = "";
                    //SessionHelper.Add("token", string.Empty);
                }
                else
                {
                    var user = new UserAuth();
                    var res = user.Load(tokenObj.id);
                    if (res != MessageCode.SUCCESS)
                    {
                        rdsession["token"] = "";
                        //SessionHelper.Add("token", string.Empty);
                    }
                    else
                    {
                        #region 付费红包
                        var db1 = new RRLDB();
                        UserManager userManager = new UserManager();
                        var tr = userManager.GetRndRedPackage();
                        int rnd_pay_redpacket = tr.Item1;
                        string endDateTime = tr.Item2;// 
                        db1.ExeCMD($@"update rrl_user set rnd_pay_redpacket={rnd_pay_redpacket},rnd_pay_redpacket_expire='{endDateTime}' where id={user.id} and (rnd_pay_redpacket_expire is null or  rnd_pay_redpacket_expire<=getdate())");
                        db1.Close();
                        user.Load(tokenObj.id);

                        ViewBag.payRedpacket = user.rnd_pay_redpacket;



                        #endregion
                        //ViewBag.h_money_free_frz = user.h_money_free_frz;
                        ViewBag.h_money_free = user.h_money_free;
                        //ViewBag.h_money_free_frz_expire  = user.h_money_free_frz_expire;
                        ViewBag.has_received_free_money_default = user.has_received_free_money_default;
                        ViewBag.has_received_daily_free_h_money = user.has_received_daily_free_h_money;

                        var db = new RRLDB();
                        var ds = db.ExeQuery("Select [Value] From rrl_config Where [Item] = 'daily_h_money_free'");
                        if (ds == null)
                        {
                            rdsession["token"] = "";
                            //SessionHelper.Add("token", string.Empty);
                        }
                        else
                        {
                            ViewBag.daily_h_money_free = Convert.ToDouble(ds.Tables[0].Rows[0]["Value"]);
                        }
                        ds = db.ExeQuery("Select [Value] From rrl_config Where [Item] = 'first_h_money_free'");
                        if (ds == null)
                        {
                        }
                        else
                        {
                            ViewBag.first_h_money_free = Convert.ToDouble(ds.Tables[0].Rows[0]["Value"]);
                        }

                        ds = db.ExeQuery("Select [Value] From rrl_config Where [Item] = 'freeRedPackage_to_beans_rate'");
                        if (ds == null)
                        {
                        }
                        else
                        {
                            ViewBag.freeRedPackage_to_beans_rate = Convert.ToDouble(ds.Tables[0].Rows[0]["Value"]);
                        }

                        var now = DateTime.Now;
                        var timestr = now.GetDateTimeFormats('T')[0];
                        ds = db.ExeQuery($@"SELECT top 1   ([count]+1)*[money]  FROM   spreader_queue order by money  desc");
                        int spreader_redpackage = 100000;
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            spreader_redpackage = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        }
                        ViewBag.spreader_redpackage = spreader_redpackage;
                        db.Close();
                    }
                }
            }
            catch
            {
                rdsession["token"] = "";
                //SessionHelper.Add("token", string.Empty);
            }

            return View();
        }

        public ActionResult PeasPay()
        {
            return View();
        }

        public ActionResult ThirdPartyGames()
        {
            return View();
        }

        public ActionResult SelfGames()
        {
            return View();
        }

        public ActionResult GameJump()
        {
            return View();
        }

        public ActionResult CustomerService()
        {
            return View();
        }

        public ActionResult GameHall(string token)
        {
            // 获取数据库访问对象
            SqlDataBase db = new SqlDataBase();
            // 默认设置否登录标记为“未登录”状态
            ViewBag.isLogin = false;
            // 默认设置连续签到日期数为0
            ViewBag.continuousSignDays = 0;
            // 初始化token对象
            TokenObject tokenObj = new TokenObject();
            ShopManager shopManager = new ShopManager();
            var isAtUSA = shopManager.isAtUSA("");
            ViewBag.is_show_game = isAtUSA ? "0" : "1";
            if (true)
            {
                tokenObj = TokenObject.InitTokenObjFromString(token);
                // todo 测试时，模拟一个用户ID
                //tokenObj.id = 44541;
                //if (tokenObj.id > 0)
                {
                    UserAuth User = new UserAuth();
                    int res = User.Load(tokenObj.id);
                    if (res == MessageCode.SUCCESS)
                    {
                        // 如果token中的用户ID有效，则认为已登录
                        ViewBag.isLogin = true;
                    }
                    else
                    {
                        ViewBag.isLogin = false;
                        //redpackageDayConfig
                        //return View();
                    }

                }
            }

            #region 红包
            // 获取红包配置信息
            var redpackageDayConfigList = GetRedpackageConfigList();
            ViewBag.redpackageDayConfig = redpackageDayConfigList;

            // 获取与红包兑换相关的配置数据
            int intMoney = 0;
            ConfigManager configMgr = new ConfigManager();
            // 免费红包兑金豆的起兑金额
            int.TryParse(configMgr.GetConfigValue("GameCenter_RedpackageToBean_Min"), out intMoney);
            ViewBag.redpackageToBeanMin = (int)(intMoney / 10000);
            // 分享得小红包
            ViewBag.shareRedpackage = configMgr.GetConfigValue("GameCenter_Share_Redpackage");
            // 分享得金币
            ViewBag.shareGoldCoins = configMgr.GetConfigValue("GameCenter_Share_GoldCoins");

            // 初始化时间差值
            ViewBag.countdown = 0;

            if (!ViewBag.isLogin)
            {
                // 未登录。也让用户能看到可以领取的状态
                ViewBag.isCanGetRedpackage = true;
            }
            else
            {
                // 已登录。
                bool existDataInValidCycles;
                bool isCanGetRedpackage;
                var userRedpackageList = GetUserRedpackageList(tokenObj.id, out existDataInValidCycles, out isCanGetRedpackage);
                if (existDataInValidCycles)
                {
                    // 有效周期内存在数据时，将签到配置数据集合中对应的签到标记设置为“已签到”
                    redpackageDayConfigList.Take(userRedpackageList.Count).ToList().ForEach(p => p.is_signin = "1");
                    // 设置连续签到日期总天数
                    ViewBag.continuousSignDays = userRedpackageList.Count;

                    #region 倒计时
                    DateTime dtEndDatetime = userRedpackageList.First().rec_date.AddDays(CNT_SIGN_CYCLES);
                    DateTime dtNow = DateTime.Now;

                    if (dtEndDatetime > dtNow)
                    {
                        TimeSpan ts = dtEndDatetime - DateTime.Now;
                        // 获取时间差所表示的总的秒数
                        ViewBag.countdown = Math.Floor(ts.TotalSeconds);
                    }
                    #endregion
                }
                // 设置当天是否可以领取红包
                ViewBag.isCanGetRedpackage = isCanGetRedpackage;
            }
            #endregion

            #region 游戏
            // 获取所有有效的游戏数据
            var gameList = db.Select<dynamic>($@"select * from rrl_game_list where is_enable='1'");
            // 获取自营免费红包区游戏数据
            ViewBag.redpackageGame = gameList.Where(p => p.type == "11").OrderBy(p => p.sortcode).ToList();
            // 获取自营金豆游戏区数据
            ViewBag.beansGame = gameList.Where(p => p.type == "12").OrderBy(p => p.sortcode).ToList();
            // 获取第三方游戏数据
            ViewBag.casualGame = gameList.Where(p => p.type == "2").OrderBy(p => p.sortcode).ToList();
            #endregion

            return View();
        }

        // 获取红包配置信息
        private List<dynamic> GetRedpackageConfigList()
        {
            return new SqlDataBase().Select<dynamic>($@"select title ,day_index ,Ceiling([money]/10000) as [money] ,min_money ,max_money ,extra_money,'0' as is_signin  from rrl_redpackage_day_config where is_enable='1' order by day_index");
        }

        /// <summary>
        /// 获取指定用户，最近一个周期签到领取的红包数据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="existDataInValidCycles">返回参数，有效周期(7天为一个周期)内是否存在红包领取数据. true:存在; false:不存在</param>
        /// <param name="isCanGetRedpackage">返回参数，当天是否可以领取红包. true:当天可以领取; false:当天不能领取</param>
        /// <returns></returns>
        private List<dynamic> GetUserRedpackageList(int userId, out bool existDataInValidCycles, out bool isCanGetRedpackage)
        {
            string strSQl = $@"select * from rrl_user_receive_day_redpackage
                                where [uid] = @uid
                                  and rec_date >= (select max(rec_date) from rrl_user_receive_day_redpackage where ([uid] = @uid) and (day_index = 1))
                                  and rec_date <= (select max(rec_date) from rrl_user_receive_day_redpackage where ([uid] = @uid))
                                order by rec_date asc";
            var userRedpackageList = new SqlDataBase().Select<dynamic>(strSQl, new { uid = userId });
            var now = DateTime.Now;
            var oneWeekAgo = now.AddDays(-(CNT_SIGN_CYCLES - 1)).Date;
            dynamic rec_firstDate;

            // 初始化为不存在有效周期内数据(没有用户的红包领取数据，或者数据在一个有效周期之外的，都认为有效周期内没有领取过红包)
            existDataInValidCycles = false;
            if (userRedpackageList != null && userRedpackageList.Count > 0)
            {
                rec_firstDate = userRedpackageList.First().rec_date;
                if (rec_firstDate >= oneWeekAgo)
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

        public ActionResult SignInRedpackage(string token)
        {
            return Json(new { status = 99, message = "签到功能已下架" });

            int intMessageCode = -1;
            // 获取数据库访问对象
            SqlDataBase db = new SqlDataBase();

            if (string.IsNullOrWhiteSpace(token))
            {
                // token字符串无效，不做处理
                intMessageCode = MessageCode.ERROR_TOKEN_LENGTH;
                return Json(new { status = intMessageCode, message = MessageCode.TranslateMessageCode(intMessageCode) });
            }

            // 初始化token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(token);
            // todo 测试时，模拟一个用户ID
            //tokenObj.id = 44541;
            if (tokenObj.id < 1)
            {
                // Token无效不做处理
                intMessageCode = MessageCode.ERROR_TOKEN_VALIDATE;
                return Json(new { status = intMessageCode, message = MessageCode.TranslateMessageCode(intMessageCode) });
            }

            // 限制当天零点至零点五分不能领取
            DateTime dtNow = DateTime.Now;
            if (dtNow >= dtNow.Date && dtNow <= dtNow.Date.AddMinutes(5))
            {
                // Token无效不做处理
                return Json(new { status = 99, message = "由于数据处理，零点至零点五分不能领取" });
            }

            bool existDataInValidCycles;
            bool isCanGetRedpackage;
            var userRedpackageList = GetUserRedpackageList(tokenObj.id, out existDataInValidCycles, out isCanGetRedpackage);
            if (isCanGetRedpackage)
            {
                // 执行签到领红包的存储过程
                List<dynamic> result = db.ExecuteStoredProcedure<dynamic>(@"sp_V3_ReceiveRedpackage",
                    new
                    {
                        uid = tokenObj.id,
                        isExistDataInValidCycles = existDataInValidCycles
                    });
            }

            // 返回执行成功的数据
            intMessageCode = MessageCode.SUCCESS;
            return Json(new { status = intMessageCode, message = MessageCode.TranslateMessageCode(intMessageCode) });
        }

        // lcl 20180506 Insert
        public ActionResult LeisureGameArea(string token)
        {
            #region 游戏
            // 获取所有有效的游戏数据
            var gameList = new GameManager().GetGameList();
            // 获取第三方游戏(即，休闲游戏)数据
            ViewBag.casualGame = gameList.Where(p => p.type == "2").OrderBy(p => p.sortcode).ToList();
            #endregion

            return View();
        }

        // lcl 20180608 Insert
        [HttpGet]
        public ActionResult GamePlayTimesByUser(string token, string callback, string game_type = "")
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                // token字符串无效
                return Content(callback + "(" + JsonConvert.SerializeObject(DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH)) + ")");
            }

            // 获取token对象
            TokenObject tokenObject = TokenObject.InitTokenObjFromString(token);
            UserAuth user = new UserAuth();
            int intResult = user.Load(tokenObject.id);
            if (intResult != MessageCode.SUCCESS)
            {
                // Token验证未成功，返回提示信息
                return Content(callback + "(" + JsonConvert.SerializeObject(DataResult.InitFromMessageCode(intResult)) + ")");
                //return DataResult.InitFromMessageCode(intResult);
            }

            // 获取游戏数据管理对象
            GameManager GameMgr = new GameManager();

            DataResult result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);

            if(string.IsNullOrWhiteSpace( game_type))
            {
                game_type="0010";
            }


            result.data = GameMgr.GetGamePlayTimesByType(user.id, game_type);

            return Content(callback + "(" + JsonConvert.SerializeObject(result) + ")");
        }

        // lcl 2018-06-22 Insert
        /// <summary>
        /// 获取疯狂转转赚的盘面配置数据列表
        /// </summary>
        /// <param name="moneyType">资金类型(1：免费红包；2：金豆)</param>
        /// <param name="weightType">权重类型(1：用户奖励的权重高；2：用户扣除的权重高)</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CrazyCircleConfigList(int moneyType, int weightType)
        {
            string strUrl = $@"{RRLConfig.CrazyCircleConfigList_Url}?moneyType={moneyType}&weightType={weightType}";
            string strResult = MyHttpHelper.HttpGet(strUrl);

            return Content(strResult);
        }

        // lcl 2018-06-22 Insert
        /// <summary>
        /// 更新权重
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="weight">权重值</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateWeightForCrazyCircle(string id, int weight)
        {
            string strUrl = $@"{RRLConfig.UpdateWeightForCrazyCircle_Url}?id={id}&weight={weight}";
            string strResult = MyHttpHelper.HttpGet(strUrl);

            return Content(strResult);
        }

        // lcl 2018-06-22 Insert
        /// <summary>
        /// 获取疯狂转转赚的调控配置数据列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CrazyCircleWaveConfigList()
        {
            string strResult = MyHttpHelper.HttpGet(RRLConfig.CrazyCircleWaveConfigList_Url);

            return Content(strResult);
        }

        // lcl 2018-06-22 Insert
        /// <summary>
        /// 疯狂转转赚的调控配置数据更新
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateWaveConfigForCrazyCircle(string id, int winTotalMax, int baseWaveTopKneePoint, decimal waveTopPercent, decimal waveTroughPercent)
        {
            string strUrl = $@"{RRLConfig.UpdateWaveConfigForCrazyCircle_Url}?id={id}&winTotalMax={winTotalMax}&baseWaveTopKneePoint={baseWaveTopKneePoint}&waveTopPercent={waveTopPercent}&waveTroughPercent={waveTroughPercent}";
            string strResult = MyHttpHelper.HttpGet(strUrl);

            return Content(strResult);
        }

        /// <summary>
        /// 强买豆逻辑
        /// </summary>
        /// <param name="token"></param>
        /// <param name="goldbean"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BuyBeansForCrazy(string token,string forcePay)
        {
            SqlDataBase db = new SqlDataBase();
            TokenObject tokenObject = TokenObject.InitTokenObjFromString(token);
            //DataResult result
            //容错处理一下
            
            string sql_for_forcepay = @"select * from rrl_game_forcepay where id=@id";
            var one= db.Single(sql_for_forcepay, new { id = forcePay });
            if (one == null)
            {
                return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new DataResult() { status = 99, message = "找不到凭据" } };
            }
            decimal payBeans = one.payBeans;
            decimal extraBeans = one.extraBeans;
            string sql = "INSERT INTO [rrl_coupons] ([goodsid], [moeny], [countmoney], [goldbean], [starttime], [endtime], [uid],[goldcoin],[redpacket],[leastconsume],[goodsname],[type]) output inserted.id " +
                "VALUES(0, @moeny, @countmoney, @goldbean, @starttime, @endtime, @uid,0,0,@leastconsume,@goodsname,'1')";
            decimal moeny=(decimal)payBeans / 100;
            decimal leastconsume = moeny * 20;
            string goodsname = $"所有商品，购物满{leastconsume}元能抵扣,游戏中购买";
            int list=db.ExecuteScalar<int>(sql, new { moeny= moeny, countmoney= moeny, goldbean= payBeans+ extraBeans, starttime =DateTime.Now, endtime=DateTime.Now.AddMonths(6), uid= tokenObject.id, leastconsume= leastconsume, goodsname= goodsname });
            string redirectUrl = $@"/CouponsPay?list={list}&token={token}&type=1,2,3&wallet=0&jd=0&cash={moeny}& redirect=https://e-shop.rrlsz.com.cn/";
            return Redirect(redirectUrl);
            //
        }

        // lcl 2018-07-26 Insert
        /// <summary>
        /// 充值送免费红包
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RechargeForGiveRedpackage(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                // token字符串无效，不做处理
                return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH) };
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(token);
            UserAuth user = new UserAuth();
            int res = user.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = DataResult.InitFromMessageCode(res) };
            }

            SqlDataBase db = new SqlDataBase();
            ConfigManager configMgr = new ConfigManager();

            string strSql = $@"select count(0) from game_auto_redpackage_daily
                                where (uid = @uid) and (receive_date = convert(date,getdate())) and (is_receive = 0)";
            var dataCount = db.ExecuteScalar<int>(strSql, new { uid = user.id });
            if (dataCount != 1)
            {
                // 正常情况下，有且仅有1条记录，否则不能调用支付接口
                return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new DataResult() { status = 99, message = "找不到凭据" } };
            }

            // rrl_coupons表type=2，表示充值送免费红包
            strSql = $@"insert into [rrl_coupons]
                               ([goodsid] ,[moeny] ,[countmoney] ,[goldbean] ,[starttime] 
                               ,[endtime] ,[uid] ,[goldcoin] ,[redpacket] ,[leastconsume] 
                               ,[goodsname] ,[type]) output inserted.id
                        values (0 ,@moeny ,@countmoney ,@goldbean ,@starttime 
                               ,@endtime ,@uid ,0 ,0 ,@leastconsume 
                               ,@goodsname ,'2')";
            // 充值送免费红包
            decimal mMoney = 0.9m;
            decimal.TryParse(configMgr.GetConfigValue("Game_Auto_Redpackage_Pay_Money"), out mMoney);
            decimal leastconsume = mMoney * 20;
            decimal mGoldbean = mMoney * 0m; // lcl 2018-08-20 Modify 取消送金豆
            string goodsname = $"所有商品，购物满{leastconsume}元能抵扣,游戏中购买";
            int list = db.ExecuteScalar<int>(strSql, new
            {
                moeny = mMoney,
                countmoney = mMoney,
                goldbean = mGoldbean,
                starttime = DateTime.Now,
                endtime = DateTime.Now.AddMonths(6),
                uid = user.id,
                leastconsume = leastconsume,
                goodsname = goodsname
            });

            string redirectUrl = $@"/CouponsPay?list={list}&token={token}&type=1,2,3&wallet=0&jd=0&cash={mMoney}&redirect=https://e-shop.rrlsz.com.cn/#/gamehall";
            return Redirect(redirectUrl);
        }

        // lcl 2018-08-22 Insert
        /// <summary>
        /// 领取分时段充值红包
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReceiveTimeIntervalRedpaket(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                // token字符串无效，不做处理
                return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH) };
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(token);
            UserAuth user = new UserAuth();
            int res = user.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = DataResult.InitFromMessageCode(res) };
            }

            SqlDataBase sqlDB = new SqlDataBase();
            bool blnIsCanPay = false; // 是否能执行支付操作（true：允许支付；false：不能支付）
            string strSql = string.Empty;

            var receiveData = new GameManager().GetTimeIntervalRedpaketData(user);
            if (receiveData.isHasRedpacket != 1)
            {
                // 不能领取
                return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new DataResult() { status = 99, message = "不满足领取条件" } };
            }

            if (receiveData.isExistUnpaid == 1)
            {
                // 如果已经存在未支付的红包领取记录，则允许支付
                blnIsCanPay = true;
            }
            else
            {
                // 不存在领取记录的，新增领取记录，支付状态为“未支付”
                strSql = $@"insert into time_interval_redpaket_receive
                                   (receive_id ,is_paid)
                            values (@receive_id ,0)";
                res = sqlDB.Execute(strSql, new { receive_id = receiveData.receiveId });
                if (res > 0)
                {
                    // 数据写入成功
                    blnIsCanPay = true;
                }
            }

            if (!blnIsCanPay)
            {
                return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new DataResult() { status = 99, message = "不能重复领取" } };
            }
            string type = "4";
            if (receiveData.type == 1) type = "5";
             // rrl_coupons表type=4，表示分时段充值送红包
             strSql = $@"insert into [rrl_coupons]
                               ([goodsid] ,[moeny] ,[countmoney] ,[goldbean] ,[starttime] 
                               ,[endtime] ,[uid] ,[goldcoin] ,[redpacket] ,[leastconsume] 
                               ,[goodsname] ,[type] ,[relation_trans_id]) output inserted.id
                        values (0 ,@moeny ,@countmoney ,@goldbean ,@starttime 
                               ,@endtime ,@uid ,0 ,@redpacket ,@leastconsume 
                               ,@goodsname ,@type ,@relation_trans_id)";

            decimal mMoney = receiveData.payMoney;
            decimal leastconsume = mMoney * 20;
            decimal mGoldbean = 0m;
            string goodsname = $"所有商品，购物满{leastconsume}元能抵扣,游戏中购买";
            int list = sqlDB.ExecuteScalar<int>(strSql, new
            {
                moeny = mMoney,
                countmoney = mMoney,
                goldbean = mGoldbean,
                starttime = DateTime.Now,
                endtime = DateTime.Now.AddMonths(6),
                uid = user.id,
                redpacket = receiveData.redpacket,
                leastconsume = leastconsume,
                goodsname = goodsname,
                relation_trans_id = receiveData.receiveId,
                type= type
            });

            string redirectUrl = $@"/CouponsPay?list={list}&token={token}&type=1,2,3&wallet=0&jd=0&cash={mMoney}&redirect=https://e-shop.rrlsz.com.cn/#/gamehall";
            return Redirect(redirectUrl);
        }
    }
}