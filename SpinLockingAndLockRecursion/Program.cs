using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpinLockingAndLockRecursion
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
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var b = new BankAccount();
            var sl = new SpinLock();
            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 800; j++)
                    {
                        var lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            b.Deposit(105);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            if(lockTaken) sl.Exit();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 800; j++)
                    {
                        var lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            b.WithDraw(105);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            if (lockTaken) sl.Exit();
                        }
                    }
                }));

            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"final balance is : {b.Balance}");
        }
    }
}
