using System.Collections.Generic;

namespace MyNUnitWeb.Models
{
    /// <summary>
    /// A view model to work with the tests lauches history.
    /// </summary>
    public class TestsLaunchesHistoryViewModel
    {
        /// <summary>
        /// Stores the information about the assemblies containing the tests launched before.
        /// </summary>
        public List<AssemblyViewModel> Assemblies { get; set; } = null;

        /// <summary>
        /// Stores the information about the test launched before.
        /// </summary>
        public List<TestViewModel> Tests { get; set; } = null;
    }
}
