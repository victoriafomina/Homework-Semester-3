using System;
using System.IO;

namespace MyNUnit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Path was not passed through args!");

                return;
            }

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine($"Directory \'{args[0]}\' does not exist!");

                return;
            }

            var testRunner = new MyNUnit();
            testRunner.Run(args[0]);
            testRunner.PrintResult();
        }
    }
}
