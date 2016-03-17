using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Server.Notification.EventArgs;

namespace Lyralei
{
    class MessageReceivedEventArgsTest : MessageReceivedEventArgs
    {
        MessageReceivedEventArgsTest(string source) : base(CommandParameterGroupList.Parse(source))
        {

        }

        //void Parse()
        //{
              //Editing variables from here is possible
        //    this.InvokerNickname = "test";
        //}
    }
}
