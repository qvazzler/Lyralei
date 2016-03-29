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
using TS3QueryLib.Core.Server.Notification.EventArgs;
using System.Threading;
using System.Threading.Tasks;
using Lyralei.Bot;
using Lyralei.TS3_Objects.EventArguments;

using NLog;

namespace Lyralei.Addons.Base
{
    // The Addon base class assigns all the variables and stuff.
    abstract public class AddonBase : IAddonBase
    {
        // Addon dependencies
        public AddonDependencyManager dependencyManager { get; set; }

        public ServerQueryRootConnection serverQueryRootConnection;
        public Models.Subscribers subscriber;

        // 'Shortcuts'
        public QueryRunner queryRunner;
        public AsyncTcpDispatcher atd;

        protected Logger logger;

        

        public AddonBase()
        {
            logger = LogManager.GetCurrentClassLogger();

            dependencyManager = new AddonDependencyManager(this.subscriber);
        }

        public void Configure(Models.Subscribers subscriber, ServerQueryRootConnection serverQueryRootConnection)
        {
            //sql = _sql;
            this.serverQueryRootConnection = serverQueryRootConnection;
            queryRunner = serverQueryRootConnection.queryRunner;
            atd = serverQueryRootConnection.atd;
            this.subscriber = subscriber;

            this.dependencyManager.subscriber = this.subscriber;
            //QueryQueue = _serverQueryConnection.queryQueue;
            //commandlist = new BotCommandPrefaceList();
        }

        public virtual void onClientMessage(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e/*, BotCommandInput input, BotCommandPreface preface*/)
        {

        }

        protected void TextReply(MessageReceivedEventArgs e, string msg)
        {
            this.queryRunner.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Client, e.InvokerClientId, msg);
        }

        //private void Notifications_UnknownNotificationReceived(object sender, TS3QueryLib.Core.Common.EventArgs<string> e)
        //{

        //}

        public virtual void onServerMessage(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {

        }

        public virtual void onChannelMessage(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {

        }

        public virtual void onClientDisconnect(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientDisconnectEventArgs e)
        {

        }

        public virtual void onClientJoined(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientJoinedEventArgs e)
        {

        }

        public virtual void onClientKick(object sender, ClientKickEventArgs e)
        {

        }

        public virtual void onClientMovedByTemporaryChannelCreate(object sender, ClientMovedEventArgs e)
        {

        }

        public virtual void onClientMoveForced(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedByClientEventArgs e)
        {

        }

        public virtual void onClientMoved(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedEventArgs e)
        {

        }

        public virtual void onClientBan(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientBanEventArgs e)
        {

        }

        public virtual void onClientConnectionLost(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientConnectionLostEventArgs e)
        {

        }

        public virtual void onChannelCreated(object sender, ChannelCreatedEventArgs e)
        {

        }

        public virtual void onChannelMoved(object sender, ChannelMovedEventArgs e)
        {

        }

        public virtual void onChannelEdited(object sender, ChannelEditedEventArgs e)
        {

        }

        //public event EventHandler<MessageReceivedEventArgs> Sync_ChannelMessageReceived;
        //public event EventHandler<ClientBanEventArgs> Sync_ClientBan;
        //public event EventHandler<ClientConnectionLostEventArgs> Sync_ClientConnectionLost;
        //public event EventHandler<ClientDisconnectEventArgs> Sync_ClientDisconnect;
        //public event EventHandler<ClientJoinedEventArgs> Sync_ClientJoined;
        //public event EventHandler<ClientKickEventArgs> Sync_ClientKick;
        ////public event EventHandler<MessageReceivedEventArgs> Sync_ClientMessageReceived;
        //public event EventHandler<ClientMovedEventArgs> Sync_ClientMoved;
        //public event EventHandler<ClientMovedEventArgs> Sync_ClientMovedByTemporaryChannelCreate;
        //public event EventHandler<ClientMovedByClientEventArgs> Sync_ClientMoveForced;
        //public event EventHandler<MessageReceivedEventArgs> Sync_ServerMessageReceived;
        //public event EventHandler<ChannelCreatedEventArgs> Sync_ChannelCreated; //Custom event
    }
}
