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
        private string root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private string folderPath = "\\..\\..\\";
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

            server.Stop();
            client.Close();

            Assert.AreEqual("1 .\\Test\\Folder true ", listRequestTestResult);
        }

        [TestMethod]
        public void ListRequestFolderInFolderTest()
        {
            Task.Run(async () => await server.Run());

            client.Run();

            var listRequestFolderResult = client.List("Test\\Folder").Result;

            server.Stop();

            client.Close();Assert.AreEqual("1 .\\Test\\Folder\\text.txt false ", listRequestFolderResult);
        }

        [TestMethod]
        public void GetTest()
        {
            var pathToFile = Path.Combine(pathToDownloaded, "test.txt");

            if (File.Exists(pathToFile))
            {
                File.Delete(pathToFile);
            }

            Task.Run(async () =>
            {
                await server.Run();

                client.Run();

                await client.Get("Test\\Folder\\test.txt", pathToDownloaded);

                server.Stop();

                Assert.IsTrue(File.Exists(pathToFile));

                File.Delete(pathToFile);

                client.Close();
            });
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void GetFileNotFoundTest()
        {
            Task.Run(async () => await server.Run());

            client.Run();

            client.Get("olololo", pathToDownloaded).Wait();

            server.Stop();
            client.Close();
        }
    }
}