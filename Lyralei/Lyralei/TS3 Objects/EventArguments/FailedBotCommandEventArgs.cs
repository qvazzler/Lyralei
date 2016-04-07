using Lyralei.TS3_Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Server.Notification.EventArgs;

namespace Lyralei.TS3_Objects.EventArguments
{
    public class FailedBotCommandEventArgs : EventArgs
    {
        public CommandParameterGroup CommandDetails;
        public MessageReceivedEventArgs MessageDetails;
        public FailedBotCommandEventArgs(CommandParameterGroup command, MessageReceivedEventArgs message)
        {
            this.CommandDetails = command;
            this.MessageDetails = message;
        }
    }
}
