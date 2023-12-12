using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_._04_TaskCoordination
{
    internal class T16_Continuation
    {
        public static void Run()
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Boiling water");
            });

            var t2 = t1.ContinueWith(t => 
            {
                Console.WriteLine($"completed task {t.Id}");
                Console.WriteLine("pouring water");
            });

            t2.Wait();
        }

        public static void Run2()
        {
            // run when one of them completes:
            var t1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task1");
                return "Task1";
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task2");
                return "Task2";
            });

            // t3 executes when either one of t1 or t2 return:
            var t3 = Task.Factory.ContinueWhenAny(new[] {t1, t2}, 
                t =>
                {
                    Console.WriteLine($"Task completed is: {t.Result}");
                }
            );

            t3.Wait();
        }
    }
}
