using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class PrivilegeLanguage : BaseModel
    {
        public String Area { get; set; }

        [Required]
        public String Controller { get; set; }

        [Required]
        public String Action { get; set; }

        [Required]
        public String PrivilegeId { get; set; }

        [Required]
        public String LanguageId { get; set; }

        public virtual Privilege Privilege { get; set; }
        public virtual Language Language { get; set; }
    }
}
