using System;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// Attribute is used to mark test methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute : Attribute
    {
        /// <summary>
        /// Initializes an instance of TestAttribute.
        /// </summary>
        public TestAttribute(string ignore = null, Type expected = null)
        {
            ExpectedException = expected;
            IgnoreMessage = ignore;
        }

        /// <summary>
        /// Type of the expected exception.
        /// </summary>
        public Type ExpectedException { get; private set; }

        /// <summary>
        /// Message that explains why method is being ignored.
        /// </summary>
        public string IgnoreMessage { get; private set; }

        /// <summary>
        /// Lets you know if the method is being ignored.
        /// </summary>
        public bool Ignored => IgnoreMessage != null;
    }
}
