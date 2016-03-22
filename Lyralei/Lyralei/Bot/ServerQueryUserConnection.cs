using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;

using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Responses;
using TS3QueryLib.Core.Server.Entities;
using TS3QueryLib.Core.CommandHandling;
using System.Threading;
using TS3QueryLib.Core.Server.Notification.EventArgs;

using Lyralei.TS3_Objects.EventArguments;
using Lyralei.TS3_Objects.Entities;
using Lyralei.Addons.Base;
using Lyralei.Addons;
using Lyralei.Models;

namespace Lyralei.Bot
{
    public class ServerQueryUserConnection : ServerQueryBaseConnection
    {
        public ServerQueryUserConnection(Models.Subscribers _subscriber, Lyralei.Addons.ServerQuery.ServerQueryUserDetails _squser, bool autoconnect = false) : base(_subscriber, _squser, autoconnect)
        {

        }

        public ServerQueryUserConnection() : base()
        {

        }
    }
}
