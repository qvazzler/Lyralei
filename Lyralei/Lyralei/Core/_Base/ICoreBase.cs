using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Core.Base
{
    public interface ICoreBase
    {
        CoreManager.CoreDependencies CoreDependencies { get; set; }
        bool IsInitialized { get; set; }
    }
}
