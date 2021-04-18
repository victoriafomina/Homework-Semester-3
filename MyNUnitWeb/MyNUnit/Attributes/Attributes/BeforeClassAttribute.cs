using System;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// Attribute is being ran before running all test methods in the class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeClassAttribute : Attribute { }
}
