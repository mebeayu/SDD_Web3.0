using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RRL.Core.Manager
{
    public class HelpManager
    {
        public static void Mark(string str)
        {
            var db = new RRLDB();
            db.ExeCMD($@"INSERT INTO temp ([text]) VALUES('{str}')");
            db.Close();
        }

        public static void MarkPageView(string route_info)
        {
           /* RRLDB db = new RRLDB();
            db.ExecSP(@"sp_PAGE_VIEW_LOG_ADD", new SqlParameter("@route_info", route_info));
            db.Close();
            */
        }

        public static string MarkTransBody(string order_code, string encrypt_body)
        {
            RRLDB db = new RRLDB();
            int res = db.ExecSP(@"sp_TRANS_BODY_ADD",
                new SqlParameter("@order_code", order_code),
                new SqlParameter("@encrypt_body", SqlDbType.Text, 18) { Value = encrypt_body });
            int record_id = Convert.ToInt32(db.sm.Parameters["@return"].Value);
            if (res <= 0)
            {
                record_id = 0;
            }
            db.Close();
            return record_id.ToString();
        }

        public static string GetTransBody(string record_id, string order_code)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(string.Format(@"SELECT encrypt_body FROM rrl_trans_body_record WHERE id = {0} AND order_code = {1}", record_id, order_code));
            string decrypt_body = DES.DecryptDES(ds.Tables[0].Rows[0]["encrypt_body"].ToString());
            db.Close();
            return decrypt_body;
        }
    }
}