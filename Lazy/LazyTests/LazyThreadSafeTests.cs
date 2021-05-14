using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lazy;
using System;
using System.Threading;
using System.Collections.Generic;

namespace LazyTests
{
    [TestClass]
    public class ThreadSafeLazyTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThreadSafeNullExceptionTest()
        {
            Func<string> supplier = null;
            LazyFactory<string>.CreateThreadSafe(supplier);
        }

        [TestMethod]
        public void ThreadSafeLazyStringTest()
        {
            Func<string> toLower = () => "OlOLo".ToLower();
            var lazy = LazyFactory<string>.CreateThreadSafe(toLower);

            var threads = new List<Thread>();

            for (var i = 0; i < 100; ++i)
            {
                threads.Add(new Thread(() =>
                {
                    Assert.AreEqual("ololo", lazy.Get());
                }));
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        [TestMethod]
        public void ThreadSafeIncrementLazyTest()
        {
            var counter = 0;

            Func<int> increment = () => ++counter;

            var lazy = LazyFactory<int>.CreateThreadSafe(increment);

            var threads = new List<Thread>();

            for (var i = 0; i < 100; ++i)
            {
                threads.Add(new Thread(() =>
                {
                    var testGet = lazy.Get();
                    Assert.AreEqual(1, lazy.Get());
                }));
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.AreEqual(1, counter);
        }
    }
}
