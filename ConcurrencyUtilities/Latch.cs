using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
    public class Latch
    {
        private Semaphore lSemaphore;
        public Latch()
        {
            lSemaphore = new Semaphore(0);
        }

        public void Acquire()
        {
            lSemaphore.Acquire();
            lSemaphore.Release();
        }

        public void Release()
        {
            lSemaphore.Release();
        }
    }
}
