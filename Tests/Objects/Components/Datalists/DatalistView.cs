using Datalist;
using System;
using Template.Objects;

namespace Template.Tests.Objects.Components.Datalists
{
    public class DatalistView : BaseView
    {
        public String Text { get; set; }
        public DateTime Date { get; set; }
        public Decimal Number { get; set; }
        public TestEnum Enum { get; set; }
        public Boolean Boolean { get; set; }

        public DateTime? NullableDate { get; set; }
        public Decimal? NullableNumber { get; set; }
        public TestEnum? NullableEnum { get; set; }

        [DatalistColumn(Relation = "Text")]
        public DatalistView Child { get; set; }
    }

    public enum TestEnum
    {
        First,
        Second
    }
}
