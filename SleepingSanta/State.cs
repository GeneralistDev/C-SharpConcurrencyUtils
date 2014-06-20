using System;
using ConcurrencyUtils;

namespace SleepingSanta
{
	public class State
	{
		//Constants (don't need locking, as they will only be read)
		public const int HOLIDAY_TIME = 8000;
		public const int ELF_DELAY = 1500;
		public const int STD_MESSAGE_DELAY = 1000;

		public Semaphore wakeSanta = new Semaphore(0);			// Santa tries to acquire from this semaphore (sleeping state)
		public Semaphore sleigh = new Semaphore(0);				// Reinder acquire to hitch to the sleigh
		public Semaphore elfWaitPermission = new Semaphore(3); 	// Permission for elves to queue up in 'elfBarrier'
		public Semaphore holidayPermission = new Semaphore(0);	// Permission for reindeer to go on holidays

		public Barrier elfBarrier = new Barrier(3);				// Barrier to wait until a minimum of three elves needs help.
		public Barrier warmingHut = new Barrier(9);				// Barrier to wait until all Reindeer have arrived.

		private int reindeerCount = 0;
//		private int elfCount = 0;

		private Object reindeerCountLock = new Object();
//		private Object elfCountLock = new Object();

		public State()
		{

		}

		public void addReindeer()
		{
			lock(reindeerCountLock)
			{
				reindeerCount++;
			}
		}

//		public void addElf()
//		{
//			lock(elfCountLock)
//			{
//				elfCount++;
//			}
//		}

		public int getReindeerCount()
		{
			lock(reindeerCountLock)
			{
				return reindeerCount;
			}
		}

		public void clearReindeerCount()
		{
			lock(reindeerCountLock)
			{
				reindeerCount = 0;
			}
		}

//		public int getElfCount()
//		{
//			lock(elfCountLock)
//			{
//				return elfCount;
//			}
//		}
	}
}

