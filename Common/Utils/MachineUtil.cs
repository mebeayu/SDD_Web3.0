using System.Web;

namespace Common.Utils
{
    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public class MachineUtil
    {
        static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 取客户端IP
        /// </summary>
        public static string ClientIP
        {
            get
            {
                /*string sIP = string.Empty;
                try
                {
                    HttpRequest oHR = HttpContext.Current.Request;
                    if (oHR.ServerVariables["HTTP_VIA"] != null) { sIP = oHR.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim(); }
                    else { sIP = oHR.UserHostAddress; }
                }
                catch { }
                return sIP;*/

                try
                {
                    string userHostAddress = HttpContext.Current.Request.UserHostAddress;
                    if (string.IsNullOrEmpty(userHostAddress))
                    {
                        userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                    //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
                    if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
                    {
                        return userHostAddress;
                    }
                }
                catch { }
                return "(local)";
            }
        }
    }
}
