using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class Account : BaseModel
    {
        [Required]
        public String Username { get; set; }
        
        [Required]
        public String Passhash { get; set; }

        public String RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
