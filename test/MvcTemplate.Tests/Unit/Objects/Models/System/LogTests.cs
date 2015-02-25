using MvcTemplate.Objects;
using System;
using System.Web;
using Xunit;

namespace MvcTemplate.Tests.Unit.Objects
{
    public class LogTests : IDisposable
    {
        public LogTests()
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
        }
        public void Dispose()
        {
            HttpContext.Current = null;
        }

        #region Constructor: Log()

        [Fact]
        public void Log_CreatesEmptyInstance()
        {
            Log actual = new Log();

            Assert.Null(actual.AccountId);
            Assert.Null(actual.Message);
        }

        #endregion

        #region Constructor: Log(String message)

        [Fact]
        public void Log_SetsAccountId()
        {
            String expected = HttpContext.Current.User.Identity.Name;
            String actual = new Log(null).AccountId;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Log_SetsMessage()
        {
            String actual = new Log("Message").Message;
            String expected = "Message";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
