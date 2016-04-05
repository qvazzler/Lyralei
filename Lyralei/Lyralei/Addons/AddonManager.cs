using Lyralei.Addons.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Fluent;

namespace Lyralei.Addons
{
    public class AddonManager
    {
        private Logger logger;
        
        public List<IAddon> addons = new List<IAddon>();

        private Models.Subscribers subscriber;
        public Models.Subscribers Subscriber
        {
            get { return subscriber; }
            set
            {
                subscriber = value;
                logger = LogManager.GetLogger(this.GetType().Name + " - " + Subscriber.ToString());
            }
        }

        public AddonManager(Models.Subscribers subscriber, Bot.ServerQueryRootConnection serverQueryRootConnection)
        {
            this.Subscriber = subscriber;

            // Hard-coded for now..
            addons.Add(new InputOwner.InputOwnerAddon());
            addons.Add(new Test.TestAddon());
            addons.Add(new ServerQuery.ServerQueryAddon());

            List<IAddon> failedAddons = new List<IAddon>();

            // Configure each addon with the basic stuff
            for (int addonIndex = 0; addonIndex < addons.Count; addonIndex++)
            {
                try
                {
                    addons[addonIndex].Configure(subscriber, serverQueryRootConnection);
                }
                catch (Exception ex)
                {
                    logger.Error("Addon failed to load during configuration: {0}", addons[addonIndex].AddonName);
                    addons.RemoveAt(addonIndex);
                }
            }

            // Wire up any injection requests by the addons to addon manager
            foreach (IAddon addon in addons)
            {
                addon.InjectionRequest += Addon_injectionRequest1;
            }

            // Initialize each addon
            for (int addonIndex = 0; addonIndex < addons.Count; addonIndex++)
            {
                try
                {
                    addons[addonIndex].Initialize();
                }
                catch (Exception ex)
                {
                    logger.Error("Addon failed to load during initialization: {0}", addons[addonIndex].AddonName);
                    addons.RemoveAt(addonIndex);
                }
            }

            // Let addons define their dependencies
            for (int addonIndex = 0; addonIndex < addons.Count; addonIndex++)
            {
                try
                {
                    addons[addonIndex].DefineDependencies();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Addon failed to load during dependency definitions: {0}", addons[addonIndex].AddonName);
                    addons.RemoveAt(addonIndex);
                }
            }

            // Initialize the dependencies
            for (int addonIndex = 0; addonIndex < addons.Count; addonIndex++)
            {
                try
                {
                    addons[addonIndex].InitializeDependencies();
                }
                catch (Exception ex)
                {
                    //logger.Error(ex, "{0} - Addon failed to load during dependency initialization: {1}", Subscriber.ToString(), addons[addonIndex].AddonName);
                    logger.Error(ex, "Addon failed to load during dependency initialization: {0}", addons[addonIndex].AddonName);

                    addons.RemoveAt(addonIndex);
                }
            }
        }

        private void Addon_injectionRequest1(object sender, List<string> e)
        {
            var a = (AddonBase)sender;

            foreach (string requestedAddon in e)
            {
                try
                {
                    var reqAddon = addons.Single(addon => addon.AddonName == requestedAddon);
                    a.InjectDependency(reqAddon);
                }
                catch (InvalidOperationException ex)
                {
                    logger.Error(ex, "Could not inject Addon because it does not exist or is not loaded: {0}", requestedAddon);
                }
            }
        }
    }
}
