using ReadCharFromFile.ThreadManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReadCharFromFile.FileProcess
{
    class OwnThreadPool
    {
        public int NumberOfThread { get; set; }

        private List<ThreadForPool> threads;

        public Action<Object> Action { get; set; }

        public OwnThreadPool(Action<Object> action)
        {
            this.Action = action;
            threads = new List<ThreadForPool>();
        }

        public bool IsHavingFreeThread()
        {
            if(threads.Count == 0 || threads.Count<= NumberOfThread)
            {
                var temp = new ThreadForPool(Action);
                threads.Add(temp);
                new Thread(temp.StartThread).Start();
                return true;
            }
            
            foreach(var elem in threads)
            {
                if (elem.IsFree)
                    return true;
            }
            return false;
        }


        public void StartNewTask(Action<Object> action, object parametrs)
        {
            foreach(var elem in threads)
            {
                if (elem.IsFree)
                {
                    elem.Parametrs = parametrs;
                    elem.eventSetParametrs.Set();
                    elem.Function = action;
                    elem.eventSetTask.Set();
                    elem.eventWaitHandle.Set();
                    break;
                }
            }
        }

    }
}
