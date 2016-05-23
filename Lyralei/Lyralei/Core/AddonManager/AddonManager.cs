using Lyralei.Bot;
using Lyralei.TS3_Objects.Entities;
using Lyralei.TS3_Objects.EventArguments;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Core.AddonManager
{
    public class AddonManager : Base.CoreBase, Base.ICore
    {
        //private Logger logger;

        private AddonList AddonList;
        private CoreList CoreList;

        public delegate void BotCommandFail(object sender, FailedBotCommandEventArgs e);
        public event BotCommandFail onFailedBotAddonCommand;

        public delegate void BotCommand(object sender, BotCommandEventArgs e);
        public event BotCommand onBotCommand;

        CommandRuleSets commands = new CommandRuleSets();

        ServerQueryConnection.ServerQueryConnection ServerQueryConnection;

        public AddonManager(ServerQueryConnection.Models.Subscribers Subscriber) : base(Subscriber)
        {
            this.CoreDependencies.GetEverything = true;

            AddonList = new AddonList();

            // Hard-coded for now..
            AddonList.Add(new Addons.TestAddon.TestAddon(this.Subscriber));
            AddonList.Add(new Addons.Greeter.Greeter(this.Subscriber));
        }

        public void UserInitialize(CoreList CoreList)
        {
            // Core Initialization
            ServerQueryConnection = (ServerQueryConnection.ServerQueryConnection)CoreList["ServerQueryConnection"];

            // Addon Initialization
            // Just give us _all_ the cores (for now) so we can share them with the addons that need them
            this.CoreList = CoreList;
            // We can't initialize addons until the core addons have been initialized
            InitializeAddons();

            GetSchemas();
            ServerQueryConnection.BotCommandAttempt += onBotCommandAttempt;
        }

        public CommandRuleSets DefineCommandSchemas()
        {
            return null;
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
                    var addon = AddonList.SingleOrDefault(a => a.Name == theSchema.CoreName);

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
                //onBotCommand.Invoke(sender, new BotCommandEventArgs(theCmd, e));
            }
            catch (Exception ex)
            {
                // Notify parent
                if (onFailedBotAddonCommand != null)
                    onFailedBotAddonCommand.Invoke(sender, new FailedBotCommandEventArgs(cmd, e));
            }
        }

        public void InitializeAddons()
        {
            // Note: We assume all cores have already been loaded, so just ship them in with the addon initialization

            // First initialize the independent addons
            foreach (var addon in AddonList.Where(addon => addon.AddonDependencies.Count == 0))
            {
                var baseaddon = addon as Addons.Base.AddonBase;
                ComponentList componentList = new Bot.ComponentList(AddonList.Where(a => addon.AddonDependencies.Exists(depName => depName == a.Name)) as IEnumerable<IComponentBase>);
                componentList.AddRange(new CoreList(CoreList.Where(a => addon.CoreDependencies.GetEverything || addon.CoreDependencies.Exists(depName => depName == a.Name))).Cast<IComponentBase>());
                baseaddon.Initialize(componentList);
            }

            // Then initialize the ones with dependencies already initialized.. Rinse and repeat until we can't go further
            bool Change;
            do
            {
                Change = false;
                foreach (var addon in AddonList.Where(zeAddon => zeAddon.IsInitialized == false && zeAddon.AddonDependencies.All(y => AddonList.Any(z => z.Name == y && z.IsInitialized == true))))
                {
                    Change = true;
                    var baseaddon = addon as Addons.Base.AddonBase;
                    ComponentList componentList = new Bot.ComponentList(AddonList.Where(a => addon.AddonDependencies.Exists(depName => depName == a.Name)) as IEnumerable<IComponentBase>);
                    componentList.AddRange(new CoreList(CoreList.Where(a => addon.CoreDependencies.GetEverything || addon.CoreDependencies.Exists(depName => depName == a.Name))).Cast<IComponentBase>());
                    baseaddon.Initialize(componentList);
                }
            }
            while (AddonList.Count(core => core.IsInitialized) < AddonList.Count && Change);

            // The last addons who ultimate require each other.. Let's go a little crazy, but at least try to load the ones with the "most" addons already loaded
            Change = false;
            do
            {
                Change = false;
                foreach (var addon in AddonList.Where(addon => addon.IsInitialized == false).OrderByDescending(_addon => _addon.AddonDependencies.Count(__addon => AddonList.Exists(___addon => ___addon.Name == __addon && ___addon.IsInitialized == true))))
                {
                    try
                    {
                        Change = true;
                        var baseaddon = addon as Addons.Base.AddonBase;
                        ComponentList componentList = new Bot.ComponentList(AddonList.Where(a => addon.AddonDependencies.Exists(depName => depName == a.Name)) as IEnumerable<IComponentBase>);
                        componentList.AddRange(new CoreList(CoreList.Where(a => addon.CoreDependencies.GetEverything || addon.CoreDependencies.Exists(depName => depName == a.Name))).Cast<IComponentBase>());
                        baseaddon.Initialize(componentList);
                    }
                    catch (Exception ex)
                    {
                        // Didn't work :-( Try next?
                    }
                }
            }
            while (AddonList.Count(addon => addon.IsInitialized) < AddonList.Count && Change);
        }

        public void GetSchemas()
        {
            // Get command schemas from addons
            for (int addonIndex = 0; addonIndex < AddonList.Count; addonIndex++)
            {
                try
                {
                    CommandRuleSets cmds = AddonList[addonIndex].DefineCommandSchemas();

                    if (cmds != null)
                    {
                        foreach (var cmd in cmds)
                            commands.ValidateAddSchema(cmd);
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "addon {0}: Could not load command schemas", AddonList[addonIndex].Name);
                }
            }
        }
    }
}
