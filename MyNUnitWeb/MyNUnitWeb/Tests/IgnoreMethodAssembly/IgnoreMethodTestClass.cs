using MyNUnit.Attributes;

namespace IgnoreMethodTest
{
    public class IgnoreMethodTestClass
    {
        [Test("Ignore it")]
        public void IgnoreMeMethod() { }

        [Test]
        public void SuccessfulPass() { }
    }
}
