using System;
using System.Threading;
using ConcurrencyUtils;

namespace SleepingSanta
{
	public class Santa: ActiveObject
	{
		private State systemState;

		public Santa(State systemState) : base()
		{
			this.systemState = systemState;
		}

		protected override void Execute()
		{
			while (true)
			{
				systemState.wakeSanta.Acquire();					// Wait for signal to wake up.
				if (systemState.getReindeerCount() == 9)
				{
					systemState.clearReindeerCount();

					Console.WriteLine("\t\t\t\tSanta is preparing the sleigh...");
					Thread.Sleep(State.STD_MESSAGE_DELAY);
					systemState.sleigh.Release(9);					// Signal the reindeer to hitch to the sleigh

					while (systemState.getReindeerCount() != 9){}	// Wait until reindeer are hitched

					Console.WriteLine("\t\t\t\tDelivering gifts");
					Thread.Sleep(State.STD_MESSAGE_DELAY);

					systemState.clearReindeerCount();
					systemState.holidayPermission.Release(9);
					Console.WriteLine("\t\t\t\tSanta is back asleep");
				} 
				else
				{
					// Let elves in
				}
			}
		}
	}
}
