﻿using Microsoft.Data.Entity;
using System.Collections.Generic;

namespace Lyralei.Addons.Base
{
    public interface IAddon : IAddonBase
    {
        void Initialize();
        string AddonName { get; set; }
    }
}