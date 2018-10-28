using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Tables
{
    public class RRL_Shop_Cart
    {
        /// <summary>
		///   主键 
		/// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        public int id { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public int uid { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public int goods_id { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public int goods_count { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public bool isUsed { get; set; }
        /// <summary>
        ///  
        /// </summary>

        public DateTime? deletemark { get; set; }
    }
}
