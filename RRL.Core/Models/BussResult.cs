using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Models
{
    /// <summary>
    /// 业务结果
    /// </summary>
    public class BussResult
    {
        public int status { get; set; } = 0;

        /// <summary>
        /// 消息信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 结果数据集
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public object additional_data { get; set; }

        /// <summary>
        /// 其他附加数据
        /// </summary>
        public object add_data { get; set; }
    }
}
