using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Common.Utils
{
    public class CycleTimer
    {

        #region 变量&属性


        private int _second = 60;
        /// <summary>
        /// 秒钟
        /// </summary>
        public int Second
        {
            get { return _second; }
            set { _second = value; }
        }

        private Thread _clockThread = null;



        #endregion

        #region 事件

        /// <summary>
        /// 时间到就触发的时间
        /// </summary>
        public event Action TimeComing;
        #endregion

        #region 构造函数

        public CycleTimer()
        {
            _clockThread = new Thread(new ThreadStart(Run));
            _clockThread.IsBackground = true;
            _clockThread.Name = "循环触发的线程";
        }

        public CycleTimer(int second) : this()

        {
            this.Second = second;
        }

        #endregion

        #region 开始
        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            if (_clockThread != null)
            {

                _clockThread.Start();
            }
        }
        #endregion

        #region 线程运行

        /// <summary>
        /// 线程运行
        /// </summary>
        private void Run()
        {
            while (true)
            {
                if (TimeComing != null)
                {
                    try
                    {
                        DateTime beginTime = DateTime.Now;
                        TimeComing();
                        DateTime endTime = DateTime.Now;
                        int sxs = (int)(this.Second - (endTime - beginTime).TotalSeconds) * 1000;
                        Thread.Sleep(Math.Max(0, sxs));
                    }
                    catch { }
                }
            }

        }


        #endregion
    }


}