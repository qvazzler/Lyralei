using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lyralei
{
    //public abstract class BaseContext : DbContext
    //{
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        // Visual Studio 2015 | Use the LocalDb 12 instance created by Visual Studio
    //        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.ConsoleApp.NewDb;Trusted_Connection=True;");

    //        // Visual Studio 2013 | Use the LocalDb 11 instance created by Visual Studio
    //        // optionsBuilder.UseSqlServer(@"Server=(localdb)\v11.0;Database=EFGetStarted.ConsoleApp.NewDb;Trusted_Connection=True;");
    //    }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        //// Make Blog.Url required
    //        //modelBuilder.Entity<Models.Subscribers>()
    //        //    .Property(b => b.ServerIp)
    //        //    .IsRequired();
    //    }
    //}

    //public static class BaseContextUtils
    //{
    //    //Model-cleaner
    //    public static void Clear<T>(this DbSet<T> dbSet) where T : class
    //    {
    //        dbSet.RemoveRange(dbSet);
    //    }

    //    //Borked?
    //    //public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
    //    //{
    //    //    var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
    //    //    return !exists ? dbSet.Add(entity) : null;
    //    //}
    //}
}