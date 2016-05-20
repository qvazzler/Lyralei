using Lyralei.Addons.Base;
using Lyralei.Bot;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Core.Base
{
    public class CoreBase : IComponentBase
    {
        internal Logger logger;
        public string Name { get; set; }
        public CoreManager.CoreDependencies CoreDependencies { get; set; }
        public AddonManager.AddonDependencies AddonDependencies { get; set; }
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

        public CoreBase(Core.ServerQueryConnection.Models.Subscribers Subscriber)
        {
            this.Name = this.GetType().Name;

            CoreDependencies = new CoreManager.CoreDependencies();
            AddonDependencies = new AddonManager.AddonDependencies();
            this.Subscriber = Subscriber;
        }

        public void Initialize(CoreList CoreInjections)
        {
            try
            {
                var core = this as ICore;
                core.UserInitialize(CoreInjections);

                this.IsInitialized = true;
                logger.Debug(this.Name + " initialized successfully.");
            }
            catch (Exception)
            {

            }
        }
    }
}
