using System;
using System.Collections.Generic;
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

        public IList<PrivilegeLanguageView> PrivilegeLanguages { get; set; }
    }
}
