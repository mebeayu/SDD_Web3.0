using Dapper;
using RRL.Core.External;
using RRL.Core.Models;
using RRL.Core.Models.WxJSAPI;
using RRL.Core.Pay;
using RRL.Core.Tables;
using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RRL.Core.Manager
{
    public partial class TradeManager
    {
        /// <summary>
        /// 推送还查5分钟未支付的订单
        /// </summary>
        public void Push_SendSecKillUnBuy()
        {
            SqlDataBase db = new SqlDataBase();
            var sql = @"SELECT a.order_code,a.uid,a.goods_name from v_v3_order_price a  
where a.goods_type=881 and DATEDIFF(MINUTE, a.addtime, GETdate())>=23 and DATEDIFF(MINUTE, a.addtime, GETdate())<=25";
            List<dynamic> list= db.Select<dynamic>(sql);
            MessageManager messageManager = new MessageManager();
            if(list!=null &&list.Count>0)
            {
                foreach (var item in list)
                {
                    messageManager.InsertPushMessage(item.uid, string.Format("您秒杀的商品 {0} 即将失效!",item.goods_name)  , "您抢购的秒杀商品库存紧张，订单将于5分钟后自动取消，如需购买请进入【我的】->【待付款】订单进行支付", item.order_code);
                }
                Console.WriteLine("{1},秒杀的商品推送信息{0}条", list.Count, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            Console.WriteLine("{1},秒杀的商品推送信息{0}条", 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));


        }
    }
}
