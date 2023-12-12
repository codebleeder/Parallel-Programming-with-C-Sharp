using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_._03_ConcurrentCollections
{
    internal class T13_ConcurrentStack
    {
        public static void Run()
        {
            var stack = new ConcurrentStack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);

            int result;
            if (stack.TryPeek(out result))
            {
                Console.WriteLine(result);
            }

            if (stack.TryPop(out result))
            {
                Console.WriteLine(result);
            }

            var items = new int[5];
            if (stack.TryPopRange(items, 0, 5) > 0)
            {
                // returns default values when size is less than 5
                Console.WriteLine(string.Join(", ", items)); 
            }
        }
    }
}
