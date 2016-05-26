using System.Linq;

using Lyralei.Core.UserManager.Models;
using Lyralei.Core.ServerQueryConnection.Models;
using Lyralei.TS3_Objects.Entities;
using System;
using System.Collections.Generic;
using TS3QueryLib.Core.Common;
using Lyralei.TS3_Objects.EventArguments;
using TS3QueryLib.Core.Server.Entities;
using TS3QueryLib.Core.Server.Responses;
using TS3QueryLib.Core.Common.Responses;

namespace Lyralei.Core.ChannelManager
{
    public class ChannelManager : Base.CoreBase, Base.ICore
    {
        ServerQueryConnection.ServerQueryConnection ServerQueryConnection;

        #region Standards
        public ChannelManager(Subscribers Subscriber) : base(Subscriber)
        {
            CoreDependencies.Add(typeof(ServerQueryConnection.ServerQueryConnection).Name);
        }

        public void UserInitialize(CoreList CoreList)
        {
            ServerQueryConnection = (ServerQueryConnection.ServerQueryConnection)CoreList[typeof(ServerQueryConnection.ServerQueryConnection).Name];

            //TODO: Add permission list from ts3 somehow
            //var PermissionsList = ServerQueryConnection.QueryRunner.GetPermissionList().ToList();
        }

        public CommandRuleSets DefineCommandSchemas()
        {
            CommandRuleSets ruleSets = new CommandRuleSets();
            CommandParameterGroupListWithRules cmdsToSortSubChannels = new CommandParameterGroupListWithRules();
            CommandParameterGroupListWithRules cmdsToStoreChannels = new CommandParameterGroupListWithRules();
            CommandParameterGroupListWithRules cmdsToPopChannels = new CommandParameterGroupListWithRules();

            // Sort channels based on parent channel
            CommandParameterGroupWithRules cmdSortSubChannels = new CommandParameterGroupWithRules();
            cmdSortSubChannels.Add(new CommandParameterWithRules("sort")
            {
                IsBaseCommand = true
            });
            //cmdCool.Add(new CommandParameterWithRules("someparam")
            //{
            //    NameValueSetting = NameValueSetting.ValueOrValueAndName,
            //    ValueType = TS3_Objects.Entities.ValueType.Integer,
            //});
            cmdsToSortSubChannels.Add(cmdSortSubChannels);

            // Store a channel
            CommandParameterGroupWithRules cmdStoreChannel = new CommandParameterGroupWithRules();
            cmdStoreChannel.Add(new CommandParameterWithRules("store")
            {
                IsBaseCommand = true
            });
            cmdsToStoreChannels.Add(cmdStoreChannel);

            // Pop a channel
            CommandParameterGroupWithRules cmdPopChannel = new CommandParameterGroupWithRules();
            cmdPopChannel.Add(new CommandParameterWithRules("pop")
            {
                IsBaseCommand = true
            });
            cmdsToPopChannels.Add(cmdPopChannel);

            ruleSets.Add(new CommandRuleSet(this.Name, cmdsToSortSubChannels, UserSortRequest));
            ruleSets.Add(new CommandRuleSet(this.Name, cmdsToStoreChannels, UserStoreChannelRequest));
            ruleSets.Add(new CommandRuleSet(this.Name, cmdsToPopChannels, UserPopChannelRequest));

            return ruleSets;
        }
        #endregion

        #region Designation CRUD
        public void AssignDesignation(Users User, string DesignationName, int ChannelId)
        {
            using (var db = new CoreContext())
            {
                Models.ChannelDesignations designation = new Models.ChannelDesignations()
                {
                    ChannelId = ChannelId,
                    DesignatedByUserId = User.UserId,
                    DesignationName = DesignationName,
                    SubscriberId = this.Subscriber.SubscriberId,
                    SubscriberUniqueId = this.Subscriber.SubscriberUniqueId
                };

                db.ChannelDesignations.Add(designation);
                db.SaveChanges();
            }
        }

        public void DeleteDesignation(string DesignationName, int ChannelId)
        {
            using (var db = new CoreContext())
            {
                var designation = db.ChannelDesignations.SingleOrDefault(x => x.SubscriberId == this.Subscriber.SubscriberId && x.DesignationName == DesignationName);

                if (designation != null)
                {
                    db.ChannelDesignations.Remove(designation);
                    db.SaveChanges();
                }
            }
        }

        public List<string> GetChannelDesignations(string DesignationName, int ChannelId)
        {
            using (var db = new CoreContext())
            {
                var permSearch = db.ChannelDesignations.Where(x => x.SubscriberId == this.Subscriber.SubscriberId && x.ChannelId == ChannelId).Select(x => x.DesignationName).ToList();
                return permSearch;
            }
        }

        public void ClearDesignations(string DesignationName, int ChannelId)
        {
            using (var db = new CoreContext())
            {
                var designations = db.ChannelDesignations.Where(x => x.SubscriberId == this.Subscriber.SubscriberId && x.ChannelId == ChannelId);

                if (designations != null)
                {
                    foreach (var designation in designations)
                    {
                        db.ChannelDesignations.Remove(designation);
                    }

                    db.SaveChanges();
                }

            }
        }
        #endregion

        #region Command Events

        private void UserSortRequest(BotCommandEventArgs obj)
        {
            //No parameters, just need to call sort and assume the channel the user is in.
            var clientInfo = ServerQueryConnection.QueryRunner.GetClientInfo(obj.MessageInfo.InvokerClientId);
            SortSubChannels((int)clientInfo.ChannelId);
        }

        private void UserStoreChannelRequest(BotCommandEventArgs obj)
        {
            //No parameters, just need to store on the channel the user is in (for now).
            try
            {
                var clientInfo = ServerQueryConnection.QueryRunner.GetClientInfo(obj.MessageInfo.InvokerClientId);
                StoreChannel((int)clientInfo.ChannelId);
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Failed to store channel by user {1} ({2})", obj.MessageInfo.InvokerClientId, obj.MessageInfo.InvokerNickname);
            }
        }

        private void UserPopChannelRequest(BotCommandEventArgs obj)
        {
            //No parameters, for now
            try
            {
                Models.StoredChannels firstStoredChannel;
                List<Models.StoredChannels> allChannelsUnderSameSubscriber;

                using (var db = new CoreContext())
                {
                    allChannelsUnderSameSubscriber = db.StoredChannels.Where(x => x.SubscriberId == this.Subscriber.SubscriberId).ToList();
                    firstStoredChannel = allChannelsUnderSameSubscriber.First(x => x.ChannelId == 52);
                }

                if (firstStoredChannel.ParentChannelId != 0)
                {
                    var parent = ServerQueryConnection.QueryRunner.GetChannelInfo((uint)firstStoredChannel.ParentChannelId);

                    if (parent.IsErroneous)
                        throw new Exception("Parent channel no longer exists");
                }

                Props.ChannelWithSubChannelsPackager ChannelWithSubChannels = new Props.ChannelWithSubChannelsPackager(this.Subscriber.SubscriberId, this.Subscriber.SubscriberUniqueId);
                ChannelWithSubChannels.Pop(allChannelsUnderSameSubscriber, firstStoredChannel, PopChannel);
                
                //PopChannel((int)firstStoredChannel.ChannelId);
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Failed to pop channel by user {1} ({2})", obj.MessageInfo.InvokerClientId, obj.MessageInfo.InvokerNickname);
            }
        }

        #endregion

        public void SortSubChannels(int ParentChannelId)
        {
            List<Props.ChannelWithSubChannelsPackager> channels = new List<Props.ChannelWithSubChannelsPackager>();
            List<ChannelListEntry> channelEntries = new List<ChannelListEntry>();

            var tsChannels = ServerQueryConnection.QueryRunner.GetChannelList();

            foreach (var tsChannel in tsChannels)
            {
                if (tsChannel.ParentChannelId == ParentChannelId)
                {
                    channelEntries.Add(tsChannel);
                }
            }

            channelEntries.Sort(new Utils.ChannelListEntryOrderComparer());

            List<ChannelModification> channelModifications = new List<ChannelModification>();

            for (int i = 0; i < channelEntries.Count; i++)
            {
                channelModifications.Add(new ChannelModification());

                if (i == 0)
                    channelModifications[i].ChannelOrder = 0;
                else
                    channelModifications[i].ChannelOrder = channelEntries[i - 1].ChannelId;
            }

            for (int j = 0; j < channelEntries.Count; j++)
                ServerQueryConnection.QueryRunner.EditChannel(channelEntries[j].ChannelId, channelModifications[j]);
        }

        public void StoreChannel(int ChannelId)
        {
            var ThisChannelInfo = ServerQueryConnection.QueryRunner.GetChannelInfo((uint)ChannelId);
            Models.StoredChannels storedChannel = new Models.StoredChannels(this.Subscriber.SubscriberId, this.Subscriber.SubscriberUniqueId);

            List<ChannelListEntry> ChannelList = ServerQueryConnection.QueryRunner.GetChannelList(true).ToList();
            ChannelListEntry ThisChannelEntry = ChannelList.SingleOrDefault(x => x.ChannelId == ChannelId);

            Props.ChannelWithSubChannelsPackager ChannelWithSubChannels = new Props.ChannelWithSubChannelsPackager(this.Subscriber.SubscriberId, this.Subscriber.SubscriberUniqueId);
            ChannelWithSubChannels.Store(ServerQueryConnection.QueryRunner, ChannelList, ThisChannelInfo, ThisChannelEntry, StoreSingleChannel);
        }

        private void StoreSingleChannel(int ChannelId, ChannelInfoResponse ChannelInfo, ChannelListEntry Channel)
        {
            //var channel = ServerQueryConnection.QueryRunner.GetChannelInfo((uint)ChannelId);
            var channel = ChannelInfo;

            Models.StoredChannels storedChannel = new Models.StoredChannels(this.Subscriber.SubscriberId, this.Subscriber.SubscriberUniqueId);
            storedChannel.Parse(ChannelId, channel);

            using (var db = new CoreContext())
            {
                // Store the channel
                db.StoredChannels.Add(storedChannel);

                // Store the channel group assignments
                var channelGroupClients = ServerQueryConnection.QueryRunner.GetChannelGroupClientList((uint?)ChannelId, null, null).ToList();
                foreach (var channelGroupClient in channelGroupClients)
                {
                    var storeChannel = new Models.StoredChannelGroupClients(Subscriber.SubscriberId, Subscriber.SubscriberUniqueId, channelGroupClient);
                    db.StoredChannelGroupClients.Add(storeChannel);
                }


                // Successfully stored the channel, now we can delete it from the server
                var response = ServerQueryConnection.QueryRunner.DeleteChannel((uint)ChannelId);

                do
                {
                    if (response.IsErroneous)
                    {
                        if (response.ErrorMessage == "channel not empty")
                        {
                            var usersInChannel = ServerQueryConnection.QueryRunner.GetClientList(false).Where(client => client.ChannelId == ChannelId);

                            foreach (var user in usersInChannel)
                            {
                                var kickResponse = ServerQueryConnection.QueryRunner.KickClient(user.ClientId, TS3QueryLib.Core.CommandHandling.KickReason.Channel, "Channel is being stored away.");

                                if (kickResponse.IsErroneous)
                                    throw new Exception(kickResponse.ResponseText + " (" + kickResponse.ErrorMessage + ")");

                                response = ServerQueryConnection.QueryRunner.DeleteChannel((uint)ChannelId);
                            }
                        }
                        else
                        {
                            throw new Exception(response.ResponseText + " (" + response.ErrorMessage + ")");
                        }
                    }
                } while (response.IsErroneous == true);

                db.SaveChanges();
            }
        }

        public void PopChannel(string StoredChannelUniqueId, int? ChannelParentId = null, int? ChannelOrder = null)
        {
            PopChannel(StoredChannelUniqueId, null, ChannelParentId, ChannelOrder);
        }

        public void PopChannel(int ChannelId, int? ChannelParentId = null, int? ChannelOrder = null)
        {
            PopChannel(null, ChannelId, ChannelParentId, ChannelOrder);
        }

        private int PopChannel(string StoredChannelUniqueId, int? ChannelId, int? ChannelParentId = null, int? ChannelOrder = null)
        {
            using (var db = new CoreContext())
            {
                // Remove the stored channel
                Models.StoredChannels storedChannel;

                if (ChannelId != null)
                    storedChannel = db.StoredChannels.SingleOrDefault(channel => channel.ChannelId == ChannelId && channel.SubscriberId == this.Subscriber.SubscriberId);
                else
                    storedChannel = db.StoredChannels.SingleOrDefault(channel => channel.StoredChannelUniqueId == StoredChannelUniqueId && channel.SubscriberId == this.Subscriber.SubscriberId);

                if (storedChannel == null)
                    throw new Exception("Stored channel does not exist");

                db.Remove(storedChannel);

                // Create the new channel
                var channelMod = storedChannel.ToChannelModification();

                if (channelMod.IconId == 0)
                    channelMod.IconId = null;

                if (ChannelParentId != null)
                    channelMod.ParentChannelId = (uint?)ChannelParentId;

                if (ChannelOrder != null)
                    channelMod.ChannelOrder = (uint?)ChannelOrder;

                SingleValueResponse<uint?> creationResponse = null;

                try
                {
                    //TODO: Fix the incorrect input format bug in ts3querylib
                    creationResponse = ServerQueryConnection.QueryRunner.CreateChannel(channelMod);

                    if (creationResponse.IsErroneous)
                        throw new Exception("Could not pop stored channel: " + creationResponse.ResponseText + " (" + creationResponse.ErrorMessage + ")");
                }
                catch (Exception ex)
                {
                    if (ex.Message != "Input string was not in a correct format.")
                    {
                        // Uh oh, something else happened
                        throw ex;
                    }
                }

                if (creationResponse.Value == null)
                    throw new Exception("Could not pop stored channel, no result given.");

                // Save changes
                db.SaveChanges();
                return (int)creationResponse.Value; // Return the new channel id
            }
        }
    }
}
