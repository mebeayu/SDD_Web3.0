using System.Web.UI;

namespace Common.Utils
{
    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public class ScriptUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string Alert(string txt)
        {
            return "alert(\"" + FormatUtil.HtmlEncode(txt) + "\");";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string Reload()
        {
            return "window.location.reload();";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string SelfReload()
        {
            return "self.location.reload();";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string TopReload()
        {
            return "top.location.reload();";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static string HistoryGo(int go)
        {
            return "history.go(" + go + ");";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static string HistoryBack()
        {
            return "history.back();";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static string HistoryBack(int timeout)
        {
            return "setTimeout(function(){history.back();}," + timeout + ");";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static string SelfHref(string link)
        {
            return "self.location.href=\"" + link + "\";";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static string TopHref(string link)
        {
            return "top.location.href=\"" + link + "\";";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="ifScript"></param>
        /// <param name="elseScript"></param>
        /// <returns></returns>
        public static string Confirm(string txt, string ifScript, string elseScript)
        {
            return "if(confirm(\"" + FormatUtil.HtmlEncode(txt) + "\")){" + ifScript + "}else{" + elseScript + "}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetValue(string id, string value)
        {
            return "document.getElementById(\"" + id + "\").value=\"" + FormatUtil.HtmlEncode(value) + "\";";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string SetInnerHTML(string id, string html)
        {
            return "document.getElementById(\"" + id + "\").innerHTML=\"" + FormatUtil.HtmlEncode(html) + "\";";
        }
    }
}
