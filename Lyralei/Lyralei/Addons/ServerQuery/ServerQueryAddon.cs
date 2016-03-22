using Lyralei.Addons.Base;
using Lyralei.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Notification.EventArgs;

using Microsoft.Data.Entity;
using System.Threading;

namespace Lyralei.Addons.ServerQuery
{
    class ServerQueryAddon : AddonBase, IAddon
    {
        public void Initialize()
        {
            this.serverQueryRootConnection.BotCommandReceived += onBotCommand;

            ModelCustomizer.AddModelCustomization(Hooks.ModelCustomizer.OnModelCreating);
        }

        private void onBotCommand(object sender, CommandParameterGroup cmdPG, MessageReceivedEventArgs e)
        {
            //Command cmd = new Command("help", new string[] { "serverinfo", "additionaltest" });
            Command cmd = new Command(cmdPG);
            Bot.UserManager userManager = new Bot.UserManager(this.subscriber.SubscriberId);

            //userManager.GetUserInformation

            if (cmd.Name.ToLower() == "serverquery")
            {
                try
                {

                    //CommandParameter cmdP;
                    //if ((cmdP = cmdPG.SingleOrDefault(x => x.Name == "action")) != null)
                    //{
                    //    if (cmdP.Value == "register")
                    //    {

                    //    }
                    //}


                    //TODO: DO PROPER RECOGNITION FOR NOT BEING REGISTERED!!! DO REGISTER
                    if (cmdPG.Exists(cmdP => cmdP.Name.ToLower() == "action" && cmdP.Value.ToLower() == "register"))
                    {
                        CommandParameter Username = null;
                        CommandParameter Password = null;

                        if (cmdPG.Exists(cmdP => (Username = cmdP).Name.ToLower() == "username") && cmdPG.Exists(cmdP => (Password = cmdP).Name.ToLower() == "password"))
                        {
                            using (var db = new CoreContext())
                            {
                                    var user = userManager.QueryUser(subscriber.SubscriberId, subscriber.SubscriberUniqueId, e.InvokerUniqueId);

                                    ServerQueryUserDetails sqUser = new ServerQueryUserDetails()
                                    {
                                        UserId = user.UserId,
                                        SubscriberId = user.SubscriberId,
                                        ServerQueryUsername = Username.Value,
                                        ServerQueryPassword = Password.Value,
                                    };

                                ServerQueryUserConnection serverQueryUserConnection = new ServerQueryUserConnection(subscriber, sqUser);

                                //serverQueryUserConnection.Initialize();

                                Thread t = new Thread((ThreadStart)new SynchronizationCallback(serverQueryUserConnection.Initialize));
                                t.Start();

                                do
                                {
                                    Thread.Sleep(10);
                                } while (t.ThreadState == ThreadState.Running);

                                try
                                {
                                    //TODO: We already logged in, so instead change this to a "whoami" command perhaps? To validate that the username matches or something maybe, not sure.
                                    //serverQueryUserConnection.Login();

                                    db.ServerQueryUserDetails.Add(sqUser);
                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {
                                    this.serverQueryRootConnection.queryRunner.SendTextMessage(MessageTarget.Client, e.InvokerClientId, "Error, please check your command and try again.");
                                }
                            }

                            //var username = cmdPG.Single(cmdP => cmdP.Name.ToLower() == "username").Value;
                            //var password = cmdPG.Single(cmdP => cmdP.Name.ToLower() == "password").Value;

                            //var serverUniqueId = queryRunner.GetServerInfo().UniqueId;

                            //var uniqueId = e.InvokerUniqueId;
                            //var databaseId = queryRunner.GetClientDatabaseIdsByUniqueId(uniqueId);
                        }
                    }

                    //if(cmdPG.Exists(cmdP =>
                    //{
                    //    if (cmdP.Name == "action")
                    //    {
                    //        if (cmdP.Value == "register")
                    //        {
                    //            return true;
                    //        }
                    //    }

                    //    return false;
                    //}));
                }
                catch (Exception ex)
                {
                    this.serverQueryRootConnection.queryRunner.SendTextMessage(MessageTarget.Client, e.InvokerClientId, "Error, please check your command and try again.");
                }
            }

            foreach (CommandName cmdName in Enum.GetValues(typeof(CommandName)))
            {
                if (cmdName.ToString().ToLower() == cmd.Name.ToLower())
                {
                    Lyralei.Addons.ServerQuery.ServerQueryUserDetails serverQueryUser = null;

                    try
                    {
                        using (var db = new CoreContext())
                        {
                            //user = userManager.GetUser(e.InvokerUniqueId);
                            var User = db.Users.Single(usr => usr.UserTeamSpeakClientUniqueId == e.InvokerUniqueId && usr.SubscriberUniqueId == subscriber.SubscriberUniqueId);
                            serverQueryUser = db.ServerQueryUserDetails.Single(sqUser => sqUser.UserId == User.UserId);
                        }
                    }
                    catch (Exception ex) when (ex.Message == "Sequence contains no elements")
                    {
                        this.serverQueryRootConnection.queryRunner.SendTextMessage(MessageTarget.Client, e.InvokerClientId, "You do not have access to this command.");
                        return;
                    }
                    catch (Exception)
                    {
                        this.serverQueryRootConnection.queryRunner.SendTextMessage(MessageTarget.Client, e.InvokerClientId, "There was an error performing this command.");
                        return;
                    }

                    try
                    {
                        string result = SendServerQueryCommand(cmd, serverQueryUser);
                        this.serverQueryRootConnection.queryRunner.SendTextMessage(MessageTarget.Client, e.InvokerClientId, result);
                    }
                    catch (Exception)
                    {
                        this.serverQueryRootConnection.queryRunner.SendTextMessage(MessageTarget.Client, e.InvokerClientId, "There was an error performing this command, or you lacked permissions.");
                        return;
                    }
                }

                break;
            }
        }

        private void blah(object state)
        {
            Console.WriteLine(
     "Background Thread: SynchronizationContext.Current is " +
     (SynchronizationContext.Current != null ?
      SynchronizationContext.Current.ToString() : "null"));


        }

        private string SendServerQueryCommand(Command cmd, Lyralei.Addons.ServerQuery.ServerQueryUserDetails user)
        {
            using (ServerQueryUserConnection serverQueryUserConnection = new ServerQueryUserConnection(this.subscriber, user))
            {
                return serverQueryUserConnection.queryRunner.SendCommand(cmd);
            }
        }
    }
}
