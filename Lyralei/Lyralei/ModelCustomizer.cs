using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyralei
{
    public static class ModelCustomizer
    {
        public static List<Action<ModelBuilder>> _modelCustomization = new List<Action<ModelBuilder>>();

        public static void AddModelCustomization(Action<ModelBuilder> modelCustomization)
        {
            _modelCustomization.Add(modelCustomization);
        }
    }
}
