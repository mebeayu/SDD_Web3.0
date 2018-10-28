using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleTask
{
    class Program
    {
        static MessagePush messagePush = new MessagePush();
        static SecKillPush secKillPush = new SecKillPush();
        static void Main(string[] args)
        {
            //messagePush.Start();
            //secKillPush.Start();




            List<TopBottomPoint> points = new List<TopBottomPoint>();

            double start = 5000;
            double end = 80000;
           // double increase_rate = 0.1f;
            //var times =Math.Log(end /  start, (1+ increase_rate) );
            TopBottomPoint currentPoint = new TopBottomPoint() { top = start, bottom = start*(1-0.9), increase_value= start };
            points.Add(currentPoint);
            for (int i = 0; i < 200; i++)
            {
                double increase_value = getRandAddNum();
                double tempTop = currentPoint.bottom+ increase_value;
                double tempbottom = tempTop- getRandSubNum(increase_value);
                if (tempTop >= end)
                    break;
                currentPoint = new TopBottomPoint() {top= tempTop, bottom = tempbottom, increase_value= increase_value };
                points.Add(currentPoint);
            }
            StringBuilder sb = new StringBuilder();
            points.ForEach(v => { sb.Append(v.ToString()); });
            var s=sb.ToString();
            Console.ReadKey();
        }
        static List<double> randomList = new List<double>() { 5000, 20000 };
        static int[] randomIndexArray ={  0,0,0,0,0, 1 };
        static double getRandSubNum(double increase_value)
        {
            return increase_value * 0.9 ;
        }
        static Random r = new Random();
        static double getRandAddNum()
        {
            
            var ix = r.Next(0, randomIndexArray.Length);
            var index = randomIndexArray[ix];
            if(index==1)
            {
                return randomList[index];
            }
            return randomList[index];
        }

        class TopBottomPoint
        {
            public double top { get; set; }
            public double bottom { get; set; }

            public double increase_value { get; set; }
            public override string ToString()
            {
                return this.top+"\t"+this.bottom+"\t";
            }
             
        }
    }
}
