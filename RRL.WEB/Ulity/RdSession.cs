using System;
using System.Web;
using RedisHelp;
using RRL.Config;

namespace RRL.WEB.Ulity
{
    /// <summary>
    /// 
    /// </summary>
    public class RdSession
    {
        private HttpContext Context { get; }
        private bool Readonly { get; }
        //private RedisHelper Redis { get; } = new RedisHelper(RRLConfig.RedisDbNum, "10.29.216.244:6381");
        public RedisHelper Redis { get; } = new RedisHelper(RRLConfig.RedisDbNum, RRLConfig.RedisConn);
        private int TimeOut { get; }
        private static string  SessionName = "Redis_Session";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isReadOnly"></param>
        /// <param name="timeOut"></param>
        public RdSession(HttpContext context,bool isReadOnly = true,int timeOut = 20)
        {
            Context = context;
            Readonly = isReadOnly;
            TimeOut = timeOut;

            Redis.KeyExpire(SessionId, new TimeSpan(0, TimeOut, 0));
        }

        /// <summary>
        /// 计算SessionID
        /// </summary>
        public string SessionId
        {
            get
            {
                var cookie = Context.Request.Cookies.Get(SessionName);
                if (!string.IsNullOrEmpty(cookie?.Value)) return "Session_" + cookie.Value;
                var isessionid = Guid.NewGuid().ToString();
                var icookie = new HttpCookie(SessionName, isessionid)
                {
                    HttpOnly = Readonly,
                    Expires = DateTime.Now.AddMinutes(TimeOut)
                };
                Context.Response.Cookies.Add(icookie);
                return "Session_" + isessionid;

            }
        }

        /// <summary>
        /// 获取或设置session
        /// </summary>
        /// <param name="sessionKey"></param>
        public string this[string sessionKey]
        {
            get { return Redis.HashGetStr(SessionId, sessionKey);}
            set { Redis.HashSetStr(SessionId, sessionKey, value); }
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public bool Exist(string sessionKey)
        {
            return Redis.HashExists(SessionId, sessionKey);
        }

        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="sessionValue"></param>
        public void Add(string sessionKey, string sessionValue)
        {
            Redis.HashSetStr(SessionId, sessionKey, sessionValue);
        }
    }
}