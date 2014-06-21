using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
	/// <summary>
	/// 	Bound channel. Inherits from the ADT ConcurrencyUtils.Channel class.
	/// 
	/// 	Author: Daniel Parker 971328X
	/// </summary>
    public class BoundChannel<T> : Channel<T>
    {
        private Semaphore putPermission;

		/// <summary>
		///     Initializes a new instance of this class
		///     with a maximum size for the channel.
		/// </summary>
		/// <param name="size">Size.</param>
        public BoundChannel(UInt64 size)
        {
            putPermission = new Semaphore(size);
        }

		/// <summary>
		/// Put the specified item into the Channel if there is space, will wait
		/// on Acquire() if there isn't. Will try again if interrupted.
		/// </summary>
		/// <param name="item">Item.</param>
        public override void Put(T item)
        {
			Boolean interrupted = false;
			Boolean acquired = false;

			while (!acquired)
			{
				try
				{
					putPermission.Acquire();
					acquired = true;
				} 
				catch (ThreadInterruptedException)
				{
					interrupted = true;
				}

				base.Put(item);

				if (interrupted)
				{
					Thread.CurrentThread.Interrupt();
				}
			}   
        }

		/// <summary>
		/// Tries to put an item on the channel. May be interrupted.
		/// </summary>
		/// <param name="item">Item.</param>
		public override void TryPut(T item)
		{
			putPermission.Acquire();
			base.TryPut(item);
		}

		/// <summary>
		/// Tries to put an item on the channel. May be interrupted.
		/// </summary>
		/// <param name="timout">Time to try putting for before ceasing to try.</param>
		/// <param name="item">Item.</param>
		/// <param name="timeout">Timeout.</param>
		public override bool Offer(int timeout, T item)
		{
			return base.Offer(timeout, item);
		}

		/// <summary>
		/// Forcefully take an item from the Channel
		/// </summary>
		public override T Take()
		{
			T item;
			try 
			{
				item = base.Take();
			}
			catch (ThreadInterruptedException)
			{
				throw;
			}
			finally
			{
				putPermission.Release();
			}

			return item;
		}

		/// <summary>
		/// Tries to take from the channel indefinitely. May get interrupted and not return anything useful.
		/// </summary>
		/// <returns>The take.</returns>
		public override T TryTake()
        {
			T item = base.TryTake();
			putPermission.Release();		// Will not be run if base.TryTake() is interrupted.
            return item;
        }

		public override bool Poll(int timeout, out T item)
		{
			return base.Poll(timeout, out item);
		}
    }
}