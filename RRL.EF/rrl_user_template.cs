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
    
    public partial class rrl_user_template
    {
        public int id { get; set; }
        public Nullable<System.DateTime> addtime { get; set; }
        public string username { get; set; }
        public string real_name { get; set; }
        public byte sex { get; set; }
        public string area_code { get; set; }
        public int head_pic { get; set; }
        public int spreader_uid { get; set; }
        public int default_receive { get; set; }
        public decimal total_cash { get; set; }
        public decimal plate_to_return_money { get; set; }
        public decimal total_plate_to_return_money { get; set; }
        public int point { get; set; }
        public decimal r_money { get; set; }
        public decimal x_money { get; set; }
        public decimal y_money { get; set; }
        public string open_id { get; set; }
        public string long_time_token { get; set; }
        public System.DateTime long_time_token_expir { get; set; }
        public string short_time_token { get; set; }
        public System.DateTime short_time_token_expir { get; set; }
        public decimal total_recomend_money { get; set; }
    }
}
