using RRL.Config;
using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Models.WxJSAPI;
using RRL.Core.Utility;
using RRL.WEB.Models.Wx;
using RRL.WEB.Ulity;
using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RRL.DB;
using System.Threading;
using RRL.WEB.Models.Response;

namespace RRL.WEB.Controllers
{
    /// <summary>
    /// 基础视图控制器
    /// </summary>
    public class ViewRouteController : Controller
    {
        private string APPID = Core.Pay.WxPay.WxPayConfig.WxMPAPPID;
        private string APPSECRET = Core.Pay.WxPay.WxPayConfig.WxMPAPPSECRET;
        private RdSession rdsession = new RdSession(System.Web.HttpContext.Current);
        ConfigManager configMgr = new ConfigManager();
        #region 重新获取Code的跳转链接(没有用户授权的，只能获取基本信息）

        /// <summary>
        /// 重新获取Code,以后面实现带着Code重新跳回目标页面(没有用户授权的，只能获取基本信息（openid））
        /// </summary>
        /// <param name="url">目标页面</param>
        /// <returns></returns>
        private string GetCodeUrl(string url, string state = "1#wechat_redirect")
        {
            string CodeUrl = "";
            //对url进行编码
            url = HttpUtility.UrlEncode(url);
            CodeUrl = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + APPID + "&redirect_uri=" + url + $@"&response_type=code&scope=snsapi_base&state={state}");
            return CodeUrl;
        }

        #endregion 重新获取Code的跳转链接(没有用户授权的，只能获取基本信息）

        #region 以Code换取用户的openid、access_token

        /// <summary>
        /// 根据Code获取用户的openid、access_token
        /// </summary>
        private string GetOauthAccessOpenId(string code)
        {
            //Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");
            string Openid = "";
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + APPID + "&secret=" + APPSECRET + "&code=" + code + "&grant_type=authorization_code";
            string gethtml = MyHttpHelper.HttpGet(url);
            //log.log("拿到的url是：" + url);
            //log.log("获取到的gethtml是" + gethtml);
            WxOauthAccessToken ac = new WxOauthAccessToken();
            ac = Newtonsoft.Json.JsonConvert.DeserializeObject<WxOauthAccessToken>(gethtml);
            //log.log("能否从html里拿到openid=" + ac.openid);
            Openid = ac.openid;
            return Openid;
        }

        #endregion 以Code换取用户的openid、access_token

        #region 获取客户端ip

        /// <summary>
        /// 获取客户端ip
        /// </summary>
        /// <returns>ip</returns>
        private string GetHostAddress()
        {
            string userHostAddress = HttpContext.Request.UserHostAddress;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 校验ip合法性
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>校验结果</returns>
        private bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        #endregion 获取客户端ip

        // GET: ViewRoute
        /// <summary>
        /// 根节点视图(/)
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Index()
        {
            if (Request.QueryString["token"] != null)
            {
                rdsession["token"] = Request.QueryString["token"];
            }
            else
            {
                if (Request.Headers["token"] != null)
                {
                    rdsession["token"] = Request.Headers["token"];
                }
                else
                {
                    rdsession["token"] = "";
                }
                if (!rdsession.Exist("token"))
                {
                    rdsession["token"] = "";
                    //SessionHelper.Add("token", "");
                }
            }
            var token = rdsession["token"];
            ViewBag.token = rdsession["token"];
            ViewBag.Title = RRLConfig.PlateName;

            var openid = "";
            if (rdsession.Exist("openid"))
            {
                openid = rdsession["openid"];
            }
            try
            {
                ViewBag.OpenId = /*System.Web.HttpContext.Current.Session["openid"]*/openid;
                ShopManager shopManager = new ShopManager();
                var isAtUSA = shopManager.isAtUSA("");
                ViewBag.is_show_game = isAtUSA ? "0" : "1";
                ViewBag.longtoken = "";
                ViewBag.h_money_pay = 0;
                ViewBag.m_play_total_times = configMgr.GetConfigValue("GameCenter_can_change_goldbeans_times");
                ViewBag.platform_hold_money = configMgr.GetConfigValue("platform_hold_h_money");
                // lcl 2018-08-03 Insert 用于在Vue中使用的配置数据的键值对形式(json格式)
                ViewBag.key_value_config = configMgr.GetConfigValue("js_key_value_config");
                ViewBag.Order_Shared_discount_money_rate = configMgr.GetConfigValue<decimal>("Order_Shared_discount_money_rate", 0.29m);
                ViewBag.Order_Shared_discount_beans_rate = configMgr.GetConfigValue<decimal>("Order_Shared_discount_beans_rate", 0.71m);
                ViewBag.Order_Shared_people_num = configMgr.GetConfigValue<int>("Order_Shared_people_num", 2);
                #region 发红包逻辑
                ViewBag.h_money_free = 0;
                ViewBag.daily_h_money_free = 0;
                ViewBag.has_received_free_money_default = true;
                ViewBag.has_received_daily_free_h_money = false;
                ViewBag.first_h_money_free = 0;
            }
            catch (Exception ex)
            {
                ConfigManager.WriteTextLog("140", ex.Message + "\r\n" + ex.StackTrace, DateTime.Now);
            }
            TokenObject tokenObj = null;
            UserAuth user = null;
            int res = -1;
            try
            {
                tokenObj = TokenObject.InitTokenObjFromString(token);
                user = new UserAuth();
                res = user.Load(tokenObj.id);
            }
            catch (Exception ex)
            {
                ConfigManager.WriteTextLog("168", ex.Message + "\r\n" + ex.StackTrace, DateTime.Now);

            }

            if (res == MessageCode.SUCCESS)
            {
                RRLDB db = null;
                try
                {
                    ViewBag.h_money_pay = user.h_money_pay;
                    ViewBag.longtoken = user.long_time_token;
                    #region 付费红包

                    #endregion
                    ViewBag.h_money_free = user.h_money_free;
                    ViewBag.has_received_free_money_default = user.has_received_free_money_default;
                    ViewBag.has_received_daily_free_h_money = user.has_received_daily_free_h_money;
                    // lcl 2018-6-28 Insert
                    if (new UserManager().IfNewUser(user))
                    {
                        // 如果是新用户，给允许红包兑换金豆的游戏次数重新赋值
                        ViewBag.m_play_total_times = configMgr.GetConfigValue("GameCenter_can_change_goldbeans_times_NewUser");
                        ViewBag.is_new_user = 1;
                    }
                    else
                    {
                        ViewBag.is_new_user = 0;
                    }

                    db = new RRLDB();
                    db.ExeCMD($@"update rrl_user set last_access_time=getdate() where id={user.id}");
                    var ds = db.ExeQuery(@"Select [Value] From rrl_config Where [Item] = 'daily_h_money_free'
                                        union  ALL
                                       Select [Value] From rrl_config Where [Item] = 'first_h_money_free'
                                        union all
                                    Select [Value] From rrl_config Where [Item] = 'freeRedPackage_to_beans_rate'");
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 3)
                    {
                        ViewBag.daily_h_money_free = Convert.ToDouble(ds.Tables[0].Rows[0]["Value"]);
                        ViewBag.first_h_money_free = Convert.ToDouble(ds.Tables[0].Rows[1]["Value"]);
                        ViewBag.freeRedPackage_to_beans_rate = Convert.ToDouble(ds.Tables[0].Rows[2]["Value"]);
                    }
                    ViewBag.spreader_redpackage = 0;
                }
                catch (Exception ex)
                {
                    ViewBag.daily_h_money_free =0;
                    ViewBag.first_h_money_free = 0;
                    ViewBag.freeRedPackage_to_beans_rate =0;
                    ViewBag.spreader_redpackage = 0;
                    ConfigManager.WriteTextLog("218", ex.Message + "\r\n" + ex.StackTrace, DateTime.Now);

                }
                finally
                {
                    db.Close();
                }



            }

            #endregion

            HelpManager.MarkPageView("/");
            return View();
        }

        public ActionResult Login()
        {
            //if(/*System.Web.HttpContext.Current.Session["openid"] == null*/!rdsession.Exist("token"))
            //{
            //    //SessionHelper.Add("openid", "");
            //    //SessionHelper.Add("code", "");
            //    rdsession["token"] = "";
            //    rdsession["code"] = "";
            //}
            //string url = System.Web.HttpContext.Current.Request.Url.ToString();//获取当前url
            //if (/*System.Web.HttpContext.Current.Session["openid"].ToString() == "" || System.Web.HttpContext.Current.Session["openid"] == null*/!rdsession.Exist("openid") || string.IsNullOrWhiteSpace(rdsession["openid"].ToString()))
            //{
            //    //先要判断是否是获取code后跳转过来的
            //    if (/*System.Web.HttpContext.Current.Request.QueryString["code"] == "" || System.Web.HttpContext.Current.Request.QueryString["code"] == null*/!rdsession.Exist("code")||string.IsNullOrWhiteSpace(rdsession["code"].ToString()))
            //    {
            //        //Code为空时，先获取Code
            //        string GetCodeUrls = GetCodeUrl(url);
            //        System.Web.HttpContext.Current.Response.Redirect(GetCodeUrls);//先跳转到微信的服务器，取得code后会跳回来这页面的
            //    }
            //    else
            //    {
            //        //Code非空，已经获取了code后跳回来啦，现在重新获取openid
            //        //Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");
            //        string openid = "";
            //        openid = GetOauthAccessOpenId(System.Web.HttpContext.Current.Request.QueryString["Code"]);//重新取得用户的openid
            //        rdsession["openid"] = openid;
            //        //System.Web.HttpContext.Current.Session["openid"] = openid;
            //    }
            //}
            var hash = Request.QueryString["hash"] ?? "/";
            return new RedirectResult("/#" + hash);
        }

        /// <summary>
        /// 微信JS支付节点
        /// </summary>
        /// <returns></returns>
        public ActionResult WxJsPay()
        {
            ViewBag.Title = RRLConfig.WxJSPayViewTitle;
            if (/*System.Web.HttpContext.Current.Session["openid"] == null*/!rdsession.Exist("token"))
            {
                //SessionHelper.Add("openid", "");
                //SessionHelper.Add("code", "");
                rdsession["token"] = "";
                rdsession["code"] = "";
            }
            var uri = System.Web.HttpContext.Current.Request.Url;//获取当前url

            var url = $@"{uri.Scheme}://{uri.Host}{uri.PathAndQuery}";

            //HelpManager.Mark(url);
            //HelpManager.Mark(JsonConvert.SerializeObject(ob));

            if (/*System.Web.HttpContext.Current.Session["openid"].ToString() == "" || System.Web.HttpContext.Current.Session["openid"] == null*/!rdsession.Exist("openid") || string.IsNullOrWhiteSpace(rdsession["openid"]))
            {
                //先要判断是否是获取code后跳转过来的
                if (System.Web.HttpContext.Current.Request.QueryString["code"] == "" || System.Web.HttpContext.Current.Request.QueryString["code"] == null)
                {
                    //Code为空时，先获取Code
                    var getCodeUrls = GetCodeUrl(url);
                    System.Web.HttpContext.Current.Response.Redirect(getCodeUrls);//先跳转到微信的服务器，取得code后会跳回来这页面的
                }
                else
                {
                    //Code非空，已经获取了code后跳回来啦，现在重新获取openid
                    //Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");
                    string openid;
                    openid = GetOauthAccessOpenId(System.Web.HttpContext.Current.Request.QueryString["Code"]);//重新取得用户的openid
                    //System.Web.HttpContext.Current.Session["openid"] = openid;
                    rdsession["openid"] = openid;
                }
            }

            if (string.IsNullOrEmpty(Request.QueryString["orderlist"]) || string.IsNullOrEmpty(Request.QueryString["0"]) || string.IsNullOrEmpty(Request.QueryString["token"]))
            {
                string orderlist = Request.QueryString["orderlist"];
                double discount = Convert.ToDouble(Request.QueryString["discount"]);
                string token = Request.QueryString["token"];
                string sperador = Request.QueryString["sperador"];
                //string is_beans_pay = Request.QueryString["is_beans_pay"];
                if (sperador == null)
                {
                    sperador = "";
                }
                string IP = GetHostAddress();

                if (IP == null)
                {
                    IP = "0.0.0.0";
                }
                int res;
                string appid = "";
                string timeStamp = "";
                string nonceStr = "";
                string package = "";
                string signType = "";
                string paySign = "";
                string message = "";
                TokenObject Token = TokenObject.InitTokenObjFromString(token);
                if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
                {
                    res = MessageCode.ERROR_TOKEN_VALIDATE;
                }
                else
                {
                    UserAuth User = new UserAuth();
                    res = User.Load(Token.id);
                    //string open_id = User.open_id;
                    if (res == MessageCode.SUCCESS)
                    {
                        TradeManager tm = new TradeManager();
                        string openid = /*SessionHelper.Get("openid")*/rdsession["openid"].ToString();
                        JsPayConfigObject ConfigObject = tm.ApplyWxJsPayV3(User, orderlist, IP, openid, out res, out message, sperador);
                        // tm.ApplyWxJsPay(User.id, orderlist, discount, User.plate_to_return_money + User.ex_plate_to_return_money, IP, openid, out res, sperador);
                        if (res != 0)
                        {
                            ViewBag.status = res;
                            ViewBag.message = message;
                            return View();
                        }
                        appid = ConfigObject.appId;
                        timeStamp = ConfigObject.timeStamp.ToString();
                        nonceStr = ConfigObject.nonceStr;
                        package = ConfigObject.package;
                        signType = ConfigObject.signType;
                        paySign = ConfigObject.paySign;
                    }
                }
                ViewBag.open_id = rdsession["openid"].ToString();
                ViewBag.status = res;
                ViewBag.appId = appid;
                ViewBag.timeStamp = timeStamp;
                ViewBag.nonceStr = nonceStr;
                ViewBag.package = package;
                ViewBag.signType = signType;
                ViewBag.paySign = paySign;

                return View();
            }

            return Redirect("/");
        }

        /// <summary>
        /// 卡券支付节点
        /// </summary>
        /// <returns></returns>
        public ActionResult CouponsPay()
        {
            ViewBag.Title = PlatFormConfig.WxJSPayViewTitle;
            if (/*System.Web.HttpContext.Current.Session["openid"] == null*/!rdsession.Exist("token"))
            {
                //SessionHelper.Add("openid", "");
                //SessionHelper.Add("code", "");
                rdsession["token"] = "";
                rdsession["code"] = "";
            }
            var uri = System.Web.HttpContext.Current.Request.Url;//获取当前url
            var url = $@"{uri.Scheme}://{uri.Host}{uri.PathAndQuery}";
            if (/*System.Web.HttpContext.Current.Session["openid"].ToString() == "" || System.Web.HttpContext.Current.Session["openid"] == null*/!rdsession.Exist("openid") || string.IsNullOrWhiteSpace(rdsession["openid"].ToString()))
            {
                //先要判断是否是获取code后跳转过来的
                if (System.Web.HttpContext.Current.Request.QueryString["code"] == "" || System.Web.HttpContext.Current.Request.QueryString["code"] == null)
                {
                    //Code为空时，先获取Code
                    string getCodeUrls = GetCodeUrl(url);
                    System.Web.HttpContext.Current.Response.Redirect(getCodeUrls);//先跳转到微信的服务器，取得code后会跳回来这页面的
                }
                else
                {
                    //Code非空，已经获取了code后跳回来啦，现在重新获取openid
                    //Log log = new Log(AppDomain.CurrentDomain.BaseDirectory + @"/log/Log.txt");
                    string openid = "";
                    openid = GetOauthAccessOpenId(System.Web.HttpContext.Current.Request.QueryString["Code"]);//重新取得用户的openid
                    //System.Web.HttpContext.Current.Session["openid"] = openid;
                    rdsession["openid"] = openid;
                }
            }

            if (!string.IsNullOrEmpty(Request.QueryString["list"]) && !string.IsNullOrEmpty(Request.QueryString["token"]))
            {
                string list = Request.QueryString["list"];
                string token = Request.QueryString["token"];
                string ip = GetHostAddress() ?? "0.0.0.0";

                int res;
                string appid = "";
                string timeStamp = "";
                string nonceStr = "";
                string package = "";
                string signType = "";
                string paySign = "";
                double money = 0;
                TokenObject Token = TokenObject.InitTokenObjFromString(token);
                if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
                {
                    res = MessageCode.ERROR_TOKEN_VALIDATE;
                }
                else
                {
                    UserAuth User = new UserAuth();
                    res = User.Load(Token.id);
                    //string open_id = User.open_id;
                    if (res == MessageCode.SUCCESS)
                    {
                        TradeManager tm = new TradeManager();
                        string openid = /*SessionHelper.Get("openid")*/rdsession["openid"].ToString();
                        //todo
                        JsPayConfigObject configObject = tm.ApplyWxJsCouponPay(list, ip, openid, out res, out money);
                        appid = configObject.appId;
                        timeStamp = configObject.timeStamp.ToString();
                        nonceStr = configObject.nonceStr;
                        package = configObject.package;
                        signType = configObject.signType;
                        paySign = configObject.paySign;

                        // lcl 20180716 Insert
                        try
                        {
                            // 调用做任务领红包的存储过程
                            new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_TaskRedpackageDaily", new { uid = User.id, task_type = "recharge" });
                        }
                        catch { }
                    }
                }
                ViewBag.token = token;
                ViewBag.money = money;
                ViewBag.open_id = /*SessionHelper.Get("openid")*/rdsession["openid"].ToString();
                ViewBag.status = res;
                ViewBag.appId = appid;
                ViewBag.timeStamp = timeStamp;
                ViewBag.nonceStr = nonceStr;
                ViewBag.package = package;
                ViewBag.signType = signType;
                ViewBag.paySign = paySign;

                return View();
            }

            return Redirect($@"/Game/Roulette?token={Request.QueryString["token"]}");
        }

        /// <summary>
        /// 产品节点视图(/Product)
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Product()
        {
            ViewBag.Title = PlatFormConfig.PlateName;
            HelpManager.MarkPageView("/Product");
            return View();
        }

        /// <summary>
        /// 产品节点视图(/Product)
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult ProductForGame()
        {
            ViewBag.Title = PlatFormConfig.PlateName;
            /*HelpManager.MarkPageView("/ProductForGame");*/
            return View();
        }



        ///// <summary>
        ///// 产品节点视图(/Product)
        ///// </summary>
        ///// <returns>视图</returns>
        //public ActionResult Mobile()
        //{
        //    ViewBag.Title = PlatFormConfig.PlateName;
        //    HelpManager.MarkPageView("/Mobile");
        //    return View();
        //}

        //public ActionResult Test()
        //{
        //    var requestUrl = $@"https://open.weixin.qq.com/connect/oauth2/authorize?appid={APPID}&redirect_uri=" + $@"{HttpUtility.UrlEncode("https://e-shop.rrlsz.com.cn/Test/")}&response_type=code&scope={"snsapi_userinfo"}&state={"state"}#wechat_redirect";

        //    if (System.Web.HttpContext.Current.Request.QueryString["code"] == "" ||
        //        System.Web.HttpContext.Current.Request.QueryString["code"] == null)
        //    {

        //    }
        //    else
        //    {
        //        string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + APPID + "&secret=" + APPSECRET + "&code=" + System.Web.HttpContext.Current.Request.QueryString["code"] + "&grant_type=authorization_code";
        //        string gethtml = MyHttpHelper.HttpGet(url);
        //        ViewBag.result = gethtml;
        //        return View();
        //    }

        //    return Redirect(requestUrl);
        //}
        public ActionResult payway()
        {


            return View();
        }

        /// <summary>
        /// 接收微信消息
        /// </summary>
        /// <returns></returns>
        public ActionResult WxMessageReceiver()
        {
            //扫二维码事件 <xml> <ToUserName>< ![CDATA[toUser] ]></ToUserName> <FromUserName>< ![CDATA[FromUser] ]></FromUserName> <CreateTime>123456789</CreateTime> <MsgType>< ![CDATA[event] ]></MsgType> <Event>< ![CDATA[SCAN] ]></Event> <EventKey>< ![CDATA[SCENE_VALUE] ]></EventKey> <Ticket>< ![CDATA[TICKET] ]></Ticket> </xml>
            if (Request.InputStream.Length > 0)
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(Request.InputStream, System.Text.Encoding.UTF8);
                string s = reader.ReadToEnd();//xml字符串
                reader.Close();


                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(s);
                System.Xml.XmlNodeList xnl = doc.GetElementsByTagName("Event");
                var openidNode = doc.GetElementsByTagName("FromUserName");
                string openid = "";
                if (openidNode != null && openidNode.Count > 0)
                {
                    openid = openidNode[0].InnerText;
                }

                DateTime createTime = DateTime.Now;
                if (xnl.Count > 0 && xnl[0].InnerText == "SCAN")//扫码事件
                {
                    xnl = doc.GetElementsByTagName("EventKey");
                    if (xnl.Count > 0)
                    {
                        string scene_id = xnl[0].InnerText;///
                        //....其他操作
                    }
                }
            }
            return Content("ok");
        }

        /// <summary>
        /// 获取和保存用户在微信中的OpenId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetOpenIdForWx()
        {
            if (!rdsession.Exist("openid") || string.IsNullOrWhiteSpace(rdsession["openid"]))
            {
                // 生成作为发送到微信接口的state参数值，用于回调时确定是本次发出的请求，以便在轮询时查询对应的Redis值
                string guid_state = Guid.NewGuid().ToString("N");

                // 获取当前url
                var uri = System.Web.HttpContext.Current.Request.Url;
                // 构造调用微信code服务的url参数，该地址参数作为微信端执行成功后的回调
                var url = $@"{uri.Scheme}://{uri.Authority}/SetOpenIdForWxRedirect";
                var getCodeUrls = GetCodeUrl(url, guid_state);
                // 跳转到微信的服务器
                MyHttpHelper.HttpGet(getCodeUrls);

                int times = 0;
                int maxTimes = 50;
                int sleep_minisecond = 100;

                while (times < maxTimes)
                {
                    string openId = string.Empty;
                    if (rdsession.Redis.KeyExists(guid_state))
                    {
                        openId = rdsession.Redis.StringGet(guid_state);
                    }
                    if (!string.IsNullOrWhiteSpace(openId))
                    {
                        rdsession["openid"] = openId;
                        return Json(new { status = 0, message = "OK", data = openId }, JsonRequestBehavior.AllowGet);
                    }
                    Thread.Sleep(sleep_minisecond);
                    times++;
                }

                // 轮询指定次数后仍然未能获取到openid
                return Json(new { status = 999, message = "未获取到用户身份信息" }, JsonRequestBehavior.AllowGet);
            }

            // Redis中存在OpenId，直接返回
            return Json(new { status = 0, message = "OK", data = rdsession["openid"] }, JsonRequestBehavior.AllowGet);
        }

        public void SetOpenIdForWxRedirect()
        {
            string strState = System.Web.HttpContext.Current.Request.QueryString["state"];
            string strCode = System.Web.HttpContext.Current.Request.QueryString["code"];

            string openid = GetOauthAccessOpenId(strCode);

            rdsession.Redis.StringSet(strState, openid, new TimeSpan(0, 20, 0));
        }

        // lcl 2018-10-20 Insert
        /// <summary>
        /// 卡券商品支付
        /// </summary>
        /// <param name="token">用户身份令牌</param>
        /// <param name="card_id">卡券商品在卡券商城中的唯一标识。使用该ID来查询3.0中对应的商品信息</param>
        /// <param name="goods_count">购买的数量</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CardGoodsPay(string token, string card_id, int goods_count)
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
            string strSql = string.Empty;

            // 根据卡券商城中的商品ID，查找3.0中关联的商品信息
            strSql = "select id ,price from rrl_goods(nolock) where card_goods_id = @card_goods_id";
            var goodsInfo = sqlDB.Single<dynamic>(strSql, new { card_goods_id = card_id });
            if (goodsInfo == null)
            {
                // 在3.0中没有找到关联的商品信息
                return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new DataResult() { status = 99, message = "没有找到关联的卡券商品信息!" } };
            }

            TradeManager tradeMgr = new TradeManager();
            // 创建卡券订单
            BussResult bussResult = tradeMgr.CreateOrderFromGoodsV3(user.id, goodsInfo.id, goods_count, "", "", "", "", out int? intOrderId);
            if (intOrderId == null || intOrderId.Value < 1 || bussResult.status != 0)
            {
                // 订单创建失败
                return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new DataResult() { status = bussResult.status, message = bussResult.message } };
            }
            // 为了遵守下单验证逻辑，订单创建成功后，更新收获地址
            sqlDB.Execute("update rrl_order set area_code = 0 ,receive_address = '卡券商品' where (id = @id)", new { id = intOrderId });

            // 跳转到支付接口页面
            string redirectUrl = $@"/payway?list={intOrderId}&token={token}&type=1,2,3&cash={goodsInfo.price}&wallet={user.x_money}&jd={user.h_money}";

            return Redirect(redirectUrl);
        }
    }
}