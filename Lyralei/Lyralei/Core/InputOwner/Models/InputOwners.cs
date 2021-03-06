﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lyralei.Core.InputOwner.Models
{
    public class InputOwners
    {
        public int InputOwnersId { get; set; }

        public string Name;
        public bool HasOwnership;
        public bool SingleInputAndRelease;

        public DateTime Created;
        public TimeSpan? InputWaitDuration;
        public Props.ReleaseRequestAction ReleaseRequestAction;
        public Props.QueuePosition QueuePosition;

        public int UserId { get; set; } // foreign
        public Core.UserManager.Models.Users Users { get; set; } // dependant
    }
}
