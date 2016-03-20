using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lyralei.Addons.ServerQuery
{
    public class ServerQueryUserDetails
    {
        [Key]
        public int UserId { get; set; }
        public int SubscriberId { get; set; }
        public string ServerQueryUsername { get; set; }
        public string ServerQueryPassword { get; set; }

        public Models.Users Users { get; set; }
        public int ServerQueryUserDetailsForeignKey { get; set; }
    }
}
