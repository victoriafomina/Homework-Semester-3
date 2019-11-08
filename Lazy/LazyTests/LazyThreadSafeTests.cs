using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lazy;
using System;
using System.Threading;

namespace LazyTests
{
    [TestClass]
    public class LazyThreadSafeTests
    {
        [TestMethod]
        public void SmokeTest()
        {
            Func<int> thirdPowerOf5 = () => 5 * 5 * 5;
            var lazy = LazyFactory<int>.CreateThreadSafe(thirdPowerOf5);
            var numberOfThreads = 100;

            var threads = new Thread[numberOfThreads];

            for (var i = 0; i < numberOfThreads; ++i)
            {
                threads[i] = new Thread(() =>
                {
                    Assert.AreEqual(1, lazy.Get());
                });
            }
        }
    }
}
