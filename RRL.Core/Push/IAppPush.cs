using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Push
{
    public interface IAppPush
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="title"></param>
        /// <param name="moblies"></param>
        void SendPushToAlias(  string title, string[] moblies);
    }
}
