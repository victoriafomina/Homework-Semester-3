using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyNUnitWeb.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyViewModel
    {
        public AssemblyViewModel() => Tests = new List<TestViewModel>();

        /// Id of the assembly.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the assembly.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tests contained in the assembly.
        /// </summary>
        public List<TestViewModel> Tests { get; set; }
    }
}