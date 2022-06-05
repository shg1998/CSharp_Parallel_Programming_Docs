using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ConcurrentDictionary
{
    class Program
    {
        private static ConcurrentDictionary<string, string> capitals = new ConcurrentDictionary<string, string>();

        public static void addParis()
        {
            var success = capitals.TryAdd("France", "paris");
            var who = Task.CurrentId.HasValue ? ("Task"+Task.CurrentId) : "Main Thread";
            Console.WriteLine(who);
        }
        static void Main(string[] args)
        {
            Task.Factory.StartNew(addParis).Wait();
            addParis();

            capitals["Iran"] = "Tehran";

            capitals.AddOrUpdate("Iran", "Esfahan", (key, old) => old + "--> Esfahan");
            Console.WriteLine(capitals["Iran"]);
        }
    }
}
