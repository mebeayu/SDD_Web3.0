using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Models
{
    public class ApplyPayV3_SumPay
    {
        /// <summary>
        /// 支付的总价钱
        /// </summary>
        public decimal sum_money { get; set; }
        /// <summary>
        /// 支付的金币总数
        /// </summary>
        public decimal sum_pay_gold_coin { get; set; }

        /// <summary>
        /// 支付总价钱
        /// </summary>
        public decimal sum_need_pay_money { get; set; }

        /// <summary>
        /// 支付的金豆数
        /// </summary>
        public decimal sum_need_pay_beans { get; set; }

        /// <summary>
        /// 支付的券总价钱
        /// </summary>
        public decimal sum_user_coupons_countmoney { get; set; }

        /// <summary>
        /// 最大订单号
        /// </summary>
        public decimal max_ordercode { get; set; }

        /// <summary>
        /// 总邮费
        /// </summary>
        public decimal sum_postage { get; set; }

        /// <summary>
        /// 是否   1=金豆(+钱)支付,0=纯现金
        /// </summary>
        public string is_beans_pay { get; set; }

    }
}
