using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class Privilege : BaseModel
    {
        public String Area { get; set; }

        [Required]
        public String Controller { get; set; }

        [Required]
        public String Action { get; set; }

        public virtual IList<PrivilegeLanguage> PrivilegeLanguages { get; set; }
    }
}
