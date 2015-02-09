using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Security
{
    [TestFixture]
    public class AuthorizationProviderTests
    {
        private AuthorizationProvider provider;

        [TestFixtureSetUp]
        public void SetUp()
        {
            provider = new AuthorizationProvider(Assembly.GetExecutingAssembly());
        }

        [TearDown]
        public void TearDown()
        {
            DependencyResolver.SetResolver(Substitute.For<IDependencyResolver>());
        }

        #region Method: IsAuthorizedFor(String accountId, String area, String controller, String action)

        #region Not attributed

        [Test]
        [TestCase("NotAttributed", "Get")]
        [TestCase("NotAttributed", "Post")]
        [TestCase("NotAttributed", "GetName")]
        [TestCase("NotAttributed", "PostName")]
        public void IsAuthorizedFor_AuthorizesNotAttributedActions(String controller, String action)
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, controller, action));
        }

        #endregion

        #region Authorized without privileges

        [Test]
        [TestCase(null, "NotAttributed", "AuthorizedGet")]
        [TestCase(null, "NotAttributed", "AuthorizedPost")]
        [TestCase(null, "NotAttributed", "AuthorizedGetName")]
        [TestCase(null, "NotAttributed", "AuthorizedPostName")]

        [TestCase(null, "Authorized", "Get")]
        [TestCase(null, "Authorized", "Post")]
        [TestCase(null, "Authorized", "GetName")]
        [TestCase(null, "Authorized", "PostName")]
        [TestCase(null, "Authorized", "AuthorizedGet")]
        [TestCase(null, "Authorized", "AuthorizedPost")]
        [TestCase(null, "Authorized", "AuthorizedGetName")]
        [TestCase(null, "Authorized", "AuthorizedPostName")]

        [TestCase("Area", "Authorized", "Get")]
        [TestCase("Area", "Authorized", "Post")]
        [TestCase("Area", "Authorized", "GetName")]
        [TestCase("Area", "Authorized", "PostName")]
        [TestCase("Area", "Authorized", "AuthorizedGet")]
        [TestCase("Area", "Authorized", "AuthorizedPost")]
        [TestCase("Area", "Authorized", "AuthorizedGetName")]
        [TestCase("Area", "Authorized", "AuthorizedPostName")]

        [TestCase(null, "AllowAnonymous", "AuthorizedGet")]
        [TestCase(null, "AllowAnonymous", "AuthorizedPost")]
        [TestCase(null, "AllowAnonymous", "AuthorizedGetName")]
        [TestCase(null, "AllowAnonymous", "AuthorizedPostName")]

        [TestCase(null, "AllowUnauthorized", "AuthorizedGet")]
        [TestCase(null, "AllowUnauthorized", "AuthorizedPost")]
        [TestCase(null, "AllowUnauthorized", "AuthorizedGetName")]
        [TestCase(null, "AllowUnauthorized", "AuthorizedPostName")]

        [TestCase(null, "InheritedAuthorized", "InheritanceGet")]
        [TestCase(null, "InheritedAuthorized", "InheritancePost")]
        [TestCase(null, "InheritedAuthorized", "InheritanceGetName")]
        [TestCase(null, "InheritedAuthorized", "InheritancePostName")]
        public void IsAuthorizedFor_DoesNotAuthorizeAuthorizedAction(String area, String controller, String action)
        {
            SetUpDependencyResolver();
            provider.Refresh();

            Assert.IsFalse(provider.IsAuthorizedFor(null, area, controller, action));
        }

        #endregion

        #region Authorized with privileges

        [Test]
        [TestCase(null, "NotAttributed", "AuthorizedGet")]
        [TestCase(null, "NotAttributed", "AuthorizedPost")]
        [TestCase(null, "NotAttributed", "AuthorizedGetName")]
        [TestCase(null, "NotAttributed", "AuthorizedPostName")]

        [TestCase(null, "Authorized", "Get")]
        [TestCase(null, "Authorized", "Post")]
        [TestCase(null, "Authorized", "GetName")]
        [TestCase(null, "Authorized", "PostName")]
        [TestCase(null, "Authorized", "AuthorizedGet")]
        [TestCase(null, "Authorized", "AuthorizedPost")]
        [TestCase(null, "Authorized", "AuthorizedGetName")]
        [TestCase(null, "Authorized", "AuthorizedPostName")]

        [TestCase("Area", "Authorized", "Get")]
        [TestCase("Area", "Authorized", "Post")]
        [TestCase("Area", "Authorized", "GetName")]
        [TestCase("Area", "Authorized", "PostName")]
        [TestCase("Area", "Authorized", "AuthorizedGet")]
        [TestCase("Area", "Authorized", "AuthorizedPost")]
        [TestCase("Area", "Authorized", "AuthorizedGetName")]
        [TestCase("Area", "Authorized", "AuthorizedPostName")]

        [TestCase(null, "AllowAnonymous", "AuthorizedGet")]
        [TestCase(null, "AllowAnonymous", "AuthorizedPost")]
        [TestCase(null, "AllowAnonymous", "AuthorizedGetName")]
        [TestCase(null, "AllowAnonymous", "AuthorizedPostName")]

        [TestCase(null, "AllowUnauthorized", "AuthorizedGet")]
        [TestCase(null, "AllowUnauthorized", "AuthorizedPost")]
        [TestCase(null, "AllowUnauthorized", "AuthorizedGetName")]
        [TestCase(null, "AllowUnauthorized", "AuthorizedPostName")]

        [TestCase(null, "InheritedAuthorized", "InheritanceGet")]
        [TestCase(null, "InheritedAuthorized", "InheritancePost")]
        [TestCase(null, "InheritedAuthorized", "InheritanceGetName")]
        [TestCase(null, "InheritedAuthorized", "InheritancePostName")]
        public void IsAuthorizedFor_AuthorizesAuthorizedAction(String area, String controller, String action)
        {
            Account account = CreateAccountWithPrivilegeFor(area, controller, action);

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, area, controller, action));
        }

        #endregion

        #region Allow anonymous

        [Test]
        [TestCase(null, "NotAttributed", "AnonymousGet")]
        [TestCase(null, "NotAttributed", "AnonymousPost")]
        [TestCase(null, "NotAttributed", "AnonymousGetName")]
        [TestCase(null, "NotAttributed", "AnonymousPostName")]

        [TestCase(null, "Authorized", "AnonymousGet")]
        [TestCase(null, "Authorized", "AnonymousPost")]
        [TestCase(null, "Authorized", "AnonymousGetName")]
        [TestCase(null, "Authorized", "AnonymousPostName")]

        [TestCase("Area", "Authorized", "AnonymousGet")]
        [TestCase("Area", "Authorized", "AnonymousPost")]
        [TestCase("Area", "Authorized", "AnonymousGetName")]
        [TestCase("Area", "Authorized", "AnonymousPostName")]

        [TestCase(null, "AllowAnonymous", "Get")]
        [TestCase(null, "AllowAnonymous", "Post")]
        [TestCase(null, "AllowAnonymous", "GetName")]
        [TestCase(null, "AllowAnonymous", "PostName")]
        [TestCase(null, "AllowAnonymous", "AnonymousGet")]
        [TestCase(null, "AllowAnonymous", "AnonymousPost")]
        [TestCase(null, "AllowAnonymous", "AnonymousGetName")]
        [TestCase(null, "AllowAnonymous", "AnonymousPostName")]

        [TestCase(null, "AllowUnauthorized", "AnonymousGet")]
        [TestCase(null, "AllowUnauthorized", "AnonymousPost")]
        [TestCase(null, "AllowUnauthorized", "AnonymousGetName")]
        [TestCase(null, "AllowUnauthorized", "AnonymousPostName")]

        [TestCase(null, "InheritedAllowAnonymous", "InheritanceGet")]
        [TestCase(null, "InheritedAllowAnonymous", "InheritancePost")]
        [TestCase(null, "InheritedAllowAnonymous", "InheritanceGetName")]
        [TestCase(null, "InheritedAllowAnonymous", "InheritancePostName")]
        public void IsAuthorizedFor_AuthorizesAnonymousAction(String area, String controller, String action)
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, controller, action));
        }

        #endregion

        #region Allow unauthorized

        [Test]
        [TestCase(null, "NotAttributed", "UnauthorizedGet")]
        [TestCase(null, "NotAttributed", "UnauthorizedPost")]
        [TestCase(null, "NotAttributed", "UnauthorizedGetName")]
        [TestCase(null, "NotAttributed", "UnauthorizedPostName")]

        [TestCase(null, "Authorized", "UnauthorizedGet")]
        [TestCase(null, "Authorized", "UnauthorizedPost")]
        [TestCase(null, "Authorized", "UnauthorizedGetName")]
        [TestCase(null, "Authorized", "UnauthorizedPostName")]

        [TestCase("Area", "Authorized", "UnauthorizedGet")]
        [TestCase("Area", "Authorized", "UnauthorizedPost")]
        [TestCase("Area", "Authorized", "UnauthorizedGetName")]
        [TestCase("Area", "Authorized", "UnauthorizedPostName")]

        [TestCase(null, "AllowAnonymous", "UnauthorizedGet")]
        [TestCase(null, "AllowAnonymous", "UnauthorizedPost")]
        [TestCase(null, "AllowAnonymous", "UnauthorizedGetName")]
        [TestCase(null, "AllowAnonymous", "UnauthorizedPostName")]

        [TestCase(null, "AllowUnauthorized", "Get")]
        [TestCase(null, "AllowUnauthorized", "Post")]
        [TestCase(null, "AllowUnauthorized", "GetName")]
        [TestCase(null, "AllowUnauthorized", "PostName")]
        [TestCase(null, "AllowUnauthorized", "UnauthorizedGet")]
        [TestCase(null, "AllowUnauthorized", "UnauthorizedPost")]
        [TestCase(null, "AllowUnauthorized", "UnauthorizedGetName")]
        [TestCase(null, "AllowUnauthorized", "UnauthorizedPostName")]

        [TestCase(null, "InheritedAllowUnauthorized", "InheritanceGet")]
        [TestCase(null, "InheritedAllowUnauthorized", "InheritancePost")]
        [TestCase(null, "InheritedAllowUnauthorized", "InheritanceGetName")]
        [TestCase(null, "InheritedAllowUnauthorized", "InheritancePostName")]
        public void IsAuthorizedFor_AuthorizesUnauthorizedAction(String area, String controller, String action)
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, area, controller, action));
        }

        #endregion

        [Test]
        public void IsAuthorizedFor_CanBeAuthorizedAsOtherAction()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizedGetName");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizedAsOther"));
        }

        [Test]
        public void IsAuthorizedFor_CanBeAuthorizedAsOtherSelf()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizedAsSelf");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizedAsSelf"));
        }

        [Test]
        public void IsAuthorizedFor_CachesAccountPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizedGet");
            TearDownData();

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizedGet"));
        }

        [Test]
        public void IsAuthorizedFor_IgnoresCase()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "AuthorizedGet");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, "ArEa", "Authorized", "AUTHORIZEDGET"));
        }

        [Test]
        public void IsAuthorizedFor_OnNotExistingActionThrows()
        {
            String actual = Assert.Throws<Exception>(() => provider.IsAuthorizedFor(null, null, "NotAttributed", "Test")).Message;
            String expected = "'NotAttributedController' does not have 'Test' action.";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Refresh()

        [Test]
        public void Refresh_RefreshesPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "AuthorizedGet");
            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedGet"));

            TearDownData();
            SetUpDependencyResolver();

            provider.Refresh();

            Assert.IsFalse(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedGet"));
        }

        #endregion

        #region Test helpers

        private Account CreateAccountWithPrivilegeFor(String area, String controller, String action)
        {
            TearDownData();

            using (TestingContext context = new TestingContext())
            {
                Account account = ObjectFactory.CreateAccount();
                Role role = ObjectFactory.CreateRole();
                account.RoleId = role.Id;
                account.Role = role;

                role.RolePrivileges = new List<RolePrivilege>();
                RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege();
                Privilege privilege = ObjectFactory.CreatePrivilege();
                rolePrivilege.PrivilegeId = privilege.Id;
                rolePrivilege.Privilege = privilege;
                rolePrivilege.RoleId = role.Id;
                rolePrivilege.Role = role;

                privilege.Controller = controller;
                privilege.Action = action;
                privilege.Area = area;

                role.RolePrivileges.Add(rolePrivilege);

                context.Set<Account>().Add(account);
                context.SaveChanges();

                SetUpDependencyResolver();
                provider.Refresh();

                return account;
            }
        }

        private void SetUpDependencyResolver()
        {
            IDependencyResolver resolver = Substitute.For<IDependencyResolver>();
            resolver.GetService<IUnitOfWork>().Returns(new UnitOfWork(new TestingContext()));

            DependencyResolver.SetResolver(resolver);
        }

        private void TearDownData()
        {
            using (TestingContext context = new TestingContext())
            {
                context.Set<Privilege>().RemoveRange(context.Set<Privilege>());
                context.Set<Account>().RemoveRange(context.Set<Account>());
                context.Set<Role>().RemoveRange(context.Set<Role>());
                context.SaveChanges();
            }
        }

        #endregion
    }
}
