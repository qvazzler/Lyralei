﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lyralei.Addons.ServerQuery.Models
{
    public class ServerQueryUser
    {
        //[Key]
        public int ServerQueryUserId { get; set; }
        //public int SubscriberId { get; set; }
        public string ServerQueryUsername { get; set; }
        public string ServerQueryPassword { get; set; }

        public int UserId { get; set; } // foreign
        public Lyralei.Models.Users Users { get; set; } // dependant
    }
}
