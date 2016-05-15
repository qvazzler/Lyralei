using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Bot
{
    public class ComponentList : List<IComponentBase>
    {
        public ComponentList() : base()
        {

        }

        public ComponentList(IEnumerable<IComponentBase> collection) : base(collection)
        {

        }

        public IComponentBase this[string index]
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
