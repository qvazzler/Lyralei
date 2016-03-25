using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyralei.Addons.InputOwner;

namespace Lyralei.Addons.InputOwner.Hooks
{
    public class ModelCustomizer
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.InputOwners>()
                .Property(b => b.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Models.InputOwners>()
                 .Property(b => b.QueuePosition)
                 .HasDefaultValue(Props.QueuePosition.Last);

            modelBuilder.Entity<Models.InputOwners>()
                 .Property(b => b.ReleaseRequestAction)
                 .HasDefaultValue(Props.ReleaseRequestAction.Ask);

            modelBuilder.Entity<Models.InputOwners>()
                 .Property(b => b.InputWaitDuration)
                 .HasDefaultValue(TimeSpan.FromSeconds(120));

            // Make Blog.Url required
            //modelBuilder.Entity<ServerQueryUserDetails>()
            //.HasOne(p => p.Users)
            //.WithOne()
            //.HasForeignKey<ServerQueryUserDetails>(b => b.ServerQueryUserDetailsForeignKey);
        }
    }
}
