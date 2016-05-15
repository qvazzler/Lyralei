using Lyralei.TS3_Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Bot
{
    public interface IComponent : IComponentBase
    {
        void UserInitialize(Core.CoreList CoreList);

        // KEEP
        CommandRuleSets DefineCommandSchemas();
    }
}
