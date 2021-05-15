using System;
using System.Threading;
using MyNUnit.Attributes;

namespace AfterClassTest
{
    public class AfterClassTestClass
    {
        private static int count = 0;

        [AfterClass]
        public static void AfterClassTest()
        {
            if (count != 2)
            {
                throw new InvalidOperationException();
            }
        }

        [Test]
        public void TestMethod1()
        {
            Interlocked.Increment(ref count);
        }

        [Test]
        public void TestMethod2()
        {
            Interlocked.Increment(ref count);
        }
    }
}
