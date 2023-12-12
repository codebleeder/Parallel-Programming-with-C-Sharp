using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_
{
    internal class T10_ReaderWriterLock
    {
        static ReaderWriterLockSlim padlock = new ReaderWriterLockSlim();

        public static void Run()
        {
            var x = 0; // common resource
            var tasks = new List<Task>();

            for(var i=0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    padlock.EnterReadLock();
                    Console.WriteLine($"Entered read lock, x = {x}");
                    Thread.Sleep( 5000 );

                    padlock.ExitReadLock();
                    Console.WriteLine($"Exited read lock, x = {x}");
                }));
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch( AggregateException ae )
            {
                ae.Handle((e) =>
                {
                    Console.WriteLine(e);
                    return true;
                });
            }

            // write in the main thread:
            while(true)
            {
                Console.ReadKey();
                padlock.EnterWriteLock();
                Console.WriteLine("write lock acquired");
                ++x;
                Console.WriteLine($"set x = {x}");
                padlock.ExitWriteLock();
                Console.WriteLine("write lock released");
            }
        }

        // upgradeableReadLock:
        public static void Run2()
        {
            var x = 0; // common resource
            var tasks = new List<Task>();

            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    padlock.EnterUpgradeableReadLock();
                    // we can upgrade this to a write lock
                    // depending on the condition. 
                    if(i % 2 == 0)
                    {
                        padlock.ExitWriteLock();
                        x = 123;
                        padlock.ExitWriteLock();
                    }
                    Console.WriteLine($"Entered read lock, x = {x}");
                    Thread.Sleep(5000);

                    padlock.EnterUpgradeableReadLock();
                    Console.WriteLine($"Exited read lock, x = {x}");
                }));
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle((e) =>
                {
                    Console.WriteLine(e);
                    return true;
                });
            }

            // write in the main thread:
            while (true)
            {
                Console.ReadKey();
                padlock.EnterWriteLock();
                Console.WriteLine("write lock acquired");
                ++x;
                Console.WriteLine($"set x = {x}");
                padlock.ExitWriteLock();
                Console.WriteLine("write lock released");
            }
        }
    }
}
