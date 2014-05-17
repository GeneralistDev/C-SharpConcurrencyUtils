using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrencyUtils
{
    /// <summary>
    ///     The mutex (single token semaphore) class which inherits from 
	///		ConcurrencyUtils.Semaphore
    /// </summary>
    public class Mutex: Semaphore
    {
        /// <summary>
        ///     Calls Semaphore constructor with single token
        /// </summary>
        public Mutex(): base(1) {}

        /// <summary>
        ///     Overidding Release method which throws an argument error if
		///		either more than one token is being released or if the total 
		///		tokens is already larger than 0.
        /// </summary>
        /// <param name="n"></param>
        public override void Release(ulong n = 1)
        {
            lock (this)
            {
                if (n > 1 || base.tokens > 0)
                {
                    throw new System.ArgumentException("A mutex cannot contain 
						more than 1 token");
                }
                else
                {
                    base.Release(1);
                }
            }
        }
    }
}
