using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrencyUtils
{
    /// <summary>
    /// 
    /// </summary>
    public class ReaderWriterLock
    {
        Semaphore thisLock;
        Mutex writerTS, readerTS;
        int numberOfReaders;
        LightSwitch readers;
        Object lockObject;

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public void ReaderAcquire()
        {
            writerTS.Acquire();
            writerTS.Release(1);

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
        /// 
        /// </summary>
        public void WriterAcquire()
        {
            readerTS.Acquire();
            writerTS.Acquire();
            thisLock.Acquire();
            writerTS.Release(1);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReaderRelease()
        {
            readers.Release();
        }

        /// <summary>
        /// 
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
                readerTS.Release(1);
            }
        }
    }
}
