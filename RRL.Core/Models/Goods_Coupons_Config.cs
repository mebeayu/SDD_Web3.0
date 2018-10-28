using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Models
{
    public class Goods_Coupons_Config
    {
        public string id { get; set; }
        public int minMoney { get; set; }
        public int maxMoney { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }

    }
}
