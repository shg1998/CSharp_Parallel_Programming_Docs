using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mutex
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
            this._balanced += amount;
        }

        public void WithDraw(int amount)
        {
            this._balanced -= amount;
        }

        public void Transfer(BankAccount where, int amount)
        {
            this.Balance -= amount;
            where.Balance += amount;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var b1 = new BankAccount();
            var b2 = new BankAccount();
            var mutex = new System.Threading.Mutex();
            var mutex2 = new System.Threading.Mutex();
            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 800; j++)
                    {
                        var haveLock = mutex.WaitOne();
                        try
                        {
                            b1.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock) mutex.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 800; j++)
                    {
                        var haveLock = mutex2.WaitOne();
                        try
                        {
                            b2.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock) mutex2.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 800; j++)
                    {
                        var haveLock = WaitHandle.WaitAll(new WaitHandle[] {mutex, mutex2});
                        try
                        {
                            b1.Transfer(b2,1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex2.ReleaseMutex();
                                mutex.ReleaseMutex();
                            }
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"final balance 1 is : {b1.Balance}");
            Console.WriteLine($"final balance 2  is : {b2.Balance}");
        }
    }
}
