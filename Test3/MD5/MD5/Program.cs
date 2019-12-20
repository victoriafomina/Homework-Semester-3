using System;
using System.IO;

namespace CheckSum
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(Path.GetDirectoryName("..\\Testing"));

            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
            myStopwatch.Start();
            Console.WriteLine(CheckSum.HashDirectory(@"C:\Users\Victoria\Documents\Учеба\Моё"));
            myStopwatch.Stop();
            Console.WriteLine($"Non multithreading: {myStopwatch.ElapsedMilliseconds}");

            System.Diagnostics.Stopwatch myStopwatchMultithreading = new System.Diagnostics.Stopwatch();
            myStopwatchMultithreading.Start();
            Console.WriteLine(CheckSumMultithreading.HashDirectory(@"C:\Users\Victoria\Documents\Учеба\Моё"));
            myStopwatchMultithreading.Stop();
            Console.WriteLine($"Multithreading: {myStopwatchMultithreading.ElapsedMilliseconds}");
        }
    }
}
