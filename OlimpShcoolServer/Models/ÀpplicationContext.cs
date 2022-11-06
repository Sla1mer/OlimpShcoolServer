using Microsoft.EntityFrameworkCore;

namespace FoodServer.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<FoodEntries> FoodEntries { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=rakdav9e.beget.tech;user=rakdav9e_olimpdb;password=XFRa5*9S;database=rakdav9e_olimpdb;",
                new MySqlServerVersion(new Version(8, 0, 11))
            );
        }
    }
}
