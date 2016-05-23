using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lyralei.Core.ChannelManager.Models
{
    public class ChannelDesignations
    {
        [Key]
        public int ChannelDesignationId { get; private set; }

        public int ChannelId { get; set; }

        public string DesignationName { get; set; }
        public int DesignatedByUserId { get; set; }

        public int SubscriberId { get; set; }
        public string SubscriberUniqueId { get; set; }

        //public string ServerQueryUsername { get; set; }
        //public string ServerQueryPassword { get; set; }

        //public string UserTeamSpeakClientUniqueId { get; set; }
        //public int UserId { get; set; } // foreign
        //public Core.UserManager.Models.Users Users { get; set; } // dependant
    }

}
