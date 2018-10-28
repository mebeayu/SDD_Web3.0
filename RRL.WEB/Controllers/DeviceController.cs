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

namespace RRL.WEB.Controllers
{
    // STORY #6 lcl 20180419 Insert
    public class DeviceController : Controller
    {
        /// <summary>
        /// 用户登录后跳转到本Action，以供移动设备获取用户信息
        /// 1)  /Device/LoginInfo?token=xosdfsdf&redirect=/#/my
        /// 2)  加密用DES  加密秘钥“rrl@2334"      加密字符串  token=xosdfsdf&deviceId=3253452345  得到加密后字符串 
        /// 3） 接口情况用post /WebApi/Game/SpreaderShareHongMoneyPay,{DeviceInfo:加密后字符串}
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginInfo()
        {
            // 获取跳转返回的页面地址
            if (!string.IsNullOrWhiteSpace(Request.QueryString["redirect"]))
            {
                ViewBag.goBackUrl = Server.UrlDecode(Request.QueryString["redirect"]);
            }

            return View();
        }

        /// <summary>
        /// 拨打电话 /Device/Dialing?tel=13827470343
        /// </summary>
        /// <returns></returns>
        public ActionResult Dialing()
        {
            return View();
        }

        /// <summary>
        /// 第三方登录类型,wx   /Device/ThirdPartyLogin?type=wx
        /// </summary>
        /// <returns></returns>
        public ActionResult ThirdPartyLogin()
        {
            return View();
        }

        // lcl 2018-08-24 Insert
        /// <summary>
        /// 通过APP进行用户登录的跳转地址
        /// </summary>
        /// <returns></returns>
        public ActionResult UserLogin()
        {
            return View();
        }
    }
}