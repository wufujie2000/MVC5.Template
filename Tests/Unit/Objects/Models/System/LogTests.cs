using MvcTemplate.Objects;
using NUnit.Framework;
using System;

namespace MvcTemplate.Tests.Unit.Objects
{
    [TestFixture]
    public class LogTests
    {
        #region Constructor: Log(String accountId, String message)

        [Test]
        public void Log_SetsAccountId()
        {
            String expected = Guid.NewGuid().ToString();
            String actual = new Log(expected, null).AccountId;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Log_SetsMessage()
        {
            String expected = "Test message";
            String actual = new Log(null, expected).Message;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
