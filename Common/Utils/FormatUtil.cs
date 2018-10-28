using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System;


namespace Common.Utils
{
    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public class FormatUtil
    {
        /// <summary>
        /// 将字符串进行HTML格式化
        /// </summary>
        /// <param name="str">字符串</param>
        public static string HtmlEncode(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = System.Web.HttpContext.Current.Server.HtmlEncode(str);
            }
            return str;
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="len">大小字节</param>
        /// <returns>返回大小文本串</returns>p
        public static string FormatLength(long len)
        {
            if (len > 0x40000000) { return string.Format("{0} GB", ConvertUtil.ToInt32(len / 0x40000000)); }
            if (len > 0x00100000) { return string.Format("{0} MB", ConvertUtil.ToInt32(len / 0x00100000)); }
            if (len > 0x00000400) { return string.Format("{0} KB", ConvertUtil.ToInt32(len / 0x00000400)); }
            else { return string.Format("{0} byte", ConvertUtil.ToInt32(len)); }
        }

        /// <summary>
        /// 清除HTML
        /// </summary>
        /// <param name="html">原始代码</param>
        /// <returns>返回纯文本</returns>
        public static string ClearHtml(string html)
        {
            if (!string.IsNullOrEmpty(html))
            {
                html = Regex.Replace(html, @"<[^>]*>", string.Empty);
            }
            return html;
        }

        /// <summary>
        /// URL解密
        /// </summary>
        /// <param name="strVal">解密字串</param>
        public static string UrlDecode(string strVal)
        {
            if (!string.IsNullOrEmpty(strVal))
            {
                strVal = HttpUtility.UrlDecode(strVal, Encoding.UTF8);
            }
            return strVal;
        }

        /// <summary>
        /// URL加密
        /// </summary>
        /// <param name="strVal">加密字串</param>
        public static string UrlEncode(string strVal)
        {
            if (!string.IsNullOrEmpty(strVal))
            {
                strVal = HttpUtility.UrlEncode(strVal, Encoding.UTF8);
            }
            return strVal;
        }

        /// <summary>
        /// 处理原本格式并进行HTML编码
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <returns>返回处理后的代码</returns>
        //public static string Pre(string text)
        //{
        //    if (!string.IsNullOrEmpty(text))
        //    {
        //        text = HtmlEncode(text).Replace("\n", "<br />");
        //    }
        //    return text;
        //}

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="len">截取的长度</param>
        /// <returns>返回截取后的字符串</returns>
        public static string StrCut(string str, int len)
        {
            if (!string.IsNullOrEmpty(str) && len > 0)
            {
                int l = str.Length;
                int clen = 0;
                while (clen < len && clen < l)
                {
                    //每次遇到一个中文，则将该长度减一   
                    if ((int)str[clen] > 128) { len--; }
                    clen++;
                }
                if (clen < l && clen > 2) { str = str.Substring(0, clen - 2) + ".."; }
            }
            return str;
        }

        /// <summary>
        /// 将明文加密为系统字符
        /// </summary>
        /// <param name="str">即将加密的明文字符</param>
        public static string MD5AndSHA1(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
                str = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
            }
            return str;
        }

        /// <summary>
        /// 将明文加密为系统字符
        /// </summary>
        /// <param name="str">即将加密的明文字符</param>
        public static string SHA1AndMD5(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
                str = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }
            return str;
        }

        /// <summary>
        /// 将明文加密为系统字符
        /// </summary>
        /// <param name="str">即将加密的明文字符</param>
        public static string MD5(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }
            return str;
        }

        /// <summary>
        /// 将明文加密为系统字符
        /// </summary>
        /// <param name="str">即将加密的明文字符</param>
        public static string SHA1(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
            }
            return str;
        }

        /// <summary>
        /// 获取艾特的文字（不含@符号）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<string> SelectAt(string content)
        {
            List<string> result = new List<string>();
            foreach (Match m in Regex.Matches(content, @"@([^\s]+)\s"))
            {
                result.Add(m.Groups[1].Value);
            }
            return result;
        }
    }
}
