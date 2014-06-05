using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrencyUtils;

namespace Smokers
{
	/// <summary>
	/// 	Agent class.
	/// </summary>
    class Agent
    {
        private Object agentLock = new Object();
		private Boolean isTobacco, isPaper, isMatch;

		// Semaphores that are pulsed to represent the availability of an ingredient.
		// Pushers watch these semaphores.
        public readonly Semaphore tobaccoSemaphore = new Semaphore(0);
        public readonly Semaphore matchSemaphore = new Semaphore(0);
		public readonly Semaphore paperSemaphore = new Semaphore (0);

		// Semaphores that will be watched by smokers and pulsed by pushers.
        public readonly Semaphore tobaccoSmoker = new Semaphore(0);
        public readonly Semaphore matchSmoker = new Semaphore(0);
        public readonly Semaphore paperSmoker = new Semaphore(0);

		/// <summary>
		/// 	Initializes a new instance of the Agent class.
		/// </summary>
        public Agent() 
        {
            isTobacco = isPaper = isMatch = false;
        }

		/// <summary>
		/// 	Waits until tobacco becomes available then checks if paper or matches
		/// 	is already available to be able to pulse one of the smokers.
		/// 	If neither are available it sets the availability of tobacco to true.
		/// </summary>
        public void TobaccoPusher()
        {
            while (true)
            {
                tobaccoSemaphore.Acquire();
                lock (agentLock)
                {
                    if (isPaper)
                    {
                        isPaper = false;
                        matchSmoker.Release();
                    }
                    else if (isMatch)
                    {
                        isMatch = false;
                        paperSmoker.Release();
                    }
                    else
                    {
                        isTobacco = true;
                    }
                }
            }
        }

		/// <summary>
		/// 	Waits until paper becomes available then checks if tobacco or matches
		/// 	is already available to be able to pulse one of the smokers.
		/// 	If neither are available it sets the availability of paper to true.
		/// </summary>
        public void PaperPusher()
        {
            while (true)
            {
                paperSemaphore.Acquire();
                lock (agentLock)
                {
                    if (isTobacco)
                    {
                        isTobacco = false;
                        matchSmoker.Release();
                    }
                    else if (isMatch)
                    {
                        isMatch = false;
                        tobaccoSmoker.Release();
                    }
                    else
                    {
                        isPaper = true;
                    }
                }
            }
        }

		/// <summary>
		/// 	Waits until a match becomes available then checks if paper or tobacco
		/// 	is already available to be able to pulse one of the smokers.
		/// 	If neither are available it sets the availability of a match to true.
		/// </summary>
        public void MatchPusher()
        {
            while (true)
            {
                matchSemaphore.Acquire();
                lock (agentLock)
                {
                    if (isPaper)
                    {
                        isPaper = false;
                        tobaccoSmoker.Release();
                    }
                    else if (isTobacco)
                    {
                        isTobacco = false;
                        paperSmoker.Release();
                    }
                    else
                    {
                        isMatch = true;
                    }
                }
            }
        }
    }
}
