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
using TS3QueryLib.Core.CommandHandling;
using System.Threading;
using TS3QueryLib.Core.Server.Notification.EventArgs;

using Lyralei.TS3_Objects.EventArguments;
using Lyralei.TS3_Objects.Entities;
using Lyralei.Addons.Base;
using Lyralei.Addons;
using Lyralei.Models;
using NLog;
using NLog.Fluent;

namespace Lyralei.Bot
{
    public class ServerQueryBaseConnection : IDisposable
    {
        bool disposing = false;
        //private Logger logger = LogManager.GetCurrentClassLogger();
        private Logger logger;

        //Serverquery user information
        public WhoAmIResponse whoAmI;
        private string Name;

        //Command stuff
        public string commandPrefix;

        //Event stuff for getting disconnected
        public delegate void StatusUpdateHandler(object sender, EventArgs e);
        public event StatusUpdateHandler ConnectionDown;
        public event StatusUpdateHandler ConnectionUp;

        public delegate void BotCommandAttemptReceive(object sender, CommandParameterGroup cmd, MessageReceivedEventArgs e);
        public event BotCommandAttemptReceive BotCommandAttemptReceived;


        //Needed stuff for connection
        public AsyncTcpDispatcher atd;
        public QueryRunner queryRunner;

        private Subscribers subscriber;
        public Subscribers Subscriber
        {
            get { return subscriber; }
            set
            {
                subscriber = value;
                logger = LogManager.GetLogger(this.GetType().Name + " - " + subscriber.ToString());
            }
        }

        //Timer-related stuff for connecting to the server
        //public readonly TimeSpan MaxWait = TimeSpan.FromMilliseconds(5000);
        private AutoResetEvent connectionChange;

        //Threading-related
        //SemaphoreSlim unknownEventQueue; //Used to keep track of double-events and streamline multi-threading to pseudo single-thread

        public void Dispose()
        {
            if (disposing)
                return;

            disposing = true;

            queryRunner.Logout();
            atd.Disconnect();
            queryRunner.Dispose();
            atd.Dispose();
        }

        public ServerQueryBaseConnection()
        {
            logger = LogManager.GetLogger(this.GetType().Name);
        }

        public ServerQueryBaseConnection(Models.Subscribers _subscriber, bool autoconnect = false)
        {
            logger = LogManager.GetLogger(this.GetType().Name);

            UpdateSubscriberInfo(_subscriber);

            if (autoconnect)
                Initialize();
        }

        public void UpdateSubscriberInfo(Models.Subscribers _subscriber)
        {
            Subscriber = _subscriber;
        }

        public void Initialize()
        {
            if (Name == null)
                Name = subscriber.AdminUsername;

            this.connectionChange = new AutoResetEvent(false);
            //unknownEventQueue = new SemaphoreSlim(1);
            atd = new AsyncTcpDispatcher(Subscriber.ServerIp, (ushort)Subscriber.ServerPort);
            queryRunner = new QueryRunner(atd);

            //atd.ServerClosedConnection += atd_ServerClosedConnection;
            atd.ReadyForSendingCommands += atd_ReadyForSendingCommands;
            atd.SocketError += atd_SocketError;

            Connect();

            SimpleResponse loginResult = Login();

            if (loginResult.IsBanned)
                logger.Info("Login failure due to ban: {0}", loginResult.ResponseText);
            else if (loginResult.IsErroneous)
                logger.Info("Login failure due to error: {0}", loginResult.ResponseText);
            else
            {
                SendSetNameCmd();

                SelectServer(subscriber);

                SendSetNameCmd();

                UpdateServerUniqueId();

                whoAmI = queryRunner.SendWhoAmI();

                RegisterEvents();
            }
        }

        public void InitializeQuiet()
        {
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Could not initialize connection");
            }
        }

        void UpdateServerUniqueId()
        {
            string uniqueId = queryRunner.GetServerInfo().UniqueId;

            using (var db = new CoreContext())
            {
                Subscribers sub = db.Subscribers.Single(s => s.SubscriberId == subscriber.SubscriberId);
                //using (SubscriberUniqueId
                if (sub.SubscriberUniqueId != uniqueId)
                {
                    sub.SubscriberUniqueId = uniqueId;

                    db.SaveChanges();
                }
            }
        }

        private void SendSetNameCmd()
        {
            string result = "";
            int attempts = 1;
            string modnickname = Name;

            do
            {
                Command cmd = new Command("clientupdate");
                cmd.AddParameter("client_nickname", modnickname);

                result = queryRunner.SendCommand(cmd);
                attempts += 1;
                modnickname = Name + attempts;

                if (attempts > 100)
                    throw new Exception(result);

            }
            while (result.StartsWith("error id=513")); //Name taken
        }

        public void SetName(string nickname)
        {
            Name = nickname;

            try
            {
                if (atd.IsConnected)
                    SendSetNameCmd();
            }
            catch (Exception)
            {
                //Suppress
            }
        }

        void SelectServer(Subscribers subscriber)
        {
            SimpleResponse selectserver = queryRunner.SelectVirtualServerById((uint)subscriber.VirtualServerId);

            if (selectserver.IsErroneous)
                throw new Exception(selectserver.ErrorMessage);
        }

        public SimpleResponse Login()
        {
            SimpleResponse login = queryRunner.Login(Subscriber.AdminUsername, Subscriber.AdminPassword);

            return login; //Does not throw any exceptions
        }

        public void Connect()
        {
            //Console.WriteLine(
            //"Background Thread: SynchronizationContext.Current is " +
            //(SynchronizationContext.Current != null ?
            //SynchronizationContext.Current.ToString() : "null"));

            atd.Connect();
            connectionChange.WaitOne();

            //TODO: Add logic here that checks if socket is null. That can mean many things..
            //      But apparently one of them is that you have been banned (for not being whitelisted mainly)
            if (atd.Socket == null)
            {
                throw new Exception("Socket did not initialize: Are you whitelisted?");
            }
            if (atd.Socket.Connected)
            {
                logger.Info("Connected");
            }
            else
            {
                throw new Exception("Could not connect to teamspeak server");
            }
        }

        public void RegisterEvents()
        {
            queryRunner.UnknownNotificationReceived -= Notifications_UnknownNotificationReceived;

            queryRunner.Notifications.ClientMoved += Notifications_ClientMoved;
            queryRunner.Notifications.ClientJoined += Notifications_ClientJoined;
            queryRunner.Notifications.ClientMoveForced += Notifications_ClientMoveForced;
            queryRunner.Notifications.ClientDisconnect += Notifications_ClientDisconnect;
            queryRunner.Notifications.ClientConnectionLost += Notifications_ClientConnectionLost;
            queryRunner.Notifications.ServerMessageReceived += Notifications_ServerMessageReceived;
            queryRunner.Notifications.ChannelMessageReceived += Notifications_ChannelMessageReceived;
            queryRunner.Notifications.ClientMessageReceived += Notifications_ClientMessageReceived;

            queryRunner.Notifications.ClientBan += Notifications_ClientBan;
            queryRunner.Notifications.ClientMovedByTemporaryChannelCreate += Notifications_ClientMovedByTemporaryChannelCreate;
            queryRunner.Notifications.ClientKick += Notifications_ClientKick;

            queryRunner.Notifications.ChannelCreated += Notifications_ChannelCreated;
            queryRunner.Notifications.ChannelEdited += Notifications_ChannelEdited;
            queryRunner.Notifications.ChannelMoved += Notifications_ChannelMoved;


            if (atd != null)
            {
                if (atd.Socket != null)
                {
                    if (atd.Socket.Connected == true)
                    {
                        SimpleResponse regNot = queryRunner.RegisterForNotifications(ServerNotifyRegisterEvent.Server);
                        regNot = queryRunner.RegisterForNotifications(ServerNotifyRegisterEvent.Channel, 0);
                        regNot = queryRunner.RegisterForNotifications(ServerNotifyRegisterEvent.TextPrivate);
                    }
                }
            }
        }

        public void UnregisterEvents()
        {
            queryRunner.UnknownNotificationReceived -= Notifications_UnknownNotificationReceived;

            queryRunner.Notifications.ClientMoved -= Notifications_ClientMoved;
            queryRunner.Notifications.ClientJoined -= Notifications_ClientJoined;
            queryRunner.Notifications.ClientMoveForced -= Notifications_ClientMoveForced;
            queryRunner.Notifications.ClientDisconnect -= Notifications_ClientDisconnect;
            queryRunner.Notifications.ClientConnectionLost -= Notifications_ClientConnectionLost;
            queryRunner.Notifications.ServerMessageReceived -= Notifications_ServerMessageReceived;
            queryRunner.Notifications.ChannelMessageReceived -= Notifications_ChannelMessageReceived;
            queryRunner.Notifications.ClientMessageReceived -= Notifications_ClientMessageReceived;

            queryRunner.Notifications.ClientBan -= Notifications_ClientBan;
            queryRunner.Notifications.ClientMovedByTemporaryChannelCreate -= Notifications_ClientMovedByTemporaryChannelCreate;
            queryRunner.Notifications.ClientKick -= Notifications_ClientKick;

            queryRunner.Notifications.ChannelCreated -= Notifications_ChannelCreated;
            queryRunner.Notifications.ChannelEdited -= Notifications_ChannelEdited;
            queryRunner.Notifications.ChannelMoved -= Notifications_ChannelMoved;


            if (atd != null)
            {
                if (atd.Socket != null)
                {
                    if (atd.Socket.Connected == true)
                    {
                        SimpleResponse regNot = queryRunner.UnregisterNotifications();
                        //regNot = queryRunner.UnregisterNotifications(ServerNotifyRegisterEvent.Channel, 0);
                        //regNot = queryRunner.UnregisterNotifications(ServerNotifyRegisterEvent.TextPrivate);
                    }
                }
            }
        }

        #region ATD Events

        void atd_ReadyForSendingCommands(object sender, EventArgs e)
        {
            connectionChange.Set();
        }

        void atd_SocketError(object sender, TS3QueryLib.Core.Communication.SocketErrorEventArgs e)
        {
            connectionChange.Set();
        }

        void atd_ServerClosedConnection(object sender, EventArgs e)
        {
            ConnectionDown.Invoke(this, new EventArgs());
            //AsyncTcpDispatcher was disconnected due to inactivity or other reasons. We need to try to reconnect.
            while (true)
            {
                try
                {
                    Thread.Sleep(5000);
                    Connect();
                    Login();

                    //All is well, we can exit this function now
                    ConnectionUp.Invoke(this, new EventArgs());
                    return;
                }
                catch (Exception)
                {
                    //Could not connect or login to serverquery
                }
            }
        }

        #endregion

        #region Events

        //Unknown or misc event
        private void Notifications_UnknownNotificationReceived(object sender, TS3QueryLib.Core.Common.EventArgs<string> e)
        {
            //try
            //{
            //    unknownEventQueue.Wait();
            //    string[] query = e.Value.ToString().Replace("\\/", "/").TrimEnd(new char[] { '\r', '\n' }).Split(' ');

            //}
            //catch (Exception)
            //{

            //}
            //finally
            //{
            //    unknownEventQueue.Release();
            //}
        }

        //NO LONGER NEEDED

        private void Notifications_ChannelMoved(object sender, ChannelMovedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Notifications_ChannelEdited(object sender, ChannelEditedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Notifications_ChannelCreated(object sender, ChannelCreatedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        ////Server message
        private void Notifications_ServerMessageReceived(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {
            //addonManager.addons.ForEach(f => f.onServerMessage(sender, e));
        }

        //Channel message
        private void Notifications_ChannelMessageReceived(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {
            //addonManager.addons.ForEach(f => f.onChannelMessage(sender, e));
        }

        //Client message
        private void Notifications_ClientMessageReceived(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {
            //Bot Commands
            if (e.InvokerClientId != whoAmI.ClientId)
            {
                if (e.Message.StartsWith("!"))
                {
                    string cmd = e.Message.Remove(0, 1);

                    var cmdPGL = CommandParameterGroupList.Parse(cmd);

                    foreach (CommandParameterGroup cmdPG in cmdPGL)
                        BotCommandAttemptReceived.Invoke(this, cmdPG, e);
                }
            }
        }

        //Client disconnected from server
        private void Notifications_ClientDisconnect(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientDisconnectEventArgs e)
        {
            //addonManager.addons.ForEach(f => f.onClientDisconnect(sender, e));
        }

        //Client joins server
        private void Notifications_ClientJoined(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientJoinedEventArgs e)
        {
            //addonManager.addons.ForEach(f => f.onClientJoined(sender, e));
        }

        //Client is kicked
        private void Notifications_ClientKick(object sender, ClientKickEventArgs e)
        {
            //addonManager.addons.ForEach(f => f.onClientKick(sender, e));
        }

        //Client moved by temporary channel (double-evented)
        private void Notifications_ClientMovedByTemporaryChannelCreate(object sender, ClientMovedEventArgs e)
        {
            //addonManager.addons.ForEach(f => f.onClientMovedByTemporaryChannelCreate(sender, e));
        }

        //Client moved by mod/admin (double-evented) x
        private void Notifications_ClientMoveForced(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedByClientEventArgs e)
        {
            //addonManager.addons.ForEach(f => f.onClientMoveForced(sender, e));
        }

        //Client moved (double-evented) x
        private void Notifications_ClientMoved(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedEventArgs e)
        {
            //addonManager.addons.ForEach(f => f.onClientMoved(sender, e));
        }

        //Client banned from server
        private void Notifications_ClientBan(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientBanEventArgs e)
        {
            //addonManager.addons.ForEach(f => f.onClientBan(sender, e));
        }

        //Client timed out
        private void Notifications_ClientConnectionLost(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientConnectionLostEventArgs e)
        {
            //addonManager.addons.ForEach(f => f.onClientConnectionLost(sender, e));
        }

        #endregion

        public string EscapeChars(string value)
        {
            string newval = value;
            newval.Replace(@"\", @"\\");
            newval.Replace(@"/", @"\/");
            newval.Replace(@" ", @"\s");
            newval.Replace(@"|", @"\p");
            newval.Replace(Convert.ToString((char)7), @"\a");
            newval.Replace(Convert.ToString((char)8), @"\b");
            newval.Replace(Convert.ToString((char)12), @"\f");
            newval.Replace(Convert.ToString((char)10), @"\n");
            newval.Replace(Convert.ToString((char)13), @"\r");
            newval.Replace(Convert.ToString((char)9), @"\t");
            newval.Replace(Convert.ToString((char)11), @"\v");

            return newval;
        }

        public string UnEscapeChars(string value)
        {
            throw new NotImplementedException();
        }
    }
}
