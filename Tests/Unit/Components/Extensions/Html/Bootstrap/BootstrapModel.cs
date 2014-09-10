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
        public DateTime? Date { get; set; }

        public String NotReadOnly { get; set; }

        [ReadOnly(false)]
        public String ReadOnlyFalse { get; set; }

        [ReadOnly(true)]
        public String ReadOnlyTrue { get; set; }

        public BootstrapModel Relation { get; set; }

        public BootstrapModel()
        {
            NotRequired = "NotRequired";
            Required = "Required";
            Date = DateTime.Now;
            NotReadOnly = "Not";
            ReadOnlyFalse = "F";
            ReadOnlyTrue = "T";
            Number = 10.7854M;

            Relation = new BootstrapModel(true)
            {
                Date = new DateTime(2011, 01, 01, 1, 1, 1),
                NotRequired = "NotRequiredRelation",
                Required = "RequiredRelation",
                NotReadOnly = "NotRelation",
                ReadOnlyFalse = "FRelation",
                ReadOnlyTrue = "TRelation",
                Number = 1.6666M,
            };
        }
        private BootstrapModel(Boolean noInit)
        {
        }
    }
}
