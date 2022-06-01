using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCancellation
{
    // how can i actually cancel this infinity task ? :)
    internal class Program
    {
        private static void Main(string[] args)
        {
            //// canonical way : 
            //var cts = new CancellationTokenSource();
            //var token = cts.Token;
            //token.Register(() =>
            //{
            //    Console.WriteLine("cancellation has been requested :)");
            //});
            //var t = new Task(() =>
            //{
            //    var inc = 0;
            //    while (true)
            //    {
            //        //if(token.IsCancellationRequested)
            //        //    throw new OperationCanceledException();
            //        // == with above
            //        token.ThrowIfCancellationRequested();
            //        Console.WriteLine($"{inc++}\n");
            //    }
            //},token);
            //t.Start();
            //Task.Factory.StartNew(() =>
            //{
            //    token.WaitHandle.WaitOne();
            //    Console.WriteLine("wait handle released,cancellation was requested :)");
            //});
            //Console.ReadKey();
            //cts.Cancel();



            // another : 
            var planned = new CancellationTokenSource();
            var preventative = new CancellationTokenSource();
            var emergency = new CancellationTokenSource();

            var paranoid = CancellationTokenSource.CreateLinkedTokenSource(planned.Token,preventative.Token,emergency.Token);

            Task.Factory.StartNew(() =>
            {
                var i = 0;
                while (true)
                {
                    paranoid.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}\n");
                    Thread.Sleep(1000);
                }
            },paranoid.Token);

            Console.ReadKey();
            emergency.Cancel();
            
            Console.ReadKey();
        }
    }
}
