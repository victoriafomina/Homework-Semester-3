using System;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// Attribute is being ran after every test method running.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterAttribute : Attribute { }
}
