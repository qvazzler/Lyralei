using Lyralei.Addons.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lyralei.Addons
{
    public class AddonManager
    {
        public List<Addon> addons;

        public AddonManager()
        {
            addons = new List<Addon>();
            addons.Add(new TestAddon());
        }
    }
}
