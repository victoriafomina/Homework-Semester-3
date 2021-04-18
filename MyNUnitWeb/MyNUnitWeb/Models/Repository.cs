using Microsoft.EntityFrameworkCore;

namespace MyNUnitWeb.Models
{
    /// <summary>
    /// The Repository class that is DbContext.
    /// </summary>
    public class Repository : DbContext
    {
        /// <summary>
        /// Stores the information about the test launched before.
        /// </summary>
        public DbSet<TestViewModel> Tests { get; set; }

        /// <summary>
        /// Stores the information about the assemblies containing the tests launched before.
        /// </summary>
        public DbSet<AssemblyViewModel> Assemblies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestsRunHistory;Trusted_Connection=True;");
        }
    }
}
