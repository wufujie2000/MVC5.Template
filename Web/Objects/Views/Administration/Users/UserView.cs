using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class UserView : BaseView
    {
        public String UserFirstName { get; set; }
        public String UserLastName { get; set; }
        public DateTime? UserDateOfBirth { get; set; }

        public String UserRoleId { get; set; }
        public String UserRoleName { get; set; }

        [Required]
        public String Username { get; set; }
        public String Password { get; set; }
        public String NewPassword { get; set; }
    }
}
