using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
namespace Common.Utils
{
                
    /// <summary>
    /// Jsapi Ticket
    /// </summary>
    public class WxUtilJsapiTicket
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }
        /// <summary>
        /// 字符串
        /// </summary>
        public string JsapiTicket { get; set; }
        public string AccessToken { get; set; }
    }

    /// <summary>
    /// Access Token 获取反馈
    /// </summary>
    public class WxUtilAccessTokenResult
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }

    /// <summary>
    /// jsapi_ticket 获取反馈
    /// </summary>
    public class WxUtilJsapiTicketResult
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }

    /// <summary>
    /// JsSdk wx.config 字符
    /// </summary>
    public class WxUtilJsConfig
    {
        public string appId { get; set; }
        public string timestamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }
        public string signstr { get; set; }
        public string accessToken { get; set; }
        public string jsapi_ticket { get; set; }
        public DateTime EffectiveTime { get; set; }

    }

    /// <summary>
    /// 微信微网站工具类
    /// </summary>
    public class WxUtil
    {
        static WxUtilJsapiTicket _jsapiTicket;

        const string AppId = "wxc7b26ea84765a443";

        const string AppSecret = "2774768d00653863d7679f10ac22232a";

        /// <summary>
        /// 获取Access Token
        /// </summary>
        /// <returns></returns>
        WxUtilAccessTokenResult GetAccessToken()
        {
            WxUtilAccessTokenResult result = null;
            using (WebClient wc = new WebClient())
            {
                result = new JavaScriptSerializer().Deserialize<WxUtilAccessTokenResult>(wc.DownloadString("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppId + "&secret=" + AppSecret));
            }
            return result;
        }

        /// <summary>
        /// 获取Jsapi Ticket
        /// </summary>
        /// <returns></returns>
        public WxUtilJsapiTicket GetJsapiTicket()
        {
            //return null;
            //以下是缓存机制
            if (_jsapiTicket == null || _jsapiTicket.ExpirationTime <= DateTime.Now)
            {
                var accessTokenResult = GetAccessToken();
                if (accessTokenResult != null)
                {
                    using (WebClient wc = new WebClient())
                    {
                        var result = new JavaScriptSerializer().Deserialize<WxUtilJsapiTicketResult>(wc.DownloadString("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + accessTokenResult.access_token + "&type=jsapi"));
                        if (result.errcode == 0)
                        {


                            //var wxTB = new Model.YYDB.wxtable();
                            //wxTB.access_token = accessTokenResult.access_token;
                            //wxTB.access_tokentime = DateTime.Now.AddSeconds(result.expires_in);
                            //wxTB.jsapi_ticket = result.ticket;
                            //wxTB.Token_GetDate = System.DateTime.Now;


                            _jsapiTicket = new WxUtilJsapiTicket()
                            {
                                ExpirationTime = DateTime.Now.AddSeconds(7200),//result.expires_in
                                JsapiTicket = result.ticket,
                                AccessToken = accessTokenResult.access_token
                            };
                        }
                        else { throw new Exception(result.errmsg); }
                    }
                }
            }
            return _jsapiTicket;
        }



        /// <summary>
        /// 创建微网站配置信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public WxUtilJsConfig CreateWxConfig(string url)
        {
            //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //DateTime nowTime = DateTime.Now;

            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string unixTime= Convert.ToInt64(ts.TotalSeconds).ToString();

            //long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
   
            //
            var Jsapi=GetJsapiTicket();
            string jsapiTicket = Jsapi.JsapiTicket;
            string nonceStr = Guid.NewGuid().ToString();
            //
            string signStr = "jsapi_ticket=" + jsapiTicket + "&noncestr=" + nonceStr + "&timestamp=" + unixTime + "&url=" + url;

            string signEncode = FormatUtil.SHA1(signStr).ToLower();
            //
            var result = new WxUtilJsConfig()
            {
                appId = AppId,
                nonceStr = nonceStr,
                timestamp = unixTime,
                signature = signEncode,
                signstr=signStr,
                accessToken =Jsapi.AccessToken,
                jsapi_ticket = Jsapi.JsapiTicket,
                EffectiveTime = Jsapi.ExpirationTime

            };
            return result;
        }
    }
}
