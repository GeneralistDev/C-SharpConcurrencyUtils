using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
	/// <summary>
	/// Bound channel. Inherits from the ADT ConcurrencyUtils.Channel class
	/// </summary>
    public class BoundChannel<T> : Channel<T>
    {
        private Semaphore putPermission;

		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="ConcurrencyUtils.BoundChannel`1"/> class
		/// with a maximum size for the channel.
		/// </summary>
		/// <param name="size">Size.</param>
        public BoundChannel(UInt64 size)
        {
            putPermission = new Semaphore(size);
        }

		/// <summary>
		/// Put the specified item into the Channel if there is space, will wait
		/// on Acquire() if there isn't.
		/// </summary>
		/// <param name="item">Item.</param>
        public override void Put(T item)
        {
            putPermission.Acquire();
            base.Put(item);
        }

		/// <summary>
		/// Take an item from the Channel and release token
		/// </summary>
        public override T Take()
        {
            T item = base.Take();
            putPermission.Release();
            return item;
        }
    }
}
