using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatik
{
    public class Server
    {
        public Server(int port)
        {
            this.udpClient = new UdpClient(port);
            working = true;
        }

        public async Task Work()
        {
            while (working)
            {
                await Listen();
                await Write();
            }
        }

        private async Task Listen()
        {
           
            Console.WriteLine("Server is ready to listen");

            UdpReceiveResult getInfo = await udpClient.ReceiveAsync();

            var getStr = getInfo.ToString();

            if (getStr == "exit")
            {
                udpClient.Close();
                working = false;
            }
            else
            {
                Console.WriteLine(getStr);
            }
            
        }

        private async Task Write()
        {
            if (working)
            {
                Console.WriteLine("Server is ready to write");

                var message = Console.ReadLine();
                var dataToSend = Encoding.UTF8.GetBytes(message);
                await udpClient.SendAsync(dataToSend, dataToSend.Length);

                if (message == "exit")
                {
                    udpClient.Close();
                    working = false;
                }
            }         
        }

        private readonly UdpClient udpClient;
        private bool working;
    }
}
