using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class LoginView : BaseView
    {
        [Required]
        public String Username { get; set; }

        [Required]
        public String Password { get; set; }
    }
}
