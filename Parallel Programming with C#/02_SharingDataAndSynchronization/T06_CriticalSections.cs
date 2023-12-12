using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Programming_with_C_
{
    internal class T06_CriticalSections
    {
        public static void Run()
        {
            var ba = new NonAtomicBankAccount();
            var tasks = new List<Task>();
            
            for(var i=0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => 
                { 
                    for(var i=0; i<1000; ++i)
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

        // using critical section - now the balance is always zero
        public static void Run2()
        {
            var ba = new LockBankAccount();
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
    
    // models
    public class NonAtomicBankAccount
    {
        public int Balance { get; private set; }
        
        // this method is not atomic:
        // op1: temp <- get_balance() + amount
        // op2: set_balance(temp)
        // because of this, the balance in the code above is not always 0.
        public void Deposit(int amount)
        {
            Balance += amount;            
        }
        public void Withdraw(int amount)
        {
            Balance -= amount;
        }

        public void Transfer(NonAtomicBankAccount where, int amount)
        {
            Balance -= amount;
            where.Balance += amount;
        }
    }

    public class LockBankAccount
    {
        public int Balance { get; private set; }
        public object Padlock = new object();        
        public void Deposit(int amount)
        {
            // we make this operation critical
            // only one thread will be able to make the change
            lock(Padlock)
            {
                Balance += amount;
            }            
        }
        public void Withdraw(int amount)
        {
            lock(Padlock)
            {
                Balance -= amount;
            }            
        }
    }
}
