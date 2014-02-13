using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class ProfileView : BaseView
    {
        [Required]
        public String Username { get; set; }

        [Required]
        public String CurrentPassword { get; set; }
        public String NewPassword { get; set; }

        public String UserFirstName { get; set; }
        public String UserLastName { get; set; }
        public DateTime? UserDateOfBirth { get; set; }
    }
}
