using System;

namespace ConcurrencyUtils
{
	/// <summary>
	/// 	A concurrency utility to facilitate the exchange of two
	/// 	objects of type T between two threads 'a' and 'b'.
	/// </summary>
	public class Exchanger<T>
	{
		private T object1;
		private T object2;
		private Semaphore aArrived = new Semaphore(0);
		private Semaphore bArrived = new Semaphore(0);

		/// <summary>
		/// 	Public constructor.
		/// </summary>
		public Exchanger() { }

		/// <summary>
		/// 	Thread 'a' exchange method.
		/// </summary>
		/// <returns>Other thread's object</returns>
		/// <param name="object1">Object to give to thread 'b'.</param>
		public T aExchange(T object1)
		{
			this.object1 = object1;
			aArrived.Release();
			bArrived.Acquire();
			return this.object2;
		}

		/// <summary>
		/// 	Thread 'b' exchange method.
		/// </summary>
		/// <returns>The exchange.</returns>
		/// <param name="object2">Object to give to thread 'a'.</param>
		public T bExchange(T object2)
		{
			this.object2 = object2;
			bArrived.Release();
			aArrived.Acquire();
			return this.object1;
		}
	}
}
