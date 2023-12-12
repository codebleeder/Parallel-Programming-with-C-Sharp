using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_._03_ConcurrentCollections
{
    internal class T14_ConcurrentBag
    {
        public static void Run()
        {
            // unordered list
            var bag = new ConcurrentBag<int>();
            var tasks = new List<Task>();

            for(var i = 0; i < 10; i++)
            {
                // this is important. Do this when passing value to tasks in loop
                // in C#/.NET
                var i1 = i;
                var task = new Task(() =>
                {
                    bag.Add(i1);
                    Console.WriteLine($"{Task.CurrentId} has added {i1}");
                    int result;
                    if (bag.TryPeek(out result))
                    {
                        // Each thread will peek the value it added.
                        // It will not peek a value added by another thread
                        // in the same bag.
                        Console.WriteLine($"{Task.CurrentId} has peeked {result}");
                    }
                });
                tasks.Add(task);
                task.Start();
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
