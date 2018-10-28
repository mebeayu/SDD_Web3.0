using Common.Utils;
using RRL.Core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleTask
{
    public class SecKillPush
    {

        TradeManager manager = new TradeManager();
        public void Start()
        {
                CycleTimer timer = new CycleTimer();
                timer.Second = 60;
                timer.TimeComing += Timer_TimeComing;
                timer.Start();
        }

        private  void Timer_TimeComing()
        {
            //manager.SendPushTask();
            manager.Push_SendSecKillUnBuy();
        }
    }
}
