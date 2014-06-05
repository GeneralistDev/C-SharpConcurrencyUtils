using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrencyUtils;

namespace DiningPhilosophers
{
	/// <summary>
	/// 	Philosopher class.
	/// </summary>
    class Philosopher
    {
        private Mutex leftFork;
        private Mutex rightFork;
        private FIFOSemaphore eatPermission;
		private const int DELAY = 5000;

		/// <summary>
		/// 	Initializes a new instance of the Philosopher class.
		/// </summary>
		/// <param name="leftFork">Left fork.</param>
		/// <param name="rightFork">Right fork.</param>
		/// <param name="eatPermission">Eat permission.</param>
        public Philosopher(Mutex leftFork, Mutex rightFork, FIFOSemaphore eatPermission)
        {
            this.leftFork = leftFork;
            this.rightFork = rightFork;
            this.eatPermission = eatPermission;
        }

		/// <summary>
		/// 	Begins main Philosopher loop. (Will be threaded).
		/// </summary>
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

		/// <summary>
		/// 	Spend some time thinking.
		/// </summary>
        public void Think()
        {
            PrintStatus("Thinking...");
			System.Threading.Thread.Sleep(DELAY);
        }

		/// <summary>
		/// 	Eat for some time.
		/// </summary>
        public void Eat()
        {
            PrintStatus("Eating...");
			System.Threading.Thread.Sleep(DELAY);
        }

		/// <summary>
		/// 	Pick up the forks.
		/// </summary>
        public void GetForks()
        {
            eatPermission.Acquire();
            PrintStatus("Getting forks...");
            leftFork.Acquire();
            rightFork.Acquire();
            PrintStatus("... got forks");
        }

		/// <summary>
		/// 	Puts down the forks.
		/// </summary>
        public void PutForks()
        {
            PrintStatus("Returning forks");
            leftFork.Release();
            rightFork.Release();
            eatPermission.Release();
        }

		/// <summary>
		/// 	Status printing method.
		/// </summary>
		/// <param name="statusString">Status string.</param>
        public void PrintStatus(String statusString)
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.Name + ": " + statusString + "\n");
        }
    }
}
