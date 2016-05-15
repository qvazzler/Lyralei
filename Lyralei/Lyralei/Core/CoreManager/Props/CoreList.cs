using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyralei.Core.Base;

namespace Lyralei.Core
{
    public class CoreList : List<Base.ICore>
    {
        public CoreList(IEnumerable<ICore> collection) : base(collection)
        {

        }

        public Base.ICore this[string index]
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
