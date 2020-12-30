using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReadCharFromFile.ThreadManage
{
    class ThreadForPool
    {
        public  EventWaitHandle eventWaitHandle { get; private set; } = new AutoResetEvent(false);
        public EventWaitHandle eventSetTask { get; private set; } = new AutoResetEvent(false);
        public EventWaitHandle eventSetParametrs { get; private set; } = new AutoResetEvent(false);

        public Action<object> Function { get; set; }

        public object Parametrs {private get; set;}

        public bool IsFree { get; private set; } = true;

        public ThreadForPool(Action<object> function)
        {
            this.Function = function;
        }

        public void StartThread()
        {
            while (true)
            {
                eventSetParametrs.WaitOne();
                eventSetTask.WaitOne();
                eventWaitHandle.WaitOne();
                while(Parametrs ==null && Function == null)
                {
                    Thread.Sleep(10);
                }
                IsFree = false;
                Function.Invoke(Parametrs);
                Parametrs = null;
                Function = null;
                IsFree = true;
            }

        }

    }
}
