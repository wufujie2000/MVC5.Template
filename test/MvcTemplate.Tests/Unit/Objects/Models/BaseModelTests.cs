using MvcTemplate.Objects;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Objects
{
    public class BaseModelTests
    {
        private BaseModel model;

        public BaseModelTests()
        {
            model = Substitute.For<BaseModel>();
        }

        #region Constructor: BaseModel()

        [Fact]
        public void BaseModel_SetsCreationDateToNow()
        {
            Int64 actual = Substitute.For<BaseModel>().CreationDate.Ticks;
            Int64 from = DateTime.Now.Ticks - TimeSpan.TicksPerSecond;
            Int64 to = DateTime.Now.Ticks + TimeSpan.TicksPerSecond;

            Assert.InRange(actual, from, to);
        }

        [Fact]
        public void BaseModel_TruncatesMicrosecondsFromCreationDate()
        {
            DateTime actual = Substitute.For<BaseModel>().CreationDate;
            DateTime expected = new DateTime(actual.Year, actual.Month, actual.Day, actual.Hour, actual.Minute, actual.Second, actual.Millisecond);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BaseModel_KeepsCurrentDateKind()
        {
            DateTimeKind actual = Substitute.For<BaseModel>().CreationDate.Kind;
            DateTimeKind expected = DateTime.Now.Kind;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Property: Id

        [Fact]
        public void Id_AlwaysGetsNotNull()
        {
            model.Id = null;

            Assert.NotNull(model.Id);
        }

        [Fact]
        public void Id_AlwaysGetsUniqueValue()
        {
            String expected = model.Id;
            model.Id = null;
            String actual = model.Id;

            Assert.NotEqual(expected, actual);
        }

        [Fact]
        public void Id_AlwaysGetsSameValue()
        {
            String expected = model.Id;
            String actual = model.Id;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
