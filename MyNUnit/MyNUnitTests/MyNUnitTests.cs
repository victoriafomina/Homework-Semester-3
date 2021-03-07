using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using MyNUnit;

namespace MyNUnitTests
{
    /// <summary>
    /// Тесты для MyNUnit
    /// </summary>
    [TestClass]
    public class MyNUnitTests
    {
        private List<TestMethodInfo> regularTestsResults;
        private List<string> expectedRegularResultsMethods;

        [TestInitialize]
        public void Initialize()
        {
            regularTestsResults = new List<TestMethodInfo>();
            expectedRegularResultsMethods = new List<string>();
            expectedRegularResultsMethods.Add("Success");
            expectedRegularResultsMethods.Add("Ignore");
            expectedRegularResultsMethods.Add("IgnoreException");
            expectedRegularResultsMethods.Add("ExpectedException");
            expectedRegularResultsMethods.Add("FailException");
            expectedRegularResultsMethods.Add("UnexpectedException");
        }

        public void SmokeTest()
        {
            MyNUnit.MyNUnit.Run("..\\..\\..\\..\\TestFolder\\SmokeTest\\SmokeTest\\Assembly");
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void MethodsFormatTest()
        {
            MyNUnit.MyNUnit.Run("..\\..\\..\\..\\TestFolder\\WrongFormatTest\\Assembly");
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void MethodsParametersFormatTest()
        {
            MyNUnit.MyNUnit.Run("..\\..\\..\\..\\TestFolder\\WrongParametersFormatTest\\Assembly");
        }

        [TestMethod]
        public void CorrectMethodsAreTestedTest()
        {
            var resultsTestPath = "..\\..\\..\\..\\TestFolder\\TestResult\\Assembly";

            var regularTestsReport = MyNUnit.MyNUnit.RunTestsAndGetReport(resultsTestPath);

            foreach (var list in regularTestsReport.Values)
            {
                foreach (var info in list)
                {
                    regularTestsResults.Add(info);
                }
            }

            var names = new List<string>();

            foreach (var res in regularTestsResults)
            {
                names.Add(res.Name);
            }

            Assert.AreEqual(names.Intersect(expectedRegularResultsMethods).Count(), expectedRegularResultsMethods.Count);
        }

        [TestMethod]
        public void RegularTestPassedTest()
        {
            var resultsTestPath = "..\\..\\..\\..\\TestFolder\\TestResult\\Assembly";

            var regularTestsReport = MyNUnit.MyNUnit.RunTestsAndGetReport(resultsTestPath);

            foreach (var list in regularTestsReport.Values)
            {
                foreach (var info in list)
                {
                    regularTestsResults.Add(info);
                }
            }

            var successInfo = regularTestsResults.Find(i => i.Name == "Success");

            Assert.IsTrue(successInfo.Passed);
        }

        [TestMethod]
        public void IgnoreTest()
        {
            var resultsTestPath = "..\\..\\..\\..\\TestFolder\\TestResult\\Assembly";

            var regularTestsReport = MyNUnit.MyNUnit.RunTestsAndGetReport(resultsTestPath);

            foreach (var list in regularTestsReport.Values)
            {
                foreach (var info in list)
                {
                    regularTestsResults.Add(info);
                }
            }

            var ignoreInfo = regularTestsResults.Find(i => i.Name == "Ignore");
            var exceptionIgnoreInfo = regularTestsResults.Find(i => i.Name == "IgnoreException");

            Assert.IsTrue(ignoreInfo.Ignored);
            Assert.AreEqual("Let's ignore this method", ignoreInfo.IgnoreMessage);
            Assert.IsTrue(exceptionIgnoreInfo.Ignored);
        }

        [TestMethod]
        public void ExpectedExceptionTest()
        {
            var resultsTestPath = "..\\..\\..\\..\\TestFolder\\TestResult\\Assembly";

            var regularTestsReport = MyNUnit.MyNUnit.RunTestsAndGetReport(resultsTestPath);

            foreach (var list in regularTestsReport.Values)
            {
                foreach (var info in list)
                {
                    regularTestsResults.Add(info);
                }
            }

            var expectedInfo = regularTestsResults.Find(i => i.Name == "ExpectedException");

            Assert.AreEqual(expectedInfo.ExpectedException, expectedInfo.ThrownException);
            Assert.IsTrue(expectedInfo.Passed);
        }

        [TestMethod]
        public void FailExceptionTest()
        {
            var resultsTestPath = "..\\..\\..\\..\\TestFolder\\TestResult\\Assembly";

            var regularTestsReport = MyNUnit.MyNUnit.RunTestsAndGetReport(resultsTestPath);

            foreach (var list in regularTestsReport.Values)
            {
                foreach (var info in list)
                {
                    regularTestsResults.Add(info);
                }
            }

            var failInfo = regularTestsResults.Find(i => i.Name == "FailException");

            Assert.AreEqual(null, failInfo.ExpectedException);
            Assert.AreNotEqual(null, failInfo.ThrownException);
            Assert.IsFalse(failInfo.Passed);
        }

        [TestMethod]
        public void UnexpectedExceptionTest()
        {
            var resultsTestPath = "..\\..\\..\\..\\TestFolder\\TestResult\\Assembly";

            var regularTestsReport = MyNUnit.MyNUnit.RunTestsAndGetReport(resultsTestPath);

            foreach (var list in regularTestsReport.Values)
            {
                foreach (var info in list)
                {
                    regularTestsResults.Add(info);
                }
            }

            var exceptionInfo = regularTestsResults.Find(i => i.Name == "UnexpectedException");

            Assert.AreNotEqual(exceptionInfo.ThrownException, exceptionInfo.ExpectedException);
            Assert.IsFalse(exceptionInfo.Passed);
        }
    }
}