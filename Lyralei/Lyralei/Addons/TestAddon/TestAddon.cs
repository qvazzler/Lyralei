﻿using Lyralei.Addons.Base;
using Lyralei.Core.ServerQueryConnection.Models;
using Lyralei.TS3_Objects.Entities;
using Lyralei.TS3_Objects.EventArguments;
using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS3QueryLib.Core.CommandHandling;

namespace Lyralei.Addons.TestAddon
{
    class TestAddon : Base.AddonBase, Base.IAddon
    {
        private Core.ServerQueryConnection.ServerQueryConnection ServerQueryConnection;
        private Core.ServerQueryShell.ServerQueryShell ServerQueryAddon; //Just for testing
        private Addons.TestAddon.TestAddon testAddon; // Reference ourselves lol?

        public TestAddon(Subscribers Subscriber) : base(Subscriber)
        {
            this.Name = this.GetType().Name;
            this.CoreDependencies.Add(typeof(Core.ServerQueryConnection.ServerQueryConnection).Name);
            this.CoreDependencies.Add(typeof(Core.ServerQueryShell.ServerQueryShell).Name);
            this.AddonDependencies.Add(typeof(Addons.TestAddon.TestAddon).Name);
        }

        public void UserInitialize(Bot.ComponentList ComponentList)
        {
            // Hello
            this.Name = this.GetType().Name;
            ModelCustomizer.AddModelCustomization(Hooks.ModelCustomizer.OnModelCreating);

            this.ServerQueryConnection = ComponentList[typeof(Core.ServerQueryConnection.ServerQueryConnection).Name] as Core.ServerQueryConnection.ServerQueryConnection;
            this.ServerQueryAddon = ComponentList[typeof(Core.ServerQueryShell.ServerQueryShell).Name] as Core.ServerQueryShell.ServerQueryShell;
            this.testAddon = ComponentList[typeof(TestAddon).Name] as TestAddon;

            // Native serverquery events are in queryRunner object. Uncomment if you want to use them.
            this.ServerQueryConnection.BotCommandAttempt += ServerQueryConnection_BotCommandReceived;
            this.ServerQueryConnection.QueryRunner.Notifications.ClientMoved += Notifications_ClientMoved;
        }

        public void DefineDependencies()
        {

        }

        public void InitializeDependencies()
        {

        }

        public CommandRuleSets DefineCommandSchemas()
        {
            CommandRuleSets ruleSets = new CommandRuleSets();
            CommandParameterGroupListWithRules cmds = new CommandParameterGroupListWithRules();

            CommandParameterGroupWithRules cmdCool = new CommandParameterGroupWithRules();
            cmdCool.Add(new CommandParameterWithRules("coolcommand")
            {
                IsBaseCommand = true
            });
            cmdCool.Add(new CommandParameterWithRules("someparam")
            {
                NameValueSetting = NameValueSetting.ValueOrValueAndName,
                ValueType = TS3_Objects.Entities.ValueType.Integer,
            });

            cmds.Add(cmdCool);
            ruleSets.Add(new CommandRuleSet(this.Name, cmds, Test));

            return ruleSets;
        }

        public void Test(BotCommandEventArgs e)
        {

        }

        private void ServerQueryConnection_BotCommandReceived(object sender, TS3QueryLib.Core.CommandHandling.CommandParameterGroup cmd, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {
            logger.Info("Command objects are tricky business! This command was: {0}", cmd[0].Name);

            if (cmd[0].Name.ToLower() == "test")
            {
                // Both lines below do the same thing, but ServerQueryConnection.TextReply from the AddonBase class tries to make it more convenient for replies :-)

                //this.ServerQueryConnection.queryRunner.SendTextMessage(MessageTarget.Client, e.InvokerClientId, "That's a nice test you have there..");
                ServerQueryConnection.TextReply(e, "That's a nice test you have there..");

                CommandParameter someparam = null;

                if (cmd.Exists(c => (someparam = c).Name == "someparam"))
                {
                    ServerQueryConnection.TextReply(e, String.Format("Nice, a parameter! BTW, If you encode it, it will look like this: {0}", someparam.EncodedValue));
                }
            }
        }

        private void Notifications_ClientMoved(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedEventArgs e)
        {
            var clientInfo = this.ServerQueryConnection.QueryRunner.GetClientInfo(e.ClientId);

            logger.Debug("Client changed channel! His name was {0}", clientInfo.Nickname);
        }
    }
}
