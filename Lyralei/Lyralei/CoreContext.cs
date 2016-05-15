using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;

namespace Lyralei
{
    public partial class CoreContext : DbContext
    {
        //public DbSet<Blog> Blog { get; set; }
        //public DbSet<Post> Posts { get; set; }
        public DbSet<Core.ServerQueryConnection.Models.Subscribers> Subscribers { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //EnableSensitiveDataLogging();
            optionsBuilder.EnableSensitiveDataLogging();
            // Visual Studio 2015 | Use the LocalDb 12 instance created by Visual Studio
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Lyralei;Trusted_Connection=True;");

            // Visual Studio 2013 | Use the LocalDb 11 instance created by Visual Studio
            // optionsBuilder.UseSqlServer(@"Server=(localdb)\v11.0;Database=EFGetStarted.ConsoleApp.NewDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make Blog.Url required
            modelBuilder.Entity<Core.ServerQueryConnection.Models.Subscribers>()
                .Property(b => b.ServerIp)
                .IsRequired();

            ModelCustomizer._modelCustomization.ForEach(x => x.Invoke(modelBuilder));
        }
    }

    public static class CoreContextUtils
    {
        //Model-cleaner
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }

        public static void Test<T>() where T : NLog.Logger
        {
            
        }

        //Borked?
        //public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
        //{
        //    var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
        //    return !exists ? dbSet.Add(entity) : null;
        //}
    }
}