using Core;
using System;
using System.Web.Mvc;

namespace RRL.Core
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class CheckUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AuthUser authUser = new AuthUser();
            if (!authUser.IsAuth)
            {
                filterContext.HttpContext.Response.Write("<script>top.location.href=\"/Home/Login\";</script>");
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }

        }
    }
}
