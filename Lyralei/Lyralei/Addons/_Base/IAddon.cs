using Lyralei.TS3_Objects.Entities;
using Lyralei.TS3_Objects.EventArguments;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;

namespace Lyralei.Addons.Base
{
    public interface IAddon : IAddonBase
    {
        string AddonName { get; set; }

        void Initialize();
        void DefineDependencies();
        void InitializeDependencies();

        CommandRuleSets DefineCommandSchemas();
    }
}