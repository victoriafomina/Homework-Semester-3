using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace SimpleFTPServer
{
    /// <summary>
    /// Server that handles two requests: List (listing files of the server's directory) and Get (saving
    /// file from server's directory).
    /// </summary>
    public class Server
    {
        private TcpListener listener;
        private int port;
        private CancellationTokenSource cancellationToken;

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            this.port = port;
            cancellationToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Runs the server.
        /// </summary>
        public async Task Run()
        {
            listener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync();
                HandleClient(client);
            }
        }

        /// <summary>
        /// Handles a client.
        /// </summary>
        private void HandleClient(TcpClient client)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var reader = new StreamReader(client.GetStream());
                    var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };

                    var request = await reader.ReadLineAsync();

                    RequestHandler.HandleRequest(request, writer);
                }
            });
        }
    }
}
