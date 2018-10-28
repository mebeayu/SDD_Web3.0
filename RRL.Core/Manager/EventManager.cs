using RRL.DB;

namespace RRL.Core.Manager
{
    public class EventManager
    {
        /// <summary>
        /// 记录活动
        /// </summary>
        /// <param name="str">记录文本</param>
        /// <param name="code">活动编码</param>
        /// <returns>记录结果</returns>
        public int EventMark(string str, int code)
        {
            RRLDB db = new RRLDB();
            int res = db.ExeCMD($@"INSERT INTO event ([text],[event]) VALUES('{str}',{code})");
            db.Close();
            return res;
        }
    }
}