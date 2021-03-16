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
        private TcpClient client = null;

        /// <summary>
        /// Runs the client.
        /// </summary>
        public async void Run(string server, int port)
        {
            client = new TcpClient();
            await client.ConnectAsync(server, port);
        }

        /// <summary>
        /// Does listing.
        /// </summary>
        /// <exception cref="ClientNotRunningException">Thrown when the client is not running.</exception>
        /// <exception cref="AccessToDirectoryOnServerDeniedException">Exception is thrown when the client writer gets "denied" string from server.</exception>
        public async Task<List<(string, bool)>> List(string path)
        {
            if (!CheckRunning())
            {
                throw new ClientNotRunningException();
            }

            var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            var reader = new StreamReader(client.GetStream());

            if (path.ToLower() == "1current")
            {
                await writer.WriteLineAsync(path);
            }
            else
            {
                await writer.WriteLineAsync("1" + path);
            }

            var response = await reader.ReadLineAsync();

            if (response == "denied")
            {
                throw new AccessToDirectoryOnServerDeniedException();
            }

            return ParseListResponse(response);
        }

        /// <summary>
        /// Parses the result of list response.
        /// </summary>
        /// <exception cref="InvalidResponseException">Thrown when first symbol is not a number.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when request directory was not found.</exception>
        private List<(string, bool)> ParseListResponse(string response)
        {
            var splitResponse = response.Split(' ');

            if (!int.TryParse(splitResponse[0], out var numberOfFilesAndFolders))
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
                parsedResponse.Add((splitResponse[i], bool.Parse(splitResponse[i + 1])));
            }

            return parsedResponse;
        }

        /// <summary>
        /// Downloads the file from the server.
        /// </summary>
        /// <exception cref="ClientNotRunningException">Thrown when the client is not running.</exception>
        public async Task Get(string downloadFrom, string downloadTo)
        {
            if (!CheckRunning())
            {
                throw new ClientNotRunningException();
            }

            var formattedDownloadFrom = downloadFrom.Substring(2);
            var temp = formattedDownloadFrom.Split('\\');
            var fileName = temp[temp.Length - 1];

            var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            var reader = new StreamReader(client.GetStream());

            await writer.WriteLineAsync("2" + formattedDownloadFrom);

            var response = await reader.ReadLineAsync();

            var size = int.Parse(response);

            if (size == -1)
            {
                throw new FileNotFoundException("File was not found on the server!");
            }

            var fileStream = new FileStream(downloadTo + fileName, FileMode.OpenOrCreate);
            await reader.BaseStream.CopyToAsync(fileStream);
        }

        /// <summary>
        /// Checks if the client is running.
        /// </summary>
        public bool CheckRunning() => client != null && client.Connected;

        /// <summary>
        /// Closes TCP-client and disposes of resources.
        /// </summary>
        public void Dispose()
        {
            client.Close();
            client.Dispose();
        }
    }
}