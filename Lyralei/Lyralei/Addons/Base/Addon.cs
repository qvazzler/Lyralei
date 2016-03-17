﻿using System;
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
using Lyralei.Bot.Commands;

namespace Lyralei.Addons.Base
{
    //The Addon base class assigns all the variables. This saves some space for creating multiple addon classes.
    abstract public class Addon
    {
        //public delegate void Sync_ClientMessageReceived(object source, MyEventArgs e);

        public List<string> eventDumpStrings; //This object is used to keep track of double-firing events in channel-related events.. Messy but works
        public ServerQueryConnection serverQueryConnection;
        //public MySQLInstance sql;
        public Models.Subscribers subscriber;
        public QueryRunner queryRunner;
        public AsyncTcpDispatcher atd;
        public SemaphoreSlim QueryQueue;
        //public BotCommandPrefaceList commandlist;

        public Addon()
        {

        }

        public Addon(ServerQueryConnection _serverQueryConnection, Models.Subscribers _subscriber)
        {
            Load(_serverQueryConnection, _subscriber);
        }

        public void Load(ServerQueryConnection _serverQueryConnection, Models.Subscribers _subscriber)
        {
            //sql = _sql;
            serverQueryConnection = _serverQueryConnection;
            queryRunner = _serverQueryConnection.queryRunner;
            atd = _serverQueryConnection.atd;
            subscriber = _subscriber;
            //QueryQueue = _serverQueryConnection.queryQueue;
            //commandlist = new BotCommandPrefaceList();
            eventDumpStrings = new List<string>();
        }

        public virtual void onClientMessage(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e/*, BotCommandInput input, BotCommandPreface preface*/)
        {

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