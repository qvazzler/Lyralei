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
using Lyralei.TS3_Objects.Entities;
using Lyralei.TS3_Objects.EventArguments;
using Lyralei.Core;
using Lyralei.Core.ServerQueryConnection.Models;

namespace Lyralei.Core.InputOwner
{
    class InputOwnerAddon : Core.Base.CoreBase, Core.Base.ICore
    {
        ServerQueryConnection.ServerQueryConnection ServerQueryConnection;
        UserManager.UserManager UserManager;

        public delegate bool AddonOwnershipChanged(object sender, EventArguments.InputOwnerEventArgs e);
        public event AddonOwnershipChanged onAddonOwnershipChanged;

        public delegate bool AddonOwnershipReleaseRequest(object sender, EventArguments.InputOwnerEventArgs e);
        public event AddonOwnershipReleaseRequest onAddonOwnershipReleaseRequest;

        public delegate bool AddonOwnershipRevoked(object sender, EventArguments.InputOwnerEventArgs e);
        public event AddonOwnershipRevoked onAddonOwnershipRevoked;

        public delegate bool AddonInputReceived(object sender, EventArguments.InputDetailsEventArgs e);
        public event AddonInputReceived onInputReceived;

        //private Test.TestAddon testAddon;

        public InputOwnerAddon(Subscribers Subscriber) : base(Subscriber)
        {
            this.Name = this.GetType().Name;
            this.CoreDependencies.Add(typeof(ServerQueryConnection.ServerQueryConnection).Name);
            this.CoreDependencies.Add(typeof(UserManager.UserManager).Name);
        }

        public void UserInitialize(CoreList AddonInjections)
        {
            this.ServerQueryConnection = AddonInjections[typeof(ServerQueryConnection.ServerQueryConnection).Name] as ServerQueryConnection.ServerQueryConnection;
            this.UserManager = AddonInjections[typeof(UserManager.UserManager).Name] as UserManager.UserManager;

            this.ServerQueryConnection.QueryRunner.Notifications.ClientMessageReceived += Notifications_ClientMessageReceived;

            ModelCustomizer.AddModelCustomization(Hooks.ModelCustomizer.OnModelCreating);

            //logger.Info("I have been initialized yo!");
        }

        public CommandRuleSets DefineCommandSchemas()
        {
            return null;
        }

        public void RequestInput(IComponent Component, Core.UserManager.Models.Users user, int? inputWaitDuration = null, Props.ReleaseRequestAction releaseRequestAction = Props.ReleaseRequestAction.Ask, Props.QueuePosition queuePosition = Props.QueuePosition.Last)
        {
            TimeSpan? ts = null;
            if (inputWaitDuration != null)
                ts = TimeSpan.FromSeconds(Convert.ToInt32(inputWaitDuration));

            Models.InputOwners inputOwner = new Models.InputOwners()
            {
                Name = Component.Name,
                ReleaseRequestAction = releaseRequestAction,
                QueuePosition = queuePosition,
                InputWaitDuration = ts,
                UserId = user.UserId,
                Users = user,
            };

            using (var db = new CoreContext())
            {
                db.InputOwners.Add(inputOwner);
            }

            UpdateInputQueue(user.SubscriberId);
        }

        private List<Models.InputOwners> GetOrderedAddonsInQueue(int? subscriberId, int? clientId, Props.QueuePosition queuePosition)
        {
            List<Models.InputOwners> inputOwners = new List<Models.InputOwners>();

            using (var db = new CoreContext())
            {
                var owners = db.InputOwners.Where(x => x.QueuePosition == queuePosition && x.HasOwnership == false);

                if (subscriberId != null)
                    owners = owners.Where(y => y.Users.SubscriberId == subscriberId);
                if (clientId != null)
                    owners = owners.Where(y => y.UserId == clientId);

                owners = owners = owners.OrderBy(x => x.Created);

                return owners.ToList();
            }
        }

        private void UpdateInputQueue()
        {
            UpdateInputQueue(null, null);
        }

        private void UpdateInputQueue(int? subscriberId)
        {
            UpdateInputQueue(subscriberId, null);
        }

        private void UpdateInputQueue(int? subscriberId, int? clientId)
        {
            using (var db = new CoreContext())
            {
                var AddonThatOwnsInput = db.InputOwners.SingleOrDefault(x => x.HasOwnership);
                Models.InputOwners AddonThatWillOwnInput = null;

                var interruptRequests = GetOrderedAddonsInQueue(subscriberId, clientId, Props.QueuePosition.TryInterruptCurrent);
                var firstRequests = GetOrderedAddonsInQueue(subscriberId, clientId, Props.QueuePosition.First);
                var lastRequests = GetOrderedAddonsInQueue(subscriberId, clientId, Props.QueuePosition.Last);
                var noqueueRequests = GetOrderedAddonsInQueue(subscriberId, clientId, Props.QueuePosition.NotQueueing);

                if (interruptRequests.Count() > 0) { AddonThatWillOwnInput = interruptRequests.First(); }
                else if (firstRequests.Count() > 0) { AddonThatWillOwnInput = firstRequests.First(); }
                else if (lastRequests.Count() > 0) { AddonThatWillOwnInput = lastRequests.First(); }
                else if (noqueueRequests.Count() > 0) { AddonThatWillOwnInput = noqueueRequests.First(); }
                else { /* nobody to give input to, let's */ return; }

                if (AddonThatOwnsInput == null)
                {
                    //Sure, have the input
                    AddonThatOwnsInput = AddonThatWillOwnInput;

                    db.SaveChanges();

                    NotifyAddonOwnershipChanged(AddonThatOwnsInput);
                }
                else if (AddonThatOwnsInput.ReleaseRequestAction == Props.ReleaseRequestAction.Ask)
                {
                    if (NotifyAddonInputReleaseRequest(AddonThatOwnsInput))
                    {
                        AddonThatOwnsInput = AddonThatWillOwnInput;

                        db.SaveChanges();

                        NotifyAddonOwnershipChanged(AddonThatOwnsInput);
                    }
                }
                else if (AddonThatOwnsInput.ReleaseRequestAction == Props.ReleaseRequestAction.Allow)
                {
                    var AddonThatOwnedInput = AddonThatOwnsInput;

                    AddonThatOwnsInput = AddonThatWillOwnInput;

                    db.SaveChanges();

                    NotifyAddonInputOwnershipRevoked(AddonThatOwnedInput);
                    NotifyAddonOwnershipChanged(AddonThatOwnsInput);
                }
                else
                {
                    return;
                }
            }
        }

        private void NotifyAddonOwnershipChanged(Models.InputOwners InputOwnerAddon)
        {
            onAddonOwnershipChanged.Invoke(this, new EventArguments.InputOwnerEventArgs(InputOwnerAddon));
        }

        private bool NotifyAddonInputReleaseRequest(Models.InputOwners InputOwnerAddon)
        {
            if (onAddonOwnershipReleaseRequest.Invoke(this, new EventArguments.InputOwnerEventArgs(InputOwnerAddon)))
                return true;
            else
                return false;
        }

        private void NotifyAddonInputOwnershipRevoked(Models.InputOwners InputOwnerAddon)
        {
            onAddonOwnershipRevoked.Invoke(this, new EventArguments.InputOwnerEventArgs(InputOwnerAddon));
        }

        private void Notifications_ClientMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            using (var db = new CoreContext())
            {
                var user = UserManager.QueryUser(this.Subscriber.SubscriberId, this.Subscriber.SubscriberUniqueId, e.InvokerUniqueId);

                var AddonThatOwnsInput = db.InputOwners.SingleOrDefault(x => x.UserId == user.UserId && x.HasOwnership);

                if (AddonThatOwnsInput != null)
                {
                    onInputReceived.Invoke(this, new EventArguments.InputDetailsEventArgs(AddonThatOwnsInput, e));

                    if (AddonThatOwnsInput.SingleInputAndRelease)
                    {
                        // Addon only needed one input, do we release immediately
                        db.Remove(AddonThatOwnsInput);
                        db.SaveChanges();

                        // Update queue to let next one get input
                        UpdateInputQueue();
                    }
                }
            }
        }
    }
}
