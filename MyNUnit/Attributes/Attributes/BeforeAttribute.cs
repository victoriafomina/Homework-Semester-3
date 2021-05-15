using System;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// Attribute is being ran before every test method running.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeAttribute : Attribute { }
}
