using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Server;

namespace Lyralei.TS3_Objects.Entities
{
    public class CommandRuleSets : List<CommandRuleSet>
    {
        public CommandRuleSets()
        {

        }

        public CommandRuleSets(CommandRuleSet schema)
        {
            this.Add(schema);
        }

        public void ValidateAddSchema(CommandRuleSet schema)
        {
            var nativeCommands = Enum.GetValues(typeof(CommandName));
            foreach (var nativeCmd in nativeCommands)
            {
                if (schema.Commands.Count(x => x.First().Name == nativeCmd.ToString()) > 1)
                {
                    throw new Exception(String.Format("Command name '{0}' is reserved for serverquery, please define a different command name!", nativeCmd.ToString()));
                }
            }

            foreach (var liveSchema in this)
            {
                if (schema.Commands.Any(testCmds => liveSchema.Commands.Any(liveCmds => testCmds.First().Name == liveCmds.First().Name)))
                    throw new Exception("One or more command names already used by a different addon");
            }

            // No exceptions? Let's add it
            this.Add(schema);
        }
    }
}
