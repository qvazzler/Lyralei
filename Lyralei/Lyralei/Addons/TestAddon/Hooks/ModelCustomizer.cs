﻿using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyralei.Core.ServerQueryShell;

namespace Lyralei.Addons.TestAddon.Hooks
{
    public class ModelCustomizer
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            // For customizing the models manually, not usually required
        }
    }
}
