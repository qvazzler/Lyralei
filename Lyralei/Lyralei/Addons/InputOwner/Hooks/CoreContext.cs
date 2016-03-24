using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyralei.Addons.InputOwner;

using Lyralei.Addons.InputOwner.Models;

namespace Lyralei
{
    public partial class CoreContext : DbContext
    {
        public DbSet<InputOwners> InputOwners { get; set; }
    }
}
