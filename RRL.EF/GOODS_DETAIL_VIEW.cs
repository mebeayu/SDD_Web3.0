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
    
    public partial class GOODS_DETAIL_VIEW
    {
        public int shop_id { get; set; }
        public string shop_name { get; set; }
        public int shop_type { get; set; }
        public Nullable<int> shop_fav_count { get; set; }
        public int id { get; set; }
        public int goods_type { get; set; }
        public string name { get; set; }
        public Nullable<int> goods_fav_count { get; set; }
        public decimal price { get; set; }
        public decimal local_price { get; set; }
        public string propaganda { get; set; }
        public int sell_count { get; set; }
        public int inv_count { get; set; }
        public bool sell_count_visible { get; set; }
        public bool inv_count_visible { get; set; }
        public long pic_id { get; set; }
        public string goods_info_html { get; set; }
        public string return_money_rate { get; set; }
        public bool is_bad_refund { get; set; }
        public bool is_can_not_refund { get; set; }
        public bool is_quick_refund { get; set; }
        public bool is_real { get; set; }
        public bool is_unuse_refund { get; set; }
        public int rerund_data { get; set; }
        public string specification { get; set; }
        public decimal postage { get; set; }
        public decimal discount { get; set; }
    }
}
