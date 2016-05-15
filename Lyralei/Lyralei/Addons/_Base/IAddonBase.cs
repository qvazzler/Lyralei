using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Addons.Base
{
    public interface IAddonBase
    {
        Core.CoreManager.CoreDependencies CoreDependencies { get; set; }
        List<string> AddonDependencies { get; set; }
        bool IsInitialized { get; set; }
    }
}
