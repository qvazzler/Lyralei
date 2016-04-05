using Lyralei.Addons.Base;
using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS3QueryLib.Core.CommandHandling;

namespace Lyralei.Addons.Test
{
    class TestAddon : AddonBase, IAddon
    {
        public string AddonName { get; set; } = "Test";

        public void Initialize()
        {
            ModelCustomizer.AddModelCustomization(Hooks.ModelCustomizer.OnModelCreating);

            // Native serverquery events are in queryRunner object. Uncomment if you want to use them.
            this.serverQueryRootConnection.BotCommandReceived += ServerQueryRootConnection_BotCommandReceived;
            this.serverQueryRootConnection.queryRunner.Notifications.ClientMoved += Notifications_ClientMoved;

            logger.Debug("TestAddon initialized!");
        }

        public void DefineDependencies()
        {

        }

        public void InitializeDependencies()
        {

        }

        private void ServerQueryRootConnection_BotCommandReceived(object sender, TS3QueryLib.Core.CommandHandling.CommandParameterGroup cmd, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {
            logger.Debug("Command objects are tricky business! This command was: {0}", cmd[0].Name);

            if (cmd[0].Name.ToLower() == "test")
            {
                // Both lines below do the same thing, but TextReply from the AddonBase class tries to make it more convenient for replies :-)

                //this.serverQueryRootConnection.queryRunner.SendTextMessage(MessageTarget.Client, e.InvokerClientId, "That's a nice test you have there..");
                TextReply(e, "That's a nice test you have there..");

                CommandParameter someparam = null;

                if (cmd.Exists(c => (someparam = c).Name == "someparam"))
                {
                    TextReply(e, String.Format("Nice, a parameter! BTW, If you encode it, it will look like this: {0}", someparam.EncodedValue));
                }
            }
        }

        private void Notifications_ClientMoved(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedEventArgs e)
        {
            var clientInfo = queryRunner.GetClientInfo(e.ClientId);

            logger.Debug("Client changed channel! His name was {0}", clientInfo.Nickname);
        }
    }
}
