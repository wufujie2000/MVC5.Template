using MvcTemplate.Components.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class AccountLoginView : BaseView
    {
        [Required]
        public String Username { get; set; }

        [Required]
        [NotTrimmed]
        public String Password { get; set; }
    }
}
