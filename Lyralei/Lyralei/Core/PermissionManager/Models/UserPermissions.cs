using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lyralei.Core.PermissionManager.Models
{
    public class UserPermissions
    {
        [Key]
        public int UserPermissionId { get; private set; }

        public string PermissionName { get; set; }
        public int? PermissionValue { get; set; }
        //public int SubscriberId { get; set; }
        //public string SubscriberUniqueId { get; set; }

        //public string ServerQueryUsername { get; set; }
        //public string ServerQueryPassword { get; set; }

        //public string UserTeamSpeakClientUniqueId { get; set; }
        public int UserId { get; set; } // foreign
        public Core.UserManager.Models.Users Users { get; set; } // dependant
    }

}
