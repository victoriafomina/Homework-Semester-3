using MyNUnit.Attributes;
using System;
using System.Threading;

namespace BeforeClassTest
{
    public class BeforeClassTestClass
    {
        private static int count = 0;

        [BeforeClass]
        public static void BeforeClassMethod()
        {
            Interlocked.Increment(ref count);
        }

        [Test]
        public void TestMethod()
        {
            if (count != 1)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
