using System;
using System.Threading;

namespace LockRecursion
{
    internal class Program
    {
        static SpinLock sl = new SpinLock(true);
        public static void LockRecursion(int x)
        {
            var lockTaken = false;

            try
            {
                sl.Enter(ref lockTaken);
            }
            catch (LockRecursionException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (lockTaken)
                {
                    Console.WriteLine($"Took a lock , x = {x}");
                    LockRecursion(x - 1);
                    sl.Exit();
                }
                else
                {
                    Console.WriteLine($"Failed to take a lock,x={x}");
                }
            }
        }

        private static void Main(string[] args)
        {
            LockRecursion(5);
        }
    }
}
