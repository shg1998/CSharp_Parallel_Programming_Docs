using System;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    class Program
    {
        public static void Write(object o)
        {
            var i = 1000;
            while (i-->0) Console.WriteLine(o);
        }

        public static int GetTextLength(object o)
        {
            Console.WriteLine($"\nTask with id : {Task.CurrentId} processing object : {o}");
            return o.ToString().Length;
        }
        static void Main(string[] args)
        {
            //var t = new Task(Write, "salam");
            //t.Start();
            //Task.Factory.StartNew(Write, 12);
            const string text1 = "testing";
            const string text2 = "aleik";
            var task1 = new Task<int>(GetTextLength, text1);
            task1.Start();
            var task2 = Task.Factory.StartNew<int>(GetTextLength, text2);
            Console.WriteLine($"Length of {text1} is : {task1.Result}");
            Console.WriteLine($"Length of {text2} is : {task2.Result}");
            Console.ReadKey();
        }
    }
}
