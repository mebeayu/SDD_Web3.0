using RRL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Manager
{
    public class SkillRemindManager
    {
        public List<dynamic> selectAllUnAddPushMsgData()
        {
            var sql = @"select * from rrl_skill_remind where isnull(is_add_push_msg,'0')='0' and datediff(minute,getdate(),online_time)<=10";
            SqlDataBase db = new SqlDataBase();
            var list=db.Select<dynamic>(sql);
            return list;
        }


        public int Update_had_add_push_msg()
        {
            var sql = @"update rrl_skill_remind set is_add_push_msg='1' where isnull(is_add_push_msg,'0')='0'";
            SqlDataBase db = new SqlDataBase();
            return db.Execute(sql);
        }


    }
}
