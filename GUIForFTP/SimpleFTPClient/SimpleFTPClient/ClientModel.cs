using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimpleFTPClient;

namespace SimpleFTPClient
{
    public class ClientModel
    {
        private Client client = null;

        public async Task RunClientAsync(string server, int port)
        {
            if (client == null)
            {
                client = new Client(server, port);
            }

            client.Run();
            await client.List("current");
        }
    }
}
