using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_
{
    internal class T02_CancelingTasks
    {
        public static void Run()
        {
            var cancellationTokenSrc = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSrc.Token;

            var task = new Task(() =>
            {
                int i = 0;
                while(true)
                {
                    if(cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"{i++}\t");
                    }
                }

            }, cancellationToken);
            task.Start();

            Console.ReadKey();
            cancellationTokenSrc.Cancel();

            Console.WriteLine("main program done");
            Console.ReadKey();
        }

        // This is the recommended way: throw OperationCanceled Exception
        // It won't cause exception on the higher level
        public static void Run2()
        {
            var cancellationTokenSrc = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSrc.Token;

            var task = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw new OperationCanceledException();                        
                    }
                    else
                    {
                        Console.WriteLine($"{i++}\t");
                    }
                }

            }, cancellationToken);
            task.Start();

            Console.ReadKey();
            cancellationTokenSrc.Cancel();

            Console.WriteLine("main program done");
            Console.ReadKey();
        }

        // shorter version - no need of if-else statement above:
        // use ThrowIfCancellationRequested()
        public static void Run3()
        {
            var cancellationTokenSrc = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSrc.Token;

            var task = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}\t");
                }

            }, cancellationToken);
            task.Start();

            Console.ReadKey();
            cancellationTokenSrc.Cancel();

            Console.WriteLine("main program done");
            Console.ReadKey();
        }

        // to notify about cancellation, we can subscribe using the token:
        public static void Run4()
        {
            var cancellationTokenSrc = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSrc.Token;

            // subscribe:
            cancellationToken.Register(() =>
            {
                Console.WriteLine("Cancelation has been requested");
            });
            var task = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}\t");
                }

            }, cancellationToken);
            task.Start();

            // wait until first task finishes in another task:
            Task.Factory.StartNew(() =>
            {
                cancellationToken.WaitHandle.WaitOne();
                Console.WriteLine("wait handle released, cancelation was requested");
            });

            Console.ReadKey();
            cancellationTokenSrc.Cancel();

            Console.WriteLine("main program done");
            Console.ReadKey();
        }

        // composite cancelation tokens:
        public static void Run5()
        {
            var planned = new CancellationTokenSource();
            var preventative = new CancellationTokenSource();
            var emergency = new CancellationTokenSource();

            // composite cancelation token source 
            var paranoid = CancellationTokenSource.CreateLinkedTokenSource(
                planned.Token, preventative.Token, emergency.Token
                );
                        
            var task = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    paranoid.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}\t");
                    Thread.Sleep( 1000 );
                }

            }, paranoid.Token);
            task.Start();

            

            Console.ReadKey();
            // we can use any of the source to cancel: planned, preventative,
            // or emergency. Since they are linked to paranoid, it will cancel:
            planned.Cancel();

            Console.WriteLine("main program done");
            Console.ReadKey();
        }
    }
}
