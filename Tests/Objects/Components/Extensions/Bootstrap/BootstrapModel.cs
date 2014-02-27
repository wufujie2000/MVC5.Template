using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Tests.Objects.Components.Extensions.Bootstrap
{
    public class BootstrapModel
    {
        [Required]
        public String Required { get; set; }
        public String NotRequired { get; set; }
        public Decimal Number { get; set; }
        public DateTime Date { get; set; }

        public String Method()
        {
            return String.Empty;
        }
    }
}
