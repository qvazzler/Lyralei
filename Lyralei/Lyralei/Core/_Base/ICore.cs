using Lyralei.Bot;
using Lyralei.TS3_Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Core.Base
{
    public interface ICore : IComponentBase
    {
        void UserInitialize(CoreList CoreList);
        //string Name { get; set; }

        // KEEP
        CommandRuleSets DefineCommandSchemas();
    }
}
