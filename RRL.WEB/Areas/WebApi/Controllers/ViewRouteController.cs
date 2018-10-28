/*
 *  lcl 20180831 Insert 前后端代码分离
 */

using System;
using System.Dynamic;
using System.Web.Http;

using RRL.Core.Manager;
using RRL.Core.Utility;
using RRL.WEB.Models.Request;
using RRL.WEB.Models.Response;


namespace RRL.WEB.Areas.WebApi.Controllers
{
    public partial class ViewRouteController : ApiController
    {
        // 商城平台首页初始化
        [HttpGet]
        public DataResult PageInitialize([FromUri]BaseTokenRequest request)
        {
            dynamic jsVariables = new ExpandoObject();
            ConfigManager configMgr = new ConfigManager();

            //if (Request.QueryString["token"] != null)
            //{
            //    rdsession["token"] = Request.QueryString["token"];
            //}
            //else
            //{
            //    if (Request.Headers["token"] != null)
            //    {
            //        rdsession["token"] = Request.Headers["token"];
            //    }
            //    if (/*System.Web.HttpContext.Current.Session["token"] == null*/!rdsession.Exist("token"))
            //    {
            //        rdsession["token"] = "";
            //        //SessionHelper.Add("token", "");
            //    }
            //}
            //var token = rdsession["token"];
            //ViewBag.token = rdsession["token"];
            //ViewBag.Title = RRLConfig.PlateName;

            //var openid = "";
            //if (rdsession.Exist("openid"))
            //{
            //    openid = rdsession["openid"];
            //}
            //ViewBag.OpenId = /*System.Web.HttpContext.Current.Session["openid"]*/openid;
            //ShopManager shopManager = new ShopManager();
            //var isAtUSA = shopManager.isAtUSA("");
            //ViewBag.is_show_game = isAtUSA ? "0" : "1";
            //ViewBag.longtoken = "";
            //ViewBag.h_money_pay = 0;
            //ViewBag.m_play_total_times = configMgr.GetConfigValue("GameCenter_can_change_goldbeans_times");
            //ViewBag.platform_hold_money = configMgr.GetConfigValue("platform_hold_h_money");
            //// 用于在Vue中使用的配置数据的键值对形式(json格式)
            //ViewBag.key_value_config = configMgr.GetConfigValue("js_key_value_config");
            //ViewBag.Order_Shared_discount_money_rate = configMgr.GetConfigValue<decimal>("Order_Shared_discount_money_rate", 0.29m);
            //ViewBag.Order_Shared_discount_beans_rate = configMgr.GetConfigValue<decimal>("Order_Shared_discount_beans_rate", 0.71m);
            //ViewBag.Order_Shared_people_num = configMgr.GetConfigValue<int>("Order_Shared_people_num", 2);
            //#region 发红包逻辑
            //ViewBag.h_money_free = 0;
            //ViewBag.daily_h_money_free = 0;
            //ViewBag.has_received_free_money_default = true;
            //ViewBag.has_received_daily_free_h_money = false;
            //ViewBag.first_h_money_free = 0;
            //var tokenObj = TokenObject.InitTokenObjFromString(token);
            //var user = new UserAuth();
            //var res = user.Load(tokenObj.id);
            //if (res == MessageCode.SUCCESS)
            //{
            //    ViewBag.h_money_pay = user.h_money_pay;
            //    ViewBag.longtoken = user.long_time_token;
               
            //    ViewBag.h_money_free = user.h_money_free;
            //    ViewBag.has_received_free_money_default = user.has_received_free_money_default;
            //    ViewBag.has_received_daily_free_h_money = user.has_received_daily_free_h_money;
            //    if (new UserManager().IfNewUser(user))
            //    {
            //        // 如果是新用户，给允许红包兑换金豆的游戏次数重新赋值
            //        ViewBag.m_play_total_times = configMgr.GetConfigValue("GameCenter_can_change_goldbeans_times_NewUser");
            //        ViewBag.is_new_user = 1;
            //    }
            //    else
            //    {
            //        ViewBag.is_new_user = 0;
            //    }

            //    RRLDB db = null;

            //    db = new RRLDB();
            //    db.ExeCMD($@"update rrl_user set last_access_time=getdate() where id={user.id}");
            //    var ds = db.ExeQuery(@"Select [Value] From rrl_config Where [Item] = 'daily_h_money_free'
            //                            union  ALL
            //                           Select [Value] From rrl_config Where [Item] = 'first_h_money_free'
            //                            union all
            //                        Select [Value] From rrl_config Where [Item] = 'freeRedPackage_to_beans_rate'");
            //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 3)
            //    {
            //        ViewBag.daily_h_money_free = Convert.ToDouble(ds.Tables[0].Rows[0]["Value"]);
            //        ViewBag.first_h_money_free = Convert.ToDouble(ds.Tables[0].Rows[1]["Value"]);
            //        ViewBag.freeRedPackage_to_beans_rate = Convert.ToDouble(ds.Tables[0].Rows[2]["Value"]);
            //    }

            //    ViewBag.spreader_redpackage = 0;
            //}

            //#endregion





            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            result.data = jsVariables;

            return result;
        }
    }
}