using Lyralei.Bot;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;

namespace Lyralei.Addons.Base
{
    public interface IAddonBaseOld
    {
        void Configure(Core.ServerQueryConnection.Models.Subscribers subscriber, Core.ServerQueryConnection.ServerQueryConnection ServerQueryConnection);
        event EventHandler<List<string>> InjectionRequest;
    }
}