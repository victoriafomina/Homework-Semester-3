using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatik
{
    public class Server
    {
        public Server(int port) => this.udpClient = new UdpClient(port);

        public async Task Work()
        {
            await Listen();
            await Write();
        }

        private async Task Listen()
        {
            Console.WriteLine("Server is ready to listen");

            UdpReceiveResult getInfo = await udpClient.ReceiveAsync();

            var getStr = getInfo.ToString();

            if (getStr == "exit")
            {
                udpClient.Close();
            }
            else
            {
                Console.WriteLine(getStr);
            }
        }

        private async Task Write()
        {
            Console.WriteLine("Server is ready to write");

            var message = Console.ReadLine();
            var dataToSend = Encoding.UTF8.GetBytes(message);
            await udpClient.SendAsync(dataToSend, dataToSend.Length);

            if (message == "exit")
            {
                udpClient.Close();
            }
        }

        private readonly UdpClient udpClient;
    }
}
