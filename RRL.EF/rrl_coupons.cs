//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace RRL.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class rrl_coupons
    {
        public int id { get; set; }
        public int goodsid { get; set; }
        public decimal moeny { get; set; }
        public decimal countmoney { get; set; }
        public int goldbean { get; set; }
        public System.DateTime starttime { get; set; }
        public System.DateTime endtime { get; set; }
        public int uid { get; set; }
        public bool is_used { get; set; }
        public System.DateTime addtime { get; set; }
        public bool is_paid { get; set; }
        public int combine_id { get; set; }
        public string order_code { get; set; }
        public int goldcoin { get; set; }
        public decimal redpacket { get; set; }
        public Nullable<decimal> leastconsume { get; set; }
        public Nullable<int> pay_order_id { get; set; }
        public string goodsname { get; set; }
        public Nullable<int> goods_coupons_id { get; set; }
        public string type { get; set; }
    }
}
