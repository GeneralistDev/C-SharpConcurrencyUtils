using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrencyUtils
{
    /// <summary>
    ///     The semaphore class from which other concurrency utilities are derived.
	/// 
	/// 	Author: Daniel Parker 971328X
    /// </summary>
    public class Semaphore
    {
        /// <summary>
        ///     The number of tokens available in the semaphore.
        /// </summary>
        protected UInt64 tokens;
		protected UInt64 waitingThreads;

        /// <summary>
        ///     Object to lock when intending to read or write the 'tokens' variable.
        /// </summary>
        protected readonly Object lockObject = new Object();

        /// <summary>
        ///     Constructor taking an optional initial token amount. Default is 0 tokens
        /// </summary>
        /// <param name="nTokens"></param>
        public Semaphore(UInt64 nTokens = 0)
        {
            tokens = nTokens;
        }

        /// <summary>
        ///     Acquire a token from the semaphore. Threads will wait if none available 
        /// </summary>
        public virtual void Acquire()
        {
			lock(lockObject)
			{
				waitingThreads++;
				try
				{
					while (tokens == 0)
					{
						Monitor.Wait(lockObject);
					}
					waitingThreads--;
					tokens--;

					if (tokens > 0 && waitingThreads > 0)
					{
						Monitor.Pulse(lockObject);
					}
				} catch (ThreadInterruptedException e)
				{
					lock(lockObject)
					{
						waitingThreads--;
						Monitor.Pulse(lockObject);
					}
				}
			}
        }

        /// <summary>
        ///     Release a number of tokens to the semaphore (default 1) and pulse threads waiting at Acquire()
        /// </summary>
        /// <param name="n"></param>
        public virtual void Release(UInt64 n = 1)
        {
            lock (lockObject)
            {
                tokens += n;
				if (waitingThreads > 0) 
				{
					Monitor.Pulse (lockObject);
				}
            }
        }
    }
}
