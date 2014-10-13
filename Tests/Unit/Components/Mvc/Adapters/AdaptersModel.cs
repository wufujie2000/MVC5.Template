using MvcTemplate.Components.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class AdaptersModel
    {
        [Required]
        public Int32? Required { get; set; }

        [MinValue(128)]
        public String MinValue { get; set; }

        [MaxValue(128)]
        public String MaxValue { get; set; }

        [MinLength(128)]
        public String MinLength { get; set; }

        [StringLength(128)]
        public String StringLength { get; set; }

        [EmailAddress]
        public String Email { get; set; }
    }
}
