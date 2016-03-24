using Lyralei.Addons.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Lyralei.Addons
{
    public class AddonManager
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        public List<IAddon> addons = new List<IAddon>();

        public AddonManager(Models.Subscribers subscriber, Bot.ServerQueryRootConnection serverQueryRootConnection)
        {
            // Hard-coded for now..
            addons.Add(new Test.TestAddon());
            addons.Add(new ServerQuery.ServerQueryAddon());

            // Wire up any injection requests by the addons
            foreach (IAddon addon in addons)
            {
                addon.dependencyManager.injectionRequest += AddonDependencyManager_injectionRequest;
            }

            // Configure each addon with the basic stuff
            foreach (IAddonBase addon in addons)
            {
                addon.Configure(subscriber, serverQueryRootConnection);
            }

            // Initialize each addon, it will probably raise injection requests
            foreach (IAddon addon in addons)
            {
                addon.Initialize();
            }
        }

        private void AddonDependencyManager_injectionRequest(object sender, List<string> RequestedAddons)
        {
            var test = (AddonDependencyManager)sender;

            foreach (string requestedAddon in RequestedAddons)
            {
                try
                {
                    test.InjectDependency(addons.Single(addon => addon.AddonName == requestedAddon));
                }
                catch (InvalidOperationException)
                {
                    logger.Error(String.Format("Could not inject Addon because it does not exist or is not loaded: {0}", requestedAddon));
                }
            }
        }
    }
}
