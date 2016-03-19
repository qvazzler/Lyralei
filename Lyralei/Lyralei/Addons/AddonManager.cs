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

            var addon1 = new TestAddon();
            var addon2 = new ServerQueryAddon();

            addons.Add(addon1);
            addons.Add(addon2);

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
