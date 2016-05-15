using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Core.CoreManager
{
    public class CoreDependencies : List<string>
    {
        public bool GetEverything;

        public CoreDependencies()
        {
            GetEverything = false;
        }
    }
}
