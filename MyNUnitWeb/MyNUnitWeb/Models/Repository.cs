using Microsoft.EntityFrameworkCore;
using MyNUnitWeb.Models;

namespace MyNUnitWeb.Models
{
    public class Repository : DbContext
    {
        public DbSet<TestViewModel> Tests { get; set; }

        public DbSet<AssemblyViewModel> Assemblies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestsRunHistory;Trusted_Connection=True;");
        }
    }
}
