using System.Web.Mvc;
using System.Web.Routing;

namespace RRL.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{controller}/Index");
            //主视图路由
            routes.MapRoute(
                name: "ViewRoute",
                url: "{action}",
                defaults: new
                {
                    controller = "ViewRoute",
                    action = "Index"
                });
            //支付回调路由
            routes.MapRoute(
                name: "PayCallBack",
                url: "PayCallBack/{action}",
                defaults: new
                {
                    controller = "PayCallBack",
                    action = "Index"
                });
            //活动页
            routes.MapRoute(
                name: "Event",
                url: "Event/{action}",
                defaults: new
                {
                    controller = "Event",
                    action = "Index"
                });

            //活动页
            routes.MapRoute(
                name: "VerCode",
                url: "VerCode/{action}",
                defaults: new
                {
                    controller = "VerCode",
                    action = "Picture"
                });
            //活动页
            routes.MapRoute(
                name: "Game",
                url: "Game/{action}",
                defaults: new
                {
                    controller = "Game",
                    action = "Index"
                });
            // STORY #6 lcl 20180419 Insert
            routes.MapRoute(
                name: "Device",
                url: "Device/{action}",
                defaults: new
                {
                    controller = "Device",
                    action = "Index"
                });

            routes.MapRoute(
                name: "Sport",
                url: "Sport/{action}",
                defaults: new
                {
                    controller = "Sport",
                    action = "Index"
                });

            // TASK #28 lcl 20180613 Insert
            // 商铺、商品
            routes.MapRoute(
                name: "Shop",
                url: "Shop/{action}",
                defaults: new
                {
                    controller = "Shop",
                    action = "Index"
                });
        }
    }
}