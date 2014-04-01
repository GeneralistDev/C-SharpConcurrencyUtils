using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConcurrencyUtils
{
    public class ReaderWriterLock
    {
        Semaphore thisLock;
        Mutex writerTS, readerTS;
        int NumberOfReaders;
        LightSwitch readers;
        Object lockObject;

        public ReaderWriterLock()
        {
            thisLock = new Semaphore(1);

            writerTS = new Mutex();
            readerTS = new Mutex();

            NumberOfReaders = 0;

            readers = new LightSwitch(thisLock);

            lockObject = new Object();
        }

        public void ReaderAcquire()
        {
            writerTS.Acquire();
            writerTS.Release();

            lock (lockObject)
            {
                NumberOfReaders++;
            }

            readers.Acquire();

            lock (lockObject)
            {
                NumberOfReaders--;
                Monitor.PulseAll(lockObject);
            }
        }

        public void WriterAcquire()
        {
            readerTS.Acquire();
            writerTS.Acquire();
            thisLock.Acquire();
            writerTS.Release();
        }

        public void ReaderRelease()
        {
            readers.Release();
        }

        public void WriterRelease()
        {
            thisLock.Release();
            lock (lockObject)
            {
                if (NumberOfReaders > 0)
                {
                    Monitor.Wait(lockObject);
                }
                readerTS.Release();
            }
        }
    }
}
