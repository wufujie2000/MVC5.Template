using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MvcTemplate.Tests.Unit.Security
{
    [TestFixture]
    public class AuthorizationProviderTests
    {
        private AuthorizationProvider provider;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            provider = new AuthorizationProvider(Assembly.GetExecutingAssembly(), new UnitOfWork(context));

            TearDownData();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
            provider.Dispose();
        }

        #region Method: IsAuthorizedFor(String accountId, String area, String controller, String action)

        #region Not attributed controller

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "NotAttributed", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "NotAttributed", "NotAttributedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnNotAttributedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "NotAttributed", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnNotAttributedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionOnNotAttributedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "NotAttributed", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionOnNotAttributedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizePostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "NotAttributed", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "NotAttributed", "AllowAnonymousPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "NotAttributed", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionOnNotAttributedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "NotAttributed", "AllowUnauthorizedPostAction"));
        }

        #endregion

        #region Authorized controller

        [Test]
        public void IsAuthorizedFor_NotAuthorizesNotAttributedGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "Authorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "NotAttributedGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesNotAttributedPostActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "Authorized", "NotAttributedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "NotAttributedPostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "NotAttributedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "Authorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionOnAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "Authorized", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionOnAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizePostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "Authorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "Authorized", "AllowAnonymousPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "Authorized", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionOnAuthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "Authorized", "AllowUnauthorizedPostAction"));
        }

        #endregion

        #region Allow anonymous controller

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowAnonymous", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowAnonymous", "NotAttributedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "AllowAnonymous", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowAnonymous", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionOnAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "AllowAnonymous", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionOnAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowAnonymous", "AuthorizePostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowAnonymous", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowAnonymous", "AllowAnonymousPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowAnonymous", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionOnAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowAnonymous", "AllowUnauthorizedPostAction"));
        }

        #endregion

        #region Allow unauthorized controller

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowUnauthorized", "NotAttributedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesNotAttributedPostActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowUnauthorized", "NotAttributedPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "AllowUnauthorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedGetActionOnAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowUnauthorized", "AuthorizeGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowUnauthorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesAuthorizedPostActionOnAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "AllowUnauthorized", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAuthorizedPostActionOnAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "AllowUnauthorized", "AuthorizePostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "AllowUnauthorized", "AuthorizePostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowUnauthorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousPostActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowUnauthorized", "AllowAnonymousPostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedGetActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowUnauthorized", "AllowUnauthorizedGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedPostActionOnAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "AllowUnauthorized", "AllowUnauthorizedPostAction"));
        }

        #endregion

        #region Inherited authorized controller

        [Test]
        public void IsAuthorizedFor_NotAuthorizesGetActionOnInheritedAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "InheritedAuthorized", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesGetActionOnInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "GetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesPostActionOnInheritedAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "InheritedAuthorized", "PostAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesPostActionOnInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "PostAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "PostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesInheritedAllowAnonymouActionOnInheritedAuthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "InheritedAuthorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesInheritedAllowAnonymouActionOnInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAuthorized", "AllowAnonymousGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "AllowAnonymousGetAction"));
        }

        #endregion

        #region Inherited allow anonymous controller

        [Test]
        public void IsAuthorizedFor_AuthorizesGetActionOnInheritedAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "InheritedAllowAnonymous", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesPostActionOnInheritedAllowAnonymousController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "InheritedAllowAnonymous", "PostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesInheritedAllowAnonymouActionOnInheritedAllowAnonymousController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "InheritedAllowAnonymous", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesInheritedAllowAnonymouActionOnInheritedAllowAnonymousController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAllowAnonymous", "AllowAnonymousGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAllowAnonymous", "AllowAnonymousGetAction"));
        }

        #endregion

        #region Inherited allow unauthorized controller

        [Test]
        public void IsAuthorizedFor_AuthorizesGetActionOnInheritedAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "InheritedAllowUnauthorized", "GetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesPostActionOnInheritedAllowUnauthorizedController()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "InheritedAllowUnauthorized", "PostAction"));
        }

        [Test]
        public void IsAuthorizedFor_NotAuthorizesInheritedAllowAnonymouActionOnInheritedAllowUnauthorizedController()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, null, "InheritedAllowUnauthorized", "AllowAnonymousGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_AuthorizesInheritedAllowAnonymouActionOnInheritedAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "InheritedAllowUnauthorized", "AllowAnonymousGetAction");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "InheritedAllowUnauthorized", "AllowAnonymousGetAction"));
        }

        #endregion

        [Test]
        public void IsAuthorizedFor_CachesAccountPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizeGetAction");
            TearDownData();

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizeGetAction"));
        }

        [Test]
        public void IsAuthorizedFor_OnNotExistingActionThrows()
        {
            Assert.Throws<Exception>(() => provider.IsAuthorizedFor(null, null, "NotAttributed", "Test"));
        }

        #endregion

        #region Method: Refresh()

        [Test]
        public void Refresh_RefreshesPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizeGetAction");

            TearDownData();
            provider.Refresh();

            Assert.IsFalse(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizeGetAction"));
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesUnitOfWork()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            unitOfWork.Repository<Account>().Returns(new Repository<Account>(context));

            new AuthorizationProvider(Assembly.GetExecutingAssembly(), unitOfWork).Dispose();

            unitOfWork.Received().Dispose();
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            provider.Dispose();
            provider.Dispose();
        }

        #endregion

        #region Test helpers

        private Account CreateAccountWithPrivilegeFor(String area, String controller, String action)
        {
            Account account = ObjectFactory.CreateAccount();
            Role role = ObjectFactory.CreateRole();
            account.RoleId = role.Id;

            context.Set<Account>().Add(account);

            role.RolePrivileges = new List<RolePrivilege>();
            RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege();
            Privilege privilege = ObjectFactory.CreatePrivilege();
            rolePrivilege.PrivilegeId = privilege.Id;
            rolePrivilege.Privilege = privilege;
            rolePrivilege.RoleId = role.Id;
            rolePrivilege.Role = role;

            privilege.Area = area;
            privilege.Controller = controller;
            privilege.Action = action;

            role.RolePrivileges.Add(rolePrivilege);

            context.Set<Role>().Add(role);
            context.SaveChanges();
            provider.Refresh();

            return account;
        }

        private void TearDownData()
        {
            context.Set<Privilege>().RemoveRange(context.Set<Privilege>());
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        #endregion
    }
}
