using MvcTemplate.Objects;
using NSubstitute;
using System;
using Xunit;

namespace MvcTemplate.Tests.Unit.Objects
{
    public class BaseViewTests
    {
        private BaseView view;

        public BaseViewTests()
        {
            view = Substitute.For<BaseView>();
        }

        #region Constructor: BaseView()

        [Fact]
        public void BaseView_SetsCreationDateToNow()
        {
            Int64 actual = Substitute.For<BaseView>().CreationDate.Ticks;
            Int64 from = DateTime.Now.Ticks - TimeSpan.TicksPerSecond;
            Int64 to = DateTime.Now.Ticks + TimeSpan.TicksPerSecond;

            Assert.InRange(actual, from, to);
        }

        [Fact]
        public void BaseView_TruncatesMicrosecondsFromCreationDate()
        {
            DateTime actual = Substitute.For<BaseView>().CreationDate;
            DateTime expected = new DateTime(actual.Year, actual.Month, actual.Day, actual.Hour, actual.Minute, actual.Second, actual.Millisecond);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BaseView_KeepsCurrentDateKind()
        {
            DateTimeKind actual = Substitute.For<BaseView>().CreationDate.Kind;
            DateTimeKind expected = DateTime.Now.Kind;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Property: Id

        [Fact]
        public void Id_AlwaysGetsNotNull()
        {
            view.Id = null;

            Assert.NotNull(view.Id);
        }

        [Fact]
        public void Id_AlwaysGetsUniqueValue()
        {
            String expected = view.Id;
            view.Id = null;
            String actual = view.Id;

            Assert.NotEqual(expected, actual);
        }

        [Fact]
        public void Id_AlwaysGetsSameValue()
        {
            String expected = view.Id;
            String actual = view.Id;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
