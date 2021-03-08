using System;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// Attribute is being ran after running all test methods in the class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterClassAttribute : Attribute { }
}
