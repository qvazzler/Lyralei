using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TS3QueryLib.Core.Server.Responses;
using TS3QueryLib.Core.Server.Entities;
using Lyralei.Core.ServerQueryConnection.Models;

namespace Lyralei.Core.ChannelManager.Models
{
    public class StoredChannelGroupClients
    {
        public StoredChannelGroupClients()
        {

        }

        public StoredChannelGroupClients(ChannelGroupClient ChannelGroupClient)
        {
            Parse(ChannelGroupClient);
        }

        public StoredChannelGroupClients(int SubscriberId, string SubscriberUniqueId, ChannelGroupClient ChannelGroupClient)
        {
            this.SubscriberId = SubscriberId;
            this.SubscriberUniqueId = SubscriberUniqueId;
            Parse(ChannelGroupClient);
        }

        [Key]
        public int StoredChannelGroupClientId { get; private set; }

        [Required]
        public int SubscriberId { get; set; }
        public string SubscriberUniqueId { get; set; }

        // Properties
        public int? ChannelId { get; protected set; }
        public int? ClientDatabaseId { get; protected set; }
        public int? ChannelGroupId { get; protected set; }

        // Parsing TS3 Channels
        public void Parse(ChannelGroupClient ChannelGroupClient)
        {
            ChannelId = (int)ChannelGroupClient.ChannelId;
            ClientDatabaseId = (int)ChannelGroupClient.ClientDatabaseId;
            ChannelGroupId = (int)ChannelGroupClient.ChannelGroupId;
        }
    }

}
