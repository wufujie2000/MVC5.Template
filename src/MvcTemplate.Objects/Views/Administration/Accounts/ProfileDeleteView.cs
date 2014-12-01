using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class ProfileDeleteView : BaseView
    {
        [Required]
        public String Password { get; set; }
    }
}
