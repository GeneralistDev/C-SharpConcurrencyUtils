using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
    public class Barrier
    {
        public Semaphore ReleaseSemaphore;
        public Mutex TS;
        public UInt32 barrierSize;
        public UInt32 count;

        public Barrier(UInt32 barrierStartSize)
        {
            ReleaseSemaphore = new Semaphore(0);
            TS = new Mutex();
            barrierSize = barrierStartSize;
        }

        public bool Arrive()
        {
            TS.Acquire();
            lock (this)
            {
                count++;
                if (count == barrierSize)
                {
                    count--;
                    ReleaseSemaphore.Release(barrierSize - 1);
                    return true; // The TS does not get released here because the thread exits all together.
                }
            }

            TS.Release();
            ReleaseSemaphore.Acquire();

            lock (this)
            {
                count--;
                if (count == 0)
                {
                    TS.Release();
                }
            }
            return false;
        }
    }
}
