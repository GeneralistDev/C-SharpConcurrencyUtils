using System;
using ConcurrencyUtils;

namespace SleepingSanta
{
	/// <summary>
	/// 	All the program state so that access is easily controlled and thread safety can be ensured.
	/// </summary>
	public class State
	{
		//Constants (don't need locking, as they will only be read)
		public const int HOLIDAY_TIME = 9000;					// Time that the reindeer holiday for (ms)
		public const int STD_MESSAGE_DELAY = 1000;				// Standard time to wait after a message is printed (ms)
		public const int MIN_ELF_WAIT = 4000;					// The minimum time an elf should wait between asks for help
		public const int MAX_ELF_WAIT = 7000;					// The maximum time an elf should wait between asks for help

		public Semaphore wakeSanta = new Semaphore(0);			// Santa tries to acquire from this semaphore (sleeping state)
		public Semaphore sleigh = new Semaphore(0);				// Reinder acquire to hitch to the sleigh
		public Semaphore elfWaitPermission = new Semaphore(3); 	// Permission for elves to queue up in 'elfBarrier'
		public Semaphore holidayPermission = new Semaphore(0);	// Permission for reindeer to go on holidays

		public Barrier elfBarrier = new Barrier(3);				// Barrier to wait until a minimum of three elves needs help.
		public Barrier warmingHut = new Barrier(9);				// Barrier to wait until all Reindeer have arrived.

		private int reindeerCount = 0;							// Number of reindeer waiting

		private Object reindeerCountLock = new Object();

		/// <summary>
		/// 	Public constructor.
		/// </summary>
		public State()
		{

		}

		/// <summary>
		/// 	Adds a reindeer.
		/// </summary>
		public void addReindeer()
		{
			lock(reindeerCountLock)
			{
				reindeerCount++;
			}
		}

		/// <summary>
		/// 	Gets the reindeer count.
		/// </summary>
		/// <returns>The reindeer count.</returns>
		public int getReindeerCount()
		{
			lock(reindeerCountLock)
			{
				return reindeerCount;
			}
		}

		/// <summary>
		/// 	Clears the reindeer count to zero.
		/// </summary>
		public void clearReindeerCount()
		{
			lock(reindeerCountLock)
			{
				reindeerCount = 0;
			}
		}
	}
}

