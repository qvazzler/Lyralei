using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Responses;
using TS3QueryLib.Core.Server.Entities;

namespace Lyralei.Core.ChannelManager.Props
{
    public class ChannelWithSubChannelsPackager : Models.StoredChannels
    {
        public List<ChannelWithSubChannelsPackager> SubChannels = new List<ChannelWithSubChannelsPackager>();
        public QueryRunner QueryRunner;

        public ChannelWithSubChannelsPackager(int SubscriberId, string SubscriberUniqueId) : base(SubscriberId, SubscriberUniqueId)
        {
            
        }

        public ChannelWithSubChannelsPackager(int SubscriberId, string SubscriberUniqueId, QueryRunner QueryRunner, List<ChannelListEntry> ChannelList, ChannelInfoResponse ChannelInfo, ChannelListEntry Channel, Action<int, ChannelInfoResponse, ChannelListEntry> StoreMethod) : base(SubscriberId, SubscriberUniqueId)
        {
            Store(QueryRunner, ChannelList, ChannelInfo, Channel, StoreMethod);
        }

        public ChannelWithSubChannelsPackager(int SubscriberId, string SubscriberUniqueId, List<Models.StoredChannels> StoredChannelList, Models.StoredChannels Channel, Func<string, int?, int?, int?, int> PopMethod) : base(SubscriberId, SubscriberUniqueId)
        {
            Parse(Channel);
            Pop(StoredChannelList, Channel, PopMethod);
        }

        public void Pop(List<Models.StoredChannels> StoredChannelList, Models.StoredChannels Channel, Func<string, int?, int?, int?, int> PopMethod)
        {
            if (this.ChannelId == null)
                Parse(Channel);

            int changedChannelId = PopMethod.Invoke(null, this.ChannelId, null, null);

            for (int i = 0; i < StoredChannelList.Count; i++)
            {
                if (StoredChannelList[i].ParentChannelId == Channel.ChannelId)
                {
                    StoredChannelList[i].ParentChannelId = changedChannelId;
                    this.ChannelId = changedChannelId;
                    SubChannels.Add(new ChannelWithSubChannelsPackager(this.SubscriberId, this.SubscriberUniqueId, StoredChannelList, StoredChannelList[i], PopMethod));
                }
            }
        }

        public void Store(QueryRunner QueryRunner, List<ChannelListEntry> ChannelList, ChannelInfoResponse ChannelInfo, ChannelListEntry Channel, Action<int, ChannelInfoResponse, ChannelListEntry> StoreMethod)
        {
            this.QueryRunner = QueryRunner;
            this.Parse(ChannelInfo, Channel);

            for (int i = 0; i < ChannelList.Count; i++)
            {
                if (ChannelList[i].ParentChannelId == Channel.ChannelId)
                {
                    var SubChannelInfo = QueryRunner.GetChannelInfo(ChannelList[i].ChannelId);
                    SubChannels.Add(new ChannelWithSubChannelsPackager(this.SubscriberId, this.SubscriberUniqueId, QueryRunner, ChannelList, SubChannelInfo, ChannelList[i], StoreMethod));
                }
            }

            StoreMethod.Invoke((int)this.ChannelId, ChannelInfo, Channel);
        }

        public void Parse(Models.StoredChannels StoredChannel)
        {
            this.ChannelIconId = StoredChannel.ChannelIconId;
            this.ChannelId = StoredChannel.ChannelId;
            this.Codec = StoredChannel.Codec;
            this.CodecQuality = StoredChannel.CodecQuality;
            this.DeleteDelay = StoredChannel.DeleteDelay;
            this.Description = StoredChannel.Description;
            this.FilePath = StoredChannel.FilePath;
            this.ForcedSilence = StoredChannel.ForcedSilence;
            this.IconId = StoredChannel.IconId;
            this.IsDefaultChannel = StoredChannel.IsDefaultChannel;
            this.IsMaxClientsUnlimited = StoredChannel.IsMaxClientsUnlimited;
            this.IsMaxFamilyClientsInherited = StoredChannel.IsMaxFamilyClientsInherited;
            this.IsMaxFamilyClientsUnlimited = StoredChannel.IsMaxFamilyClientsUnlimited;
            this.IsPasswordProtected = StoredChannel.IsPasswordProtected;
            this.IsPermanent = StoredChannel.IsPermanent;
            this.IsSemiPermanent = StoredChannel.IsSemiPermanent;
            this.IsSpacer = StoredChannel.IsSpacer;
            this.IsUnencrypted = StoredChannel.IsUnencrypted;
            this.LatencyFactor = StoredChannel.LatencyFactor;
            this.MaxClients = StoredChannel.MaxClients;
            this.MaxFamilyClients = StoredChannel.MaxFamilyClients;
            this.Name = StoredChannel.Name;
            this.NeededSubscribePower = StoredChannel.NeededSubscribePower;
            this.NeededTalkPower = StoredChannel.NeededTalkPower;
            this.Order = StoredChannel.Order;
            this.ParentChannelId = StoredChannel.ParentChannelId;
            this.PasswordHash = StoredChannel.PasswordHash;
            this.PhoneticName = StoredChannel.PhoneticName;
            this.SecondsEmpty = StoredChannel.SecondsEmpty;
            this.SecuritySalt = StoredChannel.SecuritySalt;
            this.StoredChannelUniqueId = StoredChannelUniqueId;
            this.Topic = StoredChannel.Topic;
            this.TotalClients = StoredChannel.TotalClients;
            this.TotalClientsFamily = StoredChannel.TotalClientsFamily;
        }
    }
}
