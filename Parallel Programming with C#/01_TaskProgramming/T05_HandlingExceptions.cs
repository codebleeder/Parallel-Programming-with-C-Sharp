using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_
{
    internal class T05_HandlingExceptions
    {
        public static void Run()
        {
            var task1 = Task.Run(() =>
            {
                throw new InvalidOperationException("Can't do this") { Source = "task1"};
            });

            var task2 = Task.Run(() =>
            {
                throw new AccessViolationException("Can't access this") { Source = "task2"};
            });
            // This will throw exception that is unhandled: 
            // Task.WaitAll(task1, task2);

            // we can handle exceptions by using try-catch block:
            // aggregate exception is specifically designed for the TPL
            try
            {
                Task.WaitAll(task1, task2);
            }
            catch (AggregateException ae) { 
                // we can go through each of the exceptions of the aggregate exceptions
                foreach(var ex in ae.InnerExceptions)
                {
                    Console.WriteLine($"Exception {ex.GetType()} from {ex.Source}");
                }
            }
            Console.WriteLine("main program done");
            Console.ReadKey();
        }

        // we can only handle part of the aggregate exception
        // and propagate the rest of the exception to up the hierarchy
        // lets say we only handle the invalid exception:
        // the code will crash as access violation exception is not handled:
        public static void Run2()
        {
            var task1 = Task.Run(() =>
            {
                throw new InvalidOperationException("Can't do this") { Source = "task1" };
            });

            var task2 = Task.Run(() =>
            {
                throw new AccessViolationException("Can't access this") { Source = "task2" };
            });            

            // we can handle exceptions by using try-catch block:
            // aggregate exception is specifically designed for the TPL
            try
            {
                Task.WaitAll(task1, task2);
            }
            catch (AggregateException ae)
            {
                // handle partial exception
                
                ae.Handle(e =>
                {
                    if(e is InvalidOperationException)
                    {
                        Console.WriteLine("invalid operation exception handled");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                });
            }
            Console.WriteLine("main program done");
            Console.ReadKey();
        }

        // we can handle the rest of the exception by wrapping with another
        // try-catch block:
        public static void Run3()
        {
            try
            {
                Run2();
            }
            catch(AggregateException ae)
            {
                foreach(var ex in ae.InnerExceptions)
                {
                    Console.WriteLine($"Exception {ex.GetType()} from {ex.Source}");
                }               
            }
            
        }
    }
}
