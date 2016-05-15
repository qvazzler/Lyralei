using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Core.InputOwner.Props
{
    public enum QueuePosition
    {
        TryInterruptCurrent = 0,
        First = 1,
        Last = 2,
        NotQueueing = 3,
    }
}
