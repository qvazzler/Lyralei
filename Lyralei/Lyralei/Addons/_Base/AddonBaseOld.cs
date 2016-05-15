using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;

using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Responses;
using TS3QueryLib.Core.Server.Entities;
using TS3QueryLib.Core.Server.Notification.EventArgs;
using System.Threading;
using System.Threading.Tasks;
using Lyralei.Bot;
using Lyralei.TS3_Objects.EventArguments;

using NLog;

namespace Lyralei.Addons.Base
{
    // The Addon base class assigns all the variables and stuff.
    abstract public class AddonBaseOld : IAddonBaseOld
    {
        protected Logger logger;

        // Addon dependencies
        //public AddonDependencyManager dependencyManager { get; set; }

        public Core.ServerQueryConnection.ServerQueryConnection ServerQueryConnection;

        private Core.ServerQueryConnection.Models.Subscribers subscriber;
        public Core.ServerQueryConnection.Models.Subscribers Subscriber
        {
            get { return subscriber; }
            set
            {
                subscriber = value;
                logger = LogManager.GetLogger(this.GetType().Name + " - " + subscriber.ToString());
            }
        }

        // 'Shortcuts'
        //public QueryRunner queryRunner;
        //public AsyncTcpDispatcher atd;

        public AddonBaseOld()
        {
            logger = LogManager.GetLogger(this.GetType().Name);
        }

        public void Configure(Core.ServerQueryConnection.Models.Subscribers subscriber, Core.ServerQueryConnection.ServerQueryConnection ServerQueryConnection)
        {
            this.Subscriber = subscriber;

            //dependencyManager = new AddonDependencyManager(this.Subscriber);

            this.ServerQueryConnection = ServerQueryConnection;
        }

        public virtual void onClientMessage(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e/*, BotCommandInput input, BotCommandPreface preface*/)
        {

        }

        public void TextReply(MessageReceivedEventArgs messageInfo, string v)
        {
            //TODO: Remove stub
            throw new NotImplementedException();
        }

        //private void Notifications_UnknownNotificationReceived(object sender, TS3QueryLib.Core.Common.EventArgs<string> e)
        //{

        //}

        #region Dependency Management

        private List<IAddon> LoadedAddons = new List<IAddon>();
        private List<string> RequestedAddons = new List<string>();

        //public delegate void InjectionRequest(object sender, List<string> RequestedAddons);
        public event EventHandler<List<string>> InjectionRequest;
        //public event EventHandler InjectionRequest2;

        public void InjectDependency(IAddon Addon)
        {
            LoadedAddons.Add(Addon);

            //logger.Debug("Addon injected: {0}", Addon.CoreName);
        }

        public void AddDependencyRequirement(string CoreName, bool raiseInjectionRequest = false)
        {
            if (RequestedAddons.Exists(AddonInList => AddonInList == CoreName))
            {
                //logger.Debug("Dependency requirement {0} by {1} already exists, ignoring..", CoreName);
            }
            else
            {
                RequestedAddons.Add(CoreName);
            }

            if (raiseInjectionRequest)
                UpdateInjections();
        }

        public void UpdateInjections()
        {
            List<string> NeededAddonInjections = new List<string>();

            // First remove any unwanted injections
            foreach (IAddon loadedAddon in LoadedAddons)
            {
                if (!RequestedAddons.Exists(requestedAddon => requestedAddon == loadedAddon.CoreName))
                {
                    LoadedAddons.RemoveAll(addon => addon.CoreName == loadedAddon.CoreName);

                    logger.Debug("Removing injected dependency as it is no longer required: {0}", loadedAddon.CoreName);
                }
            }

            // Then add needed injections
            foreach (string requestedAddon in RequestedAddons)
            {
                if (!LoadedAddons.Exists(loadedAddon => loadedAddon.CoreName == requestedAddon))
                    NeededAddonInjections.Add(requestedAddon);
            }

            // Finally raise event and hope someone listens
            if (InjectionRequest != null)
            {
                //logger.Debug("Raising injection request for following addons: {0}", String.Join(Environment.NewLine, NeededAddonInjections));

                InjectionRequest.Invoke(this, NeededAddonInjections);
            }
            else
            {
                logger.Warn("Unmonitored injection request for {0} new dependencies", NeededAddonInjections.Count);
            }
        }

        public List<string> GetRequestedDependencies()
        {
            return RequestedAddons;
        }

        public IAddon GetDependencyReference(string CoreName)
        {
            try
            {
                var result = LoadedAddons.Single(addon => addon.CoreName == CoreName);

                return result;
            }
            catch (InvalidOperationException ex)
            {
                logger.Warn("Addon reference not found: {0}", CoreName);
                throw ex;
            }
            catch (ArgumentNullException ex)
            {
                logger.Warn("AddonDependencyManager not loaded, failed when requesting Addon: {0}", CoreName);
                throw ex;
            }
        }

        #endregion

        public virtual void onServerMessage(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {

        }

        public virtual void onChannelMessage(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {

        }

        public virtual void onClientDisconnect(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientDisconnectEventArgs e)
        {

        }

        public virtual void onClientJoined(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientJoinedEventArgs e)
        {

        }

        public virtual void onClientKick(object sender, ClientKickEventArgs e)
        {

        }

        public virtual void onClientMovedByTemporaryChannelCreate(object sender, ClientMovedEventArgs e)
        {

        }

        public virtual void onClientMoveForced(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedByClientEventArgs e)
        {

        }

        public virtual void onClientMoved(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientMovedEventArgs e)
        {

        }

        public virtual void onClientBan(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientBanEventArgs e)
        {

        }

        public virtual void onClientConnectionLost(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.ClientConnectionLostEventArgs e)
        {

        }

        public virtual void onChannelCreated(object sender, ChannelCreatedEventArgs e)
        {

        }

        public virtual void onChannelMoved(object sender, ChannelMovedEventArgs e)
        {

        }

        public virtual void onChannelEdited(object sender, ChannelEditedEventArgs e)
        {

        }

        //public event EventHandler<MessageReceivedEventArgs> Sync_ChannelMessageReceived;
        //public event EventHandler<ClientBanEventArgs> Sync_ClientBan;
        //public event EventHandler<ClientConnectionLostEventArgs> Sync_ClientConnectionLost;
        //public event EventHandler<ClientDisconnectEventArgs> Sync_ClientDisconnect;
        //public event EventHandler<ClientJoinedEventArgs> Sync_ClientJoined;
        //public event EventHandler<ClientKickEventArgs> Sync_ClientKick;
        ////public event EventHandler<MessageReceivedEventArgs> Sync_ClientMessageReceived;
        //public event EventHandler<ClientMovedEventArgs> Sync_ClientMoved;
        //public event EventHandler<ClientMovedEventArgs> Sync_ClientMovedByTemporaryChannelCreate;
        //public event EventHandler<ClientMovedByClientEventArgs> Sync_ClientMoveForced;
        //public event EventHandler<MessageReceivedEventArgs> Sync_ServerMessageReceived;
        //public event EventHandler<ChannelCreatedEventArgs> Sync_ChannelCreated; //Custom event
    }
}
