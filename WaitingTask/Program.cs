using System;
using System.Threading;
using System.Threading.Tasks;

namespace WaitingTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var t1 = new Task(() =>
            {
                token.ThrowIfCancellationRequested();
                Console.WriteLine("salam bache ha , shorou konam ? ");
                Thread.Sleep(4000);
                Console.WriteLine("khob shrou shod :) ");
            }, token);
            t1.Start();
            var t2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("salam man task 2 hastam va daram miam :)");
                Thread.Sleep(2000);
                Console.WriteLine("OOOOmadam Task2 :///");
            });
            //Task.WaitAll(t1, t2);
            //Task.WaitAny(t1, t2);
            Task.WaitAny(new[] {t1, t2}, 4000, token);
            Console.ReadKey();
            Console.WriteLine("Hello World!");
        }
    }
}
