using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class Role : BaseModel
    {
        [Required]
        public String Name { get; set; }

        public virtual IList<RolePrivilege> RolePrivileges { get; set; }
    }
}
