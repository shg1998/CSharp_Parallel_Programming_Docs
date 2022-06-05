using System;
using System.Threading.Tasks;

namespace TaskCoordination.Continuations
{
    internal class Program
    {
        private static void Main()
        {
            // desc 1 : 
            //var t1 = Task.Factory.StartNew(() =>
            //{
            //    Console.WriteLine("boiling water :)");
            //});
            //var t2 = t1.ContinueWith(t =>
            //{
            //    Console.WriteLine($"Completed Task {t.Id} , use water : )");
            //});
            //// waiting for Task 2 : 
            //t2.Wait();

            // desc 2: 
            var task = Task.Factory.StartNew(() => "Task 1");
            var task2 = Task.Factory.StartNew(() => "Task 2");
            var task3 = Task.Factory.ContinueWhenAll(new[] {task, task2}, // or ContinueWhenAny
                tasks =>
                {
                    Console.WriteLine("Tasks Completed : ");
                    foreach (var t in tasks) Console.WriteLine(" / " + t.Result);
                    Console.WriteLine("All Tasks Done ://");
                });

            task3.Wait();
        }
    }
}
