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
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", controller, action));
        }

        #endregion

        #region Authorized without privileges

        [Test]
        [TestCase("NotAttributed", "AuthorizedGet")]
        [TestCase("NotAttributed", "AuthorizedPost")]
        [TestCase("NotAttributed", "AuthorizedGetName")]
        [TestCase("NotAttributed", "AuthorizedPostName")]

        [TestCase("Authorized", "Get")]
        [TestCase("Authorized", "Post")]
        [TestCase("Authorized", "GetName")]
        [TestCase("Authorized", "PostName")]
        [TestCase("Authorized", "AuthorizedGet")]
        [TestCase("Authorized", "AuthorizedPost")]
        [TestCase("Authorized", "AuthorizedGetName")]
        [TestCase("Authorized", "AuthorizedPostName")]

        [TestCase("AllowAnonymous", "AuthorizedGet")]
        [TestCase("AllowAnonymous", "AuthorizedPost")]
        [TestCase("AllowAnonymous", "AuthorizedGetName")]
        [TestCase("AllowAnonymous", "AuthorizedPostName")]

        [TestCase("AllowUnauthorized", "AuthorizedGet")]
        [TestCase("AllowUnauthorized", "AuthorizedPost")]
        [TestCase("AllowUnauthorized", "AuthorizedGetName")]
        [TestCase("AllowUnauthorized", "AuthorizedPostName")]

        [TestCase("InheritedAuthorized", "InheritanceGet")]
        [TestCase("InheritedAuthorized", "InheritancePost")]
        [TestCase("InheritedAuthorized", "InheritanceGetName")]
        [TestCase("InheritedAuthorized", "InheritancePostName")]
        public void IsAuthorizedFor_DoesNotAuthorizeAuthorizedAction(String controller, String action)
        {
            SetUpDependencyResolver();
            provider.Refresh();

            Assert.IsFalse(provider.IsAuthorizedFor(null, "Security", controller, action));
        }

        #endregion

        #region Authorized with privileges

        [Test]
        [TestCase("NotAttributed", "AuthorizedGet")]
        [TestCase("NotAttributed", "AuthorizedPost")]
        [TestCase("NotAttributed", "AuthorizedGetName")]
        [TestCase("NotAttributed", "AuthorizedPostName")]

        [TestCase("Authorized", "Get")]
        [TestCase("Authorized", "Post")]
        [TestCase("Authorized", "GetName")]
        [TestCase("Authorized", "PostName")]
        [TestCase("Authorized", "AuthorizedGet")]
        [TestCase("Authorized", "AuthorizedPost")]
        [TestCase("Authorized", "AuthorizedGetName")]
        [TestCase("Authorized", "AuthorizedPostName")]

        [TestCase("AllowAnonymous", "AuthorizedGet")]
        [TestCase("AllowAnonymous", "AuthorizedPost")]
        [TestCase("AllowAnonymous", "AuthorizedGetName")]
        [TestCase("AllowAnonymous", "AuthorizedPostName")]

        [TestCase("AllowUnauthorized", "AuthorizedGet")]
        [TestCase("AllowUnauthorized", "AuthorizedPost")]
        [TestCase("AllowUnauthorized", "AuthorizedGetName")]
        [TestCase("AllowUnauthorized", "AuthorizedPostName")]

        [TestCase("InheritedAuthorized", "InheritanceGet")]
        [TestCase("InheritedAuthorized", "InheritancePost")]
        [TestCase("InheritedAuthorized", "InheritanceGetName")]
        [TestCase("InheritedAuthorized", "InheritancePostName")]
        public void IsAuthorizedFor_AuthorizesAuthorizedAction(String controller, String action)
        {
            Account account = CreateAccountWithPrivilegeFor("Security", controller, action);

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, "Security", controller, action));
        }

        #endregion

        #region Allow anonymous

        [Test]
        [TestCase("NotAttributed", "AnonymousGet")]
        [TestCase("NotAttributed", "AnonymousPost")]
        [TestCase("NotAttributed", "AnonymousGetName")]
        [TestCase("NotAttributed", "AnonymousPostName")]

        [TestCase("Authorized", "AnonymousGet")]
        [TestCase("Authorized", "AnonymousPost")]
        [TestCase("Authorized", "AnonymousGetName")]
        [TestCase("Authorized", "AnonymousPostName")]

        [TestCase("AllowAnonymous", "Get")]
        [TestCase("AllowAnonymous", "Post")]
        [TestCase("AllowAnonymous", "GetName")]
        [TestCase("AllowAnonymous", "PostName")]
        [TestCase("AllowAnonymous", "AnonymousGet")]
        [TestCase("AllowAnonymous", "AnonymousPost")]
        [TestCase("AllowAnonymous", "AnonymousGetName")]
        [TestCase("AllowAnonymous", "AnonymousPostName")]

        [TestCase("AllowUnauthorized", "AnonymousGet")]
        [TestCase("AllowUnauthorized", "AnonymousPost")]
        [TestCase("AllowUnauthorized", "AnonymousGetName")]
        [TestCase("AllowUnauthorized", "AnonymousPostName")]

        [TestCase("InheritedAllowAnonymous", "InheritanceGet")]
        [TestCase("InheritedAllowAnonymous", "InheritancePost")]
        [TestCase("InheritedAllowAnonymous", "InheritanceGetName")]
        [TestCase("InheritedAllowAnonymous", "InheritancePostName")]
        public void IsAuthorizedFor_AuthorizesAnonymousAction(String controller, String action)
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", controller, action));
        }

        #endregion

        #region Allow unauthorized

        [Test]
        [TestCase("NotAttributed", "UnauthorizedGet")]
        [TestCase("NotAttributed", "UnauthorizedPost")]
        [TestCase("NotAttributed", "UnauthorizedGetName")]
        [TestCase("NotAttributed", "UnauthorizedPostName")]

        [TestCase("Authorized", "UnauthorizedGet")]
        [TestCase("Authorized", "UnauthorizedPost")]
        [TestCase("Authorized", "UnauthorizedGetName")]
        [TestCase("Authorized", "UnauthorizedPostName")]

        [TestCase("AllowAnonymous", "UnauthorizedGet")]
        [TestCase("AllowAnonymous", "UnauthorizedPost")]
        [TestCase("AllowAnonymous", "UnauthorizedGetName")]
        [TestCase("AllowAnonymous", "UnauthorizedPostName")]

        [TestCase("AllowUnauthorized", "Get")]
        [TestCase("AllowUnauthorized", "Post")]
        [TestCase("AllowUnauthorized", "GetName")]
        [TestCase("AllowUnauthorized", "PostName")]
        [TestCase("AllowUnauthorized", "UnauthorizedGet")]
        [TestCase("AllowUnauthorized", "UnauthorizedPost")]
        [TestCase("AllowUnauthorized", "UnauthorizedGetName")]
        [TestCase("AllowUnauthorized", "UnauthorizedPostName")]

        [TestCase("InheritedAllowUnauthorized", "InheritanceGet")]
        [TestCase("InheritedAllowUnauthorized", "InheritancePost")]
        [TestCase("InheritedAllowUnauthorized", "InheritanceGetName")]
        [TestCase("InheritedAllowUnauthorized", "InheritancePostName")]
        public void IsAuthorizedFor_AuthorizesUnauthorizedAction(String controller, String action)
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, "Security", controller, action));
        }

        #endregion

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
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizedGet");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, null, "notattributed", "AUTHORIZEDGET"));
        }

        [Test]
        public void IsAuthorizedFor_OnNotExistingActionThrows()
        {
            String actual = Assert.Throws<Exception>(() => provider.IsAuthorizedFor(null, "Security", "NotAttributed", "Test")).Message;
            String expected = "'NotAttributedController' does not have 'Test' action.";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Refresh()

        [Test]
        public void Refresh_RefreshesPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor("Security", "Authorized", "AuthorizedGet");
            Assume.That(provider.IsAuthorizedFor(account.Id, "Security", "Authorized", "AuthorizedGet"));

            TearDownData();

            SetUpDependencyResolver();
            provider.Refresh();

            Assert.IsFalse(provider.IsAuthorizedFor(account.Id, "Security", "Authorized", "AuthorizedGet"));
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
