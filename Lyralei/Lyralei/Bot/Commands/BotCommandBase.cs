using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lyralei.Bot.Commands
{
    public class BotCommandBase
    {
        public int Id;
        public string Name;
        public string Usage;
        public string Docs;

        public BotCommandBase()
        {

        }

        public BotCommandBase(int pId, string pName, string pUsage, string pDocs)
        {
            Id = pId;
            Name = pName;
            Usage = pUsage;
            Docs = pDocs;
        }
    }
}
