using System;
using System.Web;
using System.Web.Mvc;

namespace RRL.Core
{
    /// <summary>
    /// 所有控制器的基类
    /// </summary>
    public abstract class BaseController : Controller
    {
        protected DateTime startDate;
        protected DateTime endDate;

        public bool IsAjax
        {
            get
            {
                return Request.IsAjaxRequest();
            }
        }

        /// <summary>
        /// 如果是ajax访问则返回json对象,否则返回view
        /// </summary>
        /// <param name="o">要返回的对象</param>
        /// <param name="viewName">制定返回的view</param>
        /// <param name="isAllowGet">是否允许get方式访问，默认是post方式</param>
        /// <returns></returns>
        public ActionResult IsAjaxReturnJsonElseView(object o, string viewName = "", bool isAllowGet = false)
        {
            if (IsAjax)
            {
                return Json(o, isAllowGet ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet);
            }
            if (string.IsNullOrEmpty(viewName))
            {
                return View(o);
            }
            return View(viewName, o);
        }

        /// <summary>
        /// ajax访问返回view
        /// </summary>
        /// <param name="o"></param>
        /// <param name="newViewName"></param>
        /// <returns></returns>
        public ActionResult IsAjaxReturnNewView(object o, string newViewName)
        {
            if (IsAjax)
            {
                return View(newViewName, o);
            }

            return View(o);
        }

        protected ActionResult ToUrl(string url)
        {
            return new RedirectResult(url);
        }

        protected void WriteCookie(string key, string value)
        {
            Response.SetCookie(new System.Web.HttpCookie(key, value)
            {
                HttpOnly = false
            });
        }

        protected void WriteCookie(HttpCookie cookie)
        {
            Response.SetCookie(cookie);
        }

        protected string ReadCookie(string key)
        {
            string ret = string.Empty;
            if (Request.Cookies[key] != null)
            {
                ret = Request.Cookies[key].Value;
            }

            return ret;
        }

        protected string RemoveCookie(string key)
        {
            string ret = string.Empty;
            if (Response.Cookies[key] != null)
            {
                HttpCookie myCookie = new HttpCookie(key);
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.SetCookie(myCookie);
            }

            return ret;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            startDate = DateTime.Now;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            endDate = DateTime.Now;
        }
    }
}