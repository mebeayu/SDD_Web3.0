using System;
using System.Collections.Generic;
using System.Web;

namespace Common.Utils
{
    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public class CookieUtil
    {
        static string _cookieKey = "RFCOOKIE";

        /// <summary>
        /// 取得用户 Cookie
        /// </summary>
        public static Dictionary<string, string> Get(string cookieKey)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            HttpCookie cookie = HttpContext.Current.Request.Cookies[_cookieKey + cookieKey];
            if (cookie != null)
            {
                foreach (string key in cookie.Values.AllKeys)
                {
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        result.Add(key, FormatUtil.UrlDecode(cookie.Values[key]));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 设置用户Cookie
        /// </summary>
        public static void Set(string cookieKey, Dictionary<string, string> values, DateTime? expires, string path)
        {
            HttpCookie cookie = new HttpCookie(_cookieKey + cookieKey);
            if (!string.IsNullOrWhiteSpace(path)) { cookie.Path = path; }
            if (expires != null) { cookie.Expires = Convert.ToDateTime(expires); }
            foreach (KeyValuePair<string, string> item in values)
            {
                if (!string.IsNullOrWhiteSpace(item.Key))
                {
                    cookie.Values.Add(item.Key, FormatUtil.UrlEncode(item.Value));
                }
            }
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 设置用户Cookie
        /// </summary>
        public static void Set(string cookieKey, Dictionary<string, string> values, DateTime? expires)
        {
            Set(cookieKey, values, expires, null);
        }

        /// <summary>
        /// 设置用户Cookie
        /// </summary>
        public static void Set(string cookieKey, Dictionary<string, string> values)
        {
            Set(cookieKey, values, null, null);
        }

        /// <summary>
        /// 清除用户会话
        /// </summary>
        public static void Clear(string cookieKey)
        {
            HttpCookie cookie = new HttpCookie(_cookieKey + cookieKey);
            if (cookie != null)
            {
                cookie.Values.Clear();
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }
    }
}