using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
    public class BoundChannel<T> : Channel<T>
    {
        private Semaphore putPermission;

        public BoundChannel(UInt64 size)
        {
            putPermission = new Semaphore(size);
        }

        public override void Put(T item)
        {
            putPermission.Acquire();
            base.Put(item);
        }

        public override T Take()
        {
            T item = base.Take();
            putPermission.Release();
            return item;
        }
    }
}
