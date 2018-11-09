using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RRL.Core.Manager
{
    public class ConfigManager
    {
        const string CNT_CONFIG_CACHE_KEY = "RRL_CONFIG";

        /// <summary>
        /// 获取精选页轮播图
        /// </summary>
        /// <returns>数据集</returns>
        public DataSet GetGoodsCarousel()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(SqlQueryUtility.CAROUSEL_FIGURE_QUERY_STR,
                new SqlParameter("@CarouselType", SqlDbType.Int, 4) { Value = 2 });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取游戏专区分类
        /// </summary>
        /// <returns></returns>
        public int GetGameArea()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery($@"SELECT [Value] FROM rrl_config WHERE [Item] = 'game_area'");
            db.Close();
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 获取金币专区分类
        /// </summary>
        /// <returns></returns>
        public int GetGoldenCoinArea()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery($@"SELECT [Value] FROM rrl_config WHERE [Item] = 'golden_coin_area'");
            db.Close();
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 获取主页面轮播图
        /// </summary>
        /// <returns>数据集</returns>
        public DataSet GetMainCarousel()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(SqlQueryUtility.CAROUSEL_FIGURE_QUERY_STR);

            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取活动页面轮播图
        /// </summary>
        /// <returns></returns>
        public DataSet GetEventCarousel()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(SqlQueryUtility.CAROUSEL_FIGURE_QUERY_STR,
                new SqlParameter("@CarouselType", SqlDbType.Int, 4) { Value = 3 });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取兑换页面轮播图
        /// </summary>
        /// <returns></returns>
        public DataSet GetExchangeCarousel()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(SqlQueryUtility.CAROUSEL_FIGURE_QUERY_STR,
                new SqlParameter("@CarouselType", SqlDbType.Int, 4) { Value = 4 });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取安卓版本APP版本信息
        /// </summary>
        /// <returns>版本升级信息数据集</returns>
        public DataSet GetAndroidAppVersionInfo()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(SqlQueryUtility.ANDROID_APP_VERSION_QUERY_STR);
            db.Close();
            return ds;
        }


        /// <summary>
        /// 获取安卓版本APP版本信息
        /// </summary>
        /// <returns>版本升级信息数据集</returns>
        public DataSet GetAppleAppVersionInfo()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(SqlQueryUtility.APPLE_APP_VERSION_QUERY_STR);
            db.Close();
            return ds;
        }



        /// <summary>
        /// 获取APP介绍文本
        /// </summary>
        /// <returns>HTML文本</returns>
        public string GetAppSummary()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery("SELECT [Value] FROM rrl_config WHERE [Item] = 'app_about_us'");
            db.Close();
            return Convert.ToString(ds.Tables[0].Rows[0][0]);
        }

        public string GetConfigValue(string key)
        {
            Dictionary<string, string> dicConfig = GetConfigList();
            if (dicConfig.ContainsKey(key))
            {
                // 字典中存在，则直接获取
                return dicConfig[key];
            }

            return string.Empty;
        }

        public T GetConfigValue<T>(string key,T defaultValue)
        {
            Dictionary<string, string> dicConfig = GetConfigList();
            if (dicConfig.ContainsKey(key))
            {
                try
                {
                    // 字典中存在，则直接获取
                    return (T)Convert.ChangeType(dicConfig[key], typeof(T));
                }catch
                {
                    return defaultValue;
                }
            }

            return defaultValue;
        }

        private Dictionary<string, string> GetConfigList()
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache[CNT_CONFIG_CACHE_KEY] != null)
            {
                // 缓存有效，直接从缓存中获取
                return (Dictionary<string, string>)objCache[CNT_CONFIG_CACHE_KEY];
            }

            Dictionary<string, string> dicConfig = new Dictionary<string, string>();
            var configList = new SqlDataBase().Select<dynamic>("select [item] ,[value] from rrl_config(nolock)");
            if (configList != null && configList.Count > 0)
            {
                foreach (var configItem in configList)
                {
                    dicConfig.Add(configItem.item, configItem.value);
                }
                // 写缓存
                objCache.Insert(CNT_CONFIG_CACHE_KEY, dicConfig, null, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return dicConfig;
        }
        public static void WriteTextLog(string action, string strMessage, DateTime time)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Log\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".System.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + time.ToString() + "\r\n");
            str.Append("Action:  " + action + "\r\n");
            str.Append("Message: " + strMessage + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
    }

}