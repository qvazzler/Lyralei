using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lyralei.Addons.Test
{
    public class TestDataModel
    {
        [Key]
        public int SomeAddonDataId { get; set; }
        public int OtherImportantNumber { get; set; }
        public string RequiredDataHere { get; set; }
    }
}
