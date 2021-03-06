using Lazy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LazyTests
{
    [TestClass]
    public class LazyTests
    {
        [TestMethod]
        public void SmokeTest()
        {
            Func<int> square = () => 5;
            var lazy = LazyFactory<int>.Create(square);

            Assert.AreEqual(5, lazy.Get());
            Assert.AreEqual(5, lazy.Get());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LazyNullExceptionTest()
        {
            Func<int> supplier = null;
            LazyFactory<int>.Create(supplier);
        }

        [TestMethod]
        public void StupidLazyStringTest()
        {
            Func<string> toLower = () => "OlOLo".ToLower();
            var lazy = LazyFactory<string>.Create(toLower);
            Assert.AreEqual("ololo", lazy.Get());
            Assert.AreEqual("ololo", lazy.Get());
        }

        [TestMethod]
        public void IncrementLazyTest()
        {
            var counter = 0;

            Func<int> increment = () => ++counter;

            var lazy = LazyFactory<int>.Create(increment);

            for (var i = 0; i < 100; ++i)
            {
                var testGet = lazy.Get();
                Assert.AreEqual(1, lazy.Get());
            }

            Assert.AreEqual(1, counter);
        }
    }
}
