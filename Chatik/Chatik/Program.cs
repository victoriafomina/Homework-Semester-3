using System;
using System.Net;

namespace Chatik
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Solution();
        }

        public static async void Solution()
        {
            Console.WriteLine("Input port");
            var port = Console.ReadLine();

            Console.WriteLine("Input IPAddress. Input '!' if you do not have to input ip");
            var ip = Console.ReadLine();

            var portNumber = int.Parse(port);

            if (ip != "!")
            {
                while (true)
                {
                    //var ipAddress = new IPAddress(BitConverter.GetBytes(int.Parse(ip)));
                    //var ip = new IPAddress.
                    var client = new Client(portNumber, IPAddress.Any);
                    await client.Work();
                }
            }
            else
            {
                while (true)
                {
                    var server = new Server(portNumber);
                    await server.Work();
                }
            }
        }
    }
}
