using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Lyralei.Addons.Base
{
    public class AddonDependencyManager
    {
        private List<IAddon> LoadedAddons = new List<IAddon>();
        private List<string> RequestedAddons = new List<string>();

        public delegate void InjectionRequest(object sender, List<string> RequestedAddons);
        public event InjectionRequest injectionRequest;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public void InjectDependency(IAddon Addon)
        {
            LoadedAddons.Add(Addon);
            logger.Debug("Addon injected: {0}", Addon.AddonName);
        }

        public void AddDependencyRequirement(string AddonName, bool raiseInjectionRequest = false)
        {
            if (RequestedAddons.Exists(AddonInList => AddonInList == AddonName))
            {
                logger.Debug(String.Format("Dependency requirement {0} already exists, ignoring..", AddonName));
            }
            else
            {
                RequestedAddons.Add(AddonName);
                logger.Debug(String.Format("Dependency requirement added: {0}", AddonName));
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
                    logger.Debug("Removing injected dependency as it is no longer required: {0}", loadedAddon.AddonName);
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
                logger.Debug("Raising injection request for following addons: {0}", String.Join(Environment.NewLine, NeededAddonInjections));
                injectionRequest.Invoke(this, NeededAddonInjections);
            }
            else
            {
                logger.Warn(String.Format("Unmonitored injection request for {0} new dependencies", NeededAddonInjections.Count));
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
                logger.Debug(String.Format("Addon reference returned: {0}", AddonName));

                return result;
            }
            catch (InvalidOperationException ex)
            {
                logger.Warn(String.Format("Addon reference not found: {0}", AddonName));
                throw ex;
            }
            catch (ArgumentNullException ex)
            {
                logger.Warn(String.Format("AddonDependencyManager not loaded, failed when requesting Addon: {0}", AddonName));
                throw ex;
            }
        }
    }
}
