using System;
using RRL.Core.Manager;
using System.Web.Mvc;
using RRL.DB;

namespace RRL.WEB.Controllers
{
    /// <summary>
    /// 活动控制器
    /// </summary>
    public class EventController : Controller
    {
        /// <summary>
        /// 活动1：一元抢购纸巾
        /// </summary>
        /// <returns>视图</returns>   
        public ActionResult SecondKill()
        {
            return View();
        }
        public ActionResult DetonatingWeek()
        {
            
            return View();
        }
        public ActionResult One()
        {
            
            return View();
        }
        public ActionResult ShopActivte()
        {

            return View();
        }


        /// <summary>
        /// 活动2：秋日特饮
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Two()
        {
         
            return View();
        }

        /// <summary>
        /// 活动3：火锅活动
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Three()
        {
            HelpManager.MarkPageView("/Event/Three");
            return View();
        }

        /// <summary>
        /// 活动4：七夕玫瑰
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Four()
        {
            HelpManager.MarkPageView("/Event/Four");
            return View();
        }

        /// <summary>
        /// 活动5：一元抢购牛肉酱
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Five()
        {
            HelpManager.MarkPageView("/Event/Five");
            return View();
        }

        /// <summary>
        /// 活动6：中秋大礼包
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Six()
        {
            HelpManager.MarkPageView("/Event/Six");
            return View();
        }

        /// <summary>
        /// 活动7：一元抢购
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Seven()
        {
            HelpManager.MarkPageView("/Event/Seven");
            return View();
        }

        /// <summary>
        /// 活动8：豆浆
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Eight()
        {
            HelpManager.MarkPageView("/Event/Eight");
            return View();
        }

        /// <summary>
        /// 活动9：啤酒节
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Nine()
        {
            HelpManager.MarkPageView("/Event/Nine");
            return View();
        }

        /// <summary>
        /// 活动10：爱牙日
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Ten()
        {
            HelpManager.MarkPageView("/Event/Ten");
            return View();
        }


        /// <summary>
        /// 活动11：国庆
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Eleven()
        {
            HelpManager.MarkPageView("/Event/Eleven");
            return View();
        }

        /// <summary>
        /// 活动十二:使用说明
        /// </summary>
        /// <returns></returns>
        public ActionResult Twelve()
        {
            HelpManager.MarkPageView("/Event/Twelve");
            return View();
        }

        /// <summary>
        /// 活动十三:分销说明
        /// </summary>
        /// <returns></returns>
        public ActionResult Thirteen()
        {
            HelpManager.MarkPageView("/Event/Thirteen");
            return View();
        }

        /// <summary>
        /// 活动十四:一元专区
        /// </summary>
        /// <returns></returns>
        public ActionResult Fourteen()
        {
            HelpManager.MarkPageView("/Event/Fourteen");
            return View();
        }

        /// <summary>
        /// 活动十五:申请成为掌柜
        /// </summary>
        /// <returns></returns>
        public ActionResult Fifteen()
        {
            HelpManager.MarkPageView("/Event/Fifteen");
            return View();
        }

        /// <summary>
        /// 分享获取红包
        /// </summary>
        /// <returns></returns>
        public ActionResult Xxxshare()
        {
            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT [Value] FROM rrl_config WHERE Item = 'spreader_h_money'");
            var jindou = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            db.Close();

            ViewBag.jindou = jindou;
            ViewBag.type = Request.QueryString["type"];
            ViewBag.orderid = Request.QueryString["orderid"];
            ViewBag.taskId = Request.QueryString["tid"];
            HelpManager.MarkPageView("/Event/Xxxshare");
            return View();
        }

        /// <summary>
        /// 分享获取红包
        /// </summary>
        /// <returns></returns>
        public ActionResult Newshare()
        {
            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT [Value] FROM rrl_config WHERE Item = 'spreader_h_money'");
            var jindou = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            db.Close();

            ViewBag.jindou = jindou;
            ViewBag.type = Request.QueryString["type"];
            ViewBag.orderid = Request.QueryString["orderid"];
            ViewBag.taskId = Request.QueryString["tid"];
            HelpManager.MarkPageView("/Event/Newshare");
            return View();
        }



        public ActionResult XxxshareWx()
        {
            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT [Value] FROM rrl_config WHERE Item = 'spreader_h_money'");
            var jindou = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            db.Close();

            ViewBag.jindou = jindou;
            ViewBag.type = Request.QueryString["type"];
            ViewBag.orderid = Request.QueryString["orderid"];
            HelpManager.MarkPageView("/Event/Xxxshare");
            return View();
        }

        public ActionResult XxxshareEx(string val)
        {
            string type = "";
            string value="",  token="";
            var sp= val.Split('-');
            if(sp.Length>0)
            {
                token = sp[0];
            }
            if (sp.Length > 1)
            {
                type = sp[1];
            }
            if (sp.Length > 2)
            {
                value = sp[2];
            }
            return Redirect($"/Event/Xxxshare?type={type}&token={token}&orderid={value}");
        }
        public ActionResult SharingGuide()
        {
            return View();
        }

        public ActionResult ProductDes()
        {
            return View();
        }

        public ActionResult GoWechatCircle()
        {
            return View();
        }

        public ActionResult GoWechatFriend()
        {
            return View();
        }

        // TASK #28 lcl 20180620 Insert
        /// <summary>
        /// 分享商品到微信朋友圈
        /// </summary>
        /// <returns></returns>
        public ActionResult ShareGoodsToWechatCircle()
        {
            return View();
        }

        // TASK #28 lcl 20180620 Insert
        /// <summary>
        /// 分享商品给微信好友
        /// </summary>
        /// <returns></returns>
        public ActionResult ShareGoodsToWechatFriend()
        {
            return View();
        }

        public ActionResult RedpackageFissionShare()
        {
            return View();
        }
        //检查更新
        public ActionResult Update()
        {
            return View();
        }

        public ActionResult OpenFissionRedpackage()
        {
            return View();
        }
    }
}