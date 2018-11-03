using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RRL.WEB.Models
{
    public class Order
    {
        public string Token { get; set; }
        public int id { get; set; }
        public string ordercode { get; set; }
        public int buyer_id { get; set; }
        public double money { get; set; }
        public int status { get; set; }
        public decimal pay_bean { get; set; }
        public decimal pay_money { get; set; }
        public int spreader_uid { get; set; }
        public int order_type { get; set; }//970
    }
}