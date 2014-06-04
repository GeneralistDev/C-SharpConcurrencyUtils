using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrencyUtils;

namespace Smokers
{
    class Smoker
    {
        private Semaphore watchSemaphore;

        public Smoker(Semaphore watchSemaphore)
        {
            this.watchSemaphore = watchSemaphore;
        }

        public void MakeAndSmokeCigarette()
        {
            while (true)
            {
                watchSemaphore.Acquire();
                Console.WriteLine("\t" + System.Threading.Thread.CurrentThread.Name + " is smoking...\n");
            }
        }
    }
}
