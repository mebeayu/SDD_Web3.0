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
    
    public partial class rrl_user
    {
        public int id { get; set; }
        public string username { get; set; }
        public string real_name { get; set; }
        public byte sex { get; set; }
        public string area_code { get; set; }
        public string head_pic { get; set; }
        public int spreader_uid { get; set; }
        public Nullable<decimal> total_cash { get; set; }
        public Nullable<decimal> plate_to_return_money { get; set; }
        public int point { get; set; }
        public string open_id { get; set; }
        public System.DateTime addtime { get; set; }
        public int default_receive { get; set; }
        public decimal total_plate_to_return_money { get; set; }
        public decimal r_money { get; set; }
        public decimal x_money { get; set; }
        public decimal y_money { get; set; }
        public string long_time_token { get; set; }
        public System.DateTime long_time_token_expir { get; set; }
        public string short_time_token { get; set; }
        public System.DateTime short_time_token_expir { get; set; }
        public decimal total_recomend_money { get; set; }
        public string spreader_code { get; set; }
        public int plate_to_return_weight { get; set; }
        public decimal y_money_frz { get; set; }
        public decimal ex_plate_to_return_money { get; set; }
        public bool if_info_has_read { get; set; }
        public bool is_can_spreader { get; set; }
        public string wx_open_id { get; set; }
        public string wx_mp_open_id { get; set; }
        public int one_yuan_remark { get; set; }
        public bool is_shop_keeper { get; set; }
        public int recommand_count { get; set; }
        public int roulette_count { get; set; }
        public int roulette_win_count { get; set; }
        public decimal h_money { get; set; }
        public decimal h_money_free { get; set; }
        public decimal h_money_free_frz { get; set; }
        public System.DateTime h_money_free_frz_expire { get; set; }
        public bool has_received_free_money_default { get; set; }
        public bool has_received_spreader_h_money { get; set; }
        public bool has_received_daily_free_h_money { get; set; }
        public int daily_free_h_money_count { get; set; }
        public string the_name { get; set; }
        public string id_code { get; set; }
        public System.DateTime last_spreader_h_money_time { get; set; }
        public System.DateTime last_random_h_money_time { get; set; }
        public string spreader_route { get; set; }
        public int spreader_count { get; set; }
        public string device_code { get; set; }
        public bool has_received_h_money_five_group_spreade { get; set; }
        public Nullable<decimal> rnd_pay_redpacket { get; set; }
        public Nullable<System.DateTime> rnd_pay_redpacket_expire { get; set; }
        public Nullable<decimal> h_money_pay { get; set; }
        public string is_locked_login { get; set; }
        public string is_locked_trade { get; set; }
    }
}
