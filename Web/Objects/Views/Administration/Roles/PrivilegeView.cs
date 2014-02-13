using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class PrivilegeView : BaseView
    {
        public String Area { get; set; }

        [Required]
        public String Controller { get; set; }

        [Required]
        public String Action { get; set; }
    }
}
