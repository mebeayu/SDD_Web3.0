using System;
using System.Web.Mvc;

namespace Common.Utils
{
    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public static class FormCollectionExpand
    {
        public static int GetInt32(this FormCollection p, string key)
        {
            return ConvertUtil.ToInt32(p[key]);
        }

        public static DateTime GetDateTime(this FormCollection p, string key)
        {
            return ConvertUtil.ToDateTime(p[key]);
        }

        public static bool GetBoolean(this FormCollection p, string key)
        {
            return ConvertUtil.ToBoolean(p[key]);
        }

        public static double GetDouble(this FormCollection p, string key)
        {
            return ConvertUtil.ToDouble(p[key]);
        }

        public static decimal GetDecimal(this FormCollection p, string key)
        {
            return ConvertUtil.ToDecimal(p[key]);
        }
    }
}