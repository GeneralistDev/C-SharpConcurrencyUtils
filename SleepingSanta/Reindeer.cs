using System;
using System.Threading;
using ConcurrencyUtils;

namespace SleepingSanta
{
	/// <summary>
	/// 	The Reindeer class.
	/// </summary>
	public class Reindeer: ActiveObject
	{
		private State systemState;

		/// <summary>
		/// 	Initializes a new instance of the Reindeer class.
		/// </summary>
		/// <param name="systemState">System state.</param>
		/// <param name="name">The name of this reindeer (used in messages).</param>
		public Reindeer(State systemState, string name): base(name)
		{
			this.systemState = systemState;
		}

		/// <summary>
		/// Execute reindeer logic.
		/// </summary>
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
