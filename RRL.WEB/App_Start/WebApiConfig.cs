using System.Web.Http;

namespace RRL.WEB
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Formatters.Add(config.Formatters.JsonFormatter);
            // Web API 路由
            config.MapHttpAttributeRoutes();
        }
    }
}