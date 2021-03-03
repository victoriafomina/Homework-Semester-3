using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleFTPClient
{
    /// <summary>
    /// Client that can make two requests: List (listing files in the server's directory) and Get (downloading a
    /// file from the server's directory).
    /// </summary>
    public class Client
    {
        private string server;

        private int port;

        private TcpClient client = null;

        public Client(string server, int port)
        {
            this.server = server;
            this.port = port;
        }

        /// <summary>
        /// Runs the client.
        /// </summary>
        public void Run()
        {
            client = new TcpClient(server, port);
        }

        /// <summary>
        /// Does listing.
        /// </summary>
        /// <exception cref="ClientNotRunningException">Thrown when the client is not running.</exception>
        public async Task<string> List(string path)
        {
            CheckRunning();

            var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            var reader = new StreamReader(client.GetStream());

            await writer.WriteLineAsync("1" + path);

            return await reader.ReadLineAsync();
        }

        /// <summary>
        /// Downloads the file from the server.
        /// </summary>
        /// <exception cref="ClientNotRunningException">Thrown when the client is not running.</exception>
        public async Task Get(string downloadFrom, string downloadTo)
        {
            CheckRunning();

            var temp = downloadFrom.Split('\\');
            var fileName = temp[temp.Length - 1];

            var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            var reader = new StreamReader(client.GetStream());

            await writer.WriteLineAsync("2" + downloadFrom);

            var response = await reader.ReadLineAsync();

            var size = int.Parse(response);

            if (size == -1)
            {
                throw new FileNotFoundException("File was not found on the server!");
            }

            using (var fileStream = new FileStream(downloadTo + fileName, FileMode.CreateNew))
            {
                await reader.BaseStream.CopyToAsync(fileStream);
            }
        }

        /// <summary>
        /// Checks if the client is running.
        /// </summary>
        private void CheckRunning()
        {
            if (client == null || !client.Connected)
            {
                throw new ClientNotRunningException("Client is not running!");
            }
        }

        /// <summary>
        /// Stops the client.
        /// </summary>
        public void Close() => client.Close();
    }
}