using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrencyUtils
{
    /// <summary>
	/// 	Concurrency utility that allows multiple reading threads
	/// 	to access a resource, but when a writing thread needs access
	/// 	only that single thread can hold the resource access permission.
	/// 	No other readers or writers can access the reasource.
	/// 
	/// 	Author: Daniel Parker 971328X
    /// </summary>
    public class ReaderWriterLock
    {
        Semaphore thisLock;
        Mutex writerTS, readerTS;
        int numberOfReaders;
        LightSwitch readers;
        Object lockObject;

        /// <summary>
		/// 	Public constructor.
        /// </summary>
        public ReaderWriterLock()
        {
            thisLock = new Semaphore(1);

            writerTS = new Mutex();
            readerTS = new Mutex();

            numberOfReaders = 0;

            readers = new LightSwitch(thisLock);

            lockObject = new Object();
        }

        /// <summary>
		/// 	Acquire permission to read. Many readers can get through this concurrently.
		/// 	This will block if a writer has access to the resource.
		/// 
		/// 	Details: Reading threads will pass through a reader turnstile after which they will
		/// 	increment the number of readers currently in the reader-writer lock. Then the 
		/// 	reading thread will acquire the 'readers' lightswitch. On exit the thread will
		/// 	remove itself from the number of readers and pulse the lockObject where any
		/// 	writers will be waiting to check if there are no longer readers in the lock.
        /// </summary>
        public void ReaderAcquire()
        {
			readerTS.Acquire();
			readerTS.Release(1);

            lock (lockObject)
            {
                numberOfReaders++;
            }

            readers.Acquire();

            lock (lockObject)
            {
                numberOfReaders--;
                Monitor.PulseAll(lockObject);
            }
        }

        /// <summary>
		/// 	Acquire permission to write. Only one thread can access the locked
		/// 	resource at a time when that thread wants to write.
		/// 
		/// 	Details: Writer will acquire the reader turnstile to block any new
		/// 	reader threads for the time being. It will also acquire the writer
		/// 	turnstile to stop any new writers entering, and then attempt to 
		/// 	acquire the main reader-writer lock. This will wait until any
		/// 	remaining readers in the lightswitch complete their read operations.
		/// 	The final operation is to release the writer turnstile and allow a
		/// 	pending writer to queue up. (New readers are still blocked at this stage)
        /// </summary>
        public void WriterAcquire()
        {
            readerTS.Acquire();
            writerTS.Acquire();
            thisLock.Acquire();
			readerTS.Release(1);
        }

        /// <summary>
		/// 	Readers only have to release to the lightswitch to announce their completion.
        /// </summary>
        public void ReaderRelease()
        {
            readers.Release();
        }

        /// <summary>
		/// 	Release writer lock.
		/// 
		/// 	Details: The main reader-writer lock is released, which may result in some lagging readers
		/// 	to acquire the lightswitch and run read operations. The writer will wait for
		/// 	the number of readers to reach zero and then release the reader turnstile to effectively
		/// 	give any waiting readers or writers an equal chance of getting read/write permission.
        /// </summary>
        public void WriterRelease()
        {
            thisLock.Release();
            lock (lockObject)
            {
                if (numberOfReaders > 0)
                {
                    Monitor.Wait(lockObject);
                }   
            }
			writerTS.Release(1);
        }
    }
}
