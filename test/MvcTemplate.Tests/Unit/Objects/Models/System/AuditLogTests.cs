using MvcTemplate.Objects;
using System;
using System.Web;
using Xunit;

namespace MvcTemplate.Tests.Unit.Objects
{
    public class AuditLogTests : IDisposable
    {
        public AuditLogTests()
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
        }
        public void Dispose()
        {
            HttpContext.Current = null;
        }

        #region Constructor: AuditLog()

        [Fact]
        public void AuditLog_CreatesEmptyInstance()
        {
            AuditLog actual = new AuditLog();

            Assert.Null(actual.EntityName);
            Assert.Null(actual.AccountId);
            Assert.Null(actual.EntityId);
            Assert.Null(actual.Changes);
        }

        #endregion

        #region Constructor: AuditLog(String action, String entityName, String entityId, String changes)

        [Fact]
        public void AuditLog_SetsAccountId()
        {
            String expected = HttpContext.Current.User.Identity.Name;
            String actual = new AuditLog(null, null, null, null).AccountId;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AuditLog_SetsAction()
        {
            String actual = new AuditLog("Action", null, null, null).Action;
            String expected = "Action";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AuditLog_SetsEntityName()
        {
            String actual = new AuditLog(null, "Nameless", null, null).EntityName;
            String expected = "Nameless";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AuditLog_SetsEntityId()
        {
            String actual = new AuditLog(null, null, "Id", null).EntityId;
            String expected = "Id";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AuditLog_SetsChagnes()
        {
            String actual = new AuditLog(null, null, null, "Changes").Changes;
            String expected = "Changes";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
