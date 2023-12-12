using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_
{
    internal class T04_WaitingForTasks
    {
        public static void Run()
        {
            var cts = new CancellationTokenSource();
            var task = Task.Run(() =>
            {
                Console.WriteLine("I take 5 seconds");
                for(var i=0; i<5; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("I am done");
            }, cts.Token);

            // wait for all the task to complete: 
            Task.WaitAll(); 

            // wait for specific task with the token:
            task.Wait(cts.Token);

            Console.ReadKey();
            Console.WriteLine("main program done");
            Console.ReadKey();
        }

        // suppose you have more than one tasks:
        public static void Run2()
        {
            var cts = new CancellationTokenSource();
            var task1 = Task.Run(() =>
            {
                Console.WriteLine("I take 5 seconds");
                for (var i = 0; i < 5; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("task1: I am done");
            }, cts.Token);

            // wait for 3 seconds
            var task2 = Task.Run(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("task2 done");
            }, cts.Token);

            // using waitAll, wait for all the tasks:
            // Task.WaitAll(task1, task2);

            // WaitAny() waits for any task to complete:
            // Task.WaitAny(task1, task2);

            // you can specify a timeout limit.
            // you need to pass an array of tasks:
            // Task.WaitAny(new[] { task1, task2 }, 4000);

            // You can pass timeout, and the token:
            // If the token is cancelled, then it fires an exception
            // Here the exception hasn't been handled
            Console.ReadKey();
            cts.Cancel();
            Task.WaitAll(new[] { task1, task2 }, 4000, cts.Token);
            

            Console.WriteLine("main program done");
            Console.ReadKey();
        }

        // Assuming we don't cancel the tasks, let's check the status:
        public static void Run3()
        {
            var cts = new CancellationTokenSource();
            var task1 = Task.Run(() =>
            {
                Console.WriteLine("I take 5 seconds");
                for (var i = 0; i < 5; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("task1: I am done");
            }, cts.Token);

            // wait for 3 seconds
            var task2 = Task.Run(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("task2 done");
            }, cts.Token);
            
            Task.WaitAll(new[] { task1, task2 }, 4000, cts.Token);
            Console.WriteLine($"task1 status: {task1.Status}");
            Console.WriteLine($"task2 status: {task2.Status}");

            Console.WriteLine("main program done");
            Console.ReadKey();
        }
    }
}
