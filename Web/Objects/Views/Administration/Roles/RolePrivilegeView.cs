using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class RolePrivilegeView : BaseView
    {
        [Required]
        public String RoleId { get; set; }

        [Required]
        public String PrivilegeId { get; set; }
    }
}
