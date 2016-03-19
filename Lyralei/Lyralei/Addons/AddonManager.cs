using Lyralei.Addons.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lyralei.Addons
{
    public class AddonManager
    {
        public List<AddonBase> addons;

        public AddonManager()
        {
            addons = new List<AddonBase>();
            addons.Add(new TestAddon());
            addons.Add(new ServerQueryAddon());
        }
    }
}
