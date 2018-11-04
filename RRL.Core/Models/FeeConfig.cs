using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Models
{
    public class FeeConfig
    {
        public double buyer_recommend_award { get; set; }
    }
    public class Spreader
    {
        public int spreader_uid { get; set; }
        public string spreader_expire { get; set; }
    }
}
