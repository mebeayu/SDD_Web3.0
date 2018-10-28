using System;
using System.Web;
using System.Collections.Generic;

namespace Common.Utils
{
    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public class RequestUtil
    {
        /// <summary>
        /// 取得GET参数值
        /// </summary>
        /// <param name="key">关键字</param>
        public static string Query(string key)
        {
            string val = HttpContext.Current.Request.QueryString[key];
            string result = string.IsNullOrEmpty(val) ? string.Empty : val.ToString().Trim();
            return result;
        }

        /// <summary>
        /// 取得POST参数值
        /// </summary>
        /// <param name="key">关键字</param>
        public static string Post(string key)
        {
            string val = HttpContext.Current.Request.Form[key];
            string result = string.IsNullOrEmpty(val) ? string.Empty : val.ToString().Trim();
            return result;
        }

        /// <summary>
        /// 直接获取日期类型的表单参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static DateTime PostDateTime(string key)
        {
            return ConvertUtil.ToDateTime(Post(key));
        }

        /// <summary>
        /// 直接获取日期类型的地址栏参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static DateTime QueryDateTime(string key)
        {
            return ConvertUtil.ToDateTime(Query(key));
        }
        
        /// <summary>
        /// 直接获取价格类型的表单参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static decimal PostDecimal(string key)
        {
            return ConvertUtil.ToDecimal(Post(key));
        }

        /// <summary>
        /// 直接获取价格类型的地址栏参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static decimal QueryDecimal(string key)
        {
            return ConvertUtil.ToDecimal(Query(key));
        }

        /// <summary>
        /// 直接获取数字类型的地址栏参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static int QueryInt32(string key)
        {
            return ConvertUtil.ToInt32(Query(key));
        }

        /// <summary>
        /// 直接获取数字类型的表单参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static int PostInt32(string key)
        {
            return ConvertUtil.ToInt32(Post(key));
        }

        /// <summary>
        /// 直接获取布尔类型的地址栏参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static bool QueryBoolean(string key)
        {
            return ConvertUtil.ToBoolean(Query(key));
        }

        /// <summary>
        /// 直接获取布尔类型的表单参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static bool PostBoolean(string key)
        {
            return ConvertUtil.ToBoolean(Post(key));
        }

        /// <summary>
        /// 直接获取双精度浮点类型的地址栏参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static double QueryDouble(string key)
        {
            return ConvertUtil.ToDouble(Query(key));
        }

        /// <summary>
        /// 直接获取双精度浮点类型的表单参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static double PostDouble(string key)
        {
            return ConvertUtil.ToDouble(Post(key));
        }
    }
}
