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

using Lyralei.Core.ServerQueryConnection.Models;
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
        //Core.AddonManager.AddonManager AddonManager;

        public BotConnection(Subscribers Subscriber)
        {
            this.Subscriber = Subscriber;
            Load();
        }

        public void LoadEx()
        {
            try
            {
                Core.CoreManager.CoreManager CoreMgr = new Core.CoreManager.CoreManager(Subscriber);
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Failed to instantiate connection");
            }
        }

        public void Load()
        {
            LoadEx();

            //try
            //{
            //    //Log.Instance.Debug(subscriber.ServerIp + " - Connecting..");
            //    logger.Debug("Connecting..");
            //    Core.ServerQueryConnection.ServerQueryConnection ServerQueryConnection = new Core.ServerQueryConnection.ServerQueryConnection(Subscriber);

            //    ServerQueryConnection.SetName("Lyralei");

            //    ServerQueryConnection.Initialize();

            //    AddonManager = new Core.AddonManager.AddonManager(Subscriber, ServerQueryConnection);

            //    AddonManager.onBotCommand += AddonManager_onBotCommand;
            //    AddonManager.onFailedBotCommand += AddonManager_onFailedBotCommand;
            //}
            //catch (Exception ex)
            //{
            //    logger.Warn(ex, "Failed to connect.");
            //}
        }

        private void AddonManager_onFailedBotCommand(object sender, TS3_Objects.EventArguments.FailedBotCommandEventArgs e)
        {
            logger.Debug("User {0} (ts3id: {1}) sent incorrect command: {2}", e.MessageDetails.InvokerNickname, e.MessageDetails.InvokerClientId, e.MessageDetails.Message);
        }

        private void AddonManager_onBotCommand(object sender, TS3_Objects.EventArguments.BotCommandEventArgs e)
        {
            logger.Debug("User {0} (ts3id: {1}) sent command: {2}", e.MessageInfo.InvokerNickname, e.MessageInfo.InvokerClientId, e.MessageInfo.Message);
        }

        void serverquery_ConnectionUp(object sender, EventArgs e)
        {
        }

        void serverquery_ConnectionDown(object sender, EventArgs e)
        {
        }
    }
}
