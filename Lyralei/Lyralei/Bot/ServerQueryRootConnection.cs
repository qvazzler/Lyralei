using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;

using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Responses;
using TS3QueryLib.Core.Server.Entities;
using TS3QueryLib.Core.CommandHandling;
using System.Threading;
using TS3QueryLib.Core.Server.Notification.EventArgs;

using Lyralei.TS3_Objects.EventArguments;
using Lyralei.TS3_Objects.Entities;
using Lyralei.Addons.Base;
using Lyralei.Addons;
using Lyralei.Models;

namespace Lyralei.Bot
{
    public class ServerQueryRootConnection : ServerQueryBaseConnection
    {
        //TODO: Add something to EventArgs to let us decide whether events should keep getting invoked to other addons..
        //      Should probably consider alterating TS3QueryLib with this, adding a simple "Handled" property to all eventargs and put the logic in by ourselves
        //      Actually, maybe it's better to keep this with the bot.. Not everyone would want to follow this implementation
        public delegate void BotCommandReceive(object sender, CommandParameterGroup cmd, MessageReceivedEventArgs e);
        public event BotCommandReceive BotCommandReceived;

        public ServerQueryRootConnection(Models.Subscribers _subscriber, bool autoconnect = false) : base(_subscriber, autoconnect)
        {
            //RegisterEvents();
            this.queryRunner.Notifications.ClientMessageReceived += Notifications_ClientMessageReceived;
        }

        private void Notifications_ClientMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //addonManager.addons.ForEach(f => f.onClientMessage(sender, e));
            if (e.InvokerClientId != whoAmI.ClientId)
            {
                if (e.Message.StartsWith("!"))
                {
                    string cmd = e.Message.Remove(0, 1);

                    var cmdPGL = CommandParameterGroupList.Parse(cmd);

                    foreach (CommandParameterGroup cmdPG in cmdPGL)
                        BotCommandReceived.Invoke(this, cmdPG, e);
                }
            }
        }
    }
}
