using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AccountView : BaseView
    {
        [Required]
        public String Username { get; set; }

        [Required]
        public String Password { get; set; }

        [Required]
        [EmailAddress]
        public String Email { get; set; }

        public String RoleId { get; set; }
        public String RoleName { get; set; }
    }
}
