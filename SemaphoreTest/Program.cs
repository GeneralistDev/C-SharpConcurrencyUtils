using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ConcurrencyUtils;

namespace SemaphoreTest
{
    class Program
    { 
        public static readonly int TEST_THREADS = 4;
        public static readonly int DELAY_SECONDS = 2;
        public static readonly UInt64 CHANNEL_SIZE = 10;
        public static readonly String DATA_STRING = "DATA";
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the utility class you would like to test:\n" +
                              "OPTIONS:\n" +
                              "   semaphore\n" +
                              "   channel\n" +
                              "   boundedchannel\n" +
                              "   lightswitch\n" +
                              "   latch\n");
            Console.Write("> ");
            String command = Console.ReadLine();
            if (command.Length > 0)
            {
                switch (command)
                {
                    case "semaphore":
                        ConcurrencyUtils.Semaphore testSemaphore = new ConcurrencyUtils.Semaphore(0);
                        List<Thread> threads = new List<Thread>();
                        for (int i = 0; i < TEST_THREADS; i++)
                        {
                            Thread newThread = new Thread(() => SemaphoreTest(testSemaphore));
                            newThread.Name = "Thread" + i;
                            Console.WriteLine(newThread.Name + " created");
                            newThread.Start();
                            threads.Add(newThread);
                        }
                        break;
                    case "channel":
                        Channel<String> testChannel = new Channel<String>();
                        List<Thread> grabberThreads = new List<Thread>();
                        Thread putterThread = new Thread(() => putData(testChannel));
                        putterThread.Name = "Putter";
                        putterThread.Start();
                        for (int i = 0; i < TEST_THREADS; i++)
                        {
                            Thread newThread = new Thread(() => grabData(testChannel));
                            newThread.Name = "Thread" + i;
                            Console.WriteLine(newThread.Name + " created");
                            newThread.Start();
                            grabberThreads.Add(newThread);
                        }
                        break;
                    case "boundedchannel":
                        BoundChannel<String> testBoundChannel = new BoundChannel<String>(CHANNEL_SIZE);
                        List<Thread> putterThreads = new List<Thread>();
                        Thread grabberThread = new Thread(() => grabData(testBoundChannel));
                        grabberThread.Name = "Grabber";
                        grabberThread.Start();
                        for (int i = 0; i < TEST_THREADS; i++)
                        {
                            Thread newThread = new Thread(() => putData(testBoundChannel));
                            newThread.Name = "Thread" + i;
                            Console.WriteLine(newThread.Name + " created");
                            newThread.Start();
                            putterThreads.Add(newThread);
                        }
                        break;
                    case "lightswitch":
                        ConcurrencyUtils.Semaphore semaphore = new ConcurrencyUtils.Semaphore(1);
                        LightSwitch writeSwitch = new LightSwitch(semaphore);
                        List<Thread> writerThreads = new List<Thread>();
                        Thread lonelyThread = new Thread(() => lonelyThreadWrite( semaphore ) );
                        lonelyThread.Start();
                        for (int i = 0; i < 10; i++)
                        {
                            Thread newThread = new Thread(() => groupThreadWrite(writeSwitch));
                            newThread.Name = "Thread" + i;
                            newThread.Start();
                            writerThreads.Add(newThread);
                        }
                        break;
                    case "latch":
                        Latch latch = new Latch();
                        List<Thread> waiters = new List<Thread>();
                        for (int i = 0; i < 100; i++)
                        {
                            Thread newThread = new Thread(() => latchAcquire(latch));
                            newThread.Name = "Thread" + i;
                            newThread.Start();
                        }
                        Thread unlocker = new Thread(() => latchRelease(latch));
                        unlocker.Start();
                        break;
                    default:
                        Console.WriteLine("Test for '" + args[0] + "' not implemented");
                        break;
                }
            }
            else
            {
                Console.WriteLine("No argument provided");
            }
        }

        public static void latchRelease(Latch latch)
        {
            Thread.Sleep(2000); // Wait two seconds before releasing the latch
            latch.Release();
        }

        public static void latchAcquire(ConcurrencyUtils.Latch latch)
        {
            Console.WriteLine(Thread.CurrentThread.Name + "trying to acquire");
            latch.Acquire();
            Console.WriteLine(Thread.CurrentThread.Name + "got through the latch");
        }

        public static void lonelyThreadWrite(ConcurrencyUtils.Semaphore semaphore)
        {
            while (true)
            {
                semaphore.Acquire();
                Console.WriteLine("I'm a lonely thread");
                Thread.Sleep(100);
                semaphore.Release();
            }
        }

        public static void groupThreadWrite(ConcurrencyUtils.LightSwitch LS)
        {
            while (true)
            {
                LS.Acquire();
                Console.WriteLine("Group thread #" + Thread.CurrentThread.Name);
                Thread.Sleep(3600);
                LS.Release();
            }
        }

        public static void SemaphoreTest(ConcurrencyUtils.Semaphore testSemaphore)
        {
            while (true)
            {
                Console.WriteLine(Thread.CurrentThread.Name + " about to release token");
                Thread.Sleep(DELAY_SECONDS * 1000);
                testSemaphore.Release();
                testSemaphore.Acquire();
                Console.WriteLine(Thread.CurrentThread.Name + " acquired token");
                Thread.Sleep(DELAY_SECONDS * 1000);
            }
        }

        public static void putData(Channel<String> dataChannel)
        {
            while (true)
            {
                Thread.Sleep(DELAY_SECONDS);
                Console.WriteLine(Thread.CurrentThread.Name + " about to put data in channel");
                dataChannel.Put(DATA_STRING);
            }
        }

        public static void grabData(Channel<String> dataChannel)
        {
            while (true)
            {
                Console.WriteLine(Thread.CurrentThread.Name + " got: " + dataChannel.Take());
            }
        }

        public static void putData(BoundChannel<String> boundDataChannel)
        {
            while (true)
            {
                Console.WriteLine(Thread.CurrentThread.Name + " about to put data on the channel");
                boundDataChannel.Put(DATA_STRING);
            }
        }

        public static void grabData(BoundChannel<String> boundDataChannel)
        {
            while (true)
            {
                Thread.Sleep(DELAY_SECONDS * 1000);
                Console.WriteLine(Thread.CurrentThread.Name + " about to take data from channel");
                Console.WriteLine("Got: " + boundDataChannel.Take());
            }
        }
    }
}
