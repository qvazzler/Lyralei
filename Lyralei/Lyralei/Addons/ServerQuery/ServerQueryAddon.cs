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
        public string AddonName { get; set; } = "ServerQuery";

        public Test.TestAddon testAddon = null;

        public void Initialize()
        {
            this.serverQueryRootConnection.BotCommandReceived += onBotCommand;

            ModelCustomizer.AddModelCustomization(Hooks.ModelCustomizer.OnModelCreating);

            //Add a dependency
            this.dependencyManager.AddDependencyRequirement("Test");
            this.dependencyManager.UpdateInjections();
            testAddon = (Test.TestAddon)dependencyManager.GetAddon("Test");
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
                    if (cmdPG.Exists(cmdP => cmdP.Name.ToLower() == "action" && cmdP.Value.ToLower() == "register"))
                    {
                        CommandParameter Username = null;
                        CommandParameter Password = null;

                        if (cmdPG.Exists(cmdP => (Username = cmdP).Name.ToLower() == "username") && cmdPG.Exists(cmdP => (Password = cmdP).Name.ToLower() == "password"))
                        {
                            using (var db = new CoreContext())
                            {
                                var user = userManager.QueryUser(subscriber.SubscriberId, subscriber.SubscriberUniqueId, e.InvokerUniqueId);

                                Models.ServerQueryUser sqUser = new Models.ServerQueryUser()
                                {
                                    UserId = user.UserId,
                                    Users = user,
                                    //SubscriberId = user.SubscriberId,
                                    ServerQueryUsername = Username.Value,
                                    ServerQueryPassword = Password.Value,
                                };

                                Lyralei.Models.Subscribers subscriberUserCredentials = new Lyralei.Models.Subscribers()
                                    {
                                    AdminPassword = sqUser.ServerQueryPassword,
                                    AdminUsername = sqUser.ServerQueryUsername,
                                    ServerIp = subscriber.ServerIp,
                                    ServerPort = subscriber.ServerPort,
                                    SubscriberId = subscriber.SubscriberId,
                                    SubscriberUniqueId = subscriber.SubscriberUniqueId,
                                    VirtualServerId = subscriber.VirtualServerId,
                                };

                                ServerQueryUserConnection serverQueryUserConnection = new ServerQueryUserConnection(subscriberUserCredentials);

                                //serverQueryUserConnection.Initialize();

                                Thread thread = new Thread((ThreadStart)new SynchronizationCallback(serverQueryUserConnection.InitializeQuiet));
                                thread.Start();

                                thread.Join();

                                //do
                                //{
                                //    Thread.Sleep(5);
                                //} while (t.ThreadState == ThreadState.Running);

                                try
                                {
                                    if (serverQueryUserConnection.atd.IsConnected)
                                    {
                                        var test = serverQueryUserConnection.whoAmI;

                                        if (test == null)
                                            throw new Exception("Login failed");
                                        if (test.IsErroneous)
                                            throw new Exception(test.ResponseText);
                                        else
                                        {
                                            db.ServerQueryUser.Add(sqUser);
                                            db.SaveChanges();

                                            serverQueryUserConnection.queryRunner.Logout();
                                            serverQueryUserConnection.atd.Disconnect();

                                            //User successfully registered
                                            TextReply(e, "Successfully registered! You can now execute serverquery commands directly to me based on your user permissions.");
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    TextReply(e, "Error, please check your command and try again.");
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
                    TextReply(e, "Error, please check your command and try again.");
                }
            }

            foreach (CommandName cmdName in Enum.GetValues(typeof(CommandName)))
            {
                if (cmdName.ToString().ToLower() == cmd.Name.ToLower())
                {
                    Models.ServerQueryUser serverQueryUser = null;

                    try
                    {
                        using (var db = new CoreContext())
                        {
                            //user = userManager.GetUser(e.InvokerUniqueId);
                            var User = db.Users.Single(usr => usr.UserTeamSpeakClientUniqueId == e.InvokerUniqueId && usr.SubscriberUniqueId == subscriber.SubscriberUniqueId);
                            serverQueryUser = db.ServerQueryUser.Single(sqUser => sqUser.Users.UserId == User.UserId);
                        }
                    }
                    catch (Exception ex) when (ex.Message == "Sequence contains no elements")
                    {
                        TextReply(e, "You do not have access to this command.");
                        return;
                    }
                    catch (Exception)
                    {
                        TextReply(e, "There was an error performing this command.");
                        return;
                    }

                    try
                    {
                        string result = SendServerQueryCommand(cmd, serverQueryUser);
                        this.serverQueryRootConnection.queryRunner.SendTextMessage(MessageTarget.Client, e.InvokerClientId, result);
                    }
                    catch (Exception)
                    {
                        TextReply(e, "There was an error performing this command, or you lacked permissions.");
                        return;
                    }
                }

                break;
            }
        }

        private string SendServerQueryCommand(Command cmd, Models.ServerQueryUser user)
        {
            using (ServerQueryUserConnection serverQueryUserConnection = new ServerQueryUserConnection(this.subscriber))
            {
                return serverQueryUserConnection.queryRunner.SendCommand(cmd);
            }
        }
    }
}
