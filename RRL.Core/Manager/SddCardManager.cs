using RRL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Manager
{
    public class SddCardManager
    {
        public List<dynamic> get_sddcard_by_uid(int uid)
        {
            var db= new SqlDataBase();
            List<dynamic> list= db.Select<dynamic>($@"select * from v_sddcard_owner where uid=@uid and is_used=0");
            return list;
        }

        public int add_sddcard_by_code(string code)
        {
            var db = new SqlDataBase();
            var sql = @"INSERT INTO  [rrl_sddcard_order] ([id], [order_id],[order_info_goods_id], [code], [money], [uid], [is_view], [addtime], [viewtime], [is_used])
 VALUES (@id, @order_id,@order_info_goods_id, @code, @money, @uid, @is_view, @addtime, @viewtime, @is_used)";
            int effect = db.Execute(sql, new RRL.Core.Models.rrl_sddcard_owner { }); 
            return effect;
        }



    }
}
