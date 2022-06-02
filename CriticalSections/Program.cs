using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CriticalSections
{
    internal class BankAccount
    {
        public object MyLock = new();
        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            lock (MyLock)
                this.Balance += amount;
        }

        public void WithDraw(int amount)
        {
            lock (MyLock)
                this.Balance -= amount;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var b = new BankAccount();
            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 800; j++)
                    {
                        b.Deposit(105);
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 800; j++)
                    {
                        b.WithDraw(105);
                    }
                }));

            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"final balance is : {b.Balance}");
        }
    }
}
