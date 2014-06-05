using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
    /// <summary>
    ///     First in first out Semaphore. A semaphore that retains 
	/// 	order of acquires so as to avoid starvation of threads.
	/// 
	/// 	Author: Daniel Parker 971328X
    /// </summary>
    public class FIFOSemaphore: Semaphore
    {
        private Queue<ConcurrencyUtils.Semaphore> waitingThreadsQueue = new Queue<ConcurrencyUtils.Semaphore>();
        private readonly Object queueLock = new Object();

        /// <summary>
        ///     Public constructor that calls the internal 'Release'
        ///     method to correctly add tokens to the semaphore.
        /// </summary>
        /// <param name="n"></param>
        public FIFOSemaphore(UInt64 n = 0)
        {
            Release(n);
        }

        /// <summary>
        ///     The acquiring thread creates a semaphore of it's own and adds it to the queue. 
        ///     The thread then tries to acquire from that semaphore. Once acquired, the thread can acquire a token.
        /// </summary>
        public override void Acquire()
        {
            Semaphore threadSemaphore;
            lock(queueLock)
            {
                threadSemaphore = new Semaphore(0);
				waitingThreadsQueue.Enqueue (threadSemaphore);
            }

            threadSemaphore.Acquire();
            base.Acquire();
        }

        /// <summary>
        ///     Release n tokens. If there are threads waiting, then dequeue and signal them in order.
        /// </summary>
        /// <param name="n">The number of tokens to release.</param>
        public override void Release(UInt64 n = 1)
        {
            base.Release(n);
            lock (queueLock)
            {
                if (waitingThreadsQueue.Count > 0)
                {
                    UInt64 queueSize = UInt64.Parse(waitingThreadsQueue.Count.ToString());
                    if (queueSize <= n)
                    {
                        for (UInt64 i = 0; i < n; i++)
                        {
                            Semaphore aThreadSemaphore = waitingThreadsQueue.Dequeue();
                            aThreadSemaphore.Release();
                        }
                    }
                    else
                    {
                        for (UInt64 i = 0; i < queueSize; i++)
                        {
                            Semaphore aThreadSemaphore = waitingThreadsQueue.Dequeue();
                            aThreadSemaphore.Release();
                        }
                    }
                }
            }
        }
    }
}
