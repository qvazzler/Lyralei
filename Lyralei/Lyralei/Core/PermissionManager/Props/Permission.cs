using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei.Core.PermissionManager.Props
{
    public class Permission
    {
        private string theType;
        private int? theValue;

        public Permission(string Name, int? Value)
        {
            theType = Name;
            theValue = Value;
        }

        public Permission(string Name, bool Value)
        {
            theType = Name;
            theValue = (Nullable<int>)Convert.ToInt32(Value);
        }

        public string Name()
        {
            return this.theType;
        }

        public int? Value()
        {
            return this.theValue;
        }
    }
}
