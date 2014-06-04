using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrencyUtils;

namespace Smokers
{
    class Agent
    {
        private Object agentLock = new Object();
        public readonly Semaphore tobaccoSemaphore = new Semaphore(0);
        public readonly Semaphore matchSemaphore = new Semaphore(0);
        public readonly Semaphore paperSemaphore = new Semaphore(0);

        private Boolean isTobacco, isPaper, isMatch;

        public readonly Semaphore tobaccoSmoker = new Semaphore(0);
        public readonly Semaphore matchSmoker = new Semaphore(0);
        public readonly Semaphore paperSmoker = new Semaphore(0);

        public Agent() 
        {
            isTobacco = isPaper = isMatch = false;
        }

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
