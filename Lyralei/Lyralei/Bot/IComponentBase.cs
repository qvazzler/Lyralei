using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Bot
{
    public interface IComponentBase
    {
        Core.CoreManager.CoreDependencies CoreDependencies { get; set; }
        Core.AddonManager.AddonDependencies AddonDependencies { get; set; }
        bool IsInitialized { get; set; }
        string Name { get; set; }
    }
}
