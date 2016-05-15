using Lyralei.Addons.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Lyralei.TS3_Objects.Entities;
using Lyralei.TS3_Objects.EventArguments;

namespace Lyralei.Core.AddonManager
{
    public class AddonManagerOld : Base.CoreBase, Base.ICore
    {
        private Core.ServerQueryConnection.ServerQueryConnection ServerQueryConnection;
        public List<IAddon> addons = new List<IAddon>();

        public delegate void BotCommandFail(object sender, FailedBotCommandEventArgs e);
        public event BotCommandFail onFailedBotCommand;

        public delegate void BotCommand(object sender, BotCommandEventArgs e);
        public event BotCommand onBotCommand;

        CommandRuleSets commands = new CommandRuleSets();

        public AddonManagerOld(Core.ServerQueryConnection.Models.Subscribers Subscriber) : base(Subscriber)
        {
            this.CoreName = this.GetType().Name;
            Dependencies.Add(typeof(ServerQueryConnection.ServerQueryConnection).Name);
        }

        public void UserInitialize(AddonInjections CoreList)
        {
            this.ServerQueryConnection = CoreList[typeof(ServerQueryConnection.ServerQueryConnection).Name] as ServerQueryConnection.ServerQueryConnection;

            ServerQueryConnection.BotCommandAttempt += onBotCommandAttempt;

            // Hard-coded for now..
            //addons.Add(new Core.InputOwner.InputOwnerAddon());
            //addons.Add(new Core.Test.TestAddon());
            //addons.Add(new Core.ServerQuery.ServerQueryAddon());

            List<IAddon> failedAddons = new List<IAddon>();

            // Configure each addon with the basic stuff
            for (int addonIndex = 0; addonIndex < addons.Count; addonIndex++)
            {
                try
                {
                    addons[addonIndex].Configure(this.Subscriber, this.ServerQueryConnection);
                }
                catch (Exception)
                {
                    logger.Error("Removing addon {0}: failed to load during configuration", addons[addonIndex].CoreName);
                    addons.RemoveAt(addonIndex);
                }
            }

            // Wire up any injection requests by the addons to addon manager
            foreach (IAddon addon in addons)
            {
                addon.InjectionRequest += onInjectionRequest;
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
                    logger.Error(ex, "Removing addon {0}: failed to load during initialization", addons[addonIndex].CoreName);
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
                    logger.Error(ex, "Removing addon {0}: failed to load during dependency definitions", addons[addonIndex].CoreName);
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
                    logger.Error(ex, "Removing addon {0}: failed to load during dependency initialization", addons[addonIndex].CoreName);
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
                    logger.Warn(ex, "addon {0}: Could not load command schemas", addons[addonIndex].CoreName);
                }
            }
        }

        private void onBotCommandAttempt(object sender, TS3QueryLib.Core.CommandHandling.CommandParameterGroup cmd, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {
            try
            {
                CommandParameterGroupWithRules theCmd = null;
                var theSchema = commands.SingleOrDefault(x => x.Commands.Any(y => (theCmd = (CommandParameterGroupWithRules)y).First().Name == cmd.First().Name));

                if (theSchema != null)
                {
                    // Find owner addon
                    var addon = addons.SingleOrDefault(a => a.CoreName == theSchema.CoreName);

                    // Validate command data
                    theCmd.ValidateAddData(cmd);

                    // Invoke defined method linked to command
                    theSchema.Method.Invoke(new BotCommandEventArgs(theCmd, e));
                }
                else
                {
                    throw new Exception("Bot command does not exist");
                }

                // Notify parent
                onBotCommand.Invoke(sender, new BotCommandEventArgs(theCmd, e));
            }
            catch (Exception)
            {
                // Notify parent
                onFailedBotCommand.Invoke(sender, new FailedBotCommandEventArgs(cmd, e));
            }
        }

        private void onInjectionRequest(object sender, List<string> e)
        {
            var a = (AddonBase)sender;

            foreach (string requestedAddon in e)
            {
                try
                {
                    var reqAddon = addons.Single(addon => addon.CoreName == requestedAddon);
                    a.InjectDependency(reqAddon);
                }
                catch (InvalidOperationException ex)
                {
                    logger.Error(ex, "Could not inject Addon because it does not exist or is not loaded: {0}", requestedAddon);
                }
            }
        }

        public CommandRuleSets DefineCommandSchemas()
        {
            throw new NotImplementedException();
        }
    }
}
