using System;
using MyNUnit.Attributes;

namespace ThrownExceptionTest
{
    public class ThrownExceptionTestClass
    {
        [Test("Ignore it because ignore it", typeof(AggregateException))]
        public void ThrowingExceptionMethod()
        {
            throw new AggregateException();
        }
    }
}
