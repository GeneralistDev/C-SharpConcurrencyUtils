using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
    public class Channel<T>
    {
        private Semaphore takePermission;
        private readonly Object lockObject = new Object();
        private Queue<T> channelQueue;

        public Channel()
        {
            channelQueue = new Queue<T>();
            takePermission = new Semaphore();
        }

        public virtual void Put(T item)
        {
            lock (lockObject)
            {
                channelQueue.Enqueue(item);
            }
            takePermission.Release();
        }

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
