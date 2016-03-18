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
    public class ServerQueryConnection
    {
        #region Semaphore-enabled Events
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
        #endregion

        //Serverquery user information
        WhoAmIResponse whoAmI;

        //Command stuff
        public string commandPrefix;

        //Event stuff for getting disconnected
        public delegate void StatusUpdateHandler(object sender, EventArgs e);
        public event StatusUpdateHandler ConnectionDown;
        public event StatusUpdateHandler ConnectionUp;

        //Needed stuff for connection
        public AsyncTcpDispatcher atd;
        public QueryRunner queryRunner;
        Subscribers subscriber;
        AddonManager addonManager;

        //Timer-related stuff for connecting to the server
        public readonly TimeSpan MaxWait = TimeSpan.FromMilliseconds(5000);
        private AutoResetEvent connectionChange;

        //Threading-related
        SemaphoreSlim unknownEventQueue; //Used to keep track of double-events and streamline multi-threading to pseudo single-thread

        public ServerQueryConnection(Models.Subscribers _subscriber/*, MySQLInstance _sql, AddonManager _addonManager*/)
        {
            subscriber = _subscriber;
            //addonManager = _addonManager;
            this.connectionChange = new AutoResetEvent(false);
            unknownEventQueue = new SemaphoreSlim(1);
            atd = new AsyncTcpDispatcher(subscriber.ServerIp, (ushort)subscriber.ServerPort);
            queryRunner = new QueryRunner(atd);

            //atd.ServerClosedConnection += atd_ServerClosedConnection;
            atd.ReadyForSendingCommands += atd_ReadyForSendingCommands;
            atd.SocketError += atd_SocketError;

            Connect();
            Login(subscriber);
            SelectServer(subscriber);

            SetName("Lyralei");
            whoAmI = queryRunner.SendWhoAmI();

            registerEvents();
            //queryRunner.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, 1, "HI THAR!");
        }

        void SetName(string nickname)
        {
            string result = "";
            int attempts = 1;
            string modnickname = nickname;

            do
            {
                Command cmd = new Command("clientupdate");
                cmd.AddParameter("client_nickname", modnickname);

                result = queryRunner.SendCommand(cmd);
                attempts += 1;
                modnickname = nickname + attempts;

                if (attempts > 100)
                    throw new Exception(result);

            }
            while (result.StartsWith("error id=513")); //Name taken
        }

        void SelectServer(Subscribers subscriber)
        {
            SimpleResponse selectserver = queryRunner.SelectVirtualServerById((uint)subscriber.VirtualServerId);

            if (selectserver.IsErroneous)
                throw new Exception(selectserver.ErrorMessage);
        }

        void Login(Subscribers subscriber)
        {
            SimpleResponse login = queryRunner.Login(subscriber.AdminUsername, subscriber.AdminPassword);

            if (login.IsErroneous == true)
                throw new Exception(login.ErrorMessage);
        }

        void Connect()
        {
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
                //Connected!
            }
            else
            {
                throw new Exception("Could not connect to teamspeak server");
            }
        }

        private void registerEvents()
        {
            queryRunner.UnknownNotificationReceived += Notifications_UnknownNotificationReceived;

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
                    Login(subscriber);

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
            //For now don't use any old command parser
            //addonManager.addons.ForEach(f => f.onClientMessage(sender, e));
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
