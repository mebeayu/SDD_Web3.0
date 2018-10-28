using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Utils
{
    public class FileHelper
    {
        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        public static bool CheckRemoteFile(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            try
            {
               
                myRequest.Method = "HEAD";　              //设置提交方式可以为＂get＂，＂head＂等
                myRequest.Timeout = 10000;　             //设置网页响应时间长度
                myRequest.AllowAutoRedirect = false;//是否允许自动重定向
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                var ccc = myResponse.StatusCode;
                myResponse.Close();
                return (ccc == HttpStatusCode.OK);
            }
           catch
            {
              
               return false;
            }
        }


        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 获取指定文件的扩展名
        /// </summary>
        /// <param name="fileName">指定文件名</param>
        /// <returns>扩展名</returns>
        public static string GetFileExtName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName.IndexOf('.') <= 0)
                return "";

            fileName = fileName.ToLower().Trim();

            return fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
        }
    }
}
