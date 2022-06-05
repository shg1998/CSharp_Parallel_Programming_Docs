using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLock
{
    class Program
    {
        private static ReaderWriterLockSlim padLock = new ReaderWriterLockSlim();
        private static Random random = new Random();
        static void Main(string[] args)
        {
            var x = 0;
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    padLock.EnterReadLock();
                    Console.WriteLine($"entered ReadLock , x= {x}");
                    Thread.Sleep(5000);
                    padLock.ExitReadLock();
                    Console.WriteLine($"Exited Readlock,x={x}");
                }));
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException e)
            {
                e.Handle(e =>
                {
                    Console.WriteLine(e);
                    return true;
                });
            }

            while (true)
            {
                Console.ReadKey();
                padLock.EnterWriteLock();
                Console.WriteLine("writeLock acquired");
                int newValue = random.Next(10);
                x = newValue;
                Console.WriteLine($"Set x = {x}");
                padLock.ExitWriteLock();
                Console.WriteLine("WriteLock released:)");
            }
        }
    }
}
