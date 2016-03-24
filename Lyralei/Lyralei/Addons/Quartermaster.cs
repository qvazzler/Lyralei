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
    public class Quartermaster : Addon
    {
        SemaphoreSlim slimtest;

        public Quartermaster(MySQLInstance _sql, ServerQueryInstance _serverq, ServerInfo _serverinfo) : base(_sql, _serverq, _serverinfo)
        {
            registerEvents();
            slimtest = new SemaphoreSlim(1);

            SortChannels(12);
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

        private uint SortChannels_FindOrder(List<ChannelListEntry> _channels, ChannelModification new_channel)
        {
            List<ChannelListEntry> relevant_channels = _channels;
            List<ChannelModification> channel_to_sort = new List<ChannelModification>();
            channel_to_sort.Add(new_channel);
            int index = 0;

            for (int j = 0; j < relevant_channels.Count; j++)
            {
                for (int i_char = 0; i_char < new_channel.Name.Length; i_char++)
                {
                    string i_digits = "";
                    string j_digits = "";

                    for (int idig = i_char; idig < new_channel.Name.Length; idig++)
                    {
                        if (Char.IsNumber(new_channel.Name[idig]) == true)
                            i_digits += new_channel.Name[idig].ToString();
                        else
                        {
                            idig = new_channel.Name.Length;
                            i_digits = "";
                        }
                    }

                    for (int idig = i_char; idig < relevant_channels[j].Name.Length; idig++)
                    {
                        if (Char.IsNumber(relevant_channels[j].Name[idig]) == true)
                            j_digits += relevant_channels[j].Name[idig].ToString();
                        else
                        {
                            idig = relevant_channels[j].Name.Length;
                            j_digits = "";
                        }
                    }

                    if (i_digits.Length > 1 || j_digits.Length > 1)
                    {
                        if (Convert.ToInt32(i_digits) > Convert.ToInt32(j_digits))
                        {
                            index++;
                            i_char = new_channel.Name.Length;
                        }
                        else
                        {
                            i_char = new_channel.Name.Length;
                        }
                    }
                    else
                    {
                        if (i_char < relevant_channels[j].Name.ToLower().Length)
                        {
                            if ((int)new_channel.Name.ToLower()[i_char] > (int)relevant_channels[j].Name.ToLower()[i_char])
                            {
                                index++;
                                i_char = new_channel.Name.Length;
                            }
                            else if ((int)new_channel.Name.ToLower()[i_char] < (int)relevant_channels[j].Name.ToLower()[i_char])
                            {
                                i_char = new_channel.Name.Length;
                            }
                        }
                    }
                }
            }

            if (index > 0)
                return relevant_channels[index - 1].ChannelId;
            else
                return 0;
        }

        /// <summary>
        /// Sorts a list of channels just like windows sorts its folders (digit-sorting supported)
        /// </summary>
        /// <param name="_channels">List of (unsorted) channels</param>
        /// <returns>A sorted array of channels</returns>
        private ChannelListEntry[] SortChannels_GetArray(List<ChannelListEntry> _channels)
        {
            List<ChannelListEntry> relevant_channels = _channels;
            ChannelListEntry[] sorted_relevant_channels;

            sorted_relevant_channels = new ChannelListEntry[relevant_channels.Count()];

            //var lengths = (from element in _channels
            //              orderby element.Name
            //              select element).ToList<ChannelListEntry>();

            for (int i = 0; i < relevant_channels.Count; i++)
            {
                int index = 0;

                for (int j = 0; j < relevant_channels.Count; j++)
                {
                    for (int i_char = 0; i_char < relevant_channels[i].Name.Length; i_char++)
                    {
                        string i_digits = "";
                        string j_digits = "";

                        for (int idig = i_char; idig < relevant_channels[i].Name.Length; idig++)
                        {
                            if (Char.IsNumber(relevant_channels[i].Name[idig]) == true)
                                i_digits += relevant_channels[i].Name[idig].ToString();
                            else
                            {
                                idig = relevant_channels[i].Name.Length;
                                i_digits = "";
                            }
                        }

                        for (int idig = i_char; idig < relevant_channels[j].Name.Length; idig++)
                        {
                            if (Char.IsNumber(relevant_channels[j].Name[idig]) == true)
                                j_digits += relevant_channels[j].Name[idig].ToString();
                            else
                            {
                                idig = relevant_channels[j].Name.Length;
                                j_digits = "";
                            }
                        }

                        if (i_digits.Length > 1 || j_digits.Length > 1)
                        {
                            if (Convert.ToInt32(i_digits) > Convert.ToInt32(j_digits))
                            {
                                index++;
                                i_char = relevant_channels[i].Name.Length;
                            }
                            else
                            {
                                i_char = relevant_channels[i].Name.Length;
                            }
                        }
                        else
                        {
                            if (i_char < relevant_channels[j].Name.ToLower().Length)
                            {
                                if ((int)relevant_channels[i].Name.ToLower()[i_char] > (int)relevant_channels[j].Name.ToLower()[i_char])
                                {
                                    index++;
                                    i_char = relevant_channels[i].Name.Length;
                                }
                                else if ((int)relevant_channels[i].Name.ToLower()[i_char] < (int)relevant_channels[j].Name.ToLower()[i_char])
                                {
                                    i_char = relevant_channels[i].Name.Length;
                                }
                            }
                        }
                    }
                }

                sorted_relevant_channels[index] = relevant_channels[i];
            }

            return sorted_relevant_channels;
        }

        /// <summary>
        /// Gets the next available room name for numeric and alphabetic auto channel areas
        /// </summary>
        /// <param name="parent_channel_id">The channel to get list of sub-channels from</param>
        /// <param name="type">Specify whether the sub-channels are named alphabetically or numerically</param>
        /// <returns>ChannelModification, containing information about the next available channel</returns>
        private ChannelModification LowestAvailableRoomName(uint parent_channel_id, AutoChannelNameType type)
        {
            List<ChannelListEntry> channels = new List<ChannelListEntry>();
            ListResponse<ChannelListEntry> all_channels = queryRunner.GetChannelList();
            ChannelModification new_channel = new ChannelModification();
            new_channel.IsPermanent = true;
            new_channel.Description = "Automated channel!";
            new_channel.Codec = (TS3QueryLib.Core.Server.Entities.Codec)Codec.OpusVoice;
            new_channel.CodecQuality = 10;
            new_channel.Topic = "";
            new_channel.ArUnlimitedMaxFamilyClientsInherited = false;
            //new_channel.HasUnlimitedMaxClients = true;
            //new_channel.HasUnlimitedMaxFamilyClients = true;
            new_channel.IsDefault = false;
            new_channel.IsTemporary = false;
            new_channel.IsSemiPermanent = false;
            //new_channel.MaxClients = 0;
            //new_channel.MaxFamilyClients = 0;
            //new_channel.NamePhonetic = "";
            new_channel.NeededTalkPower = 0;
            //new_channel.Password = "";

            foreach (ChannelListEntry channel in all_channels)
            {
                if (channel.ParentChannelId == parent_channel_id)
                    channels.Add(channel);
            }

            ChannelListEntry[] sorted_channels = SortChannels_GetArray(channels);

            //Autochannel name type is alphabetic
            if (type == AutoChannelNameType.Alphabetic)
            {
                //Go through each of the current sorted channels
                for (int i = 0; i < sorted_channels.Length + 1; i++)
                {
                    //Get name of what channel should appear next in the sort channel list
                    string name = Entities.Alphabet[i % Entities.Alphabet.Length];

                    //If channel amount exceeds alphabetic letters, add delimiter "Zulu " the amount of times needed to make the name unique
                    for (int i_alphabet = i - Entities.Alphabet.Length; i_alphabet >= 0; i_alphabet = i_alphabet - Entities.Alphabet.Length)
                    {
                        name = Entities.Alphabet_Delimiter + " " + name;
                    }

                    //As long as its not the last loop, carry on with below as normal
                    if (i < sorted_channels.Length)
                    {
                        //If next sorted channel name does not match the expected name, return that channel to be the next available one
                        if (sorted_channels[i].Name != name)
                        {
                            bool exists = false;

                            for (int k = i; k < sorted_channels.Length; k++)
                            {
                                if (sorted_channels[k].Name == name)
                                {

                                    exists = true;

                                    //if (k > i)
                                    //    new_channel.ChannelOrder = null;
                                    //else if (k == i && i != sorted_channels.Length)
                                    //    new_channel.ChannelOrder = sorted_channels[i - 1].ChannelId;
                                    //else
                                    //    new_channel.ChannelOrder = null;
                                }
                            }

                            if (!exists)
                            {
                                new_channel.Name = name; //sorted_channels[i - 1].ChannelId;
                                new_channel.ParentChannelId = parent_channel_id;
                                new_channel.ChannelOrder = null;

                                return new_channel;
                            }
                        }
                    }
                    //Last loop, last procedure! If all channels were already properly sorted, we add one to the end instead
                    else if (i == sorted_channels.Length)
                    {
                        //If all channels were already properly in order, we need to add a new channel at the end instead.
                        new_channel.Name = name;
                    }
                }

                new_channel.ChannelOrder = sorted_channels[sorted_channels.Length - 1].ChannelId;
                new_channel.ParentChannelId = parent_channel_id;
                //new_channel.Name = "RAWR";
                return new_channel;
            }
            else
            {

                for (int i = 0; i < sorted_channels.Length; i++)
                {
                    if (sorted_channels[i].Name.Substring(5) != (i + 1).ToString())
                    {
                        new_channel.Name = "Room " + (i + 1).ToString();

                        if (i != 0)
                            new_channel.ChannelOrder = sorted_channels[i - 1].ChannelId;
                        else
                            new_channel.ChannelOrder = 0;

                        new_channel.ParentChannelId = parent_channel_id;
                        //new_channel.Name = "RAWR";
                        return new_channel;
                    }
                }

                new_channel.Name = "Room " + (sorted_channels.Length + 1).ToString();
                new_channel.ChannelOrder = sorted_channels[sorted_channels.Length - 1].ChannelId;
                new_channel.ParentChannelId = parent_channel_id;
                //new_channel.Name = "RAWR";
                return new_channel;
            }
        }

        /// <summary>
        /// Sorts and reorders a list of sub-channels provided on the teamspeak server.
        /// </summary>
        /// <param name="_channels">List of channels to sort and perform changes on</param>
        private void SortChannels(List<ChannelListEntry> _channels)
        {
            ChannelListEntry[] sorted_relevant_channels = SortChannels_GetArray(_channels);

            for (int i = 0; i < sorted_relevant_channels.Count(); i++)
            {
                ChannelModification mod = new ChannelModification();
                if (i == 0)
                {
                    if (sorted_relevant_channels[i].Order != 0)
                    {
                        mod.ChannelOrder = 0;
                        queryRunner.EditChannel(sorted_relevant_channels[i].ChannelId, mod);
                    }
                }
                else
                {
                    if (sorted_relevant_channels[i].Order != sorted_relevant_channels[i - 1].ChannelId)
                    {
                        mod.ChannelOrder = sorted_relevant_channels[i - 1].ChannelId;
                        queryRunner.EditChannel(sorted_relevant_channels[i].ChannelId, mod);
                    }
                }
            }
        }

        /// <summary>
        /// Sorts and reorders a list of sub-channels based on the parent id channel provided.
        /// </summary>
        /// <param name="parent_channel_id">Channel which sub-channels to be sorted and reordered</param>
        private void SortChannels(int parent_channel_id)
        {
            ListResponse<ChannelListEntry> server_channels = queryRunner.GetChannelList();
            List<ChannelListEntry> subchannels = new List<ChannelListEntry>();

            foreach (ChannelListEntry server_channel in server_channels)
            {
                if (server_channel.ParentChannelId == parent_channel_id)
                    subchannels.Add(server_channel);
            }

            ChannelListEntry[] sorted_relevant_channels = SortChannels_GetArray(subchannels);

            for (int i = 0; i < sorted_relevant_channels.Count(); i++)
            {
                ChannelModification mod = new ChannelModification();
                if (i == 0)
                {
                    if (sorted_relevant_channels[i].Order != 0)
                    {
                        mod.ChannelOrder = 0;
                        queryRunner.EditChannel(sorted_relevant_channels[i].ChannelId, mod);
                    }
                }
                else
                {
                    if (sorted_relevant_channels[i].Order != sorted_relevant_channels[i - 1].ChannelId)
                    {
                        mod.ChannelOrder = sorted_relevant_channels[i - 1].ChannelId;
                        queryRunner.EditChannel(sorted_relevant_channels[i].ChannelId, mod);
                    }
                }
            }
        }

        private void ScanRooms()
        {
            sql.NonQuery("DELETE FROM rooms WHERE server_id = " + serverinfo.id + ";");

            ListResponse<ChannelListEntry> list = queryRunner.GetChannelList(true);
            string sql_text = "INSERT INTO rooms VALUES ";

            foreach (ChannelListEntry entry in list)
            {
                //Leave this commented out for now as we might need to store temporary rooms also
                //if (entry.IsPermanent == true || entry.IsSemiPermanent == true)
                //{
                sql_text += "(";
                sql_text += serverinfo.id + ",";
                sql_text += entry.ChannelIconId + ",";
                sql_text += entry.ChannelId + ",";
                sql_text += entry.Codec + ",";
                sql_text += entry.CodecQuality + ",";
                sql_text += Convert.ToInt32(entry.IsDefaultChannel) + ",";
                sql_text += Convert.ToInt32(entry.IsPasswordProtected) + ",";
                sql_text += Convert.ToInt32(entry.IsPermanent) + ",";
                sql_text += Convert.ToInt32(entry.IsSemiPermanent) + ",";
                sql_text += Convert.ToInt32(entry.IsSpacer) + ",";
                sql_text += entry.MaxClients + ",";
                sql_text += entry.MaxFamilyClients + ",";
                sql_text += "'" + entry.Name + "',";
                sql_text += entry.NeededTalkPower + ",";
                sql_text += entry.Order + ",";
                sql_text += entry.ParentChannelId + ",";
                //sql_text += entry.SpacerInfo + ",";
                sql_text += "'" + entry.Topic + "',";

                ChannelInfoResponse channel = queryRunner.GetChannelInfo(entry.ChannelId);
                sql_text += "'" + channel.Description + "',";

                sql_text += entry.TotalClients + "),";
                //}
            }

            sql_text = sql_text.TrimEnd(',');

            sql.NonQuery(sql_text);
        }

        private void TidyChannels(int eventChannelId, int eventClientId = -1)
        {
            Console.WriteLine("(Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId + ") "+ DateTime.Now + " - TidyChannels: Entering wait queue");
            slimtest.Wait();
            ChannelInfoResponse _target_channel = queryRunner.GetChannelInfo((uint)eventChannelId);
            ChannelInfoResponse _parent_channel = queryRunner.GetChannelInfo(_target_channel.ParentChannelId);
            bool privateChannel = false;
            slimtest.Release();
            Console.WriteLine("(Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId + ") TidyChannels: Leaving wait queue");

            AutoChannelNameType channelNameType = AutoChannelNameType.Numeric;
            int empty_channel_treshhold = 2;
            //bool private_channel = false;
            int parent_channel_id = 0;

            try
            {
                ChannelSettings settings = new ChannelSettings(_parent_channel.Topic);
                settings.SetActiveSetting("autochannel");

                if (settings.ContainsParam("private"))
                {
                    if (settings.GetParam("private") == "yes")
                        privateChannel = true;
                }

                if (settings.ContainsParam("empty_channels"))
                {
                    try
                    {
                        if (Convert.ToInt32(settings.GetParam("empty_channels")) > 0)
                            empty_channel_treshhold = Convert.ToInt32(settings.GetParam("empty_channels"));
                    }
                    catch (Exception)
                    {
                        //Value wasn't numeric
                    }
                }

                if (settings.ContainsParam("naming_type"))
                {
                    if (settings.GetParam("naming_type") == "alphabetic")
                        channelNameType = AutoChannelNameType.Alphabetic;
                    else if (settings.GetParam("naming_type") == "numeric")
                        channelNameType = AutoChannelNameType.Numeric;
                }

                VerifyHostPresent(eventChannelId, eventClientId, privateChannel);
                parent_channel_id = (int)_target_channel.ParentChannelId;
            }
            catch (Exception)
            {
                //Channel not eligible for tidying
                return;
            }

            //Now we check if the user left any other channel in the auto-section that can now be deleted
            ListResponse<ChannelListEntry> channels = queryRunner.GetChannelList(false, true, false, false, false);
            List<ChannelListEntry> empty_channels = new List<ChannelListEntry>();
            List<ChannelListEntry> relevant_channels = new List<ChannelListEntry>();
            bool channelEmpty = true;

            //Go through all the channels
            foreach (ChannelListEntry channel in channels)
            {
                channelEmpty = true;

                //Only match the ones with the same parentid.
                if (channel.ParentChannelId == parent_channel_id)
                {
                    foreach (ClientListEntry client in queryRunner.GetClientList())
                    {
                        if (client.ChannelId == channel.ChannelId)
                        {
                            //There is already someone in the channel.
                            channelEmpty = false;
                            break;
                        }
                    }

                    //If channel is empty, add to list of empty channels for later iteration
                    if (channelEmpty)
                    {
                        empty_channels.Add(channel);

                        //Also remove the password from the empty channel
                        if (channel.IsPasswordProtected == true)
                        {
                            ChannelModification mod = new ChannelModification();
                            mod.Password = "";
                            queryRunner.EditChannel(channel.ChannelId, mod);
                        }

                        ListResponse<ChannelGroupClient> channelgroupclientlist = queryRunner.GetChannelGroupClientList(channel.ChannelId, null, 10);

                        foreach (ChannelGroupClient cgclient in channelgroupclientlist)
                        {
                            queryRunner.SetClientChannelGroup(8, channel.ChannelId, cgclient.ClientDatabaseId);
                        }
                    }

                    relevant_channels.Add(channel);
                }
            }

            //Check for excessive amount of empty rooms, delete if necessary
            if (empty_channels.Count > empty_channel_treshhold)
            {
                ChannelListEntry[] sorted_empty_channels = SortChannels_GetArray(empty_channels);

                for (int i = sorted_empty_channels.Length - 1; i > empty_channel_treshhold - 1; i--)
                {
                    SimpleResponse resp = queryRunner.DeleteChannel(sorted_empty_channels[i].ChannelId);
                }
            }
            else if (empty_channels.Count < empty_channel_treshhold)
            {
                int channels_created = 0;
                //HostInfoResponse hostinfo = queryRunner.GetHostInfo();
                //ClientInfoResponse clientinfo = queryRunner.GetClientInfo(1);

                do
                {
                    if (channels_created > 1)
                    {
                        //If more than 2 channels have been created already, we add some delay to make sure not to trigger the flood protection too easily
                        Thread.Sleep(300);
                    }
                    //There are no empty channels left so we need to create a new one.

                    //First we get the lowest available index for a room name.
                    ChannelModification newroom = LowestAvailableRoomName((uint)parent_channel_id, channelNameType);
                    if (newroom.ChannelOrder == null)
                    {
                        newroom.ChannelOrder = SortChannels_FindOrder(relevant_channels, newroom);
                    }

                    string test;
                    //Then we create the channel
                    try
                    {
                        channels_created++;
                        test = queryRunner.CreateChannel(newroom).GetDumpString();
                    }
                    catch (FormatException)
                    {
                        //Not sure why we're getting an exception. The code is working the way it should.
                    }
                }
                while (empty_channels.Count + channels_created < empty_channel_treshhold);

                //SortChannels(parent_channel_id);
            }
            else
            {
                //There's still 1 empty channel after user joined the area. We dont have to do anything.
            }
        }

        private void VerifyHostPresent(int channel_id, int invoker_id = -1, bool makeChannelAdmin = false)
        {
            ListResponse<ClientListEntry> clients = queryRunner.GetClientList();
            bool channelEmpty = true;
            int clientdb_id = -1;
            bool host_changed = false;
            Random rand = new Random();
            uint host_channel_group_id = 0; //Will be set by one of the variables below

            //Some preset values. This will be properties in the mysql db later
            uint val_private_host = 10;
            uint val_host_id = 9;
            uint val_guest_id = 8;

            //Determine whether to promote/demote users to Host or to Private Host (used for private channels)
            if (makeChannelAdmin)
                host_channel_group_id = val_private_host;
            else
                host_channel_group_id = val_host_id;

            //First we check if the joined channel is empty
            foreach (ClientListEntry client in clients)
            {
                if (client.ChannelId == channel_id && client.ClientId != invoker_id)
                {
                    //There is already someone in the channel.
                    channelEmpty = false;
                    break;
                }
                else if (client.ClientId == invoker_id)
                {
                    //When we find the client ID of the invoker, save this for later (saves some time rather than doing a query later on)
                    clientdb_id = (int)client.ClientDatabaseId;
                }
            }

            if (channelEmpty)
            {
                bool alreadyHost = false;

                //Make sure any previous users are not Host
                ListResponse<ChannelGroupClient> clients_in_group = queryRunner.GetChannelGroupClientList((uint)channel_id, null, host_channel_group_id);
                foreach (ChannelGroupClient client in clients_in_group)
                {
                    if (clientdb_id != client.ClientDatabaseId)
                    {
                        SimpleResponse response = queryRunner.SetClientChannelGroup(val_guest_id, (uint)channel_id, (uint)client.ClientDatabaseId);
                    }
                    else if (clientdb_id == client.ClientDatabaseId)
                    {
                        alreadyHost = true;
                    }
                }

                if (alreadyHost == false)
                {
                    queryRunner.SetClientChannelGroup(host_channel_group_id, (uint)channel_id, (uint)clientdb_id);
                    host_changed = true;
                }
            }
            else
            {
                //Channel is not empty. Make sure it has a host.
                bool hasHost = false;

                //queryRunner.SetClientChannelGroup(9, (uint)channel_id, (uint)invoker_id);
                ListResponse<ChannelGroupClient> clients_in_group = queryRunner.GetChannelGroupClientList((uint)channel_id, null, host_channel_group_id);

                if (clients_in_group.Values.Count > 0)
                {
                    foreach (ChannelGroupClient cgclient in clients_in_group)
                    {
                        ListResponse<ClientIdEntry> clientIDs;
                        clientIDs = queryRunner.GetClientIdsByUniqueId(queryRunner.GetClientNameAndUniqueIdByClientDatabaseId(cgclient.ClientDatabaseId).ClientUniqueId);

                        foreach (ClientIdEntry clientID in clientIDs)
                        {
                            if(queryRunner.GetClientInfo(clientID.ClientId).ChannelId == channel_id)
                                hasHost = true;
                        }
                    }
                }

                if (hasHost == false)
                {
                    //No host present, so make the visitor the host.

                    //But first we remove anyone else who holds channel group admin to this channel
                    ListResponse<ChannelGroupClient> channelgroupclientlist = queryRunner.GetChannelGroupClientList((uint)channel_id, null, host_channel_group_id);

                    foreach (ChannelGroupClient cgclient in channelgroupclientlist)
                    {
                        queryRunner.SetClientChannelGroup(val_guest_id, (uint)channel_id, cgclient.ClientDatabaseId);
                    }

                    if (makeChannelAdmin)
                    {
                        //User needs to become channel admin as this is a private room (with a password)
                        queryRunner.SetClientChannelGroup(host_channel_group_id, (uint)channel_id, (uint)clientdb_id);
                        host_changed = true;
                    }
                    else
                    {
                        //User should just become a host with limited power
                        queryRunner.SetClientChannelGroup(host_channel_group_id, (uint)channel_id, (uint)clientdb_id);
                        host_changed = true;
                    }


                }
            }

            if (host_changed)
            {
                if (makeChannelAdmin)
                {
                    GenerateChannelPassword(channel_id, invoker_id);
                }
                else
                {
                    //No need to password-protect the channel since it's not in the private area
                }
            }
        }

        private void GenerateChannelPassword(int channel_id, int client_id)
        {
            ChannelModification mod = new ChannelModification();
            Random rand = new Random();
            int Password = rand.Next(0, 9999);

            mod.Password = Convert.ToString(Password).PadLeft(4, '0');

            queryRunner.EditChannel((uint)channel_id, mod);

            queryRunner.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Client, (uint)client_id, "Password for this channel ( id: " + channel_id + " ) is now " + mod.Password);
        }

        #endregion

        #region Events

        private void atd_ServerClosedConnection(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onChannelCreated(object sender, ChannelCreatedEventArgs e)
        {
            ListResponse<uint> test2 = queryRunner.GetClientDatabaseIdsByUniqueId("sR/12P6fJAJngNXQqK+JMA9CFuw=");

            uint test = queryRunner.GetClientDatabaseIdsByUniqueId(e.InvokerUniqueId).Values[0];
            ListResponse<ServerGroupLight> servergroups = queryRunner.GetServerGroupsByClientId(test);

            bool allowed = false;
            List<string> admin_servergroups = new List<string>();
            admin_servergroups.AddRange(sql.getProperty(serverinfo.id, "admins_servergroup_ids").Split(','));

            foreach (ServerGroupLight servergroup in servergroups)
            {
                foreach (string admin_servergroup in admin_servergroups)
                {
                    if (servergroup.Id == (uint)Convert.ToInt32(admin_servergroup))
                    {
                        allowed = true;
                    }
                }
            }

            if (allowed)
            {
                //User was allowed to do this, however now we want to re-scan the room list.
                ScanRooms();
            }
            else
            {
                ListResponse<ChannelListEntry> channels = queryRunner.GetChannelList(true, false, false, false, false);
                //SimpleResponse response = queryRunner.DeleteChannel((uint)Convert.ToInt32(values["cid"]));

                foreach (ChannelListEntry channel in channels)
                {
                    if (channel.Topic == "temporary_area")
                    {
                        SimpleResponse response = queryRunner.MoveChannel((uint)Convert.ToInt32(e.ChannelId), channel.ChannelId);

                        ChannelModification mod = new ChannelModification();
                        mod.IsTemporary = true;
                        mod.IsPermanent = false;
                        mod.IsSemiPermanent = false;

                        queryRunner.EditChannel((uint)Convert.ToInt32(e.ChannelId), mod);
                        break;
                    }
                }
            }
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

            if (e.Message == "!scan")
            {
                ScanRooms();

                //TS3QueryLib.Core.Common.Entities.SpacerInfo test;
                //list.Values[0]
            }
            else if (e.Message == "!getusers")
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
        }

        private void onClientKick(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientKickEventArgs e)
        {
            TidyChannels((int)e.SourceChannelId, (int)e.VictimClientId);
        }

        private void onClientBan(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientBanEventArgs e)
        {
            TidyChannels((int)e.SourceChannelId, (int)e.VictimClientId);
        }

        private void onClientConnectionLost(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientConnectionLostEventArgs e)
        {
            TidyChannels((int)e.SourceChannelId, (int)e.ClientId);
        }

        private void onClientDisconnect(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientDisconnectEventArgs e)
        {
            TidyChannels((int)e.SourceChannelId, (int)e.ClientId);
        }

        private void onClientJoined(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientJoinedEventArgs e)
        {
            TidyChannels((int)e.ChannelId, (int)e.ClientId);
        }

        private void onClientMoveForced(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedByClientEventArgs e)
        {
            TidyChannels((int)e.TargetChannelId, (int)e.ClientId);
        }

        private void onClientMoved(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedEventArgs e)
        {
            TidyChannels((int)e.TargetChannelId, (int)e.ClientId);
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
