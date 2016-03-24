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
using System.Threading;
using System.Threading.Tasks;

namespace SquallBot
{
    //The Addon base class assigns all the variables. This saves some space for creating multiple addon classes.
    abstract public class Addon
    {
        public List<string> eventDumpStrings; //This object is used to keep track of double-firing events in channel-related events.. Messy but works
        public ServerInfo serverinfo;
        public MySQLInstance sql;
        public ServerQueryInstance serverq;
        public QueryRunner queryRunner;
        public AsyncTcpDispatcher atd;

        public Addon(MySQLInstance _sql, ServerQueryInstance _serverq, ServerInfo _serverinfo)
        {
            sql = _sql;
            serverq = _serverq;
            queryRunner = serverq.queryRunner;
            atd = serverq.atd;
            serverinfo = _serverinfo;
            eventDumpStrings = new List<string>();
        }
    }
}
