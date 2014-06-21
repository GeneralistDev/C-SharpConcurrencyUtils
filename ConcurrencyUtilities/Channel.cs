using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrencyUtils
{
	/// <summary>
	/// 	The Abstract Data Type Channel (concurrent queue)
	/// 
	/// 	Author: Daniel Parker 971328X
	/// </summary>
    public class Channel<T>
    {
        private Semaphore takePermission;
        private readonly Object lockObject = new Object();
        private Queue<T> channelQueue;

		/// <summary>
		/// Initializes a new instance of this class
		/// by creating an abstract Queue and zero token Semaphore.
		/// </summary>
        public Channel()
        {
            channelQueue = new Queue<T>();
            takePermission = new Semaphore();
        }

		/// <summary>
		/// Put the specified item into the Channel. Keeps trying even if interrupted.
		/// </summary>
		/// <param name="item">Item.</param>
        public virtual void Put(T item)
        {
			Boolean success = false;
			Boolean interrupted = false;
			while (!success)
			{
				try
				{
					success = Offer(-1, item);
				}
				catch (ThreadInterruptedException)
				{
					interrupted = true;
					continue;
				}
			}
			if (interrupted)
			{
				Thread.CurrentThread.Interrupt();
			}
        }

		/// <summary>
		/// Tries to put an item on the channel. May be interrupted.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual void TryPut(T item)
		{
			Offer(-1, item);
		}

		/// <summary>
		/// Tries to put an item on the channel. May be interrupted.
		/// </summary>
		/// <param name="timout">Timeout (for use in subclasses).</param>
		/// <param name="item">Item.</param>
		public virtual bool Offer(int timeout, T item)
		{
			lock(lockObject)
			{
				channelQueue.Enqueue(item);
			}
			takePermission.ForceRelease();

			return true;
		}

		/// <summary>
		/// Forcefully take an item from the Channel
		/// </summary>
        public virtual T Take()
        {
			T item = default(T);
			Boolean success = false;
			Boolean interrupted = false;
			while (!success)
			{
				try
				{
					success = Poll(-1, out item);
				}
				catch (ThreadInterruptedException)
				{
					interrupted = true;
				}
			}
			if (interrupted)
			{
				Thread.CurrentThread.Interrupt();
			}
			return item;
        }

		/// <summary>
		/// Tries to take from the channel indefinitely. May get interrupted and not return anything useful.
		/// </summary>
		/// <returns>The take.</returns>
		public virtual T TryTake()
		{
			T item;
			Poll(-1, out item);
			return item;
		}

		/// <summary>
		/// Poll the specified timeout and item.
		/// </summary>
		/// <param name="timeout">Timeout.</param>
		/// <param name="item">Item.</param>
		public virtual bool Poll(int timeout, out T item)
		{
			item = default(T);
			if (takePermission.TryAcquire(timeout))
			{
				try
				{
					lock (lockObject)
					{
						item = channelQueue.Dequeue();
					}
				}
				catch (ThreadInterruptedException)
				{
					takePermission.ForceRelease();
					throw;
				}
				return true;
			} 
			else
			{
				return false;
			}
		}
    }
}