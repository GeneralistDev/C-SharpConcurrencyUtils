using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Smokers
{
	/// <summary>
	/// 	Main Program.
	/// </summary>
    class Program
    {
        public const int DELAY = 2000;

		/// <summary>
		/// 	The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
        static void Main(string[] args)
        {
			// Create the agent.
            Agent agent = new Agent();

			// Create the smokers (tobaccoSmoker == smoker that already has tobacco but needs other ingredients etc.)
            Smoker tobaccoSmoker = new Smoker(agent.tobaccoSmoker);
            Smoker paperSmoker = new Smoker(agent.paperSmoker);
            Smoker matchSmoker = new Smoker(agent.matchSmoker);

			// Create and start the smoker threads
            Thread tobaccoSmokerThread = new Thread(tobaccoSmoker.MakeAndSmokeCigarette);
            tobaccoSmokerThread.Name = "Tobacco Smoker";
            Thread paperSmokerThread = new Thread(paperSmoker.MakeAndSmokeCigarette);
            paperSmokerThread.Name = "Paper Smoker";
            Thread matchSmokerThread = new Thread(matchSmoker.MakeAndSmokeCigarette);
            matchSmokerThread.Name = "Match Smoker";

            tobaccoSmokerThread.Start();
            paperSmokerThread.Start();
            matchSmokerThread.Start();

			// Create pusher threads that watch for ingredients and notify the correct smoker.
            Thread tobaccoPusherThread = new Thread(agent.TobaccoPusher);
            Thread paperPusherThread = new Thread(agent.PaperPusher);
            Thread matchPusherThread = new Thread(agent.MatchPusher);

            tobaccoPusherThread.Start();
            paperPusherThread.Start();
            matchPusherThread.Start();

			// Thread that adds combinations of ingredients to the table to simulate.
            Thread ingredientAdder = new Thread(() => AddIngredients(agent));
            ingredientAdder.Start();
        }

		/// <summary>
		/// 	Adds combinations of ingredients to the table.
		/// </summary>
		/// <param name="agent">The agent.</param>
        public static void AddIngredients(Agent agent)
        {
            while (true)
            {
                Console.WriteLine("Putting match on table");
                agent.matchSemaphore.Release();
                Thread.Sleep(DELAY);
                Console.WriteLine("Putting paper on table");
                agent.paperSemaphore.Release();
                Thread.Sleep(DELAY);

                Console.WriteLine("Putting tobacco on table");
                agent.tobaccoSemaphore.Release();
                Thread.Sleep(DELAY);
                Console.WriteLine("Putting paper on table");
                agent.paperSemaphore.Release();
                Thread.Sleep(DELAY);

                Console.WriteLine("Putting match on table");
                agent.matchSemaphore.Release();
                Thread.Sleep(DELAY);
                Console.WriteLine("Putting tobacco on table");
                agent.tobaccoSemaphore.Release();
                Thread.Sleep(DELAY);
            }
        }
    }
}
