using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class Account : BaseModel
    {
        [Required]
        public String Username { get; set; }

        [Required]
        public String Passhash { get; set; }

        [Required]
        [EmailAddress]
        public String Email { get; set; }

        public String RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
