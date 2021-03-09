using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System;

namespace SimpleFTPServer
{
    /// <summary>
    /// Server that handles two requests: List (listing files in the server's directory) and Get (downloading a
    /// file from the server's directory).
    /// </summary>
    public class Server : IDisposable
    {
        private readonly TcpListener listener;
        private readonly CancellationTokenSource cancellationToken;

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
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
        /// Stops the server.
        /// </summary>
        public void Dispose() => listener.Stop();

        /// <summary>
        /// Handles a client.
        /// </summary>
        private void HandleClient(TcpClient client)
        {
            Task.Run(async () =>
            {
                using (client)
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var reader = new StreamReader(client.GetStream());
                        var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };

                        var request = await reader.ReadLineAsync();

                        await RequestHandler.HandleRequest(request, writer);
                    }
                }
            });
        }
    }
}
