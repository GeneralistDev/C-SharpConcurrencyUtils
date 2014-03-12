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
        public static readonly String DATA_STRING = "DATA";
        static void Main(string[] args)
        {
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
                    case "boundchannel":
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
                Thread.Sleep(5000);
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
    }
}
