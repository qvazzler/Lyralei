using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lyralei.Bot.Commands
{
    public class BotCommandParam
    {
        public int Id;
        public int CommandId;
        public string Name;
        public string Usage;
        public string Docs;
        public int RegexId;
        public string Regex;
        public string RegexName;
        public string RegexDescription;

        public BotCommandParam()
        {

        }

        public BotCommandParam(int pId, int pCommandId, string pName, string pUsage, string pDocs, int pRegexId, string pRegex)
        {
            Id = pId;
            CommandId = pCommandId;
            Name = pName;
            Usage = pUsage;
            Docs = pDocs;
            RegexId = pRegexId;
            Regex = pRegex;
        }
    }
}
