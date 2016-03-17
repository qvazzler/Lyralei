using Microsoft.Data.Entity;
using System.Collections.Generic;

namespace Lyralei
{
    public class AlleriaContext : DbContext
    {
        //public DbSet<Blog> Blog { get; set; }
        //public DbSet<Post> Posts { get; set; }
        public DbSet<Models.Subscribers> Subscribers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Visual Studio 2015 | Use the LocalDb 12 instance created by Visual Studio
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.ConsoleApp.NewDb;Trusted_Connection=True;");

            // Visual Studio 2013 | Use the LocalDb 11 instance created by Visual Studio
            // optionsBuilder.UseSqlServer(@"Server=(localdb)\v11.0;Database=EFGetStarted.ConsoleApp.NewDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make Blog.Url required
            modelBuilder.Entity<Models.Subscribers>()
                .Property(b => b.ServerIp)
                .IsRequired();
        }
    }

    public static class AlleriaContextUtils
    {
        //Model-cleaner
        public static void Clear<T>(this DbSet<T> dbSet) where T : Models.Subscribers
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}