using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class UserView : BaseView
    {
        [Required]
        public String Username { get; set; }
        public String Password { get; set; }

        public PersonView Person { get; set; }
    }
}
