using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyralei.Core.ServerQueryShell;

using Lyralei.Core.ServerQueryShell.Models;

namespace Lyralei
{
    public partial class CoreContext : DbContext
    {
        public DbSet<Core.ChannelManager.Models.ChannelDesignations> ChannelDesignations { get; set; }
        public DbSet<Core.ChannelManager.Models.StoredChannels> StoredChannels { get; set; }
        public DbSet<Core.ChannelManager.Models.StoredChannelGroupClients> StoredChannelGroupClients { get; set; }
    }
}
