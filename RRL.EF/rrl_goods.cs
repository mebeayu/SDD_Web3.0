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
    
    public partial class rrl_goods
    {
        public int id { get; set; }
        public int addtime { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal local_price { get; set; }
        public string propaganda { get; set; }
        public int uid { get; set; }
        public int sell_count { get; set; }
        public int inv_count { get; set; }
        public long pic_id { get; set; }
        public decimal evaluate_point { get; set; }
        public int evaluate_count { get; set; }
        public decimal evaluate_point_sum { get; set; }
        public int buy_limit { get; set; }
        public Nullable<System.DateTime> deletemark { get; set; }
        public int sid { get; set; }
        public string goods_info { get; set; }
        public string goods_info_html { get; set; }
        public int fee_id { get; set; }
        public int goods_type { get; set; }
        public bool sell_count_visible { get; set; }
        public bool inv_count_visible { get; set; }
        public string specification { get; set; }
        public decimal postage { get; set; }
        public decimal return_money_discount { get; set; }
        public decimal discount_income_rate { get; set; }
        public bool is_real { get; set; }
        public bool is_quick_refund { get; set; }
        public bool is_bad_refund { get; set; }
        public int rerund_data { get; set; }
        public bool is_unuse_refund { get; set; }
        public bool is_can_not_refund { get; set; }
        public int order_weight { get; set; }
        public int recommend_type { get; set; }
        public string supplier_code { get; set; }
        public Nullable<decimal> goods_supply_price { get; set; }
        public string source { get; set; }
        public string source_url { get; set; }
        public string goods_sku { get; set; }
    }
}
