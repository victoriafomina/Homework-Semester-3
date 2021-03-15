using System;
using System.ComponentModel.DataAnnotations;

namespace MyNUnitWeb.Models
{
    /// <summary>
    /// Model reprsenting a run test.
    /// </summary>
    public class TestViewModel
    {
        /// <summary>
        /// Id of the test.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the test.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the class containing the test method.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Informs if the test is passed, not passed or was not running.
        /// </summary>
        public bool Passed { get; set; }

        /// <summary>
        /// Informs if the test was ignored.
        /// </summary>
        public bool Ignored { get; set; }

        /// <summary>
        /// Ignore message.
        /// </summary>
        public string IgnoreMessage { get; set; }

        /// <summary>
        /// Elased for the test time.
        /// </summary>
        public TimeSpan ElapsedTime { get; set; }

        /// <summary>
        /// Test launch time.
        /// </summary>
        public DateTime TestLaunchTime { get; set; }

        /// <summary>
        /// Name of the assembly that contains the test.
        /// </summary>
        public AssemblyViewModel Assembly { get; set; }
    }
}