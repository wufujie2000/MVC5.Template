using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class AccountView : BaseView
    {
        public String Username { get; set; }
        public String Password { get; set; }

        [EmailAddress]
        public String Email { get; set; }

        public String RoleId { get; set; }
        public String RoleName { get; set; }
    }
}
