using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp.Data
{
    public class MyAppContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public MyAppContext(DbContextOptions options) 
            : base(options)
        {
        }

        protected MyAppContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.SqlConnection);
        }
    }
}
