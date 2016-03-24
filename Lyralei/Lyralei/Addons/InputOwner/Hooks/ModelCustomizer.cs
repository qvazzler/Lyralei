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

            // Make Blog.Url required
            //modelBuilder.Entity<ServerQueryUserDetails>()
            //.HasOne(p => p.Users)
            //.WithOne()
            //.HasForeignKey<ServerQueryUserDetails>(b => b.ServerQueryUserDetailsForeignKey);
        }
    }
}
