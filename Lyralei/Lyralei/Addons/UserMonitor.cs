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
using System.Threading;
using System.Threading.Tasks;


namespace SquallBot
{
    public class UserMonitor : Addon
    {
        public UserMonitor(MySQLInstance _sql, ServerQueryInstance _serverq, ServerInfo _serverinfo) : base(_sql, _serverq, _serverinfo)
        {
            registerEvents();

            MessageServerGroup(9);
        }

        private void registerEvents()
        {
            try
            {

                //queryRunner.UnknownNotificationReceived += queryRunner_UnknownNotificationReceived;
                serverq.Sync_ClientMoveForced += onClientMoveForced; //double events due to channel register bug
                serverq.Sync_ClientKick += onClientKick;
                serverq.Sync_ClientJoined += onClientJoined;
                serverq.Sync_ClientConnectionLost += onClientConnectionLost;
                serverq.Sync_ClientBan += onClientBan;
                serverq.Sync_ClientDisconnect += onClientDisconnect;
                serverq.Sync_ServerMessageReceived += onServerMessage;
                serverq.Sync_ChannelMessageReceived += onChannelMessage;
                serverq.Sync_ClientMessageReceived += onClientMessage;
                serverq.Sync_ClientMoved += onClientMoved;
                serverq.Sync_ChannelCreated += onChannelCreated;
                //Channel created = Server event
                //Channel deleted = Server event

                //if (atd != null)
                //{
                //    if (atd.Socket != null)
                //    {
                //        if (atd.Socket.Connected == true)
                //        {
                //            SimpleResponse regNot = queryRunner.RegisterForNotifications(ServerNotifyRegisterEvent.Server);
                //            regNot = queryRunner.RegisterForNotifications(ServerNotifyRegisterEvent.Channel, 0);
                //            regNot = queryRunner.RegisterForNotifications(ServerNotifyRegisterEvent.TextPrivate);
                //        }
                //    }
                //}
            }
            catch (Exception)
            {

            }
        }
        public void unregisterEvents()
        {
            try
            {
                serverq.Sync_ClientMoveForced -= onClientMoveForced; //double events due to channel register bug
                serverq.Sync_ClientKick -= onClientKick;
                serverq.Sync_ClientJoined -= onClientJoined;
                serverq.Sync_ClientConnectionLost -= onClientConnectionLost;
                serverq.Sync_ClientBan += onClientBan;
                serverq.Sync_ClientDisconnect -= onClientDisconnect;
                serverq.Sync_ServerMessageReceived -= onServerMessage;
                serverq.Sync_ChannelMessageReceived -= onChannelMessage;
                serverq.Sync_ClientMessageReceived -= onClientMessage;
                serverq.Sync_ClientMoved -= onClientMoved;
                serverq.Sync_ChannelCreated -= onChannelCreated;

                //if (atd != null)
                //{
                //    if (atd.Socket != null)
                //    {
                //        if (atd.Socket.Connected == true)
                //        {
                //            queryRunner.UnregisterNotifications();
                //        }
                //    }
                //}
            }
            catch (Exception)
            {

            }
        }



        //private void queryRunner_UnknownNotificationReceived(object sender, TS3QueryLib.Core.Common.EventArgs<string> e)
        //{
        //    //MainArea.ShowNotification(e.Value);
        //    string[] query = e.Value.ToString().Replace("\\/", "/").TrimEnd(new char[] { '\r', '\n' }).Split(' ');

        //    //notifychannelcreated cid=3 cpid=0 channel_name=awdawd channel_codec_quality=6 channel_order=1 channel_codec_is_unencrypted=1 channel_flag_maxfamilyclients_unlimited=0 channel_flag_maxfamilyclients_inherited=1 invokerid=2 invokername=qvazzler invokeruid=sR\/12P6fJAJngNXQqK+JMA9CFuw=
        //    if (query[0] == "notifychannelcreated")
        //    {
        //        //For this event we want to make sure that a temporary channel is created in the proper area of the server.
        //        //To find out the proper area, there should be at least 1 channel that has a topic saying "temporary_area"
        //        Dictionary<string, string> values = new Dictionary<string, string>();
        //        for (int i = 1; i < query.Count(); i++)
        //        {
        //            values.Add(query[i].Substring(0, query[i].IndexOf('=')), @query[i].Substring(query[i].IndexOf('=') + 1));
        //        }

        //        ListResponse<uint> test2 = queryRunner.GetClientDatabaseIdsByUniqueId("sR/12P6fJAJngNXQqK+JMA9CFuw=");

        //        uint test = queryRunner.GetClientDatabaseIdsByUniqueId(@values["invokeruid"]).Values[0];
        //        ListResponse<ServerGroupLight> servergroups = queryRunner.GetServerGroupsByClientId(test);

        //        bool allowed = false;
        //        List<string> admin_servergroups = new List<string>();
        //        admin_servergroups.AddRange(sql.getProperty(serverinfo.id, "admins_servergroup_ids").Split(','));

        //        foreach (ServerGroupLight servergroup in servergroups)
        //        {
        //            foreach (string admin_servergroup in admin_servergroups)
        //            {
        //                if (servergroup.Id == (uint)Convert.ToInt32(admin_servergroup))
        //                {
        //                    allowed = true;
        //                }
        //            }
        //        }

        //        if (allowed)
        //        {
        //            //User was allowed to do this, however now we want to re-scan the room list.
        //            ScanRooms();
        //        }
        //        else
        //        {
        //            ListResponse<ChannelListEntry> channels = queryRunner.GetChannelList(true, false, false, false, false);
        //            //SimpleResponse response = queryRunner.DeleteChannel((uint)Convert.ToInt32(values["cid"]));

        //            foreach (ChannelListEntry channel in channels)
        //            {
        //                if (channel.Topic == "temporary_area")
        //                {
        //                    SimpleResponse response = queryRunner.MoveChannel((uint)Convert.ToInt32(values["cid"]), channel.ChannelId);

        //                    ChannelModification mod = new ChannelModification();
        //                    mod.IsTemporary = true;
        //                    mod.IsPermanent = false;
        //                    mod.IsSemiPermanent = false;

        //                    queryRunner.EditChannel((uint)Convert.ToInt32(values["cid"]), mod);
        //                    break;
        //                }
        //            }
        //        }

        //        //ListResponse<Permission> list = queryRunner.GetClientPermissionList((uint)Convert.ToInt32(values["invokerid"]));

        //        //foreach (Permission perm in list)
        //        //{
        //        //    if (perm.Name == "b_channel_modify_parent")
        //        //    {

        //        //    }
        //        //}
        //    }
        //}

        //private void SocketDispatcher_BanDetected(object sender, TS3QueryLib.Core.Common.EventArgs<SimpleResponse> e)
        //{
        //    //RunOnUIThread(() => MessageBox.Show("You were banned from the server: " + e.Value.BanExtraMessage));
        //}

        #region Actions

        private void MessageServerGroup(int servergroup)
        {
            ListResponse<ServerGroupClient> clients = queryRunner.GetServerGroupClientList((uint)servergroup, true);

            foreach (ServerGroupClient client in clients)
            {
                ListResponse<ClientIdEntry> client_ids = queryRunner.GetClientIdsByUniqueId(client.UniqueId);

                foreach (ClientIdEntry client_id in client_ids)
                {
                    queryRunner.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Client, client_id.ClientId, "Hello Admin!");
                }
            }
        }

        private void FlushUser(uint clientId)
        {
            string sql_text = "UPDATE users SET ClientId=NULL,ChannelId=NULL,ClientCountry=NULL,ClientIP=NULL,ConnectedTime=NULL,IdleTime=NULL,Platform=NULL,ServerGroups=NULL,Version=NULL WHERE ClientId=" + clientId + ";";

            sql.NonQuery(sql_text);
        }

        private void UpdateUserByClientId(uint clientId)
        {
            //ClientDbEntryListResponse list = queryRunner.GetClientDatabaseList();

            ClientInfoResponse clientinfo = queryRunner.GetClientInfo(clientId);
            ClientDbInfoResponse entry = queryRunner.GetClientDatabaseInfo((int)clientinfo.DatabaseId);

            string sql_text = "REPLACE INTO users VALUES ";

            //ListResponse<ClientIdEntry> clientid = queryRunner.GetClientIdsByUniqueId(entry.UniqueId);

            sql_text += "(";
            sql_text += serverinfo.id + ",";
            sql_text += "'" + entry.Created.ToString("yyyy-MM-dd HH:mm:ss") + "',";
            sql_text += entry.DatabaseId + ",";
            sql_text += "'" + entry.Description + "',";
            sql_text += "'" + entry.LastConnected.ToString("yyyy-MM-dd HH:mm:ss") + "',";
            sql_text += "'" + entry.Nickname + "',";
            sql_text += entry.TotalConnections + ",";
            sql_text += "'" + entry.UniqueId + "',";

            if (clientinfo.Nickname != null)
            {
                sql_text += clientId + ",";
                sql_text += clientinfo.ChannelId + ",";
                sql_text += "'" + clientinfo.ClientCountry + "',";
                sql_text += "'" + clientinfo.ClientIP + "',";
                sql_text += "'" + clientinfo.ConnectedTime.TotalSeconds + "',";
                sql_text += "'" + clientinfo.IdleTime.TotalSeconds + "',";
                sql_text += "'" + clientinfo.Platform + "',";

                sql_text += "'";
                foreach (uint id in clientinfo.ServerGroups)
                {
                    sql_text += id + ";";
                }
                sql_text = sql_text.TrimEnd(';');
                sql_text += "',";

                sql_text += "'" + clientinfo.Version + "'";

            }
            else
            {
                sql_text += "NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL";
            }

            sql_text += ")";

            sql.NonQuery(sql_text);
        }

        private void UpdateAllUsers()
        {
            ClientDbEntryListResponse list = queryRunner.GetClientDatabaseList();
            string sql_text = "REPLACE INTO users VALUES ";

            foreach (ClientDbEntry entry in list)
            {
                ListResponse<ClientIdEntry> clientid = queryRunner.GetClientIdsByUniqueId(entry.UniqueId);
                ClientDbInfoResponse clientdbinfo = queryRunner.GetClientDatabaseInfo((int)entry.DatabaseId);
                ClientInfoResponse clientinfo = null;


                sql_text += "(";
                sql_text += serverinfo.id + ",";
                sql_text += "'" + clientdbinfo.Created.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                sql_text += entry.DatabaseId + ",";
                sql_text += "'" + entry.Description + "',";
                sql_text += "'" + entry.LastConnected.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                sql_text += "'" + entry.NickName + "',";
                sql_text += entry.TotalConnections + ",";
                sql_text += "'" + entry.UniqueId + "',";

                if (clientid.Values.Count > 0)
                {
                    clientinfo = queryRunner.GetClientInfo(clientid.Values[0].ClientId);

                    sql_text += clientid.Values[0].ClientId + ",";
                    sql_text += clientinfo.ChannelId + ",";
                    sql_text += "'" + clientinfo.ClientCountry + "',";
                    sql_text += "'" + clientinfo.ClientIP + "',";
                    sql_text += "'" + clientinfo.ConnectedTime.TotalSeconds + "',";
                    sql_text += "'" + clientinfo.IdleTime.TotalSeconds + "',";
                    sql_text += "'" + clientinfo.Platform + "',";

                    sql_text += "'";
                    foreach (uint id in clientinfo.ServerGroups)
                    {
                        sql_text += id + ";";
                    }
                    sql_text = sql_text.TrimEnd(';');
                    sql_text += "',";

                    sql_text += "'" + clientinfo.Version + "'";
                }
                else
                {
                    sql_text += "NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL";
                }

                sql_text += "),";
            }

            sql_text = sql_text.TrimEnd(',');

            sql.NonQuery(sql_text);
        }

        #endregion

        #region Events

        //private void atd_ServerClosedConnection(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void onChannelCreated(object sender, ChannelCreatedEventArgs e)
        {

        }

        private void onChannelMessage(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {

        }

        private void onServerMessage(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {

        }

        private void onClientMessage(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {
            //throw new NotImplementedException();

            if (e.Message == "!getusers")
            {
                ClientDbEntryListResponse list = queryRunner.GetClientDatabaseList();
                ListResponse<ClientListEntry> userlist = queryRunner.GetClientList(true);
                ClientInfoResponse user = queryRunner.GetClientInfo(2);

                foreach (ClientDbEntry item in list)
                {

                }
                foreach (ClientListEntry item in userlist)
                {

                }
            }

            //should not double-fire

        }

        private void onClientKick(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientKickEventArgs e)
        {
            //TidyChannels((int)e.SourceChannelId, (int)e.VictimClientId);
            FlushUser(e.VictimClientId);
        }

        private void onClientBan(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientBanEventArgs e)
        {
            //TidyChannels((int)e.SourceChannelId, (int)e.VictimClientId);
            FlushUser(e.VictimClientId);
        }

        private void onClientConnectionLost(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientConnectionLostEventArgs e)
        {
            //TidyChannels((int)e.SourceChannelId, (int)e.ClientId);
            FlushUser(e.ClientId);
        }

        private void onClientDisconnect(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientDisconnectEventArgs e)
        {
            //TidyChannels((int)e.SourceChannelId, (int)e.ClientId);
            FlushUser(e.ClientId);
        }

        private void onClientJoined(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientJoinedEventArgs e)
        {
            //TidyChannels((int)e.ChannelId, (int)e.ClientId);
            UpdateUserByClientId(e.ClientId);
        }

        private void onClientMoveForced(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedByClientEventArgs e)
        {
            //TidyChannels((int)e.TargetChannelId, (int)e.ClientId);
            UpdateUserByClientId(e.ClientId);
        }

        private void onClientMoved(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedEventArgs e)
        {
            //TidyChannels((int)e.TargetChannelId, (int)e.ClientId);
            UpdateUserByClientId(e.ClientId);
        }

        //private void SocketAsyncEventArgs_Completed(object sender, System.Net.Sockets.SocketAsyncEventArgs e)
        //{
        //    Console.WriteLine(e.SocketError);
        //    //throw new NotImplementedException();
        //}

        //private void atd_SocketError(object sender, TS3QueryLib.Core.Communication.SocketErrorEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void atd_NotificationReceived(object sender, EventArgs<string> e)
        //{
        //    //throw new NotImplementedException();
        //}

        #endregion
    }
}
