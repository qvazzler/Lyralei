using Lyralei.TS3_Objects.Entities;
using Lyralei.TS3_Objects.EventArguments;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;

namespace Lyralei.Addons.Base
{
    public interface IAddonOld : IAddonBaseOld
    {
        string CoreName { get; set; }

        void Initialize();
        void DefineDependencies();
        void InitializeDependencies();

        CommandRuleSets DefineCommandSchemas();
    }
}