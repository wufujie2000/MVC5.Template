using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class AdaptersModel
    {
        [Required]
        public Int32? Required { get; set; }

        [StringLength(128)]
        public String StringLength { get; set; }

        [EmailAddress]
        public String Email { get; set; }
    }
}
