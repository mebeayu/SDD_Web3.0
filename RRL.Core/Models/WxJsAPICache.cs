using RRL.Config;
using RRL.Core.Models.WxJSAPI;
using RRL.DB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;

namespace RRL.Core.Models
{
    public class WxJsAPICache
    {
        #region 字段定义

        private string access_token;
        private DateTime access_token_timestamp;
        private int access_token_expire;
        private string jsapi_ticket;
        private DateTime jsapi_ticket_timestamp;
        private int jsapi_ticket_exppire;

        #endregion 字段定义

        /// <summary>
        /// 从数据库中载入数据
        /// </summary>
        private void LoadFromDataBase()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery("SELECT TOP 1 * FROM wx_cache");
            db.Close();
            DataRow CacheRow = ds.Tables[0].Rows[0];
            InitCache(CacheRow);
        }

        /// <summary>
        /// 构造WxJsAPI缓存对象
        /// </summary>
        public WxJsAPICache()
        {
            LoadFromDataBase();
        }

        /// <summary>
        /// 保存AccessToken
        /// </summary>
        private void SaveAccessTokenCache()
        {
            RRLDB db = new RRLDB();
            int res = db.ExeCMD(@"update wx_cache set
                                    access_token = @access_token,
                                    access_token_timestamp = @access_token_timestamp,
                                    access_token_expire = @access_token_expire
                                    where id=1",
                                 new SqlParameter("@access_token", access_token),
                                 new SqlParameter("@access_token_timestamp", access_token_timestamp),
                                 new SqlParameter("@access_token_expire", SqlDbType.Int, 4) { Value = access_token_expire });
            db.Close();
        }

        /// <summary>
        /// 保存JSTicket
        /// </summary>
        private void SaveJsTicketCache()
        {
            RRLDB db = new RRLDB();
            int res = db.ExeCMD(@"update wx_cache set
                                    jsapi_ticket = @jsapi_ticket,
                                    jsapi_ticket_timestamp = @jsapi_ticket_timestamp,
                                    jsapi_ticket_exppire = @jsapi_ticket_exppire
                                    where id=1",
                                 new SqlParameter("@jsapi_ticket", jsapi_ticket),
                                 new SqlParameter("@jsapi_ticket_timestamp", jsapi_ticket_timestamp),
                                 new SqlParameter("@jsapi_ticket_exppire", SqlDbType.Int, 4) { Value = jsapi_ticket_exppire });
            db.Close();
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            TimeSpan ts = DateTime.Now - access_token_timestamp;
            int seconds = Convert.ToInt32(ts.TotalSeconds);
            if (access_token_expire == 0 || access_token.Length == 0 || seconds > access_token_expire)
            {
                GetAccessTokenFromWeb();
            }
            return access_token;
        }

        /// <summary>
        /// 通过网络获取accesstoken
        /// </summary>
        private void GetAccessTokenFromWeb()
        {
            string Appid = PlatFormConfig.WxMPAPPID;
            string AppSecret = PlatFormConfig.WxMPAPPSECRET;
            string AccessTokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + Appid + "&secret=" + AppSecret;
            //请求AccessToken
            string ResponseString = GetRequest(AccessTokenUrl);
            //查找字符串中是否含有"errorcode"子串
            int i = System.Text.RegularExpressions.Regex.Matches(ResponseString, "errcode").Count;
            if (i == 0)
            {
                SuccessAccessToken ReturnObj = Newtonsoft.Json.JsonConvert.DeserializeObject<SuccessAccessToken>(ResponseString);
                access_token = ReturnObj.access_token;
                access_token_timestamp = DateTime.Now;
                access_token_expire = ReturnObj.expires_in;
            }
            else
            {
                ErrorAccessToken ReturnObj = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorAccessToken>(ResponseString);
                access_token = "";
            }
            SaveAccessTokenCache();
        }

        /// <summary>
        /// 获取通行证
        /// </summary>
        /// <returns></returns>
        public string GetJsApiTicket()
        {
            TimeSpan ts = DateTime.Now - jsapi_ticket_timestamp;
            int seconds = Convert.ToInt32(ts.TotalSeconds);
            if (jsapi_ticket_exppire == 0 || jsapi_ticket.Length == 0 || seconds > jsapi_ticket_exppire)
            {
                GetJsAPITicketFromWeb();
            }

            return jsapi_ticket; ;
        }

        /// <summary>
        /// 通过网络获取js通行证
        /// </summary>
        private void GetJsAPITicketFromWeb()
        {
            string token = GetAccessToken();
            string JsApiTicketUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + token + "&type=jsapi";

            string ResponseString = GetRequest(JsApiTicketUrl);

            JsApiTicket ReturnObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JsApiTicket>(ResponseString);

            if (ReturnObj.errcode == 0)
            {
                jsapi_ticket = ReturnObj.ticket;
                jsapi_ticket_timestamp = DateTime.Now;
                jsapi_ticket_exppire = ReturnObj.expires_in;
            }
            else
            {
                jsapi_ticket = "";
            }
            SaveJsTicketCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        /// <param name="CacheRow">缓存记录单元</param>
        private void InitCache(DataRow CacheRow)
        {
            access_token = Convert.ToString(CacheRow["access_token"]);
            access_token_timestamp = Convert.ToDateTime(CacheRow["access_token_timestamp"]);
            access_token_expire = Convert.ToInt32(CacheRow["access_token_expire"]);
            jsapi_ticket = Convert.ToString(CacheRow["jsapi_ticket"]);
            jsapi_ticket_timestamp = Convert.ToDateTime(CacheRow["jsapi_ticket_timestamp"]);
            jsapi_ticket_exppire = Convert.ToInt32(CacheRow["jsapi_ticket_exppire"]);
        }

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="RequestString">请求字符串</param>
        /// <returns>请求结果</returns>
        private string GetRequest(string RequestString)
        {
            var Request = (HttpWebRequest)WebRequest.Create(RequestString);
            var Response = (HttpWebResponse)Request.GetResponse();
            string ResponseString = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            return ResponseString;
        }
    }
}