using Jiguang.JPush;
using Jiguang.JPush.Model;
using RRL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL.Core.Push
{
    public class JPush:IAppPush
    {
        private static string AppKey = "9f8382bfa042ea37a6706f11";
        private static string MasterSecret = "4a9767bfe33fd5820b7a73f1";
        private static JPushClient client = new JPushClient(AppKey, MasterSecret);

    
        public void SendPushToAlias(string title,  string[] moblies)
        {
           
            client.SendPush(new PushPayload()
            {
                Platform = "all", 
                Audience = new { alias = moblies },
                Notification = new Notification() { Alert = title, Android = new Android() { Alert= title }, IOS = new IOS() { Alert= title ,Badge="1"  } }
                , Options=new Options() {  IsApnsProduction=true}
            });

        
        }

      


    }
}
