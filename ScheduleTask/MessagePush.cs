using Common.Utils;
using RRL.Core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleTask
{
    public class MessagePush
    {

        MessageManager manager = new MessageManager();
        public void Start()
        {
                CycleTimer timer = new CycleTimer();
                timer.Second = 2;
                timer.TimeComing += Timer_TimeComing;
                timer.Start();
        }

        private  void Timer_TimeComing()
        {
            manager.SendPushTask();
        }
    }
}
