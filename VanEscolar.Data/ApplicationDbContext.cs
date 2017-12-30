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
        public DbSet<School> Schools { get; set; }
        public DbSet<Travel> Travels { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<TravelStudent> TravelsStudent { get; set; }
        public DbSet<Message> Messages { get; set; }

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

                builder.HasMany(m => m.Messages)
                .WithOne(p => p.Parent);
             });

            modelBuilder.Entity<Student>(builder =>
            {
                builder.HasOne(p => p.Parent)
                .WithMany(s => s.Students);

                builder.HasOne(s => s.School)
                .WithMany(st => st.Students);

                builder.HasOne(t => t.Travel)
                .WithOne(s => s.Student);

                builder.HasOne(t => t.TravelStudent)
                .WithOne(s => s.Student);
            });

            // Message
            modelBuilder.Entity<Message>(builder => 
            {
                builder.HasOne(p => p.Parent)
                .WithMany(m => m.Messages);
                
            });

            //Scholl
            modelBuilder.Entity<School>(builder => 
            {
            
                builder.HasMany(c => c.Students)
                .WithOne(s => s.School);
            });

            //Travel
            modelBuilder.Entity<Travel>(builder =>
            {
                builder.HasOne(t => t.Student)
                .WithOne(s => s.Travel);

                builder.HasOne(ts => ts.TravelStudent)
                .WithOne(t => t.Travel);
            });

            //Link
            modelBuilder.Entity<Link>()
                .HasOne(p => p.Parent)
                .WithOne(l => l.Link);

            modelBuilder.Entity<Link>()
                .HasOne(u => u.User)
                .WithOne(l => l.Link);

            //TravelsStudent
            modelBuilder.Entity<TravelStudent>(builder =>
            {
                builder.HasOne(s => s.Student)
                .WithOne(ts => ts.TravelStudent);

                builder.HasOne(t => t.Travel)
                .WithOne(ts => ts.TravelStudent);
            });
        }
    }
}
