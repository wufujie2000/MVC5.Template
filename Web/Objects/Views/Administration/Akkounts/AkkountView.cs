using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{ 
    public class AkkountView : BaseView
    {
        [Required]
        public String Username { get; set; }
        public String Password { get; set; }

        public String RoleId { get; set; }
        public String RoleName { get; set; }
    }
}
