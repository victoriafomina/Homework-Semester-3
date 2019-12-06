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
            ManualResetEvent sync = new ManualResetEvent(false);

            var threads = new Thread[numberOfThreads];
            var results = new int[numberOfThreads];

            for (var i = 0; i < numberOfThreads; ++i)
            {
                threads[i] = new Thread(() => {
                    results[i] = lazy.Get();
                });

                sync.Set();
            }

            sync.WaitOne();

            for (var i = 0; i < numberOfThreads; ++i)
            {
                threads[i].Start();
            }

            for
        }
    }
}
