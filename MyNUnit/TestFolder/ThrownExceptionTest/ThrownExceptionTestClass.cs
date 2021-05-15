using System;
using MyNUnit.Attributes;

namespace ThrownExceptionTest
{
    public class ThrownExceptionTestClass
    {
        [Test(null, typeof(AggregateException))]
        public void ThrowingExceptionMethod()
        {
            throw new AggregateException();
        }
    }
}
