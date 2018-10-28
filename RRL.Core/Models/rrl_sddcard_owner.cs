using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Models
{
    public class rrl_sddcard_owner
    {

        /// <summary>
        ///id   主键 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        public string id { get; set; }
        /// <summary>
        ///兜兜卡订单号  
        /// </summary>

        public string sddcard_order_id { get; set; }
        /// <summary>
        ///金额  
        /// </summary>

        public decimal money { get; set; }
        /// <summary>
        ///拥有者id  
        /// </summary>

        public int uid { get; set; }
        /// <summary>
        ///添加时间  
        /// </summary>

        public DateTime addtime { get; set; }
        /// <summary>
        ///是否被使用[0=没,1=有]  
        /// </summary>

        public bool is_used { get; set; }
        /// <summary>
        ///被支付的订单id  
        /// </summary>

        public int pay_order_id { get; set; }




    }
}
