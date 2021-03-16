using MyNUnit.Attributes;
using System;

namespace BeforeClassTest
{
    public class BeforeClassTestClass
    {
        private static int count = 0;

        [BeforeClass]
        public void BeforeClassMethod()
        {
            ++count;
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
