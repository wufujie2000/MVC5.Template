using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class BootstrapModel
    {
        [Required]
        public String Required { get; set; }
        public String NotRequired { get; set; }
        public Decimal Number { get; set; }
        public DateTime Date { get; set; }

        public String NotReadOnly { get; set; }

        [ReadOnly(false)]
        public String ReadOnlyFalse { get; set; }

        [ReadOnly(true)]
        public String ReadOnlyTrue { get; set; }

        public BootstrapModel Relation { get; set; }

        public String Method()
        {
            return String.Empty;
        }
    }
}
