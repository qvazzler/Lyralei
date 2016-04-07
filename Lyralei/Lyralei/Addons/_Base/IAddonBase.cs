using Lyralei.Bot;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;

namespace Lyralei.Addons.Base
{
    public interface IAddonBase
    {
        void Configure(Models.Subscribers subscriber, ServerQueryConnection ServerQueryConnection);
        event EventHandler<List<string>> InjectionRequest;
    }
}