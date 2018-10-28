using System;
using System.Data.OleDb;
using System.IO;
using System.Web.UI;

namespace Common.Utils
{
    /// <summary>
    /// 记录模式
    /// Powered by Rocby.com
    /// </summary>
    public enum LogUtilMode
    {
        /// <summary>
        /// 文件
        /// </summary>
        File,
        /// <summary>
        /// 数据库
        /// </summary>
        Database,
        /// <summary>
        /// 浏览器控制台
        /// </summary>
        Console
    }

    /// <summary>
    /// 日志配置
    /// </summary>
    public class LogUtilConfig
    {
        /// <summary>
        /// 开启日志记录
        /// </summary>
        public bool LogSwitch { get; set; }

        /// <summary>
        /// 记录模式
        /// </summary>
        public LogUtilMode Mode { get; set; }

        /// <summary>
        /// 文件模式：保存文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 数据模式：数据库连接（仅支持Access与SQLServer）
        /// </summary>
        public string DbConnection { get; set; }

        /// <summary>
        /// 控制台模式：与前端脚本上下文关联的控制类
        /// </summary>
        public Page ControlPage { get; set; }
    }

    /// <summary>
    /// 日志工具
    /// </summary>
    public class LogUtil
    {
        LogUtilConfig _cfg = new LogUtilConfig();

        public LogUtil(LogUtilConfig cfg)
        {
            _cfg = cfg;
        }

        public void Write(string msg)
        {
            if (_cfg.LogSwitch)
            {
                string log = string.Format("{0} ({1})\r\n", msg, DateTime.Now);
                switch (_cfg.Mode)
                {
                    case LogUtilMode.File:
                        {
                            if (File.Exists(_cfg.FilePath)) { File.WriteAllText(_cfg.FilePath, File.ReadAllText(_cfg.FilePath) + log); }
                            else { File.WriteAllText(_cfg.FilePath, log); }
                            break;
                        }
                    case LogUtilMode.Database:
                        {
                            using (OleDbConnection db = new OleDbConnection(_cfg.DbConnection))
                            {
                                db.Open();
                                using (OleDbCommand cmd = db.CreateCommand())
                                {
                                    cmd.CommandText = "INSERT INTO [SysLog] ([LogMsg], [LogTime]) VALUES (@LogMsg, @LogTime)";
                                    cmd.Parameters.Add(new OleDbParameter("@LogMsg", msg));
                                    cmd.Parameters.Add(new OleDbParameter("@LogTime", DateTime.Now));
                                    cmd.ExecuteNonQuery();
                                }
                                db.Close();
                            }
                            break;
                        }
                    case LogUtilMode.Console:
                        {
                            _cfg.ControlPage.ClientScript.RegisterClientScriptBlock(null, null, "console.log(\"" + FormatUtil.HtmlEncode(log) + "\");", true);
                            break;
                        }
                }
            }
        }
    }
}