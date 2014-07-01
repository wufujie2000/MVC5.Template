using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class ProfileEditView : BaseView
    {
        [Required]
        [StringLength(128)]
        public String Username { get; set; }

        [Required]
        public String Password { get; set; }
        public String NewPassword { get; set; }

        [Required]
        [EmailAddress]
        public String Email { get; set; }
    }
}
