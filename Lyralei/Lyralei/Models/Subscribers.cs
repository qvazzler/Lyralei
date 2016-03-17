using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lyralei.Models
{
    public class Subscribers
    {
        [Key]
        public int SubscriberId { get; set; }

        public string ServerIp { get; set; }
        public short ServerPort { get; set; }
        public int VirtualServerId { get; set; }

        public string AdminUsername { get; set; }
        public string AdminPassword { get; set; }
    }

}
