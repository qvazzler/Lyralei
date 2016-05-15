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
using NLog.Fluent;
using Lyralei.TS3_Objects.Entities;
using Lyralei.TS3_Objects.EventArguments;
using Lyralei.Core.UserManager;
using Lyralei.Core.ServerQueryConnection.Models;

namespace Lyralei.Core.ServerQueryShell
{
    class ServerQueryShell : Base.CoreBase, Base.ICore
    {
        private ServerQueryConnection.ServerQueryConnection ServerQueryConnection;
        private UserManager.UserManager UserManager;

        public ServerQueryShell(Subscribers Subscriber) : base(Subscriber)
        {
            this.Name = this.GetType().Name;
            this.CoreDependencies.Add(typeof(ServerQueryConnection.ServerQueryConnection).Name);
            this.CoreDependencies.Add(typeof(UserManager.UserManager).Name);
        }

        public void UserInitialize(CoreList AddonInjections)
        {
            this.ServerQueryConnection = AddonInjections[typeof(ServerQueryConnection.ServerQueryConnection).Name] as ServerQueryConnection.ServerQueryConnection;
            this.UserManager = AddonInjections[typeof(UserManager.UserManager).Name] as UserManager.UserManager;

            this.ServerQueryConnection.BotCommandAttempt += onBotCommand;

            ModelCustomizer.AddModelCustomization(Hooks.ModelCustomizer.OnModelCreating);
        }

        public CommandRuleSets DefineCommandSchemas()
        {
            CommandRuleSets ruleSets = new CommandRuleSets();
            CommandParameterGroupListWithRules cmds = new CommandParameterGroupListWithRules();

            CommandParameterGroupWithRules cmdCool = new CommandParameterGroupWithRules();
            cmdCool.Add(new CommandParameterWithRules("serverquery")
            {
                IsBaseCommand = true
            });
            cmdCool.Add(new CommandParameterWithRules("register")
            {
                NameValueSetting = NameValueSetting.NameOnly,
            });
            cmdCool.Add(new CommandParameterWithRules("username")
            {
                NameValueSetting = NameValueSetting.ValueOrValueAndName,
                Required = true,
                Help = "Your ServerQuery username",
                Nullable = false,
            });
            cmdCool.Add(new CommandParameterWithRules("password")
            {
                NameValueSetting = NameValueSetting.ValueOrValueAndName,
                Required = true,
                Help = "Your ServerQuery password",
                Nullable = false,
            });

            cmds.Add(cmdCool);
            ruleSets.Add(new CommandRuleSet(this.Name, cmds, ServerQueryUserRegistrationCommand));

            return ruleSets;
        }

        public void ServerQueryUserRegistrationCommand(BotCommandEventArgs e)
        {
            using (var db = new CoreContext())
            {
                var user = UserManager.QueryUser(Subscriber.SubscriberId, Subscriber.SubscriberUniqueId, e.MessageInfo.InvokerUniqueId);

                Models.ServerQueryUser sqUser = new Models.ServerQueryUser()
                {
                    UserId = user.UserId,
                    Users = user,
                    //SubscriberId = user.SubscriberId,
                    ServerQueryUsername = e.CommandInfo["username"].Value,
                    ServerQueryPassword = e.CommandInfo["password"].Value,
                };

                Lyralei.Core.ServerQueryConnection.Models.Subscribers subscriberUserCredentials = new Lyralei.Core.ServerQueryConnection.Models.Subscribers()
                {
                    AdminPassword = sqUser.ServerQueryPassword,
                    AdminUsername = sqUser.ServerQueryUsername,
                    ServerIp = Subscriber.ServerIp,
                    ServerPort = Subscriber.ServerPort,
                    SubscriberId = Subscriber.SubscriberId,
                    SubscriberUniqueId = Subscriber.SubscriberUniqueId,
                    VirtualServerId = Subscriber.VirtualServerId,
                };

                Core.ServerQueryConnection.ServerQueryConnection ServerQueryConnection = new Core.ServerQueryConnection.ServerQueryConnection( subscriberUserCredentials);

                Thread thread = new Thread((ThreadStart)new SynchronizationCallback(ServerQueryConnection.InitializeQuiet));
                thread.Start();
                thread.Join();

                try
                {
                    if (ServerQueryConnection.AsyncTcpDispatcher.IsConnected)
                    {
                        var test = ServerQueryConnection.whoAmI;

                        if (test == null)
                            throw new Exception("Login failure");
                        if (test.IsErroneous)
                            throw new Exception(test.ResponseText);
                        else
                        {
                            db.ServerQueryUser.Add(sqUser);
                            db.SaveChanges();

                            ServerQueryConnection.Logout();
                            ServerQueryConnection.Disconnect();

                            //User successfully registered
                            ServerQueryConnection.TextReply(e.MessageInfo, "Successfully registered! You can now execute serverquery commands directly to me based on your user permissions.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Debug(ex, "User failed to register");
                    ServerQueryConnection.TextReply(e.MessageInfo, "Whoops! Did you put in the right details?");
                }
            }
        }

        private void onBotCommand(object sender, CommandParameterGroup cmdPG, MessageReceivedEventArgs e)
        {
            //Command cmd = new Command("help", new string[] { "serverinfo", "additionaltest" });
            Command cmd = new Command(cmdPG);

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
                                var user = UserManager.QueryUser(Subscriber.SubscriberId, Subscriber.SubscriberUniqueId, e.InvokerUniqueId);

                                Models.ServerQueryUser sqUser = new Models.ServerQueryUser()
                                {
                                    UserId = user.UserId,
                                    Users = user,
                                    //SubscriberId = user.SubscriberId,
                                    ServerQueryUsername = Username.Value,
                                    ServerQueryPassword = Password.Value,
                                };

                                Lyralei.Core.ServerQueryConnection.Models.Subscribers subscriberUserCredentials = new Lyralei.Core.ServerQueryConnection.Models.Subscribers()
                                {
                                    AdminPassword = sqUser.ServerQueryPassword,
                                    AdminUsername = sqUser.ServerQueryUsername,
                                    ServerIp = Subscriber.ServerIp,
                                    ServerPort = Subscriber.ServerPort,
                                    SubscriberId = Subscriber.SubscriberId,
                                    SubscriberUniqueId = Subscriber.SubscriberUniqueId,
                                    VirtualServerId = Subscriber.VirtualServerId,
                                };

                                Core.ServerQueryConnection.ServerQueryConnection ServerQueryConnection = new Core.ServerQueryConnection.ServerQueryConnection( subscriberUserCredentials);

                                Thread thread = new Thread((ThreadStart)new SynchronizationCallback(ServerQueryConnection.InitializeQuiet));
                                thread.Start();

                                thread.Join();

                                //do
                                //{
                                //    Thread.Sleep(5);
                                //} while (t.ThreadState == ThreadState.Running);

                                try
                                {
                                    if (ServerQueryConnection.AsyncTcpDispatcher.IsConnected)
                                    {
                                        var test = ServerQueryConnection.whoAmI;

                                        if (test == null)
                                            throw new Exception("Login failed");
                                        if (test.IsErroneous)
                                            throw new Exception(test.ResponseText);
                                        else
                                        {
                                            db.ServerQueryUser.Add(sqUser);
                                            db.SaveChanges();

                                            ServerQueryConnection.QueryRunner.Logout();
                                            ServerQueryConnection.AsyncTcpDispatcher.Disconnect();

                                            //User successfully registered
                                            ServerQueryConnection.TextReply(e, "Successfully registered! You can now execute serverquery commands directly to me based on your user permissions.");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Debug(ex, "User sent invalid command: {0}");

                    ServerQueryConnection.TextReply(e, "Error, please check your command and try again.");
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
                            var User = db.Users.Single(usr => usr.UserTeamSpeakClientUniqueId == e.InvokerUniqueId && usr.SubscriberUniqueId == Subscriber.SubscriberUniqueId);
                            serverQueryUser = db.ServerQueryUser.Single(sqUser => sqUser.Users.UserId == User.UserId);
                        }
                    }
                    catch (Exception ex) when (ex.Message == "Sequence contains no elements")
                    {
                        ServerQueryConnection.TextReply(e, "You do not have access to this command.");
                        return;
                    }
                    catch (Exception)
                    {
                        ServerQueryConnection.TextReply(e, "There was an error performing this command.");
                        return;
                    }

                    try
                    {
                        string result = SendServerQueryCommand(cmd, serverQueryUser);

                        //string result = SendServerQueryCommand(cmd, serverQueryUser);
                        this.ServerQueryConnection.QueryRunner.SendTextMessage(MessageTarget.Client, e.InvokerClientId, result);
                    }
                    catch (Exception)
                    {
                        ServerQueryConnection.TextReply(e, "There was an error performing this command, or you lacked permissions.");
                        return;
                    }

                    break;
                }
            }
        }

        private string SendServerQueryCommand(Command cmd, Models.ServerQueryUser user)
        {
            Lyralei.Core.ServerQueryConnection.Models.Subscribers subscriberUserCredentials = new Lyralei.Core.ServerQueryConnection.Models.Subscribers()
            {
                AdminPassword = user.ServerQueryPassword,
                AdminUsername = user.ServerQueryUsername,
                ServerIp = Subscriber.ServerIp,
                ServerPort = Subscriber.ServerPort,
                SubscriberId = Subscriber.SubscriberId,
                SubscriberUniqueId = Subscriber.SubscriberUniqueId,
                VirtualServerId = Subscriber.VirtualServerId,
            };

            using (Core.ServerQueryConnection.ServerQueryConnection ServerQueryConnection = new Core.ServerQueryConnection.ServerQueryConnection(subscriberUserCredentials))
            {
                Thread thread = new Thread((ThreadStart)new SynchronizationCallback(ServerQueryConnection.InitializeQuiet));
                thread.Start();
                thread.Join();

                var result = ServerQueryConnection.QueryRunner.SendCommand(cmd);

                return result;
            }
        }
    }
}
