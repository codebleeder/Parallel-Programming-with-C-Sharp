using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_._04_TaskCoordination
{
    internal class T18_Barrier
    {
        public static void Run()
        {
            var barrier = new Barrier(2, b =>
            {
                Console.WriteLine($"phase {b.CurrentPhaseNumber} finished");
            });

            var water = Task.Factory.StartNew(() => 
            {
                Console.WriteLine("Putting the kettle on (takes a bit longer)");
                Thread.Sleep(3000);
                barrier.SignalAndWait();

                Console.WriteLine("Pouring water into the cup");
                barrier.SignalAndWait();

                Console.WriteLine("Putting the kettle away");
            });

            var cup = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Finding the nicest cup of tea(fast)");
                barrier.SignalAndWait();

                Console.WriteLine("Adding tea");
                barrier.SignalAndWait();

                Console.WriteLine("Adding sugar");
            });

            var tea = Task.Factory.ContinueWhenAll(new[] { water, cup }, tasks =>
            {
                Console.WriteLine("Enjoy your cup of tea");
            });
            tea.Wait();
        }
    }
}
