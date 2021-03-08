using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyNUnitTests
{
    [TestClass]
    public class MyNUnitTests
    {
        private readonly string basePath = "..\\..\\..\\..\\TestFolder\\";

        [TestMethod]
        public void SmokeTest()
        {
            var testInfo = MyNUnit.MyNUnit.Run($"{basePath}SmokeTest\\Assembly");
            Assert.AreEqual(1, testInfo.Count);
            var testMethodsInfo = testInfo.Values;

            foreach (var listOfTests in testMethodsInfo)
            {
                foreach (var test in listOfTests)
                {
                    Assert.AreEqual("SuccessfulRunning", test.Name);
                    Assert.IsTrue(test.Passed);
                   // Assert.IsFalse(test.Ignored);
                   // Assert.AreEqual(null, test.ExpectedException);
                }
            }
        }

        [TestMethod]
        public void ThrownExceptionTest()
        {
            var testInfo = MyNUnit.MyNUnit.Run($"{basePath}ThrownExceptionTest\\Assembly");
            Assert.AreEqual(1, testInfo.Count);
            var testMethodsInfo = testInfo.Values;

            foreach (var listOfTests in testMethodsInfo)
            {
                foreach (var test in listOfTests)
                {
                    Assert.AreEqual("ThrowingExceptionMethod", test.ThrownException.Name);
                    Assert.AreEqual("Ignore it because ignore it", test.Name);
                    Assert.AreEqual(test.ExpectedException, test.ThrownException);
                    Assert.IsTrue(test.Passed);
                    Assert.IsFalse(test.Ignored);
                }
            }

        }
    }
}