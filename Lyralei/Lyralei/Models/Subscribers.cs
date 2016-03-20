﻿using System;
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
    }

}
