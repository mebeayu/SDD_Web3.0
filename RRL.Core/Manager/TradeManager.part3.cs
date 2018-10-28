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
        public int GetOrderSharedTimes(int order_id)
        {
            string sql = "select count(*) from rrl_order_shareddiscount where orderid=@id";
            SqlDataBase db = new SqlDataBase();
            int count=db.ExecuteScalar<int>(sql, new { id = order_id });
            return count;
        }
    }
}
