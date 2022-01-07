using Microsoft.EntityFrameworkCore;

namespace AuthFromRole.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() { }
        public DbSet<User> user { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=EnderWarAdmin;database=firstdb;",
                new MySqlServerVersion(new Version(8, 0, 27))
            );
        }
    }
}
