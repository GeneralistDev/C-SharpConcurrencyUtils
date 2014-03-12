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
        public static int TEST_THREADS = 4;
        public static int DELAY_SECONDS = 2;
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
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
    }
}
