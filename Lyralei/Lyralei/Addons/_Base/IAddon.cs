using Lyralei.Bot;
using Lyralei.TS3_Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Addons.Base
{
    public interface IAddon : IComponentBase
    {
        void UserInitialize(Bot.ComponentList ComponentList);
        //string Name { get; set; }

        // KEEP
        CommandRuleSets DefineCommandSchemas();
    }
}
