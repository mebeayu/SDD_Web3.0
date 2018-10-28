using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Models
{


    public class rrl_sddcard_order
    {

        /// <summary>
        ///id   主键 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        public string id { get; set; }
        /// <summary>
        ///订单号  
        /// </summary>

        public int order_id { get; set; }
        /// <summary>
        ///订单商品id  
        /// </summary>

        public int order_info_goods_id { get; set; }
        /// <summary>
        ///兜兜卡号  
        /// </summary>

        public string code { get; set; }
        /// <summary>
        ///金额  
        /// </summary>

        public decimal money { get; set; }
        /// <summary>
        ///购买者id  
        /// </summary>

        public int uid { get; set; }
        /// <summary>
        ///是否查看[0=没查看,1=查看]  
        /// </summary>

        public bool is_view { get; set; }
        /// <summary>
        ///购买时间  
        /// </summary>

        public DateTime addtime { get; set; }
        /// <summary>
        ///查看时间  
        /// </summary>

        public DateTime viewtime { get; set; }
        /// <summary>
        ///是否被使用[0=没,1=有]  
        /// </summary>

        public bool is_used { get; set; }




    }

}
