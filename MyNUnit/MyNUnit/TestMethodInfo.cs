using System;

namespace MyNUnit
{
    /// <summary>
    /// The class implements logic allowing to find out information about the test results.
    /// </summary>
    public class TestMethodInfo
    {
        /// <summary>
        /// Initializes an object of TestMethodInfo.
        /// </summary>
        public TestMethodInfo(string name) => Name = name;

        /// <summary>
        /// Sets information about ignored test method.
        /// </summary>
        public void SetInfoIgnoredTest(string ignoreMessage)
        {
            Ignored = true;
            IgnoreMessage = ignoreMessage;
        }

        /// <summary>
        /// Sets information about passed test method.
        /// </summary>
        public void SetInfoPassedTest(bool passed, Type expectedException, Type thrownException, TimeSpan time)
        {
            Ignored = false;
            Passed = passed;
            ExpectedException = expectedException;
            ThrownException = thrownException;
            Time = time;
        }

        /// <summary>
        /// Test method name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Indicates if the test is passed.
        /// </summary>
        public bool Passed { get; private set; }

        /// <summary>
        /// Type of the expected exception.
        /// </summary>
        public Type ExpectedException { get; private set; }

        /// <summary>
        /// Exception thrown by the tested method.
        /// </summary>
        public Type ThrownException { get; private set; }

        /// <summary>
        /// Indicates whether the method was ignored during testing.
        /// </summary>
        public bool Ignored { get; private set; }

        /// <summary>
        /// Message that informs the reason why method was ignored.
        /// </summary>
        public string IgnoreMessage { get; private set; } = "";

        /// <summary>
        /// Time spent to test method.
        /// </summary>
        public TimeSpan Time { get; private set; }
    }
}
