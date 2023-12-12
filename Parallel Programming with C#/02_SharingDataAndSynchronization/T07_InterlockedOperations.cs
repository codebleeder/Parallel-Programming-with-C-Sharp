using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_
{
    internal class T07_InterlockedOperations
    {
        public static void Run()
        {
            var ba = new InterlockedBankAccount();
            var tasks = new List<Task>();

            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 1000; ++i)
                    {
                        ba.Deposit(100);
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 1000; ++i)
                    {
                        ba.Withdraw(100);
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            // we expect the final balance is 0.
            // But it's not always the case, and the result is inconsistent. 
            Console.WriteLine($"final balance is: {ba.Balance}");
            Console.ReadKey();
        }
    }

    public class InterlockedBankAccount
    {
        private int balance;
        public int Balance { 
            get { return balance; } 
            private set { balance = value; }
        }
        
        // lock-free programming using Interlocked(): 
        public void Deposit(int amount)
        {                        
            Interlocked.Add(ref balance, amount);
        }
        public void Withdraw(int amount)
        {
            Interlocked.Add(ref balance, -amount);
        }
    }
}
