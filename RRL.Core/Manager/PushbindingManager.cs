using RRL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Manager
{
    public class PushbindingManager
    {
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int PushBinding(object obj)
        {
            SqlDataBase db = new SqlDataBase();
            string sql = @"INSERT INTO  [sys_msg_pushbinding]
([id], [tag], [registration_id], [alias], [uid], [platform], [last_modify_time])
 VALUES(@id, @tag, @registration_id, @alias, @uid, @platform, @last_modify_time)";
            int effect = db.Execute(sql, obj);
            if (effect == 0)//插入失败,那么就更新
            {
                string sql_str = "UPDATE [sys_msg_pushbinding] SET [tag]=@tag,[alias]=@alias, [uid]=@uid, [platform]=@platform, [last_modify_time]=getdate() WHERE  [registration_id]=@registration_id";
                effect = db.Execute(sql_str, obj);
            }
            return effect;
        }
    }
}
