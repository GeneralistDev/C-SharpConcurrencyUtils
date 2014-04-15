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
    public class BoundChannel<T> : Channel<T>
    {
        private Semaphore putPermission;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public BoundChannel(UInt64 size)
        {
            putPermission = new Semaphore(size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public override void Put(T item)
        {
            putPermission.Acquire();
            base.Put(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override T Take()
        {
            T item = base.Take();
            putPermission.Release();
            return item;
        }
    }
}
