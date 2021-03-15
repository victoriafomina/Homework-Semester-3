using System.Collections.Generic;

namespace MyNUnitWeb.Models
{
    /// <summary>
    /// Represents a view model of the assembly.
    /// </summary>
    public class AssemblyViewModel
    {
        /// Id of the assembly.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the assembly.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tests contained in the assembly.
        /// </summary>
        public ICollection<TestViewModel> Tests { get; set; } = new List<TestViewModel>();
    }
}
