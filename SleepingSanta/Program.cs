using System;

namespace SleepingSanta
{
	/// <summary>
	/// 	Main program class.
	/// </summary>
	public class Program
	{
		public static void Main(string[] args)
		{
			State systemState = new State();		// Object that holds all the shared system variables

			/* Actors */
			Santa santa = new Santa(systemState);

			/* Elves */

			/* Reindeer */
			Reindeer reindeer1 = new Reindeer(systemState, "Dasher");
			Reindeer reindeer2 = new Reindeer(systemState, "Dancer");
			Reindeer reindeer3 = new Reindeer(systemState, "Prancer");
			Reindeer reindeer4 = new Reindeer(systemState, "Vixen");
			Reindeer reindeer5 = new Reindeer(systemState, "Comet");
			Reindeer reindeer6 = new Reindeer(systemState, "Cupid");
			Reindeer reindeer7 = new Reindeer(systemState, "Dunder");
			Reindeer reindeer8 = new Reindeer(systemState, "Blixen");
			Reindeer reindeer9 = new Reindeer(systemState, "Rudolph");

			/* Start everything */
			santa.Start();

			reindeer1.Start();
			reindeer2.Start();
			reindeer3.Start();
			reindeer4.Start();
			reindeer5.Start();
			reindeer6.Start();
			reindeer7.Start();
			reindeer8.Start();
			reindeer9.Start();
		}
	}
}
