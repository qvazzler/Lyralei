using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//using Lyralei.Logging;
using Lyralei.Bot;
using NLog;
//using Lyralei.Logging;
using Microsoft.Data.Entity;
using NLog.Fluent;
using TS3QueryLib.Core.CommandHandling;

namespace Lyralei
{


    //Based on guide: https://docs.efproject.net/en/latest/platforms/full-dotnet/new-db.html
    class Program
    {
        private static Logger logger = LogManager.GetLogger(typeof(Program).Name);

        static void Main(string[] args)
        {
            CommandParameterGroupList cmds;
            cmds = CommandParameterGroupList.Parse(String.Join(" ", args));

            SynchronizationContext ctx = new SynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(ctx);

            List<BotConnection> botConnections = new List<BotConnection>();

            using (var db = new CoreContext())
            {
                //Database deletion switch
                var cmds_deldb = cmds.Where(cmdPG => cmdPG.Exists(cmdP => cmdP.Name == "deletedb"));
                if (cmds_deldb.Count() > 0)
                {
                    db.Database.EnsureDeleted();
                    logger.Info("Database deleted!");
                }
            }

            using (var db = new CoreContext())
            {
                //Subscribers reset switch
                var cmds_rs = cmds.Where(cmdPG => cmdPG.Exists(cmdP => cmdP.Name == "resetsubscribers"));
                if (cmds_rs.Count() > 0)
                {
                    db.Subscribers.Clear();
                    db.SaveChanges();
                    logger.Info("Subscribers removed!");
                }

                //Subscriber-adding
                var cmds_dc = cmds.Where(cmdPG => cmdPG.Exists(cmdP => cmdP.Name == "subscriber"));
                foreach (var cmd in cmds_dc)
                {
                    try
                    {
                        var serverIp = cmd.SingleOrDefault(x => x.Name.ToLower() == "serverip").Value;
                        var adminPassword = cmd.SingleOrDefault(x => x.Name.ToLower() == "adminpassword").Value;
                        var adminUsername = cmd.SingleOrDefault(x => x.Name.ToLower() == "adminusername").Value;
                        var serverPort = cmd.SingleOrDefault(x => x.Name.ToLower() == "serverport").Value;
                        var virtualServerId = cmd.SingleOrDefault(x => x.Name.ToLower() == "virtualserverid").Value;
                        var subscriberUniqueId = cmd.SingleOrDefault(x => x.Name.ToLower() == "uniqueid").Value;
                        //bool save = cmd.SingleOrDefault(x => x.Name.ToLower() == "save").Value != "0";

                        Core.ServerQueryConnection.Models.Subscribers sub = new Core.ServerQueryConnection.Models.Subscribers()
                        {
                            ServerIp = serverIp,
                            AdminPassword = adminPassword,
                            AdminUsername = adminUsername,
                            ServerPort = Convert.ToInt16(serverPort),
                            VirtualServerId = Convert.ToInt32(virtualServerId),
                            SubscriberUniqueId = subscriberUniqueId
                        };

                        db.Subscribers.Add(sub);
                        var count = db.SaveChanges();
                        logger.Info("Subscriber {0} saved to database", sub.ToString(true));
                    }
                    catch (Exception)
                    {
                        logger.Warn("Failed to add a subscriber from command line");
                    }
                }

                foreach (var subscriber in db.Subscribers)
                {
                    logger.Info("Setting up subscriber {0}..", subscriber.ToString(true));
                    botConnections.Add(new BotConnection(subscriber));
                }
            }

            while (true)
                Thread.Sleep(1000);
        }
    }
}
