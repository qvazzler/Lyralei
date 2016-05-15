using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

// Reference to main models:  Lyralei.Models.SomeModel
// Reference to addon models: Models.SomeModel

namespace Lyralei.Addons.TestAddon.Models
{
    public class TestAddonUser
    {
        // Need relationship advice.. On Entity Framework 7 ?
        // http://ef.readthedocs.org/en/latest/modeling/relationships.html

        //[Key] //redundant, as long as the property is named either TestUserId or just Id
        public int TestUserId { get; set; }
        
        public string SomeTestInformation { get; set; }

        [Required]
        public string RequiredFieldHere { get; set; }

        //One-way relationship with Users model
        public int UserId { get; set; } // foreign key
        public Core.UserManager.Models.Users Users { get; set; }

        // REDUNDANT
        //public int InfoFromUsersModel { get; set; } //Remember, you don't need to duplicate data if you use real relationships
    }
}
