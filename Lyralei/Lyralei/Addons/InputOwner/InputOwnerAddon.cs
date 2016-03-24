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

        public void RequestInput(IAddon Addon, Lyralei.Models.Users user, Props.PriorityLevel priorityLevel, TimeSpan inputWaitDuration)
        {
            RequestInput(Addon, user, priorityLevel, Convert.ToInt32(inputWaitDuration.TotalSeconds));
        }

        public void RequestInput(IAddon Addon, Lyralei.Models.Users user, Props.PriorityLevel priorityLevel, int inputWaitDuration = 120)
        {
            Models.InputOwners inputOwner = new Models.InputOwners()
            {
                Created = DateTime.Now,
                Priority = priorityLevel,
                InputWaitDuration = TimeSpan.FromSeconds(inputWaitDuration),
                UserId = user.UserId,
                Users = user,
            };

            using (var db = new CoreContext())
            {
                db.InputOwners.Add(inputOwner);
            }
        }
    }
}
