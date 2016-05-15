using Lyralei.TS3_Objects.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Server;

namespace Lyralei.TS3_Objects.Entities
{
    public class CommandRuleSet
    {
        public CommandParameterGroupListWithRules Commands;
        public Action<BotCommandEventArgs> Method;
        public string CoreName;

        public CommandRuleSet(string CoreName, CommandParameterGroupListWithRules Commands, Action<BotCommandEventArgs> Method)
        {
            this.CoreName = CoreName;
            this.Commands = Commands;
            this.Method = Method;
        }
    }
}
