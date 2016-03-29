using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Fluent;

namespace Lyralei.Addons.Base
{
    public class AddonDependencyManager
    {
        private List<IAddon> LoadedAddons = new List<IAddon>();
        private List<string> RequestedAddons = new List<string>();

        public delegate void InjectionRequest(object sender, List<string> RequestedAddons);
        public event InjectionRequest injectionRequest;

        public Models.Subscribers subscriber;

        public AddonDependencyManager(Models.Subscribers subscriber)
        {
            this.subscriber = subscriber;
        }

        private Logger logger = LogManager.GetCurrentClassLogger();

        public void InjectDependency(IAddon Addon)
        {
            LoadedAddons.Add(Addon);

            logger.Debug()
                .Message("Addon injected: {0}", Addon.AddonName)
                .Property("subscriber", subscriber.ToString())
                .Write();
        }

        public void AddDependencyRequirement(string AddonName, bool raiseInjectionRequest = false)
        {
            if (RequestedAddons.Exists(AddonInList => AddonInList == AddonName))
            {
                logger.Debug()
                    .Message("Dependency requirement {0} already exists, ignoring..", AddonName)
                    .Property("subscriber", subscriber.ToString())
                    .Write();
            }
            else
            {
                RequestedAddons.Add(AddonName);

                logger.Debug()
                    .Message("Dependency requirement added: {0}", AddonName)
                    .Property("subscriber", subscriber.ToString())
                    .Write();
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
                if (!RequestedAddons.Exists(requestedAddon => requestedAddon == loadedAddon.AddonName))
                {
                    LoadedAddons.RemoveAll(addon => addon.AddonName == loadedAddon.AddonName);

                    logger.Debug()
                        .Message("Removing injected dependency as it is no longer required: {0}", loadedAddon.AddonName)
                        .Property("subscriber", subscriber.ToString())
                        .Write();
                }
            }

            // Then add needed injections
            foreach (string requestedAddon in RequestedAddons)
            {
                if (!LoadedAddons.Exists(loadedAddon => loadedAddon.AddonName == requestedAddon))
                    NeededAddonInjections.Add(requestedAddon);
            }

            // Finally raise event and hope someone listens
            if (injectionRequest != null)
            {
                logger.Debug()
                    .Message("Raising injection request for following addons: {0}", String.Join(Environment.NewLine, NeededAddonInjections))
                    .Property("subscriber", subscriber.ToString())
                    .Write();

                injectionRequest.Invoke(this, NeededAddonInjections);
            }
            else
            {
                logger.Warn()
                    .Message("Unmonitored injection request for {0} new dependencies", NeededAddonInjections.Count)
                    .Property("subscriber", subscriber.ToString())
                    .Write();
            }
        }

        public List<string> GetRequestedDependencies()
        {
            return RequestedAddons;
        }

        public IAddon GetAddon(string AddonName)
        {
            try
            {
                var result = LoadedAddons.Single(addon => addon.AddonName == AddonName);

                logger.Debug()
                    .Message("Addon reference returned: {0}", AddonName)
                    .Property("subscriber", subscriber.ToString())
                    .Write();

                return result;
            }
            catch (InvalidOperationException ex)
            {
                logger.Warn()
                    .Message("Addon reference not found: {0}", AddonName)
                    .Property("subscriber", subscriber.ToString())
                    .Write();
                throw ex;
            }
            catch (ArgumentNullException ex)
            {
                logger.Warn()
                    .Message("AddonDependencyManager not loaded, failed when requesting Addon: {0}", AddonName)
                    .Property("subscriber", subscriber.ToString())
                    .Write();
                throw ex;
            }
        }
    }
}
