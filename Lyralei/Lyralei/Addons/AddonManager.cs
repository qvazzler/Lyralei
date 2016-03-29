﻿using Lyralei.Addons.Base;
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
        private Logger logger = LogManager.GetCurrentClassLogger();
        public List<IAddon> addons = new List<IAddon>();
        Models.Subscribers subscriber;

        public AddonManager(Models.Subscribers subscriber, Bot.ServerQueryRootConnection serverQueryRootConnection)
        {
            this.subscriber = subscriber;

            // Hard-coded for now..
            addons.Add(new InputOwner.InputOwnerAddon());
            addons.Add(new Test.TestAddon());
            addons.Add(new ServerQuery.ServerQueryAddon());

            List<IAddon> failedAddons = new List<IAddon>();

            // Wire up any injection requests by the addons to addon manager
            foreach (IAddon addon in addons)
            {
                addon.dependencyManager.injectionRequest += AddonDependencyManager_injectionRequest;
            }

            // Configure each addon with the basic stuff
            for (int addonIndex = 0; addonIndex < addons.Count; addonIndex++)
            {
                try
                {
                    addons[addonIndex].Configure(subscriber, serverQueryRootConnection);
                }
                catch (Exception ex)
                {
                    logger.Error()
                        .Message("Addon to load during configuration: {0}", addons[addonIndex].AddonName)
                        .Exception(ex)
                        .Property("subscriber", subscriber.ToString())
                        .Write();
                    addons.RemoveAt(addonIndex);
                }
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
                    logger.Error()
                        .Message("Addon to load during initialization: {0}", addons[addonIndex].AddonName)
                        .Exception(ex)
                        .Property("subscriber", subscriber.ToString())
                        .Write();
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
                    //logger.Error(ex, "{0} - Addon to load during dependency initialization: {1}", subscriber.ToString(), addons[addonIndex].AddonName);
                    logger.Error()
                        .Message("Addon to load during dependency initialization: {0}", addons[addonIndex].AddonName)
                        .Exception(ex)
                        .Property("subscriber", subscriber.ToString())
                        .Write();

                    addons.RemoveAt(addonIndex);
                }
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
                catch (InvalidOperationException ex)
                {
                    logger.Error()
                        .Message("Could not inject Addon because it does not exist or is not loaded: {0}", requestedAddon)
                        .Exception(ex)
                        .Property("subscriber", subscriber.ToString())
                        .Write();
                }
            }
        }
    }
}
