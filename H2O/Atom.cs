using System;
using System.Threading;
using ConcurrencyUtils;

namespace H2O
{
	public class Atom: ActiveObject
	{
		private const int DELAY = 3000;

		private Semaphore atomSemaphore;
		private Barrier bondBarrier;

		public Atom(Semaphore atomSemaphore, Barrier bondBarrier, string name) : base(name)
		{
			this.atomSemaphore = atomSemaphore;
			this.bondBarrier = bondBarrier;
		}

		protected override void Execute()
		{
			while (true)
			{
				atomSemaphore.Acquire();
				Console.WriteLine(Thread.CurrentThread.Name + " waiting to bond...");
				if (bondBarrier.Arrive())
				{
					Console.WriteLine("\t\tCreating H2O...");
				}
				Bond();
				Thread.Sleep(DELAY);
			}
		}

		public void Bond()
		{
			Console.WriteLine("\t\t\t" + Thread.CurrentThread.Name + " bonding...");
			Thread.Sleep(DELAY);
			atomSemaphore.Release();
		}
	}
}

