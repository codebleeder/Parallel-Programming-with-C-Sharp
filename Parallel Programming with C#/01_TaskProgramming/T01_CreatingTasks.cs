using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_
{
    internal class T01_CreatingTasks
    {
        public static void Run()
        {
            Task.Factory.StartNew(() => Write('.'));
            // or 
            var task = new Task(() => Write('?'));
            task.Start();

            // main thread
            Write('-');            

            Console.ReadKey();
        }

        public static void Run2()
        {
            var task2 = new Task(Write, "hello");
            task2.Start();

            Task.Factory.StartNew(Write, 123);

            Console.ReadKey();
        }

        // when method has a return type
        public static void Run3()
        {
            var text1 = "testing";
            var text2 = "this";

            var task1 = new Task<int>(TextLength, text1);
            task1.Start();

            var task2 = Task.Factory.StartNew<int>(TextLength, text2);

            Console.WriteLine($"Length of {text1} is {task1.Result}");
            Console.WriteLine($"Length of {text2} is {task2.Result}");

            Console.ReadKey();
        }
        public static void Write(char c)
        {
            var i = 1000;
            while (i > 0)
            {
                Console.Write(c);
                --i;
            }
        }

        public static void Write(object o)
        {
            var i = 1000;
            while (i > 0)
            {
                Console.Write(o);
                --i;
            }
        }

        public static int TextLength(object o)
        {
            Console.WriteLine($"Task with id {Task.CurrentId} processing object {o}...");
            return o.ToString().Length;
        }
    }
}
