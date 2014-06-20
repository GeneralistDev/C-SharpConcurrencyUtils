using System;
using System.Threading;
using ConcurrencyUtils;

namespace H2O
{
	/// <summary>
	/// Atom class.
	/// </summary>
	public class Atom: ActiveObject
	{
		private const int DELAY = 3000;

		private Semaphore atomSemaphore;
		private Barrier bondBarrier;

		/// <summary>
		/// Initializes a new instance of the Atom class.
		/// </summary>
		/// <param name="atomSemaphore">Atom semaphore.</param>
		/// <param name="bondBarrier">Bond barrier.</param>
		/// <param name="name">Name.</param>
		public Atom(Semaphore atomSemaphore, Barrier bondBarrier, string name) : base(name)
		{
			this.atomSemaphore = atomSemaphore;
			this.bondBarrier = bondBarrier;
		}

		/// <summary>
		/// Execute atom logic.
		/// </summary>
		protected override void Execute()
		{
			while (true)
			{
				atomSemaphore.Acquire();		// Get permission to start bonding process
				Console.WriteLine(Thread.CurrentThread.Name + " waiting to bond...");
				if (bondBarrier.Arrive())		// Wait for other atoms
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
			atomSemaphore.Release();			// Allow other atoms to start bonding
		}
	}
}

