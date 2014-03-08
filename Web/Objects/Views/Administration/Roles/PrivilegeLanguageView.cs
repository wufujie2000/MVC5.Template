using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class PrivilegeLanguageView : BaseView
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

        public LanguageView Language { get; set; }
    }
}
