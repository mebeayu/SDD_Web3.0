using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RRL.WEB.Models
{
    public class PayNotifyModel
    {


        /// <summary>
        /// 是否支付成功
        /// </summary>
        public bool IsPaySuccess { get; set; }


        /// <summary>
        /// 附加数据
        /// </summary>
        public string AttachData { get; set; }


        /// <summary>
        /// 支付标题(商品描述)
        /// </summary>
        public string Subject_Title { get; set; }

        /// <summary>
        /// 总费用,(微信)精确到分,如:1元=100分,这里填100,阿里精确到元=1.00
        /// </summary>
        public int Total_Fee { get; set; }

        /// <summary>
        /// 外部订单号(商户的订单号) 
        /// </summary>
        public string Out_Trade_No { get; set; }

        /// <summary>
        /// 交易id,微信或支付宝那边的交易id
        /// </summary>
        public string Transaction_id { get; set; }

        public string Notify_id { get; set; }

        /// <summary>
        /// 第三方支付类型 WX_JSAPI=微信公众号支付、WX_APP=微信app支付,阿里支付=ALIPAY_APP
        /// </summary>
        public string Three_Pay_Type { get; set; }

        /// <summary>
        /// 完成交易时间
        /// </summary>
        public DateTime Completed_Trans_Time { get; set; }

    }
}