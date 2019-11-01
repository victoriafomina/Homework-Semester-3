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
            this.udpClient = new UdpClient();
            udpClient.Connect(ip, port);
        }

        public async Task Work()
        {
            await Write();
            await Listen();
        }

        public async Task Write()
        {
            Console.WriteLine("Client is ready to write");
            var message = Console.ReadLine();
            var dataToSend = Encoding.UTF8.GetBytes(message);
            await udpClient.SendAsync(dataToSend, dataToSend.Length);

            if (message == "exit")
            {
                udpClient.Close();
            }
        }

        public async Task Listen()
        {
            Console.WriteLine("Client is ready to listen");

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

        private readonly UdpClient udpClient;
    }
}
