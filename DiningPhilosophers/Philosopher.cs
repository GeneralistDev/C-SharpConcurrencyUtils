using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrencyUtils;

namespace DiningPhilosophers
{
    class Philosopher
    {
        private Mutex leftFork;
        private Mutex rightFork;
        private FIFOSemaphore eatPermission;

        public Philosopher(Mutex leftFork, Mutex rightFork, FIFOSemaphore eatPermission)
        {
            this.leftFork = leftFork;
            this.rightFork = rightFork;
            this.eatPermission = eatPermission;
        }
        
        public void BeginLifeAmbitions()
        {
            while (true)
            {
                Think();
                GetForks();
                Eat();
                PutForks();
            }
        }

        public void Think()
        {
            PrintStatus("Thinking...");
            System.Threading.Thread.Sleep(5000);
        }

        public void Eat()
        {
            PrintStatus("Eating...");
            System.Threading.Thread.Sleep(5000);
        }

        public void GetForks()
        {
            eatPermission.Acquire();
            PrintStatus("Getting forks...");
            leftFork.Acquire();
            rightFork.Acquire();
            PrintStatus("... got forks");
        }

        public void PutForks()
        {
            PrintStatus("Returning forks");
            leftFork.Release();
            rightFork.Release();
            eatPermission.Release();
        }

        public void PrintStatus(String statusString)
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.Name + ": " + statusString + "\n");
        }
    }
}
