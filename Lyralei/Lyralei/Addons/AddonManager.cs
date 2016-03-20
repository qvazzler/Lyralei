using Lyralei.Addons.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lyralei.Addons
{
    public class AddonManager
    {
        public List<AddonBase> addons;

        public AddonManager(Models.Subscribers subscriber, Bot.ServerQueryRootConnection serverQueryRootConnection)
        {
            addons = new List<AddonBase>();

            addons.Add(new Test.TestAddon());
            addons.Add(new ServerQuery.ServerQueryAddon());

            foreach (AddonBase addon in addons)
            {
                addon.BaseInitialize(subscriber, serverQueryRootConnection);
            }

            foreach (IAddon addon in addons)
            {
                addon.Initialize();
            }
        }
    }
}
