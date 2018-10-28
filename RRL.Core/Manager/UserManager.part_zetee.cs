// PROJECT #8 lcl 20180425 Insert

using System;
using System.Collections.Generic;
using System.Linq;

using RRL.Config;
using RRL.DB;

namespace RRL.Core.Manager
{
    public partial class UserManager
    { 
        /// <summary>
        /// 分享订单得优惠 2018年8月10日20:48:36
        /// </summary>
        /// <param name="orderid_str"></param>
        /// <returns></returns>
        public int insert_rrl_order_shareddiscount(string orderid_str)
        {
            if (string.IsNullOrWhiteSpace(orderid_str))
                return 99;
            int orderid = 0;
            orderid_str = orderid_str.Split(',')[0];
            if (!int.TryParse(orderid_str, out orderid)) { return 99; }
            SqlDataBase db = new SqlDataBase();
            var list= db.ExecuteStoredProcedure<int?>("sp_V3_order_shareddiscount", new { orderid = orderid });
            return 0;
        }
    }
}