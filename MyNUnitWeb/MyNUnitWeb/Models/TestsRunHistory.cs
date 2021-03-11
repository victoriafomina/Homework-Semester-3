using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNUnitWeb.Models
{
    /// <summary>
    /// The class is for keeping tests run history; 
    /// </summary>
    public class TestsRunHistory : DbContext
    {
        /// <summary>
        /// Initializes an instance of TestsRunHistory.
        /// </summary>
        public TestsRunHistory(DbContextOptions<TestsRunHistory> options) : base(options) { }

        /// <summary>
        /// Contains information about the assemblies.
        /// </summary>
        public DbSet<AssemblyViewModel> Assemblies { get; set; }

        /// <summary>
        /// Contains information about the tests.
        /// </summary>
        public DbSet<AssemblyViewModel> Tests { get; set; }
    }
}
