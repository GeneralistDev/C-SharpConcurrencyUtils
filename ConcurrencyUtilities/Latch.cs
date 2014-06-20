using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
	/// <summary>
	/// 	Concurrency Latch utility.
	/// 
	/// 	Author: Daniel Parker 971328X
	/// </summary>
    public class Latch
    {
        private Semaphore lSemaphore;

		/// <summary>
		/// 	Initializes a new instance of the 
		///		<see cref="ConcurrencyUtils.Latch"/> class.
		/// </summary>
        public Latch()
        {
            lSemaphore = new Semaphore(0);
        }

		/// <summary>
		/// 	Acquire the latch.
		/// </summary>
        public void Acquire()
        {
            lSemaphore.Acquire();
            lSemaphore.Release();
        }

		/// <summary>
		/// 	Release the latch.
		/// </summary>
        public void Release()
        {
            lSemaphore.Release();
        }
    }
}
