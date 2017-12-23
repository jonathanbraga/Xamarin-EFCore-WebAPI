using Microsoft.EntityFrameworkCore;
using VanEscolar.Domain;

namespace VanEscolar.Data
{
    public class VanEscolarContext : DbContext
    {
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Scholl> Scholls { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Travel> Travels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server = (localdb)\\mssqllocaldb; Database = VanEscolarData; Trusted_Connection = True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Parent
            modelBuilder.Entity<Parent>()
                .HasMany(c => c.Students)
                .WithOne(p => p.Parent);

            //Scholl
            modelBuilder.Entity<Scholl>()
                .HasMany(c => c.Students)
                .WithOne(s => s.Scholl);

            //Travel
            modelBuilder.Entity<Travel>()
                .HasOne(t => t.Student)
                .WithOne(s => s.Travel);

            // Link
            modelBuilder.Entity<Link>()
                .HasOne(l => l.Parent)
                .WithOne(p => p.Link);
            modelBuilder.Entity<Link>()
                .HasOne(l => l.User)
                .WithOne(u => u.Link);
        }
    }
}
