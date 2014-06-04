using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
	/// <summary>
	/// The Abstract Data Type Channel (concurrent queue)
	/// </summary>
    public class Channel<T>
    {
        private Semaphore takePermission;
        private readonly Object lockObject = new Object();
        private Queue<T> channelQueue;

		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="ConcurrencyUtils.Channel"/> class
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
            lock (lockObject)
            {
                channelQueue.Enqueue(item);
            }
            takePermission.Release();
        }

		/// <summary>
		/// Take an item from the Channel
		/// </summary>
        public virtual T Take()
        {
            takePermission.Acquire();
            lock (lockObject)
            {
                return channelQueue.Dequeue();
            }
        }
    }
}