using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Lyralei.Logging
{
    public static class BotLogManager
    {
        public static BotLogger GetCurrentClassLogger()
        {
            Logger logger;
            logger = LogManager.GetCurrentClassLogger();

            BotLogger botLogger = new BotLogger();
            botLogger = (BotLogger)logger;

            return botLogger;
        }
    }
}
