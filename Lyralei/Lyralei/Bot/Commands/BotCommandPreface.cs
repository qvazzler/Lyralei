using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lyralei.Bot.Commands
{
    public class BotCommandPreface
    {
        public BotCommandBase baseCmd;
        public List<BotCommandParam> Parameters;

        public BotCommandPreface()
        {
            baseCmd = new BotCommandBase();
            Parameters = new List<BotCommandParam>();
        }
    }
}
