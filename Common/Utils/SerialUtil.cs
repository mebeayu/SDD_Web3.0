using System;

namespace Common.Utils
{
    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public enum SerialUtilMode
    {
        /// <summary>
        /// 大写
        /// </summary>
        Upper,
        /// <summary>
        /// 小写
        /// </summary>
        Lower
    }

    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public class SerialUtil
    {
        /// <summary>
        /// 获取没有横杠的GUID
        /// </summary>
        /// <returns></returns>
        public static string GetGuidText(SerialUtilMode mode)
        {
            string guid = GetGuid(mode);
            if (guid == null) { return null; }
            else { return guid.Replace("-", string.Empty); }
        }
        /// <summary>
        /// 获取没有横杠的GUID小写形式
        /// </summary>
        /// <returns></returns>
        public static string GetGuidText()
        {
            return GetGuidText(SerialUtilMode.Lower);
        }

        /// <summary>
        /// 获取有横杠的GUID
        /// </summary>
        /// <returns></returns>
        public static string GetGuid(SerialUtilMode mode)
        {
            switch (mode)
            {
                case SerialUtilMode.Lower: { return System.Guid.NewGuid().ToString().ToLower(); }
                case SerialUtilMode.Upper: { return System.Guid.NewGuid().ToString().ToUpper(); }
            }
            return null;
        }

        /// <summary>
        /// 获取有横杠的GUID小写形式
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            return GetGuid(SerialUtilMode.Lower);
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetRandom(int len)
        {
            string result = string.Empty;
            for (int i = 1; i <= len; i++)
            {
                Random r = new Random(Guid.NewGuid().GetHashCode());
                result += r.Next(0, 9);
            }
            return result;
        }
    }
}
