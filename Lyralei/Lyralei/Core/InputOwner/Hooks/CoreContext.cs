using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyralei.Core.InputOwner;

using Lyralei.Core.InputOwner.Models;

namespace Lyralei
{
    public partial class CoreContext : DbContext
    {
        public DbSet<InputOwners> InputOwners { get; set; }
    }
}
