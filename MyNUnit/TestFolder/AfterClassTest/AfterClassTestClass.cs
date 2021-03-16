using System;
using MyNUnit.Attributes;

namespace AfterClassTest
{
    public class AfterClassTestClass
    {
        private static int count = 0;

        [AfterClass]
        public void AfterClassTest()
        {
            if (count != 2)
            {
                throw new InvalidOperationException();
            }
        }

        [Test]
        public void TestMethod1()
        {
            ++count;
        }

        [Test]
        public void TestMethod2()
        {
            ++count;
        }
    }
}
