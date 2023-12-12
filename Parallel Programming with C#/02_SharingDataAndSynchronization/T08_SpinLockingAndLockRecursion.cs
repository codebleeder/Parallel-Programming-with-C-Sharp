using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_
{
    internal class T08_SpinLockingAndLockRecursion
    {
        // using SpinLock:
        public static void Run()
        {
            var ba = new NonAtomicBankAccount();
            var tasks = new List<Task>();
            var spinlock = new SpinLock();

            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 1000; ++i)
                    {
                        bool lockTaken = false;
                        try
                        {
                            spinlock.Enter(ref lockTaken);
                            ba.Deposit(100);
                        }
                        finally { 
                            if(lockTaken)
                            {
                                spinlock.Exit();
                            }
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 1000; ++i)
                    {
                        bool lockTaken = false;
                        try
                        {
                            spinlock.Enter(ref lockTaken);
                            ba.Withdraw(100);
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                spinlock.Exit();
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

        // lock recursion
        static SpinLock sl = new SpinLock();
        public static void LockRecursion(int x)
        {
            bool LockTaken = false;
            try
            {
                sl.Enter(ref LockTaken);
            }
            catch(LockRecursionException ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
            finally { 
                if(LockTaken)
                {
                    Console.WriteLine($"Took a lock, x = {x}");
                    LockRecursion(x - 1);
                    sl.Exit();
                }
                else
                {
                    Console.WriteLine($"We failed to take a lock, x = {x}");
                }
            }

        }

        public static void Run2()
        {
            LockRecursion(5);
        }
    }
}
