using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Smokers
{
    class Program
    {
        public const int DELAY = 2000;
        static void Main(string[] args)
        {
            Agent agent = new Agent();

            Smoker tobaccoSmoker = new Smoker(agent.tobaccoSmoker);
            Smoker paperSmoker = new Smoker(agent.paperSmoker);
            Smoker matchSmoker = new Smoker(agent.matchSmoker);

            Thread tobaccoSmokerThread = new Thread(tobaccoSmoker.MakeAndSmokeCigarette);
            tobaccoSmokerThread.Name = "Tobacco Smoker";
            Thread paperSmokerThread = new Thread(paperSmoker.MakeAndSmokeCigarette);
            paperSmokerThread.Name = "Paper Smoker";
            Thread matchSmokerThread = new Thread(matchSmoker.MakeAndSmokeCigarette);
            matchSmokerThread.Name = "Match Smoker";

            tobaccoSmokerThread.Start();
            paperSmokerThread.Start();
            matchSmokerThread.Start();

            Thread tobaccoPusherThread = new Thread(agent.TobaccoPusher);
            Thread paperPusherThread = new Thread(agent.PaperPusher);
            Thread matchPusherThread = new Thread(agent.MatchPusher);

            tobaccoPusherThread.Start();
            paperPusherThread.Start();
            matchPusherThread.Start();

            Thread ingredientAdder = new Thread(() => AddIngredients(agent));
            ingredientAdder.Start();
        }

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
