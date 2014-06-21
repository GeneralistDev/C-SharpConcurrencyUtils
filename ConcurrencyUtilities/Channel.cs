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
		/// Put the specified item into the Channel.
		/// </summary>
		/// <param name="item">Item.</param>
        public virtual void Put(T item)
        {
			Offer(item);
        }

		public virtual bool Offer(T item)
		{
			lock(lockObject)
			{
				channelQueue.Enqueue(item);
			}
			takePermission.ForceRelease();

			return true;
		}

		/// <summary>
		/// 	Take an item from the Channel
		/// </summary>
        public virtual T Take()
        {
			T item;
			Poll(-1, out item);
			return item;
        }

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

		/// <summary>
		/// 	Safely get the number of elements in the underlying queue.
		/// </summary>
		public int count()
		{
			int count;
			lock (lockObject)
			{
				count = channelQueue.Count;
			}
			return count;
		}
    }
}