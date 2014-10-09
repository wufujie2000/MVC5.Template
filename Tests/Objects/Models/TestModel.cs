using MvcTemplate.Objects;
using System;

namespace MvcTemplate.Tests.Objects
{
    public class TestModel : BaseModel
    {
        public String Text { get; set; }
        public DateTime? Date { get; set; }
    }
}
