using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VanEscolar.Domain;

namespace VanEscolar.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Scholl> Scholls { get; set; }
        public DbSet<Travel> Travels { get; set; }
        public DbSet<Link> Links { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region ASP.NET Identity table renaming
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users", "dbo");
            modelBuilder.Entity<IdentityRole>()
                .ToTable("Roles", "dbo");
            modelBuilder.Entity<IdentityUserRole<string>>()
                .ToTable("UserRoles", "dbo");
            modelBuilder.Entity<IdentityUserClaim<string>>()
                .ToTable("UserClaims", "dbo");
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .ToTable("UserLogins", "dbo");
            modelBuilder.Entity<IdentityRoleClaim<string>>()
                .ToTable("RoleClaims", "dbo");
            modelBuilder.Entity<IdentityUserToken<string>>()
                .ToTable("UserTokens", "dbo");
            #endregion

            //Parent
            modelBuilder.Entity<Parent>(builder =>
            { 
                builder.HasMany(c => c.Students)
                .WithOne(p => p.Parent);
             });

            //Scholl
            modelBuilder.Entity<Scholl>(builder => 
            {
            
                builder.HasMany(c => c.Students)
                .WithOne(s => s.Scholl);

                builder.Property(p => p.CreatedAt).HasDefaultValue<DateTime>(DateTime.UtcNow);
            });
            //Travel
            modelBuilder.Entity<Travel>()
                .HasOne(t => t.Student)
                .WithOne(s => s.Travel);

            //Link
            modelBuilder.Entity<Link>()
                .HasOne(p => p.Parent)
                .WithOne(l => l.Link);

            modelBuilder.Entity<Link>()
                .HasOne(u => u.User)
                .WithOne(l => l.Link);
        }
    }
}
