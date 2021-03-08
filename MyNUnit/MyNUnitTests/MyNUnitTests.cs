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
                    Assert.IsFalse(test.Ignored);
                    Assert.AreEqual(null, test.ExpectedException);
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
                    Assert.AreEqual(null, test.IgnoreMessage);
                    Assert.AreEqual(test.ExpectedException, test.ThrownException);
                    Assert.IsTrue(test.Passed);
                    Assert.IsFalse(test.Ignored);
                }
            }
        }

        [TestMethod]
        public void IgnoreTest()
        {
            var testInfo = MyNUnit.MyNUnit.Run($"{basePath}IgnoreMethodTest\\Assembly");
            Assert.AreEqual(1, testInfo.Count);
            var testMethodsInfo = testInfo.Values;
            var count = 0;

            foreach (var listOfTests in testMethodsInfo)
            {
                var ignoreMethodName = "IgnoreMeMethod";
                var successfulPassMethodName = "SuccessfulPassing";

                foreach (var test in listOfTests)
                {
                    Assert.IsTrue(test.Name == ignoreMethodName || test.Name == successfulPassMethodName);

                    if (test.Name == ignoreMethodName)
                    {
                        Assert.IsTrue(test.Ignored);
                        Assert.AreEqual("Ignore it", test.IgnoreMessage);
                    }

                    if (test.Name == successfulPassMethodName)
                    {
                        Assert.IsFalse(test.Ignored);
                        Assert.AreNotEqual(0, test.Time.TotalMilliseconds);
                    }

                    ++count;
                }
            }

            Assert.AreEqual(2, count);
        }
    }
}