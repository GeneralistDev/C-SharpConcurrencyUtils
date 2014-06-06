using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrencyUtils;

namespace Smokers
{
	/// <summary>
	/// 	Smoker class.
	/// </summary>
    class Smoker
    {
        private Semaphore watchSemaphore;
		private Semaphore agentSemaphore;

		/// <summary>
		/// 	Initializes a new instance of the Smoker class.
		/// </summary>
		/// <param name="watchSemaphore">The agent's semaphore that will be released to when ingredients are available.</param>
		public Smoker(Semaphore watchSemaphore, Semaphore agentSemaphore)
        {
            this.watchSemaphore = watchSemaphore;
			this.agentSemaphore = agentSemaphore;
        }

		/// <summary>
		/// 	Makes the and smoke cigarette. Loops and tries to acquire from the watch semaphore.
		/// </summary>
        public void MakeAndSmokeCigarette()
        {
            while (true)
            {
                watchSemaphore.Acquire();
                Console.WriteLine("\t" + System.Threading.Thread.CurrentThread.Name + " is smoking...\n");
				agentSemaphore.Release ();
            }
        }
    }
}
