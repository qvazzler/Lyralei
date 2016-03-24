using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyralei.Models;

namespace Lyralei.Bot
{
    public class UserManager
    {
        int SubscriberId;

        public UserManager(int SubscriberId)
        {
            this.SubscriberId = SubscriberId;
        }

        //public void CreateUser(int subscriberId, string subscriberUniqueId, string userTeamSpeakClientUniqueId)
        //{
        //    CreateUser(subscriberId, subscriberUniqueId, userTeamSpeakClientUniqueId, null, null);
        //}

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
    }
}
