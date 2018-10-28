using RRL.Core.Utility;
using RRL.DB;
using System.Data.SqlClient;

namespace RRL.Core.Manager
{
    public class AuthManager
    {
        public int RecordSMS(string mobile, string code)
        {
            RRLDB db = new RRLDB();
            int res = db.ExeCMD(SqlExeUtility.RECORD_SMS_EXEC_STR,
                new SqlParameter("@mobile", mobile),
                new SqlParameter("@code", code));
            db.Close();
            if (res < 0)
                return MessageCode.ERROR_EXECUTE_SQL;
            if (res == 0)
                return MessageCode.ERROR_UNKONWN;
            return MessageCode.SUCCESS;
        }
    }
}