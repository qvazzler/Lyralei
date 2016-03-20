using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Addons.Test
{
    public partial class CoreContext
    {
        public DbSet<TestDataModel> TestDataModel { get; set; }
    }
}
