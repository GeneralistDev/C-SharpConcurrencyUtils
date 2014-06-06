using System;
using System.Collections;

namespace ConcurrencyUtils
{
	/// <summary>
	/// 	A concurrency utility to facilitate the exchange of two
	/// 	objects of type T between two threads 'a' and 'b'.
	/// </summary>
	public class Exchanger<T>
	{
		private Object lockObject = new Object ();
		private int threadsArrived = 0;
		private T firstItem;
		private T secondItem;
		private Semaphore firstThread = new Semaphore(0);
		private Semaphore secondThread = new Semaphore(0);

		/// <summary>
		/// 	Public constructor.
		/// </summary>
		public Exchanger() { }

		/// <summary>
		/// 	Thread exchange method.
		/// </summary>
		/// <returns>Other thread's object</returns>
		/// <param name="object1">Object to give to thread 'b'.</param>
		public T Exchange(T item)
		{
			int thisThread = 0;
			lock (lockObject)
			{
				thisThread = threadsArrived++;
			}
			if (thisThread == 1)
			{
				firstItem = item;
				firstThread.Release();
				secondThread.Acquire();
				return secondItem;
			} else if (thisThread == 2)
			{
				secondItem = item;
				secondThread.Release();
				firstThread.Acquire();
				return firstItem;
			} 
		}
	}
}
