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
        public String PersonId { get; set; }

        public virtual Person Person { get; set; }
    }
}
