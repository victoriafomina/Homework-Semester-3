using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyNUnitTests
{
    /// <summary>
    /// Тесты для MyNUnit
    /// </summary>
    [TestClass]
    public class MyNUnitTests
    {
        [TestMethod]
        public void SmokeTest()
        {
            var testInfo = MyNUnit.MyNUnit.Run("..\\..\\..\\..\\TestFolder\\SmokeTest\\SmokeTest\\Assembly");
            Assert.AreEqual(1, testInfo.Count);
            var testMethodsInfo = testInfo.Values;

            foreach (var listOfTests in testMethodsInfo)
            {
                foreach (var test in listOfTests)
                {
                    Assert.AreEqual("SuccessfulRunning", test.Name);
                }
            }

        }
    }
}