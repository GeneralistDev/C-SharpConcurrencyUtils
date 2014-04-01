using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
    public class LightSwitch
    {
        readonly Semaphore lSemaphore;
        private Mutex m;
        private int count;

        public LightSwitch(Semaphore s)
        {
            lSemaphore = s;
            m = new Mutex();
        }

        public void Acquire()
        {
            lock (this)
            {
                if (count == 0)
                {
                    m.Acquire();
                }
                count++;
            }
        }

        public void Release()
        {
            count--;
            if (count == 0)
            {
                m.Release();
            }
        }
    }
}
