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
    
    public partial class ORDER_GOODS_OUT_VIEW
    {
        public int id { get; set; }
        public string 订单编码 { get; set; }
        public int 用户id { get; set; }
        public decimal 订单金额 { get; set; }
        public string 付款人 { get; set; }
        public string 省份 { get; set; }
        public string 市 { get; set; }
        public string 区 { get; set; }
        public string 收货地址 { get; set; }
        public string 付款人电话 { get; set; }
        public Nullable<System.DateTime> 发货时间 { get; set; }
        public string 商品名称 { get; set; }
        public Nullable<int> 购买数量 { get; set; }
        public Nullable<decimal> 购买价格 { get; set; }
        public int status { get; set; }
        public Nullable<System.DateTime> addtime { get; set; }
    }
}