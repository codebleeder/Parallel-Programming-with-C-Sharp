using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_.ConcurrentCollections
{
    internal class T12_ConcurrentQueue
    {
        public static void Run()
        {
            var queue = new ConcurrentQueue<int>();

            queue.Enqueue(0);
            queue.Enqueue(1);

            int result;
            if (queue.TryDequeue(out result))
            {
                Console.WriteLine(result);
            }

            if(queue.TryPeek(out result))
            {
                Console.WriteLine(result);
            }
        }
    }
}
