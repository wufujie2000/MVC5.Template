using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Tests.Objects
{
    public class BootstrapModel
    {
        [Required]
        public String Required { get; set; }
        public String NotRequired { get; set; }
        public Decimal Number { get; set; }
        public DateTime Date { get; set; }

        public BootstrapModel Relation { get; set; }

        public String Method()
        {
            return String.Empty;
        }
    }
}
