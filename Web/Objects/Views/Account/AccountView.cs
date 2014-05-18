using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class AccountView : BaseView
    {
        [Required]
        public String Username { get; set; }

        [Required]
        public String Password { get; set; } // TODO: Create login view, so that required attribute can be removed

        public String RoleId { get; set; }
        public String RoleName { get; set; }
    }
}
