using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InterLockedOperations
{
    internal class BankAccount
    {
        private int _balanced;

        public int Balance
        {
            get => _balanced;
            private set => _balanced = value;
        }

        public void Deposit(int amount)
        {
            Interlocked.Add(ref this._balanced, amount);
            //Thread.MemoryBarrier();
        }

        public void WithDraw(int amount)
        {
            Interlocked.Add(ref this._balanced, -amount);
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
