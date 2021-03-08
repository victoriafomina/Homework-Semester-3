using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFTPClient;
using SimpleFTPServer;
using System.IO;
using System.Threading.Tasks;
using System;

namespace SimpleFTP.Tests
{
    [TestClass]
    public class SimpleFTPTests
    {
        private Server server;
        private Client client;
        private string address;
        private int port;
        private readonly string root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private readonly string folderPath = "\\..\\..\\";
        private string pathToDownloaded;

        [TestInitialize]
        public void Initialize()
        {
            port = 6666;
            address = "127.0.0.1";

            pathToDownloaded = Path.Combine(root, folderPath);
            server = new Server(port);
            client = new Client(address, port);
        }

        [TestMethod]
        public void ListRequestTest()
        {
            Task.Run(async () => await server.Run());

            client.Run();

            var listRequestTestResult = client.List("Test").Result;

            Assert.AreEqual(1, listRequestTestResult.Count);
            Assert.AreEqual(("1 .\\Test\\Folder", true), 
                    ($"{listRequestTestResult.Count} {listRequestTestResult[0].Item1}", listRequestTestResult[0].Item2));
        }

        [TestMethod]
        public void ListRequestFolderInFolderTest()
        {
            Task.Run(async () => await server.Run());

            client.Run();

            var listRequestFolderResult = client.List("Test\\Folder").Result;

            Assert.AreEqual(1, listRequestFolderResult.Count);
            Assert.AreEqual(("1 .\\Test\\Folder\\text.txt", false), ($"{listRequestFolderResult.Count} {listRequestFolderResult[0].Item1}", listRequestFolderResult[0].Item2));
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void GetFileNotFoundTest()
        {
            Task.Run(async () => await server.Run());

            client.Run();

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