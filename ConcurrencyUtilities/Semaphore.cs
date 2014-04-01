using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrencyUtils
{
    public class Semaphore
    {
        protected UInt64 tokens;
        private readonly Object lockObject = new Object();

        public Semaphore()
        {
            new Semaphore(0);
        }

        public Semaphore(UInt64 nTokens)
        {
            tokens = nTokens;
        }

        public void Acquire()
        {
            lock (lockObject)
            {
                while (tokens == 0)
                {
                    Monitor.Wait(lockObject);
                }
                tokens--;
            }
        }

        public void Release()
        {
            Release(1);
        }

        public virtual void Release(UInt64 n)
        {
            lock (lockObject)
            {
                tokens += n;
                Monitor.PulseAll(lockObject);
            }
        }
    }
}
