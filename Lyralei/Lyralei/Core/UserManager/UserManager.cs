using System.Linq;

using Lyralei.Core.UserManager.Models;
using Lyralei.Core.ServerQueryConnection.Models;
using Lyralei.TS3_Objects.Entities;
using System;

namespace Lyralei.Core.UserManager
{
    public class UserManager : Base.CoreBase, Base.ICore
    {
        int SubscriberId;

        public UserManager(Subscribers Subscriber) : base(Subscriber)
        {
            this.Name = this.GetType().Name;
            this.SubscriberId = this.Subscriber.SubscriberId;
        }

        //public void CreateUser(int subscriberId, string subscriberUniqueId, string userTeamSpeakClientUniqueId)
        //{
        //    CreateUser(subscriberId, subscriberUniqueId, userTeamSpeakClientUniqueId, null, null);
        //}

        public void UserInitialize(CoreList CoreList)
        {
            
        }

        public Users QueryUser(int subscriberId, string subscriberUniqueId, string userTeamSpeakClientUniqueId/*, string serverQueryUsername, string serverQueryPassword*/)
        {
            using (var db = new CoreContext())
            {
                var usersearch = db.Users.SingleOrDefault(x => x.UserTeamSpeakClientUniqueId == userTeamSpeakClientUniqueId && x.SubscriberUniqueId == subscriberUniqueId);

                if (usersearch != null)
                    return usersearch;

                Users newuser = new Users()
                {
                    SubscriberId = subscriberId,
                    SubscriberUniqueId = subscriberUniqueId,
                    UserTeamSpeakClientUniqueId = userTeamSpeakClientUniqueId,
                };                

                db.Add(newuser);
                db.SaveChanges();

                return newuser;
            }
        }

        public Users GetUser(int botUserId)
        {
            using (var db = new CoreContext())
            {
                return (db.Users.Single(x => x.UserId == botUserId));
            }
        }

        public Users GetUser(string userUniqueId)
        {
            using (var db = new CoreContext())
            {
                return (db.Users.Single(usr => usr.SubscriberId == SubscriberId && usr.UserTeamSpeakClientUniqueId == userUniqueId));
            }
        }

        public CommandRuleSets DefineCommandSchemas()
        {
            return null;
        }
    }
}
