using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Utility
{
    public class DatetimeHelper
    {
        /// <summary>
        /// 将js中的时间戳转换成c#的日期时间
        /// </summary>
        /// <param name="milliseconds">js中13位数的长整型时间戳(单位：毫秒)</param>
        /// <returns></returns>
        public static DateTime JsTimeStampConvertDateTime(long milliseconds)
        {
            /*
             * 说明：js中的时间戳表示的是，当前时间距离1970-01-01 00:00:00的毫秒数；
             *       c#中的时间是从0001-01-01 00:00:00开始的，所以转换时必须加上0001年到1970年所表示的时间距离
             */
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(milliseconds);

            return dt.ToLocalTime();
        }
    }
}
