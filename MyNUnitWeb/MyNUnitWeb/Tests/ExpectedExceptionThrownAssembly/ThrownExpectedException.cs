using System;
using MyNUnit.Attributes;

namespace ThrownExpectedException
{
    public class ThrownExpectedException
    {
        [Test(null, typeof(AggregateException))]
        public void ThrowExpectedExceptionMethod()
        {
            throw new AggregateException();
        }
    }
}
