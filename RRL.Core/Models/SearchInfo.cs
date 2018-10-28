using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Models
{
  public  class SearchInfo
    {
        /// <summary>
        /// 商品标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 店铺类别:0.自营店，1:入驻店
        /// </summary>
        public int shop_type { get; set; }

        /// <summary>
        /// 平台价格
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal local_price { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public int goods_type { get; set; }

        /// <summary>
        /// 商品简介
        /// </summary>
        public string propaganda { get; set; }

        /// <summary>
        /// 商品图
        /// </summary>
        public long pic_id { get; set; }

        /// <summary>
        /// 商品返还率
        /// </summary>
        public string return_money_rate { get; set; }

        public string goods_type_name { get; set; }
        /// <summary>
        /// 兑换所需金币数
        /// </summary>
        public decimal discount { get; set; }

        public string category_name1 { get; set; }
        public string category_name2 { get; set; }
        public string category_name3 { get; set; }

        /// <summary>
        /// 钱支付的
        /// </summary>
        public decimal cash_price { get; set; }
        /// <summary>
        /// 豆支付的钱
        /// </summary>
        public decimal beans_price { get; set; }

    }
}
