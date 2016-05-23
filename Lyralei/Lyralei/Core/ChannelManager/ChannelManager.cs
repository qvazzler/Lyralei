using System.Linq;

using Lyralei.Core.UserManager.Models;
using Lyralei.Core.ServerQueryConnection.Models;
using Lyralei.TS3_Objects.Entities;
using System;
using System.Collections.Generic;
using TS3QueryLib.Core.Common;
using Lyralei.TS3_Objects.EventArguments;
using TS3QueryLib.Core.Server.Entities;

namespace Lyralei.Core.ChannelManager
{
    public class ChannelManager : Base.CoreBase, Base.ICore
    {
        ServerQueryConnection.ServerQueryConnection ServerQueryConnection;

        #region Standards
        public ChannelManager(Subscribers Subscriber) : base(Subscriber)
        {
            CoreDependencies.Add(typeof(ServerQueryConnection.ServerQueryConnection).Name);
        }

        public void UserInitialize(CoreList CoreList)
        {
            ServerQueryConnection = (ServerQueryConnection.ServerQueryConnection)CoreList[typeof(ServerQueryConnection.ServerQueryConnection).Name];

            //TODO: Add permission list from ts3 somehow
            //var PermissionsList = ServerQueryConnection.QueryRunner.GetPermissionList().ToList();
        }

        public CommandRuleSets DefineCommandSchemas()
        {
            CommandRuleSets ruleSets = new CommandRuleSets();
            CommandParameterGroupListWithRules cmds = new CommandParameterGroupListWithRules();

            CommandParameterGroupWithRules cmdCool = new CommandParameterGroupWithRules();
            cmdCool.Add(new CommandParameterWithRules("sort")
            {
                IsBaseCommand = true
            });
            //cmdCool.Add(new CommandParameterWithRules("someparam")
            //{
            //    NameValueSetting = NameValueSetting.ValueOrValueAndName,
            //    ValueType = TS3_Objects.Entities.ValueType.Integer,
            //});
            cmds.Add(cmdCool);
            ruleSets.Add(new CommandRuleSet(this.Name, cmds, UserSortRequest));

            return ruleSets;
        }
        #endregion

        #region Designation CRUD
        public void AssignDesignation(Users User, string DesignationName, int ChannelId)
        {
            using (var db = new CoreContext())
            {
                Models.ChannelDesignations designation = new Models.ChannelDesignations()
                {
                    ChannelId = ChannelId,
                    DesignatedByUserId = User.UserId,
                    DesignationName = DesignationName,
                    SubscriberId = this.Subscriber.SubscriberId,
                    SubscriberUniqueId = this.Subscriber.SubscriberUniqueId
                };

                db.ChannelDesignations.Add(designation);
                db.SaveChanges();
            }
        }

        public void DeleteDesignation(string DesignationName, int ChannelId)
        {
            using (var db = new CoreContext())
            {
                var designation = db.ChannelDesignations.SingleOrDefault(x => x.SubscriberId == this.Subscriber.SubscriberId && x.DesignationName == DesignationName);

                if (designation != null)
                {
                    db.ChannelDesignations.Remove(designation);
                    db.SaveChanges();
                }
            }
        }

        public List<string> GetChannelDesignations(string DesignationName, int ChannelId)
        {
            using (var db = new CoreContext())
            {
                var permSearch = db.ChannelDesignations.Where(x => x.SubscriberId == this.Subscriber.SubscriberId && x.ChannelId == ChannelId).Select(x => x.DesignationName).ToList();
                return permSearch;
            }
        }

        public void ClearDesignations(string DesignationName, int ChannelId)
        {
            using (var db = new CoreContext())
            {
                var designations = db.ChannelDesignations.Where(x => x.SubscriberId == this.Subscriber.SubscriberId && x.ChannelId == ChannelId);

                if (designations != null)
                {
                    foreach (var designation in designations)
                    {
                        db.ChannelDesignations.Remove(designation);
                    }

                    db.SaveChanges();
                }

            }
        }
        #endregion

        #region Command Events

        private void UserSortRequest(BotCommandEventArgs obj)
        {
            //No parameters, just need to call sort and assume the channel the user is in.
            var clientInfo = ServerQueryConnection.QueryRunner.GetClientInfo(obj.MessageInfo.InvokerClientId);
            SortSubChannels((int)clientInfo.ChannelId);
        }

        #endregion

        public void SortSubChannels(int ParentChannelId)
        {
            List<Props.Channel> channels = new List<Props.Channel>();
            List<ChannelListEntry> channelEntries = new List<ChannelListEntry>();

            var tsChannels = ServerQueryConnection.QueryRunner.GetChannelList();

            foreach (var tsChannel in tsChannels)
            {
                if (tsChannel.ParentChannelId == ParentChannelId)
                {
                    channelEntries.Add(tsChannel);
                }
            }

            channelEntries.Sort(new Utils.ChannelListEntryOrderComparer());

            List<ChannelModification> channelModifications = new List<ChannelModification>();

            for (int i = 0; i < channelEntries.Count; i++)
            {
                channelModifications.Add(new ChannelModification());

                if (i == 0)
                    channelModifications[i].ChannelOrder = 0;
                else
                    channelModifications[i].ChannelOrder = channelEntries[i - 1].ChannelId;
            }

            for (int j = 0; j < channelEntries.Count; j++)
                ServerQueryConnection.QueryRunner.EditChannel(channelEntries[j].ChannelId, channelModifications[j]);
        }
    }
}
