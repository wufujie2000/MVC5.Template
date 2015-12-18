using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Objects
{
    public class RolePermission : BaseModel
    {
        [Required]
        public String RoleId { get; set; }
        public virtual Role Role { get; set; }

        [Required]
        public String PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
