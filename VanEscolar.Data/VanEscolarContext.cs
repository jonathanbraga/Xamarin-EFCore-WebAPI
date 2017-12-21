using Microsoft.EntityFrameworkCore;
using VanEscolar.Domain;

namespace VanEscolar.Data
{
    public class VanEscolarContext : DbContext
    {
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Scholl> Scholls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server = (localdb)\\mssqllocaldb; Database = VanEscolarData; Trusted_Connection = True");
        }
    }
}
