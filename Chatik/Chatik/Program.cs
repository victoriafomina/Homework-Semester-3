using System;
using System.Net;
using System.Threading.Tasks;

namespace Chatik
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 1)
            {
                var server = new Server(int.Parse(args[0]));
                await server.Work();
            }
            else if (args.Length == 2)
            {
                var client = new Client(int.Parse(args[0]), IPAddress.Parse(args[1]));
                await client.Work();
            }
        }
    }
}
