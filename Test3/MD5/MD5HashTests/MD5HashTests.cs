using Microsoft.VisualStudio.TestTools.UnitTesting;
using CheckSum;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;
using System.Threading.Tasks;

namespace MD5HashTests
{
    [TestClass]
    public class MD5HashTests
    { 
        [TestMethod]
        public void SmokeTestMethod()
        {
            var pathToTestDirectory = Path.GetDirectoryName("..\\Test3\\MD5\\Testing");
            var md5 = MD5.Create();
            var files = Directory.GetFiles(pathToTestDirectory);
            var file1Bytes = Encoding.UTF8.GetBytes(files[0]);
            var file2Bytes = Encoding.UTF8.GetBytes(files[1]);
            var result = "Testing";
            result += BitConverter.ToString(md5.ComputeHash(file1Bytes));
            result += BitConverter.ToString(md5.ComputeHash(file2Bytes));
            result += md5.ComputeHash(Encoding.UTF8.GetBytes(result));

            Assert.AreEqual(result, CheckSum.CheckSum.HashDirectory("..\\Testing"));
        }
    }
}
