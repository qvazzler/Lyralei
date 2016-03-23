using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Lyralei.Logging;
using Lyralei.Bot;
using NLog;
using Microsoft.Data.Entity;

namespace Lyralei
{
    //Based on guide: https://docs.efproject.net/en/latest/platforms/full-dotnet/new-db.html
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            SynchronizationContext ctx = new SynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(ctx);

            List<BotConnection> botConnections = new List<BotConnection>();

            using (var db = new CoreContext())
            {
                if (args.Contains("deletedb"))
                {
                    db.Database.EnsureDeleted();
                    
                }
                else if (args.Contains("resetdb"))
                {
                    db.Subscribers.Clear();
                    db.SaveChanges();
                }
            }

            using (var db = new CoreContext())
            {
                /*
                    qvazzler
                    cWSwCRal

                    Adam
                    vm1wzFqU
                */

                if (db.Subscribers.Count(sub => sub.ServerIp == "localhost") == 0)
                {
                    db.Subscribers.Add(new Models.Subscribers {
                        ServerIp = "localhost",
                        AdminPassword = "39e7jMad",
                        AdminUsername = "Adam",
                        //AdminPassword = "password",
                        //AdminUsername = "serveradmin",
                        ServerPort = 10011,
                        VirtualServerId = 1,
                    });
                    var count = db.SaveChanges();

                    logger.Info("{0} records saved to database", count);
                }

                //Console.WriteLine();
                //Console.WriteLine("All subs in database:");
                foreach (var subscriber in db.Subscribers)
                {
                    logger.Info("Added new connection to server: {0}", subscriber.ServerIp);
                    botConnections.Add(new BotConnection(subscriber));
                }
            }

            while (true)
                Thread.Sleep(1000);
        }
    }
}
