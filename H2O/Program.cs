using System;
using System.Collections;
using ConcurrencyUtils;

namespace H2O
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Semaphore hydrogenSemaphore = new Semaphore(2);
			Semaphore oxygenSemaphore = new Semaphore(1);
			Barrier bondBarrier = new Barrier(3);

			ArrayList atoms = new ArrayList();
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
