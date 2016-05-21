using Lyralei.TS3_Objects.Entities;
using Lyralei.TS3_Objects.EventArguments;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Core.CoreManager
{
    public class CoreManager
    {
        private Logger logger;

        private List<Base.ICore> CoreList;

        public delegate void BotCommandFail(object sender, FailedBotCommandEventArgs e);
        public event BotCommandFail onFailedBotCommand;

        public delegate void BotCommand(object sender, BotCommandEventArgs e);
        public event BotCommand onBotCommand;

        CommandRuleSets commands = new CommandRuleSets();

        private Core.ServerQueryConnection.Models.Subscribers subscriber;
        public Core.ServerQueryConnection.Models.Subscribers Subscriber
        {
            get { return subscriber; }
            set
            {
                subscriber = value;
                logger = LogManager.GetLogger(this.GetType().Name + " - " + Subscriber.ToString());
            }
        }

        public CoreManager(ServerQueryConnection.Models.Subscribers Subscriber)
        {
            this.Subscriber = Subscriber;
            CoreList = new List<Base.ICore>();

            CoreList.Add(new ServerQueryConnection.ServerQueryConnection(this.Subscriber));
            CoreList.Add(new UserManager.UserManager(this.Subscriber));
            //CoreList.Add(new Test.TestCore(this.Subscriber));
            CoreList.Add(new InputOwner.InputOwnerAddon(this.Subscriber));
            CoreList.Add(new ServerQueryShell.ServerQueryShell(this.Subscriber));
            CoreList.Add(new AddonManager.AddonManager(this.Subscriber));
            CoreList.Add(new PermissionManager.PermissionManager(this.Subscriber));

            //CoreList.Add(new AddonManager.AddonManager(this.Subscriber));

            InitializeCores();
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
                    var addon = CoreList.SingleOrDefault(a => a.Name == theSchema.CoreName);

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

        public void InitializeCores()
        {
            // First initialize the independent addons
            foreach (var core in CoreList.Where(core => core.CoreDependencies.Count == 0 && core.CoreDependencies.GetEverything == false))
            {
                var basecore = core as Core.Base.CoreBase;
                basecore.Initialize(new CoreList(CoreList.Where(a => core.CoreDependencies.Exists(depName => depName == a.Name))));
            }

            // Then initialize the ones with dependencies already initialized.. Rinse and repeat until we can't go further
            bool Change = false;

            do
            {
                Change = false;
                foreach (var core in CoreList.Where(zeCore => zeCore.IsInitialized == false && zeCore.CoreDependencies.GetEverything == false && zeCore.CoreDependencies.All(y => CoreList.Any(z => z.Name == y && z.IsInitialized == true))))
                {
                    Change = true;
                    var basecore = core as Core.Base.CoreBase;
                    basecore.Initialize(new CoreList(CoreList.Where(a => core.CoreDependencies.Exists(depName => depName == a.Name))));
                }
            }
            while (CoreList.Count(core => core.IsInitialized) < CoreList.Count && Change);

            // The last addons who ultimate require each other.. Let's go a little crazy, but at least try to load the ones with the "most" addons already loaded
            Change = false;
            do
            {
                Change = false;
                foreach (var core in CoreList.Where(xx => xx.IsInitialized == false && xx.CoreDependencies.GetEverything == false).OrderByDescending(x => x.CoreDependencies.Count(y => CoreList.Exists(z => z.Name == y && z.IsInitialized == true))))
                {
                    try
                    {
                        var basecore = core as Core.Base.CoreBase;
                        basecore.Initialize(new CoreList(CoreList.Where(a => core.CoreDependencies.Exists(depName => depName == a.Name))));
                        Change = true;
                    }
                    catch (Exception)
                    {
                        // Didn't work :-( Try next?
                    }
                }
            }
            while (CoreList.Count(core => core.IsInitialized) < CoreList.Count && Change);

            // Finally initialize the crazy bastards who wants _all_ the other cores injected (like AddonManager)
            foreach (var core in CoreList.Where(core => core.IsInitialized == false && core.CoreDependencies.GetEverything == true))
            {
                var basecore = core as Core.Base.CoreBase;
                basecore.Initialize(new CoreList(CoreList));
            }
        }

        public void GetSchemas()
        {
            // Get command schemas from addons
            for (int addonIndex = 0; addonIndex < CoreList.Count; addonIndex++)
            {
                try
                {
                    CommandRuleSets cmds = CoreList[addonIndex].DefineCommandSchemas();

                    if (cmds != null)
                    {
                        foreach (var cmd in cmds)
                            commands.ValidateAddSchema(cmd);
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "addon {0}: Could not load command schemas", CoreList[addonIndex].Name);
                }
            }
        }
    }
}
