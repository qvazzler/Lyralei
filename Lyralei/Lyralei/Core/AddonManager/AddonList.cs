using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyralei.Core.Base;

namespace Lyralei.Core
{
    public class AddonList : List<Addons.Base.IAddon>
    {
        public AddonList() : base()
        {

        }

        public AddonList(IEnumerable<Addons.Base.IAddon> collection) : base(collection)
        {

        }

        public Addons.Base.IAddon this[string index]
        {
            get
            {
                return this.SingleOrDefault(x => x.Name == index);
            }
            protected set
            {
                // NOPE
            }
        }
    }
}
