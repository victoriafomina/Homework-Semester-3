using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFTPServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const int port = 8888;
            var udpClient = new UdpClient(port);
            Console.WriteLine($"Listening on port {port}...");
            var received = await udpClient.ReceiveAsync();
            var data = Encoding.UTF8.GetString(received.Buffer);
            Console.WriteLine($"Received: {data}");

            if (data == null || data.Length < 3)
            {
                Console.WriteLine("Directory does not exist!");
            }
            else if (data[0] == '1')
            {
                var result = List(data.Substring(2, data.Length - 2));
            }
            else if (data[0] == '2')
            {
                var result = Get(data.Substring(2, data.Length - 2));
            }
            else
            {
                Console.WriteLine("Directory does not exist!");
            }
        }

        private static string List(string path)
        {

            return "";
        }
        private static string Get(string path)
        {

            return "";
        }
    }
}
