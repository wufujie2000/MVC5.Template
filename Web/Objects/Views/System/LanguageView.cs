using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Objects
{
    public class LanguageView : BaseView
    {
        [Required]
        public String Abbreviation { get; set; }

        [Required]
        public String Name { get; set; }
    }
}
