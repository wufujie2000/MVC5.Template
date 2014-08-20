using MvcTemplate.Objects;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Web;

namespace MvcTemplate.Tests.Unit.Objects
{
    [TestFixture]
    public class LogTests
    {
        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpMock().HttpContext;
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Constructor: Log()

        [Test]
        public void Log_SetsAccountId()
        {
            String expected = HttpContext.Current.User.Identity.Name;
            String actual = new Log().AccountId;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Log_LeavesNullMessage()
        {
            Assert.IsNull(new Log().Message);
        }

        #endregion

        #region Constructor: Log(String message)

        [Test]
        public void Log_Message_SetsAccountId()
        {
            String expected = HttpContext.Current.User.Identity.Name;
            String actual = new Log(null).AccountId;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Log_Message_SetsMessage()
        {
            String actual = new Log("Message").Message;
            String expected = "Message";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
