using System;

namespace MyNUnit
{
    /// <summary>
    /// The class implements logic allowing to find out information about the test results.
    /// </summary>
    public class TestMethodInfo
    {
        /// <summary>
        /// Конструктор для создания отчёта о проигнорированном методе
        /// </summary>
        public TestMethodInfo(string name) => Name = name;

        public void SetInfoIgnoredTest(string ignoreMessage)
        {
            Ignored = true;
            IgnoreMessage = ignoreMessage;
        }

        public void SetInfoPassedTest(bool passed, Type expectedException, Type thrownException, TimeSpan time)
        {
            Ignored = false;
            Passed = passed;
            ExpectedException = expectedException;
            ThrownException = thrownException;
            Time = time;
        }

        /*
        /// <summary>
        /// Конструктор для создания отчёта о выполненном тестовом методе
        /// </summary>
        public TestMethodInfo(string name, bool passed, Type expectedException, Type thrownException, TimeSpan time)
        {
            Name = name;
            Ignored = false;
            Passed = passed;
            ExpectedException = expectedException;
            ThrownException = thrownException;
            Time = time;
        }
        */

        /// <summary>
        /// Test method name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Indicates if the test passed.
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
