using System;
using System.Threading;
using ConcurrencyUtils;

namespace SleepingSanta
{
	public class Elf: ActiveObject
	{
		private State systemState;
		public Elf(State systemState): base()
		{
			this.systemState = systemState;
		}

		protected override void Execute()
		{
			Random randomGenerator = new Random((int)DateTime.Now.Ticks);
			while (true)
			{
				Thread.Sleep(randomGenerator.Next(State.MIN_ELF_WAIT, State.MAX_ELF_WAIT));
				systemState.elfWaitPermission.Acquire();
				Console.WriteLine("\t\tElf waiting for help");
				if (systemState.elfBarrier.Arrive())
				{
					Console.WriteLine("\t\tElves waking santa");
					Thread.Sleep(State.STD_MESSAGE_DELAY);
					systemState.wakeSanta.Release();
				}
			}
		}
	}
}
