using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatik
{
    public class Client
    {
        public Client(int port, IPAddress ip)
        {
            this.port = port;
            this.ip = ip;
            this.udpClient = new UdpClient();
            udpClient.Connect(ip, port);
        }

        public async Task Work()
        {
            var sending = Console.ReadLine();
            if (sending == "exit")
            {
                udpClient.Close();
            }
            else
            {
                Console.WriteLine($"Sending {sending} to port {port}...");
                var data = Encoding.UTF8.GetBytes(sending);
                await udpClient.SendAsync(data, data.Length, "localhost", port);
            }
        }

        private int port;
        private IPAddress ip;
        private UdpClient udpClient;
    }
}
