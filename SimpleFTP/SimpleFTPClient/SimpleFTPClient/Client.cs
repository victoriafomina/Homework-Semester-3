using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleFTPClient
{
    /// <summary>
    /// Client that can make two requests: List (listing files in the server's directory) and Get (downloading a
    /// file from the server's directory).
    /// </summary>
    public class Client : IDisposable 
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
        public async void Run()
        {
            var tcpclient = new TcpClient();
            await tcpclient.ConnectAsync(server, port);
        }

        /// <summary>
        /// Does listing.
        /// </summary>
        /// <exception cref="ClientNotRunningException">Thrown when the client is not running.</exception>
        public async Task<List<(string, bool)>> List(string path)
        {
            CheckRunning();

            var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            var reader = new StreamReader(client.GetStream());

            await writer.WriteLineAsync("1" + path);
            var response = await reader.ReadLineAsync();

            return ParseListResponse(response);
        }

        /// <summary>
        /// Parses the result of list response.
        /// </summary>
        /// <exception cref="InvalidResponseException">Thrown when first symbol is not a number.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when request directory was not found.</exception>
        private List<(string, bool)> ParseListResponse(string response)
        {
            var splitedResponse = response.Split(' ');

            int numberOfFilesAndFolders;

            if (!int.TryParse(splitedResponse[0], out numberOfFilesAndFolders))
            {
                throw new InvalidResponseException(response);
            }

            if (numberOfFilesAndFolders == -1)
            {
                throw new DirectoryNotFoundException(response);
            }

            var parsedResponse = new List<(string, bool)>();

            for (var i = 1; i < numberOfFilesAndFolders * 2; i += 2)
            {
                parsedResponse.Add((splitedResponse[i], bool.Parse(splitedResponse[i + 1])));
            }

            return parsedResponse;
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
                throw new ClientNotRunningException();
            }
        }

        /// <summary>
        /// Stops the client.
        /// </summary>
        public void Close() => client.Close();

        /// <summary>
        /// Disposes of resouces.
        /// </summary>
        public void Dispose() => client.Dispose();
    }
}