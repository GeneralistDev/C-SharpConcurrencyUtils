using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
	/// <summary>
	/// Concurrency Barrier. Threads rendezvous until the size of the barrier is
	/// reached and the last one opens the barrier to allow them all to continue.
	/// 
	/// Author: Daniel Parker 971328X
	/// </summary>
    public class Barrier
    {
        public Semaphore ReleaseSemaphore;
        public Mutex TS;
        public UInt32 barrierSize;
        public UInt32 count;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConcurrencyUtils.Barrier"/> class.
		/// </summary>
		/// <param name="barrierStartSize">Barrier start size.</param>
        public Barrier(UInt32 barrierStartSize)
        {
            ReleaseSemaphore = new Semaphore();
            TS = new Mutex();
            barrierSize = barrierStartSize;
        }

		/// <summary>
		/// Arrive at the barrier. When the barrier is full all threads continue.
		/// </summary>
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
