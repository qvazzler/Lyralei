using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyralei.Addons.ServerQuery;

using Lyralei.Addons.ServerQuery.Models;

namespace Lyralei
{
    public partial class CoreContext : DbContext
    {
        public DbSet<ServerQueryUser> ServerQueryUser { get; set; }
    }
}
