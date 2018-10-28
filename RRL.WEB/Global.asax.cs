using RRL.WEB.App_Start;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Timers;
using RedisHelp;
using RRL.Core.Manager;

namespace RRL.WEB
{
    public class Global : HttpApplication
    {
       
        private void Application_Start(object sender, EventArgs e)
        {
            
             
            AppUtils.Ver = DateTime.Now.ToString("yyyyMMddHHmm");
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
       
        
 
        

        public override void Init()
        {
            //注册事件
            this.AuthenticateRequest += WebApiApplication_AuthenticateRequest;
            base.Init();
        }

        void WebApiApplication_AuthenticateRequest(object sender, EventArgs e)
        {
            //启用 webapi 支持session 会话
            HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }

        protected void Application_End(object sender, EventArgs e)
        {
             
        }


    }
}