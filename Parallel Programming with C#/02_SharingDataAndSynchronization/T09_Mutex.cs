using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_
{
    internal class T09_Mutex
    {
        public static void Run()
        {
            var ba = new NonAtomicBankAccount();
            var tasks = new List<Task>();
            var mutex = new Mutex();

            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 1000; ++i)
                    {
                        bool haveLock = mutex.WaitOne();
                        try
                        {
                            ba.Deposit(100);
                        }
                        finally
                        {
                            if(haveLock)
                            {
                                mutex.ReleaseMutex();
                            }
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 1000; ++i)
                    {
                        bool haveLock = mutex.WaitOne();
                        try
                        {
                            ba.Withdraw(100);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex.ReleaseMutex();
                            }
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            // we expect the final balance is 0.
            // But it's not always the case, and the result is inconsistent. 
            Console.WriteLine($"final balance is: {ba.Balance}");
            Console.ReadKey();
        }

        public static void Run2()
        {
            var ba = new NonAtomicBankAccount();
            var ba2 = new NonAtomicBankAccount();
            var tasks = new List<Task>();
            var mutex1 = new Mutex();
            var mutex2 = new Mutex();

            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 1000; ++i)
                    {
                        bool haveLock = mutex1.WaitOne();
                        try
                        {
                            ba.Deposit(100);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex1.ReleaseMutex();
                            }
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 1000; ++i)
                    {
                        bool haveLock = mutex2.WaitOne();
                        try
                        {
                            ba2.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for(var i=0; i < 1000; ++i)
                    {
                        bool haveLock = WaitHandle.WaitAll(new[] {mutex1, mutex2});
                        try
                        {
                            ba.Transfer(ba2, 1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex1.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            // we expect the final balance is 0.
            // But it's not always the case, and the result is inconsistent. 
            Console.WriteLine($"final balance ba is: {ba.Balance}");
            Console.WriteLine($"final balance ba2 is: {ba2.Balance}");
            Console.ReadKey();
        }

        public static void Run3()
        {
            const string AppName = "MyApp";
            Mutex mutex;
            try
            {
                mutex = Mutex.OpenExisting(AppName);
                Console.WriteLine($"Sorry, {AppName} is already running");
            }
            catch(WaitHandleCannotBeOpenedException ex)
            {
                Console.WriteLine("we can run the program just fine");
                mutex = new Mutex(false, AppName);
            }
            Console.ReadKey();
            mutex.ReleaseMutex();
        }
    }
}
