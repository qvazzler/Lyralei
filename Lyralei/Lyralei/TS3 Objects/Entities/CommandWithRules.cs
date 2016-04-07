using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS3QueryLib.Core.CommandHandling;

namespace Lyralei.TS3_Objects.Entities
{
    class CommandWithRules : Command
    {
        //Not currently used

        public CommandWithRules(string Name, CommandParameterGroup commandWithParams) : base(Name, commandWithParams)
        {

        }

        public CommandWithRules(CommandParameterGroup commandWithParams) : base(commandWithParams)
        {

        }

        public CommandWithRules(TS3QueryLib.Core.Server.CommandName commandName, params string[] options) : base(commandName, options)
        {

        }

        public CommandWithRules(TS3QueryLib.Core.Common.SharedCommandName commandName, params string[] options) : base(commandName, options)
        {

        }

        public CommandWithRules(TS3QueryLib.Core.Client.CommandName commandName, params string[] options) : base(commandName, options)
        {

        }

        public CommandWithRules(string commandName, params string[] options) : base(commandName, options)
        {

        }

    }
}
