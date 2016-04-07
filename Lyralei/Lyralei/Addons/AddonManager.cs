using Lyralei.Addons.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Fluent;
using Lyralei.TS3_Objects.Entities;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Server.Notification.EventArgs;
using Lyralei.TS3_Objects.EventArguments;

namespace Lyralei.Addons
{
    public class AddonManager
    {
        private Logger logger;
        private Bot.ServerQueryRootConnection serverQueryRootConnection;
        public List<IAddon> addons = new List<IAddon>();

        public delegate void BotCommandFail(object sender, FailedBotCommandEventArgs e);
        public event BotCommandFail onFailedBotCommand;

        public delegate void BotCommand(object sender, BotCommandEventArgs e);
        public event BotCommand onBotCommand;

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

        CommandRuleSets commands = new CommandRuleSets();

        public AddonManager(Models.Subscribers subscriber, Bot.ServerQueryRootConnection serverQueryRootConnection)
        {
            this.Subscriber = subscriber;
            this.serverQueryRootConnection = serverQueryRootConnection;

            serverQueryRootConnection.BotCommandAttemptReceived += ServerQueryRootConnection_AttemptedBotCommandReceived;

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
                    addons[addonIndex].Configure(this.Subscriber, this.serverQueryRootConnection);
                }
                catch (Exception ex)
                {
                    logger.Error("Removing addon {0}: failed to load during configuration", addons[addonIndex].AddonName);
                    addons.RemoveAt(addonIndex);
                }
            }

            // Wire up any injection requests by the addons to addon manager
            foreach (IAddon addon in addons)
            {
                addon.InjectionRequest += Addon_injectionRequest;
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
                    logger.Error(ex, "Removing addon {0}: failed to load during initialization", addons[addonIndex].AddonName);
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
                    logger.Error(ex, "Removing addon {0}: failed to load during dependency definitions", addons[addonIndex].AddonName);
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
                    logger.Error(ex, "Removing addon {0}: failed to load during dependency initialization", addons[addonIndex].AddonName);
                    addons.RemoveAt(addonIndex);
                }
            }

            // Get command schemas from addons
            for (int addonIndex = 0; addonIndex < addons.Count; addonIndex++)
            {
                try
                {
                    CommandRuleSets cmds = addons[addonIndex].DefineCommandSchemas();

                    if (cmds != null)
                    {
                        foreach (var cmd in cmds)
                            commands.ValidateAddSchema(cmd);
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "addon {0}: Could not load command schemas", addons[addonIndex].AddonName);
                }
            }
        }

        private void ServerQueryRootConnection_AttemptedBotCommandReceived(object sender, TS3QueryLib.Core.CommandHandling.CommandParameterGroup cmd, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {
            //CommandParameterGroupExpectations verifiedCmd = (CommandParameterGroupExpectations)commands.SingleOrDefault(x => x.First().Name == cmd.First().Name);

            try
            {
                CommandParameterGroupWithRules theCmd = null;
                var theSchema = commands.SingleOrDefault(x => x.Commands.Any(y => (theCmd = (CommandParameterGroupWithRules)y).First().Name == cmd.First().Name));

                if (theSchema != null)
                {
                    // Find owner addon
                    var addon = addons.SingleOrDefault(a => a.AddonName == theSchema.AddonName);

                    // Validate command data
                    theCmd.ValidateAddData(cmd);

                    // Invoke defined method linked to command
                    theSchema.Method.Invoke(new BotCommandEventArgs(theCmd, e));
                }

                // Notify parent
                onBotCommand.Invoke(sender, new BotCommandEventArgs(theCmd, e));
            }
            catch (Exception)
            {
                // Notify parent
                onFailedBotCommand.Invoke(sender, new FailedBotCommandEventArgs(cmd, e));
            }
            
            //if (verifiedCmd != null)
            //{
            //    verifiedCmd.ValidateAddData(cmd);

            //    if (onBotCommand != null)
            //        onBotCommand.Invoke(this, new BotCommandEventArgs(verifiedCmd, e));

            //    var ownerAddon = addons.SingleOrDefault(addon => addon.AddonName == verifiedCmd.AddonName);

            //    //ownerAddon.
            //}
            //else
            //{
            //    // Command not recognizable
            //}
        }

        private void Addon_injectionRequest(object sender, List<string> e)
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
