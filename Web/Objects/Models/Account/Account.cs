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

        [Required]
        public String UserId { get; set; }

        public virtual User User { get; set; }
    }
}
