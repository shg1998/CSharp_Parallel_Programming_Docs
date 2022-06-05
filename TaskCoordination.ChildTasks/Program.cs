using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCoordination.ChildTasks
{
    internal class Program
    {
        private static void Main()
        {
            var parent = new Task(() =>
            {
                // detached
                var child = new Task(() =>
                {
                    Console.WriteLine("Child Process started");
                    Thread.Sleep(3800);
                    Console.WriteLine("Child Process Finished");
                },TaskCreationOptions.AttachedToParent);// important:)

                var completionHandlerTask = child.ContinueWith(t =>
                {
                    Console.WriteLine($"Task {t.Id}'s state is {t.Status}");
                },TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnRanToCompletion);

                var failHandlerTask = child.ContinueWith(t =>
                {
                    Console.WriteLine($"Failed Task {t.Id} and its status is :  {t.Status}");
                },TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);
                child.Start();
            });
            parent.Start();

            try
            {
                parent.Wait();
            }
            catch (AggregateException e)
            {
                e.Handle(e=>true);
            }

        }
    }
}
