using Lyralei.Addons.Base;
using Lyralei.Bot;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Addons.Base
{
    public class AddonBase : IComponentBase
    {
        internal Logger logger;
        public string Name { get; set; }
        public Core.CoreManager.CoreDependencies CoreDependencies { get; set; }
        public Core.AddonManager.AddonDependencies AddonDependencies { get; set; }
        public bool IsInitialized { get; set; } = false;

        private Core.ServerQueryConnection.Models.Subscribers subscriber;
        public Core.ServerQueryConnection.Models.Subscribers Subscriber
        {
            get { return subscriber; }
            set
            {
                subscriber = value;
                logger = LogManager.GetLogger(this.GetType().Name + " - " + Subscriber.ToString());
            }
        }

        //public AddonBase()
        //{
        //    logger = LogManager.GetLogger(this.GetType().Name);
        //}

        public AddonBase(Core.ServerQueryConnection.Models.Subscribers Subscriber)
        {
            CoreDependencies = new Core.CoreManager.CoreDependencies();
            AddonDependencies = new Core.AddonManager.AddonDependencies();
            this.Subscriber = Subscriber;
        }

        public void Initialize(Bot.ComponentList ComponentList)
        {
            try
            {
                var core = this as IAddon;
                core.UserInitialize(ComponentList);

                this.IsInitialized = true;
                logger.Debug(this.Name + " initialized successfully.");
            }
            catch (Exception)
            {

            }
        }
    }
}
