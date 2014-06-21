using System;
using System.Collections;
using ConcurrencyUtils;

namespace H2O
{
	/// <summary>
	/// 	H2O Program Class.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(string[] args)
		{
			Semaphore hydrogenSemaphore = new Semaphore(2);	// Controls the amount of hydrogen present in the barrier. 
			Semaphore oxygenSemaphore = new Semaphore(1);	// Controls the amount of oxygen present in the barrier.
			Barrier bondBarrier = new Barrier(3);

			/* Create six oxygen and hydrogen atoms */
			for (int i = 0; i < 6; i++)
			{
				Atom hydrogen = new Atom(hydrogenSemaphore, bondBarrier, "H" + i);
				Atom oxygen = new Atom(oxygenSemaphore, bondBarrier, "O" + i);
				hydrogen.Start();
				oxygen.Start();
			}
		}
	}
}
