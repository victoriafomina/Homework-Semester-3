using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFTP
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            const int port = 8888;
            var udpClient = new UdpClient();
            Console.WriteLine($"Sending \"Hello!\" to port {port}...");
            var data = Encoding.UTF8.GetBytes("Hello!");
            await udpClient.SendAsync(data, data.Length, "localhost", port);
        }
    }
}
