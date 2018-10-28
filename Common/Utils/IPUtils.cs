using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Common.Utils
{
    public class IPUtils
    {
        /// <summary>
        /// ip转成long
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long Ip2Long(string ip)
        {
            byte[] bytes = Ip2Bytes(ip);
            return Bytes2Long(bytes);
        }
        /// <summary>
        /// long转成ip
        /// </summary>
        /// <param name="ipLong"></param>
        /// <returns></returns>
        public static string Long2Ip(long ipLong)
        {
            byte[] bytes = Long2Bytes(ipLong);
            return Bytes2Ip(bytes);
        }
        /// <summary>
        /// long转成byte[]
        /// </summary>
        /// <param name="ipvalue"></param>
        /// <returns></returns>
        public static byte[] Long2Bytes(long ipvalue)
        {
            byte[] b = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                b[3 - i] = (byte)(ipvalue >> 8 * i & 255);
            }
            return b;
        }
        /// <summary>
        /// byte[]转成long
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public static long Bytes2Long(byte[] bt)
        {
            int x = 3;
            long o = 0;
            foreach (byte f in bt)
            {
                o += (long)f << 8 * x--;
            }
            return o;
        }
        /// <summary>
        /// ip转成byte[]
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static byte[] Ip2Bytes(string ip)
        {
            string[] sp = ip.Split('.');
            return new byte[] { Convert.ToByte(sp[0]), Convert.ToByte(sp[1]), Convert.ToByte(sp[2]), Convert.ToByte(sp[3]) };
        }
        /// <summary>
        /// byte[]转成ip
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Bytes2Ip(byte[] bytes)
        {
            return string.Format("{0}.{1}.{2}.{3}"
                                   , bytes[0]
                                   , bytes[1]
                                   , bytes[2]
                                   , bytes[3]);
        }

        public static string getIPAddress()
        {

            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (result != null && result != String.Empty)
            {
                //可能有代理   
                if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式   
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。   
                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIPAddress(temparyip[i])
                                && temparyip[i].Substring(0, 3) != "10."
                                && temparyip[i].Substring(0, 7) != "192.168"
                                && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];    //找到不是内网的地址   
                            }
                        }
                    }
                    else if (IsIPAddress(result)) //代理即是IP格式   
                        return result;
                    else
                        result = null;    //代理中的内容 非IP，取IP   
                }
            }

            string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (null == result || result == String.Empty)
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (result == null || result == String.Empty)
                result = HttpContext.Current.Request.UserHostAddress;

            return result;

        }

        //// <summary>  
        /// 判断是否是IP地址格式 0.0.0.0  
        /// </summary>  
        /// <param name="str1">待判断的IP地址</param>  
        /// <returns>true or false</returns>  
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }
    }
}
