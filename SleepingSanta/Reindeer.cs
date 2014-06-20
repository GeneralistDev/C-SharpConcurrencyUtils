using System;
using System.Threading;
using ConcurrencyUtils;

namespace SleepingSanta
{
	public class Reindeer: ActiveObject
	{
		private State systemState;
		public Reindeer(State systemState, string name): base(name)
		{
			this.systemState = systemState;
		}

		protected override void Execute()
		{
			while (true)
			{
				Thread.Sleep(State.HOLIDAY_TIME);			// Go on holidays
				systemState.addReindeer();

				Console.WriteLine("\t\t\t\t" + Thread.CurrentThread.Name + " has arrived at the north pole.");

				if (systemState.warmingHut.Arrive())		// Arrive at the north pole
				{
					Console.WriteLine("\t\t\t\tReindeer waking santa");
					Thread.Sleep(State.STD_MESSAGE_DELAY);
					systemState.wakeSanta.Release();		// Last reindeer will wake Santa
				}

				systemState.sleigh.Acquire();				// Hitch to the sleigh
				Console.WriteLine("\t\t\t\t" + Thread.CurrentThread.Name + " hitched to the Sleigh!");
				systemState.addReindeer();
				systemState.holidayPermission.Acquire();	// Acquire Permission to go on holidays
			}
		}
	}
}
