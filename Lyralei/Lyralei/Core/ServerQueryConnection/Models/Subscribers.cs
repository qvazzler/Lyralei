using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lyralei.Core.ServerQueryConnection.Models
{
    public class Subscribers
    {
        [Key]
        [Required]
        public int SubscriberId { get; set; }
        public string SubscriberUniqueId { get; set; }

        [Required]
        public string ServerIp { get; set; }
        [Required]
        public short ServerPort { get; set; }
        [Required]
        public int VirtualServerId { get; set; }

        [Required]
        public string AdminUsername { get; set; }
        [Required]
        public string AdminPassword { get; set; }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool includeUsername = true)
        {
            string result = "";

            if (includeUsername)
                result += this.AdminUsername + "@";

            result += this.ServerIp + ":" + this.ServerPort + ":" + this.VirtualServerId;

            return result;
        }
    }

}
