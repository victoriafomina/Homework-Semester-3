using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFTPClient;
using SimpleFTPServer;
using System.IO;
using System;

namespace SimpleFTP.Tests
{
    [TestClass]
    public class SimpleFTPTests
    {
        private Server server;
        private Client client;
        private readonly string address = "127.0.0.1";
        private readonly int port = 6666;
        private readonly string root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private readonly string folderPath = "\\..\\..\\";
        private string pathToDownloaded;

        [TestInitialize]
        public void Initialize()
        {
            pathToDownloaded = Path.Combine(root, folderPath);
            server = new Server(port);
            client = new Client();
        }

        [TestMethod]
        public async void ListRequestTest()
        {
            await server.Run();

            client.Run(address, port);

            var listRequestTestResult = client.List("Test").Result;

            Assert.AreEqual(1, listRequestTestResult.Count);
            Assert.AreEqual(("1 .\\Test\\Folder", true), 
                    ($"{listRequestTestResult.Count} {listRequestTestResult[0].Item1}", listRequestTestResult[0].Item2));
        }

        [TestMethod]
        public async void ListRequestFolderInFolderTest()
        {
            await server.Run();

            client.Run(address, port);

            var listRequestFolderResult = client.List("Test\\Folder").Result;

            Assert.AreEqual(1, listRequestFolderResult.Count);
            Assert.AreEqual(("1 .\\Test\\Folder\\text.txt", false), ($"{listRequestFolderResult.Count} {listRequestFolderResult[0].Item1}", listRequestFolderResult[0].Item2));
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public async void GetFileNotFoundTest()
        {
            await server.Run();

            client.Run(address, port);

            client.Get("olololo", pathToDownloaded).Wait();
        }

        [TestCleanup]
        public void CleanUp()
        {
            server.Dispose();
            client.Dispose();
        }
    }
}