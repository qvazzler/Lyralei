using Lyralei.TS3_Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS3QueryLib.Core.Server.Notification.EventArgs;

namespace Lyralei.TS3_Objects.EventArguments
{
    public class BotCommandEventArgs : EventArgs
    {
        public CommandParameterGroupWithRules CommandInfo;
        public MessageReceivedEventArgs MessageInfo;
        public BotCommandEventArgs(CommandParameterGroupWithRules command, MessageReceivedEventArgs message)
        {
            this.CommandInfo = command;
            this.MessageInfo = message;
        }
    }
}
