using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNUnitWeb.Models
{
    /// <summary>
    /// 
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
