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

            ////DB connectionstring-adding, not a good option because it won't let you use Add-Migration / Update-Database
            //var cmd_connectionstring = cmds.Where(cmdPG => cmdPG.Exists(cmdP => cmdP.Name == "connectionstring"));
            //foreach (var cmd in cmd_connectionstring)
            //{
            //    CoreContext.connectionString = cmd.SingleOrDefault(x => x.Name.ToLower() == "connectionstring").Value;
            //    string test2 = cmd.SingleOrDefault(x => x.Name.ToLower() == "connectionstring").EncodedValue;
            //}

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
                        Core.ServerQueryConnection.Models.Subscribers sub = new Core.ServerQueryConnection.Models.Subscribers()
                        {
                            ServerIp = cmd.SingleOrDefault(x => x.Name.ToLower() == "serverip").Value,
                            AdminPassword = cmd.SingleOrDefault(x => x.Name.ToLower() == "adminpassword").Value,
                            AdminUsername = cmd.SingleOrDefault(x => x.Name.ToLower() == "adminusername").Value,
                            ServerPort = Convert.ToInt16(cmd.SingleOrDefault(x => x.Name.ToLower() == "serverport").Value),
                            VirtualServerId = Convert.ToInt32(cmd.SingleOrDefault(x => x.Name.ToLower() == "virtualserverid").Value),
                            SubscriberUniqueId = cmd.SingleOrDefault(x => x.Name.ToLower() == "uniqueid").Value,
                            BotNickName = cmd.SingleOrDefault(x => x.Name.ToLower() == "botnickname").Value
                        };

                        var search = db.Subscribers.SingleOrDefault(x => x.SubscriberUniqueId == sub.SubscriberUniqueId && x.VirtualServerId == sub.VirtualServerId);

                        if (search == null)
                            db.Subscribers.Add(sub);
                        else
                        {
                            search.ServerIp = sub.ServerIp;
                            search.AdminPassword = sub.AdminPassword;
                            search.AdminUsername = sub.AdminUsername;
                            search.ServerPort = Convert.ToInt16(sub.ServerPort);
                            search.VirtualServerId = Convert.ToInt32(sub.VirtualServerId);
                            search.SubscriberUniqueId = sub.SubscriberUniqueId;
                            search.BotNickName = sub.BotNickName;
                        }

                        var count = db.SaveChanges();
                        logger.Info("Subscriber {0} saved to database", sub.ToString(true));
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex, "Failed to add a subscriber from command line");
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
