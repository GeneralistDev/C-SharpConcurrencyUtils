using System;
using System.Threading;
using ConcurrencyUtils;

namespace SleepingSanta
{
	/// <summary>
	/// 	The Elf class.
	/// </summary>
	public class Elf: ActiveObject
	{
		private State systemState;

		/// <summary>
		/// Initializes a new instance of the Elf class.
		/// </summary>
		/// <param name="systemState">System state.</param>
		public Elf(State systemState): base()
		{
			this.systemState = systemState;
		}

		/// <summary>
		/// Execute elf logic. Overrides the underlying ActiveObject execute method
		/// </summary>
		protected override void Execute()
		{
			Random randomGenerator = new Random((int)DateTime.Now.Ticks);					
			while (true)
			{
				Thread.Sleep(randomGenerator.Next(State.MIN_ELF_WAIT, State.MAX_ELF_WAIT));	// Wait a random interval
				systemState.elfWaitPermission.Acquire();									// Get permission to wait
				Console.WriteLine("\t\tElf waiting for help");
				if (systemState.elfBarrier.Arrive())										// Wait for help
				{
					Console.WriteLine("\t\tElves waking santa");
					Thread.Sleep(State.STD_MESSAGE_DELAY);
					systemState.wakeSanta.Release();										// Last elf wakes Santa
				}
			}
		}
	}
}
