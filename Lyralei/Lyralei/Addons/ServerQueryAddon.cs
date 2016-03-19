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

namespace Lyralei.Addons
{
    class ServerQueryAddon : AddonBase, IAddon 
    {
        public void Initialize()
        {
            this.serverQueryRootConnection.BotCommandReceived += onBotCommand;
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
                        var username = cmdPG.Single(cmdP => cmdP.Name.ToLower() == "username").Value;
                        var password = cmdPG.Single(cmdP => cmdP.Name.ToLower() == "password").Value;

                        var serverUniqueId = queryRunner.GetServerInfo().UniqueId;

                        var uniqueId = e.InvokerUniqueId;
                        var databaseId = queryRunner.GetClientDatabaseIdsByUniqueId(uniqueId);
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
                    Models.Users user = null;

                    try
                    {
                        user = userManager.GetUserInformation(e.InvokerUniqueId);
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
                        string result = SendServerQueryCommand(cmd, user);
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

        private string SendServerQueryCommand(Command cmd, Models.Users user)
        {
            using (ServerQueryUserConnection serverQueryUserConnection = new ServerQueryUserConnection(this.subscriber, user))
            {
                return serverQueryUserConnection.queryRunner.SendCommand(cmd);
            }
        }
    }
}
