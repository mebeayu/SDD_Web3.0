using RRL.Core.Push;
using RRL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Manager
{
    public class MessageManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public int Insert(sys_msg_message message)
        {
            SqlDataBase db = new SqlDataBase();
            int effect=db.Execute(@"INSERT INTO  [sys_msg_message] ([id], [title], [content], [receiver_id], [is_top], [is_enable], [remark], [viewtimes], [create_time], [create_user_name], [create_user_id], [is_push_message], [is_had_push],[buss_code],[cron_send_time])
    VALUES (@id, @title, @content, @receiver_id, @is_top, @is_enable, @remark, @viewtimes, @create_time, @create_user_name, @create_user_id, @is_push_message, @is_had_push,@buss_code,@cron_send_time)", message);
            return effect;

        }
        /// <summary>
        /// 插入推送信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int InsertPushMessage(int uid,string title,string content,string buss_code=null,DateTime? cron_send_time=null)
        {
            if(!string.IsNullOrWhiteSpace(buss_code))
            {
                SqlDataBase db = new SqlDataBase();
                int effect=db.ExecuteScalar<int>(@"select count(*) from sys_msg_message where buss_code=@buss_code", new { buss_code = buss_code });
                if(effect>0)
                {
                    return 1;
                }

            }
            sys_msg_message message = new sys_msg_message()
            { id=Guid.NewGuid().ToString("N"),
             content=content,
             create_time=DateTime.Now,
             is_enable="1", is_had_push="0", is_push_message="1", is_top="0", receiver_id=uid, title=title, viewtimes=0,
                buss_code= buss_code, cron_send_time= cron_send_time
            };
            return Insert(message);
        }

        public List<sys_msg_message> SelectAllUnPushMessage()
        {
            SqlDataBase db = new SqlDataBase();
            List<sys_msg_message> list = db.Select<sys_msg_message>(@"SELECT
	a.*, b.username AS mobile
FROM
	sys_msg_message a
INNER JOIN rrl_user b ON a.receiver_id = b.id
WHERE
	a.is_push_message = '1'
AND a.is_enable = '1'
AND isnull(a.is_had_push, 0) = '0'
AND (
	cron_send_time IS NULL
	OR (
		cron_send_time > DATEADD(SECOND, - 30, GETDATE())
    and cron_send_time<=DATEADD(SECOND,   30, GETDATE())
	)
)");
            return list;
        }

        public int Upate_Is_Had_Push(string id)
        {
            SqlDataBase db = new SqlDataBase();
            int effect = db.Execute("update sys_msg_message set is_had_push='1' where id=@id", new { id = id });
            return effect;
        }
        JPush  push = new JPush();

        public void SendPushTask()
        {
            
            var list=  SelectAllUnPushMessage();
            foreach (var m in list)
            {
                string mobile=  m.mobile;
                push.SendPushToAlias( m.title,  new string[] { mobile });
                Upate_Is_Had_Push(m.id);
            }
            Console.WriteLine("{1},发送推送信息{0}条", list.Count,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

         
    }

    public class sys_msg_message
    {

        /// <summary>
        ///   主键 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        public string id { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public string title { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public string content { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public int receiver_id { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public string is_top { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public string is_enable { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public string remark { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public int? viewtimes { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public DateTime? create_time { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public string create_user_name { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public string create_user_id { get; set; }
        /// <summary>
        ///是否是推送消息,1=是,0=不是  
        /// </summary>

        public string is_push_message { get; set; }
        /// <summary>
        ///是否已经推送  
        /// </summary>

        public string is_had_push { get; set; }


        public string buss_code { get; set; }

        public string mobile { get; set; }

        public DateTime? cron_send_time { get; set; }
    }
}
