using MyNUnit.Attributes;

namespace AfterTestsClass
{
    public class AfterTestsClass
    {
        [AfterClass]
        public static void AfterTestMethod1() { }

        [AfterClass]
        public static void AfterTestMethod2() { }
    }
}
