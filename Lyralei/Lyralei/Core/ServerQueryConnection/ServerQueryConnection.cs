using System;
using System.Linq;

using TS3QueryLib.Core;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Responses;
using TS3QueryLib.Core.Server.Entities;
using TS3QueryLib.Core.CommandHandling;
using System.Threading;
using TS3QueryLib.Core.Server.Notification.EventArgs;
using Lyralei.Core.ServerQueryConnection.Models;
using NLog;
using Lyralei.TS3_Objects.Entities;

namespace Lyralei.Core.ServerQueryConnection
{
    public class ServerQueryConnection : Base.CoreBase, IDisposable, Base.ICore
    {
        bool disposing = false;

        //Serverquery user information
        public WhoAmIResponse whoAmI;
        private string Nickname;

        //Command stuff
        public string commandPrefix;

        //Event stuff for getting disconnected
        public delegate void StatusUpdateHandler(object sender, EventArgs e);
        public event StatusUpdateHandler ConnectionDown;
        public event StatusUpdateHandler ConnectionUp;

        public delegate void BotCommandAttemptReceive(object sender, CommandParameterGroup cmd, MessageReceivedEventArgs e);
        public event BotCommandAttemptReceive BotCommandAttempt;

        //Needed stuff for connection
        public AsyncTcpDispatcher AsyncTcpDispatcher;
        public QueryRunner QueryRunner;

        //Timer-related stuff for connecting to the server
        //public readonly TimeSpan MaxWait = TimeSpan.FromMilliseconds(5000);
        private AutoResetEvent connectionChange;

        public void Dispose()
        {
            if (disposing)
                return;

            disposing = true;

            Logout();
            Disconnect();
            QueryRunner.Dispose();
            AsyncTcpDispatcher.Dispose();
        }

        public ServerQueryConnection(Core.ServerQueryConnection.Models.Subscribers Subscriber) : base(Subscriber)
        {
            this.Name = this.GetType().Name;
        }

        public void UserInitialize(CoreList AddonInjections)
        {
            if (Nickname == null)
            {
                if (this.Subscriber.BotNickName == null)
                    Nickname = Subscriber.AdminUsername;
                else
                    Nickname = Subscriber.BotNickName;
            }

            this.connectionChange = new AutoResetEvent(false);
            AsyncTcpDispatcher = new AsyncTcpDispatcher(Subscriber.ServerIp, (ushort)Subscriber.ServerPort);
            QueryRunner = new QueryRunner(AsyncTcpDispatcher);

            //atd.ServerClosedConnection += atd_ServerClosedConnection;
            AsyncTcpDispatcher.ReadyForSendingCommands += atd_ReadyForSendingCommands;
            AsyncTcpDispatcher.SocketError += atd_SocketError;

            Connect();

            SimpleResponse loginResult = Login();

            if (loginResult.IsBanned)
                logger.Info("Login failure due to ban: {0}", loginResult.ResponseText);
            else if (loginResult.IsErroneous)
                logger.Info("Login failure due to error: {0}", loginResult.ResponseText);
            else
            {
                SendSetNameCmd();

                SelectServer(Subscriber);

                SendSetNameCmd();

                UpdateServerUniqueId();

                whoAmI = QueryRunner.SendWhoAmI();

                RegisterEvents();
            }
        }

        public void InitializeQuiet()
        {
            try
            {
                UserInitialize(null);
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Could not initialize connection");
            }
        }

        public void TextReply(MessageReceivedEventArgs e, string msg)
        {
            this.QueryRunner.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Client, e.InvokerClientId, msg);
        }

        void UpdateServerUniqueId()
        {
            string uniqueId = QueryRunner.GetServerInfo().UniqueId;

            using (var db = new CoreContext())
            {
                Subscribers sub = db.Subscribers.Single(s => s.SubscriberId == Subscriber.SubscriberId);

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
            string modnickname = Nickname;

            do
            {
                Command cmd = new Command("clientupdate");
                cmd.AddParameter("client_nickname", modnickname);

                result = QueryRunner.SendCommand(cmd);
                attempts += 1;
                modnickname = Nickname + attempts;

                if (attempts > 100)
                    throw new Exception(result);

            }
            while (result.StartsWith("error id=513")); //Name taken
        }

        public void SetName(string nickname)
        {
            Nickname = nickname;

            try
            {
                if (AsyncTcpDispatcher.IsConnected)
                    SendSetNameCmd();
            }
            catch (Exception)
            {
                //Suppress
            }
        }

        void SelectServer(Subscribers subscriber)
        {
            SimpleResponse selectserver = QueryRunner.SelectVirtualServerById((uint)subscriber.VirtualServerId);
            logger.Debug("Virtual server selection result: {0}", selectserver.StatusText);

            if (selectserver.IsErroneous)
                throw new Exception(selectserver.ErrorMessage);
        }

        public SimpleResponse Login()
        {
            SimpleResponse login = QueryRunner.Login(Subscriber.AdminUsername, Subscriber.AdminPassword);
            logger.Debug("Login result: {0}", login.StatusText);
            return login; //Does not throw any exceptions
        }

        public SimpleResponse Logout()
        {
            SimpleResponse logout = QueryRunner.Logout();
            logger.Debug("Logout result: {0}", logout.StatusText);
            return logout; //Does not throw any exceptions
        }

        public void Disconnect()
        {
            try
            {
                AsyncTcpDispatcher.Disconnect();
                logger.Debug("Disconnected");
            }
            catch (Exception ex)
            {
                logger.Debug(ex, "Could not disconnect");
            }
        }

        public void Connect()
        {
            //Console.WriteLine(
            //"Background Thread: SynchronizationContext.Current is " +
            //(SynchronizationContext.Current != null ?
            //SynchronizationContext.Current.ToString() : "null"));

            AsyncTcpDispatcher.Connect();
            connectionChange.WaitOne();

            //TODO: Add logic here that checks if socket is null. That can mean many things..
            //      But apparently one of them is that you have been banned (for not being whitelisted mainly)
            if (AsyncTcpDispatcher.Socket == null)
            {
                throw new Exception("Socket did not initialize: Are you whitelisted?");
            }
            if (AsyncTcpDispatcher.Socket.Connected)
            {
                logger.Debug("Connected");
            }
            else
            {
                throw new Exception("Could not connect to teamspeak server");
            }
        }

        public void RegisterEvents()
        {
            QueryRunner.UnknownNotificationReceived -= Notifications_UnknownNotificationReceived;

            QueryRunner.Notifications.ClientMoved += Notifications_ClientMoved;
            QueryRunner.Notifications.ClientJoined += Notifications_ClientJoined;
            QueryRunner.Notifications.ClientMoveForced += Notifications_ClientMoveForced;
            QueryRunner.Notifications.ClientDisconnect += Notifications_ClientDisconnect;
            QueryRunner.Notifications.ClientConnectionLost += Notifications_ClientConnectionLost;
            QueryRunner.Notifications.ServerMessageReceived += Notifications_ServerMessageReceived;
            QueryRunner.Notifications.ChannelMessageReceived += Notifications_ChannelMessageReceived;
            QueryRunner.Notifications.ClientMessageReceived += Notifications_ClientMessageReceived;

            QueryRunner.Notifications.ClientBan += Notifications_ClientBan;
            QueryRunner.Notifications.ClientMovedByTemporaryChannelCreate += Notifications_ClientMovedByTemporaryChannelCreate;
            QueryRunner.Notifications.ClientKick += Notifications_ClientKick;

            QueryRunner.Notifications.ChannelCreated += Notifications_ChannelCreated;
            QueryRunner.Notifications.ChannelEdited += Notifications_ChannelEdited;
            QueryRunner.Notifications.ChannelMoved += Notifications_ChannelMoved;


            if (AsyncTcpDispatcher != null)
            {
                if (AsyncTcpDispatcher.Socket != null)
                {
                    if (AsyncTcpDispatcher.Socket.Connected == true)
                    {
                        SimpleResponse regNot = QueryRunner.RegisterForNotifications(ServerNotifyRegisterEvent.Server);
                        regNot = QueryRunner.RegisterForNotifications(ServerNotifyRegisterEvent.Channel, 0);
                        regNot = QueryRunner.RegisterForNotifications(ServerNotifyRegisterEvent.TextPrivate);
                    }
                }
            }
        }

        public void UnregisterEvents()
        {
            QueryRunner.UnknownNotificationReceived -= Notifications_UnknownNotificationReceived;

            QueryRunner.Notifications.ClientMoved -= Notifications_ClientMoved;
            QueryRunner.Notifications.ClientJoined -= Notifications_ClientJoined;
            QueryRunner.Notifications.ClientMoveForced -= Notifications_ClientMoveForced;
            QueryRunner.Notifications.ClientDisconnect -= Notifications_ClientDisconnect;
            QueryRunner.Notifications.ClientConnectionLost -= Notifications_ClientConnectionLost;
            QueryRunner.Notifications.ServerMessageReceived -= Notifications_ServerMessageReceived;
            QueryRunner.Notifications.ChannelMessageReceived -= Notifications_ChannelMessageReceived;
            QueryRunner.Notifications.ClientMessageReceived -= Notifications_ClientMessageReceived;

            QueryRunner.Notifications.ClientBan -= Notifications_ClientBan;
            QueryRunner.Notifications.ClientMovedByTemporaryChannelCreate -= Notifications_ClientMovedByTemporaryChannelCreate;
            QueryRunner.Notifications.ClientKick -= Notifications_ClientKick;

            QueryRunner.Notifications.ChannelCreated -= Notifications_ChannelCreated;
            QueryRunner.Notifications.ChannelEdited -= Notifications_ChannelEdited;
            QueryRunner.Notifications.ChannelMoved -= Notifications_ChannelMoved;


            if (AsyncTcpDispatcher != null)
            {
                if (AsyncTcpDispatcher.Socket != null)
                {
                    if (AsyncTcpDispatcher.Socket.Connected == true)
                    {
                        SimpleResponse regNot = QueryRunner.UnregisterNotifications();
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

                    // Invoke the (now deprecated) BotCommandAttempt method
                    var cmdPGL = CommandParameterGroupList.Parse(cmd);
                    foreach (CommandParameterGroup cmdPG in cmdPGL)
                        BotCommandAttempt.Invoke(this, cmdPG, e);
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

        public CommandRuleSets DefineCommandSchemas()
        {
            return null;
        }

        #endregion

        //public string EscapeChars(string value)
        //{
        //    string newval = value;
        //    newval.Replace(@"\", @"\\");
        //    newval.Replace(@"/", @"\/");
        //    newval.Replace(@" ", @"\s");
        //    newval.Replace(@"|", @"\p");
        //    newval.Replace(Convert.ToString((char)7), @"\a");
        //    newval.Replace(Convert.ToString((char)8), @"\b");
        //    newval.Replace(Convert.ToString((char)12), @"\f");
        //    newval.Replace(Convert.ToString((char)10), @"\n");
        //    newval.Replace(Convert.ToString((char)13), @"\r");
        //    newval.Replace(Convert.ToString((char)9), @"\t");
        //    newval.Replace(Convert.ToString((char)11), @"\v");

        //    return newval;
        //}

        //public string UnEscapeChars(string value)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
