using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Channel<T>
    {
        private Semaphore takePermission;
        private readonly Object lockObject = new Object();
        private Queue<T> channelQueue;

        /// <summary>
        /// 
        /// </summary>
        public Channel()
        {
            channelQueue = new Queue<T>();
            takePermission = new Semaphore();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Put(T item)
        {
            lock (lockObject)
            {
                channelQueue.Enqueue(item);
            }
            takePermission.Release();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
