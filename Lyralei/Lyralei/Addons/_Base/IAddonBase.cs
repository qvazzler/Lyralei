﻿using Lyralei.Bot;
using Microsoft.Data.Entity;
using System.Collections.Generic;

namespace Lyralei.Addons.Base
{
    public interface IAddonBase
    {
        void Configure(Models.Subscribers subscriber, ServerQueryRootConnection serverQueryRootConnection);
        AddonDependencyManager dependencyManager { get; set; }
    }
}