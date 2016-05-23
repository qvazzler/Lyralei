using Lyralei.Addons.Base;
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
using TS3QueryLib.Core.Server.Notification.EventArgs;

namespace Lyralei.Addons.Greeter
{
    class Greeter : Base.AddonBase, Base.IAddon
    {
        private Core.ServerQueryConnection.ServerQueryConnection ServerQueryConnection;
        private Addons.TestAddon.TestAddon testAddon; //Testing purposes

        public Greeter(Subscribers Subscriber) : base(Subscriber)
        {
            this.Name = this.GetType().Name;
            this.CoreDependencies.Add(typeof(Core.ServerQueryConnection.ServerQueryConnection).Name);
            this.AddonDependencies.Add(typeof(Addons.TestAddon.TestAddon).Name);
        }

        public void UserInitialize(Bot.ComponentList ComponentList)
        {
            // Hello
            this.Name = this.GetType().Name;
            ModelCustomizer.AddModelCustomization(Hooks.ModelCustomizer.OnModelCreating);

            this.ServerQueryConnection = ComponentList[typeof(Core.ServerQueryConnection.ServerQueryConnection).Name] as Core.ServerQueryConnection.ServerQueryConnection;
            this.testAddon = ComponentList[typeof(TestAddon.TestAddon).Name] as TestAddon.TestAddon;

            // Native serverquery events are in queryRunner object.
            this.ServerQueryConnection.QueryRunner.Notifications.ClientMoved += Notifications_ClientMoved;
            this.ServerQueryConnection.QueryRunner.Notifications.ClientJoined += onClientConnect;

            BotHasReturnedMessage(); //Tell everyone you're back
        }

        public CommandRuleSets DefineCommandSchemas()
        {
            return null;
        }

        #region Events

        private void onClientConnect(object sender, ClientJoinedEventArgs e)
        {
            GreetUser((int)e.ClientId);
        }

        private void GreetUser(int ClientId)
        {
            ServerQueryConnection.QueryRunner.SendTextMessage(MessageTarget.Client, (uint)ClientId, "Hey there, welcome to the server!");
        }

        private void BotHasReturnedMessage()
        {
            ServerQueryConnection.QueryRunner.SendTextMessage(MessageTarget.Server, (uint)this.Subscriber.VirtualServerId, "I'm up again!");
        }

        #endregion

        private void Notifications_ClientMoved(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedEventArgs e)
        {
            var clientInfo = this.ServerQueryConnection.QueryRunner.GetClientInfo(e.ClientId);

            logger.Debug("Client changed channel! His name was {0}", clientInfo.Nickname);
        }
    }
}
