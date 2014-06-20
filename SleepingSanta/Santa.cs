using System;
using System.Threading;
using ConcurrencyUtils;

namespace SleepingSanta
{
	/// <summary>
	/// 	The Santa class.
	/// </summary>
	public class Santa: ActiveObject
	{
		private State systemState;

		/// <summary>
		/// Initializes a new instance of the Santa class.
		/// </summary>
		/// <param name="systemState">System state.</param>
		public Santa(State systemState) : base()
		{
			this.systemState = systemState;
		}

		/// <summary>
		/// Execute Santa logic.
		/// </summary>
		protected override void Execute()
		{
			while (true)
			{
				systemState.wakeSanta.Acquire();					// Wait for signal to wake up.
				if (systemState.getReindeerCount() == 9)			// Check if reindeer woke me.
				{
					systemState.clearReindeerCount();

					Console.WriteLine("Santa is preparing the sleigh...");
					Thread.Sleep(State.STD_MESSAGE_DELAY);
					systemState.sleigh.Release(9);					// Signal the reindeer to hitch to the sleigh

					while (systemState.getReindeerCount() != 9){}	// Wait until reindeer are hitched

					Console.WriteLine("Delivering gifts");
					Thread.Sleep(State.STD_MESSAGE_DELAY);

					systemState.clearReindeerCount();
					systemState.holidayPermission.Release(9);		// Allow reindeers to leave for holidays.
				} 
				else
				{
					/* Help the waiting elves */
					Console.WriteLine("Santa is helping elves");
					Thread.Sleep(State.STD_MESSAGE_DELAY * 2);
					systemState.elfWaitPermission.Release(3);		// Allow elves to wait.
				}

				Console.WriteLine("Santa is going to sleep...");
			}
		}
	}
}
