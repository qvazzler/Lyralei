using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lyralei.Addons.Base;
using TS3QueryLib.Core.Server.Notification.EventArgs;

namespace Lyralei.Addons.InputOwner.EventArguments
{
    public class InputDetailsEventArgs : EventArgs
    {
        public Models.InputOwners InputOwner;
        public MessageReceivedEventArgs MessageDetails;
        //public 

        public InputDetailsEventArgs(Models.InputOwners InputOwner, MessageReceivedEventArgs MessageDetails)
        {
            this.InputOwner = InputOwner;
            this.MessageDetails = MessageDetails;
        }
    }
}
