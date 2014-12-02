using MvcTemplate.Objects;
using NUnit.Framework;
using System;
using System.Web;

namespace MvcTemplate.Tests.Unit.Objects
{
    [TestFixture]
    public class AuditLogTests
    {
        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Constructor: AuditLog()

        [Test]
        public void AuditLog_CreatesEmptyInstance()
        {
            AuditLog actual = new AuditLog();

            Assert.IsNull(actual.EntityName);
            Assert.IsNull(actual.AccountId);
            Assert.IsNull(actual.EntityId);
            Assert.IsNull(actual.Changes);
        }

        #endregion

        #region Constructor: AuditLog(String action, String entityName, String entityId, String changes)

        [Test]
        public void AuditLog_SetsAccountId()
        {
            String expected = HttpContext.Current.User.Identity.Name;
            String actual = new AuditLog(null, null, null, null).AccountId;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AuditLog_SetsAction()
        {
            String actual = new AuditLog("Action", null, null, null).Action;
            String expected = "Action";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AuditLog_SetsEntityName()
        {
            String actual = new AuditLog(null, "Nameless", null, null).EntityName;
            String expected = "Nameless";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AuditLog_SetsEntityId()
        {
            String actual = new AuditLog(null, null, "Id", null).EntityId;
            String expected = "Id";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AuditLog_SetsChagnes()
        {
            String actual = new AuditLog(null, null, null, "Changes").Changes;
            String expected = "Changes";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
