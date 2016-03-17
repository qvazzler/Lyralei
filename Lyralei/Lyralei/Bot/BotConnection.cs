using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;

using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Responses;
using TS3QueryLib.Core.Server.Entities;
using System.Threading;
using System.Reflection;

using Lyralei.Logging;
using Lyralei.Models;

namespace Lyralei.Bot
{
    public class BotConnection
    {
        Models.Subscribers subscriber;
        Addons.AddonManager addonManager = new Addons.AddonManager();

        public BotConnection()
        {

        }

        public BotConnection(Subscribers _subscriber)
        {
            Load(_subscriber);
        }

        public void Load(Subscribers _subscriber)
        {
            try
            {
                subscriber = _subscriber;
                Log.Instance.Debug("Connecting to " + subscriber.ServerIp + "..");
                ServerQueryConnection serverQueryConnection = new ServerQueryConnection(subscriber);
            }
            catch (Exception ex)
            {
                Log.Instance.Warn(ex, "Could not connect to " + subscriber.ServerIp + ": " + ex.Message);
            }
        }

        void serverquery_ConnectionUp(object sender, EventArgs e)
        {
        }

        void serverquery_ConnectionDown(object sender, EventArgs e)
        {
        }
    }
}
