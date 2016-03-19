using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lyralei.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; private set; }
        public int SubscriberId { get; set; }
        public string SubscriberUniqueId { get; set; }

        public string ServerQueryUsername { get; set; }
        public string ServerQueryPassword { get; set; }

        public string UserTeamSpeakClientUniqueId { get; set; }
    }

}
