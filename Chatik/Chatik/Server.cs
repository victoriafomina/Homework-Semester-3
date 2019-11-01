using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatik
{
    public class Server
    {
        public Server(int port)
        {
            this.port = port;
            this.udpClient = new UdpClient(port);
            Console.WriteLine($"Listening on port {port}...");
        }

        public async Task Work()
        {
            while (true)
            {
                Console.WriteLine("1 - listen\n2 - write");
                var cases = Console.ReadLine();

                if (cases == "1")
                {
                    await Listen();
                }
                else if (cases == "2")
                {
                    await Write();
                }
            }
        }

        private async Task Listen()
        {
            var received = await udpClient.ReceiveAsync();
            var data = Encoding.UTF8.GetString(received.Buffer);
            Console.WriteLine($"Received: {data}");
        }

        private async Task Write()
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
                //  await udpClient.SendAsync(data, data.Length, "localhost", port);
                await udpClient.SendAsync(data, data.Length);
            }
        }

        private int port;
        private UdpClient udpClient;
    }
}
