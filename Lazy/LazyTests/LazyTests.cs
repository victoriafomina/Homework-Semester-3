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

    }
}
