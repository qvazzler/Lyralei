using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyralei.Core.Test.Models; //Important to load your models

namespace Lyralei
{
    //CoreContext is THE database in this project. That's why you see the file everywhere, with partial classes.
    //It is not that pretty (prevents us from turning addons into DLL assemblies), but it'll do for now
    public partial class CoreContext : DbContext
    {
        // This is where you define tables, based on models..
        // Basically CoreContext is the Database, and each DbSet below is a table based on a model (table = model)
        public DbSet<TestUser> TestUser { get; set; }

        // By the way, structural changes to the database require you to enter the following
        // commands into Package Manager Console in Visual Studio. For more info, turn to google.

        // Add-Migration SomeChangesRegardingTestAddon
        // Update-Database
    }
}
