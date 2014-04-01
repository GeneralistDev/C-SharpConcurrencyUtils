using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrencyUtils
{
    public class Mutex: Semaphore
    {
        public Mutex(): base(1) { }

        public override void Release(ulong n)
        {
            lock (this)
            {
                if (n > 1 || base.tokens > 0)
                {
                    throw new System.ArgumentException("A mutex cannot contain more than 1 token");
                }
                else
                {
                    base.tokens = 1;
                    Monitor.PulseAll(this);
                }
            }
        }
    }
}
