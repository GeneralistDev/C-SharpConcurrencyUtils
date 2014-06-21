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
		private int threadsArrived = 0;
		private T firstItem;
		private T secondItem;
		private Semaphore firstThread = new Semaphore(0);
		private Semaphore secondThread = new Semaphore(0);
		private Semaphore threadTwoFinished = new Semaphore (0);
		private Mutex turnstile = new Mutex();

		/// <summary>
		/// 	Public constructor.
		/// </summary>
		public Exchanger() { }

		/// <summary>
		/// 	Thread exchange method.
		/// </summary>
		/// <returns>Other thread's object</returns>
		/// <param name="item">Object to give to thread 'b'.</param>
		public T Exchange(T item)
		{
			turnstile.Acquire();
			int thisThread = ++threadsArrived;
			T myItem;
			if (thisThread == 1)
			{
				turnstile.Release();
				firstItem = item;
				firstThread.Release();
				secondThread.Acquire();
				myItem = secondItem;

				// Wait for thread 2 to finish
				threadTwoFinished.Acquire();

				// Reset fields
				threadsArrived = 0;
				firstItem = default(T);
				secondItem = default(T);

				// Allow the next two threads in
				turnstile.Release();
				return myItem;
			} 
			else
			{
				secondItem = item;
				secondThread.Release();
				firstThread.Acquire();
				myItem = firstItem;

				// Signal that thread 2 has finished
				threadTwoFinished.Release();
				return myItem;
			} 
		}
	}
}
