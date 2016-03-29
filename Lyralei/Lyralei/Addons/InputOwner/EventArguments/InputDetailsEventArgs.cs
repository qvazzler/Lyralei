using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lyralei.Addons.Base;

namespace Lyralei.Addons.InputOwner.EventArguments
{
    public class InputOwnerEventArgs : EventArgs
    {
        public Models.InputOwners InputOwner;

        public InputOwnerEventArgs(Models.InputOwners InputOwner)
        {
            this.InputOwner = InputOwner;
        }
    }
}
