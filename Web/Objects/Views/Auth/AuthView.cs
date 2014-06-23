using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class AuthView : BaseView
    {
        [Required]
        public String Username { get; set; }

        [Required]
        public String Password { get; set; }

        [EmailAddress]
        public String Email { get; set; }
    }
}
