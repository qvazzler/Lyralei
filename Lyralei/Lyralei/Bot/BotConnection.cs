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

using Lyralei.Models;
using NLog;

namespace Lyralei.Bot
{
    public class BotConnection
    {
        private Logger logger;

        private Subscribers subscriber;
        public Subscribers Subscriber
        {
            get { return subscriber; }
            set
            {
                subscriber = value;
                logger = LogManager.GetLogger(this.GetType().Name + " - " + subscriber.ToString());
            }
        }
        Addons.AddonManager addonManager;

        public BotConnection(Subscribers _subscriber)
        {
            Subscriber = _subscriber;

            Load();
        }

        public void Load()
        {
            try
            {
                //Log.Instance.Debug(subscriber.ServerIp + " - Connecting..");
                logger.Debug("Connecting..");
                ServerQueryRootConnection serverQueryConnection = new ServerQueryRootConnection(subscriber, true);

                addonManager = new Addons.AddonManager(subscriber, serverQueryConnection);
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Failed to connect.");
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
