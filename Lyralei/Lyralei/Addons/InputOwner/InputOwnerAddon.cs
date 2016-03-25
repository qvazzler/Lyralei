using Lyralei.Addons.Base;
using Lyralei.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Notification.EventArgs;

using Microsoft.Data.Entity;
using System.Threading;

namespace Lyralei.Addons.InputOwner
{
    class InputOwnerAddon : AddonBase, IAddon
    {
        public string AddonName { get; set; } = "InputOwner";

        public Test.TestAddon testAddon = null;
        private Models.InputOwners AddonThatOwnsInput;

        public delegate bool AddonOwnershipChanged(object sender, IAddon Addon);
        public event AddonOwnershipChanged onAddonOwnershipChanged;

        public delegate bool AddonOwnershipReleaseRequest(object sender, IAddon Addon);
        public event AddonOwnershipReleaseRequest onAddonOwnershipReleaseRequest;

        public delegate bool AddonOwnershipRevoked(object sender, IAddon Addon);
        public event AddonOwnershipRevoked onAddonOwnershipRevoked;

        public delegate bool AddonInputReceived(object sender, IAddon Addon);
        public event AddonInputReceived onInputReceived;

        public void Initialize()
        {
            this.serverQueryRootConnection.BotCommandReceived += onBotCommand;

            ModelCustomizer.AddModelCustomization(Hooks.ModelCustomizer.OnModelCreating);

            //Add a dependency
            this.dependencyManager.AddDependencyRequirement("Test");
            this.dependencyManager.UpdateInjections();
            testAddon = (Test.TestAddon)dependencyManager.GetAddon("Test");
        }

        private void onBotCommand(object sender, CommandParameterGroup cmdPG, MessageReceivedEventArgs e)
        {
            //Command cmd = new Command("help", new string[] { "serverinfo", "additionaltest" });
            Command cmd = new Command(cmdPG);
            Bot.UserManager userManager = new Bot.UserManager(this.subscriber.SubscriberId);

            //userManager.GetUserInformation

            if (cmd.Name.ToLower() == "blah")
            {

            }
        }

        public void RequestInput(IAddon Addon, Lyralei.Models.Users user, int? inputWaitDuration = null, Props.ReleaseRequestAction releaseRequestAction = Props.ReleaseRequestAction.Ask, Props.QueuePosition queuePosition = Props.QueuePosition.Last)
        {
            TimeSpan? ts = null;
            if (inputWaitDuration != null)
                ts = TimeSpan.FromSeconds(Convert.ToInt32(inputWaitDuration));

            Models.InputOwners inputOwner = new Models.InputOwners()
            {
                AddonName = Addon.AddonName,
                ReleaseRequestAction = releaseRequestAction,
                QueuePosition = queuePosition,
                InputWaitDuration = ts,
                UserId = user.UserId,
                Users = user,
            };

            using (var db = new CoreContext())
            {
                db.InputOwners.Add(inputOwner);
            }
        }

        private void UpdateInputQueue()
        {
            using (var db = new CoreContext())
            {
                var superImportantBastards = db.InputOwners.Where(inputRequest => inputRequest.QueuePosition == Props.QueuePosition.TryInterruptCurrent).OrderBy(x => x.Created);
                var kindaImportantBastards = db.InputOwners.Where(inputRequest => inputRequest.QueuePosition == Props.QueuePosition.First).OrderBy(x => x.Created);
                var hardlyImportantBastards = db.InputOwners.Where(inputRequest => inputRequest.QueuePosition == Props.QueuePosition.Last).OrderBy(x => x.Created);
                var notImportantBastards = db.InputOwners.Where(inputRequest => inputRequest.QueuePosition == Props.QueuePosition.NotQueueing).OrderBy(x => x.Created);

                //var doNotDisturb = db.InputOwners.Where(inputRequest => inputRequest.ReleaseRequestAction == Props.ReleaseRequestAction.Refuse);
                //var knockFirst = db.InputOwners.Where(inputRequest => inputRequest.ReleaseRequestAction == Props.ReleaseRequestAction.Ask);
                //var alwaysOpen = db.InputOwners.Where(inputRequest => inputRequest.ReleaseRequestAction == Props.ReleaseRequestAction.Allow);

                if (AddonThatOwnsInput == null)
                {
                    //Sure, have the input
                    if (superImportantBastards.Count() > 0) { AddonThatOwnsInput = superImportantBastards.First(); }
                    else if (kindaImportantBastards.Count() > 0) { AddonThatOwnsInput = kindaImportantBastards.First(); }
                    else if (hardlyImportantBastards.Count() > 0) { AddonThatOwnsInput = hardlyImportantBastards.First(); }
                    else if (notImportantBastards.Count() > 0) { AddonThatOwnsInput = notImportantBastards.First(); }

                    if (AddonThatOwnsInput == /*still*/ null)
                        return;
                    else
                        NotifyInputOwnershipGiven(AddonThatOwnsInput);

                }
                else if (AddonThatOwnsInput.ReleaseRequestAction == Props.ReleaseRequestAction.Refuse)
                {
                    return;
                }
                else if (AddonThatOwnsInput.ReleaseRequestAction == Props.ReleaseRequestAction.Ask)
                {
                    return;
                }
            }
        }

        private void NotifyInputOwnershipGiven(Models.InputOwners Addon)
        {
            
        }

        private void NotifyAddonInputReleaseRequest(Models.InputOwners Addon)
        {

        }

        private void NotifyAddonInputOwnershipRevoked(Models.InputOwners Addon)
        {

        }

        private void NotifyInputReceived(Models.InputOwners Addon)
        {

        }
    }
}
