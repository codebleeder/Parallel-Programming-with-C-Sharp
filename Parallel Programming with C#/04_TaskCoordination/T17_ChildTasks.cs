using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_._04_TaskCoordination
{
    internal class T17_ChildTasks
    {
        public static void Run()
        {
            var parent = new Task(() =>
            {
                // AttachedToParent option establishes a parent-child relationship
                // waiting on the parent automatically gets you waiting for the child as well:
                var child = new Task(() =>
                {
                    // if you throw exception, then failHandler defined below will run
                    // throw new Exception();
                    Console.WriteLine("child task starting");
                    Thread.Sleep(3000);
                    Console.WriteLine("child task end");
                }, TaskCreationOptions.AttachedToParent);

                // this will run after child finishes
                // continuation options allows us to attach this task to the parent as well:
                var completionHandler = child.ContinueWith(t =>
                {
                    Console.WriteLine($"child task completed: {t.Id} status is: {t.Status}");
                }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnRanToCompletion);

                // this will run only if the child task faults
                var failHandler = child.ContinueWith(t =>
                {
                    Console.WriteLine($"child task failed: {child.Id} status is: {child.Status}");
                }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);

                child.Start();
            });

            parent.Start();

            try
            {
                parent.Wait();
            }
            catch (AggregateException ex)
            {
                ex.Handle(e => true);
            }
        }
    }
}
