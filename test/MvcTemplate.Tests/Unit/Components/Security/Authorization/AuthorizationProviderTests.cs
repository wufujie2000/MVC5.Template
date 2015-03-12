using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Security
{
    public class AuthorizationProviderTests : IDisposable
    {
        private AuthorizationProvider provider;

        public AuthorizationProviderTests()
        {
            provider = new AuthorizationProvider(Assembly.GetExecutingAssembly());
        }
        public void Dispose()
        {
            DependencyResolver.SetResolver(Substitute.For<IDependencyResolver>());
        }

        #region Method: IsAuthorizedFor(String accountId, String area, String controller, String action)

        #region Not attributed

        [Theory]
        [InlineData("NotAttributed", "Get")]
        [InlineData("NotAttributed", "Post")]
        [InlineData("NotAttributed", "GetName")]
        [InlineData("NotAttributed", "PostName")]
        public void IsAuthorizedFor_AuthorizesNotAttributedActions(String controller, String action)
        {
            Assert.True(provider.IsAuthorizedFor(null, null, controller, action));
        }

        #endregion

        #region Authorized without privileges

        [Theory]
        [InlineData(null, "NotAttributed", "AuthorizedGet")]
        [InlineData(null, "NotAttributed", "AuthorizedPost")]
        [InlineData(null, "NotAttributed", "AuthorizedGetName")]
        [InlineData(null, "NotAttributed", "AuthorizedPostName")]

        [InlineData(null, "Authorized", "Get")]
        [InlineData(null, "Authorized", "Post")]
        [InlineData(null, "Authorized", "GetName")]
        [InlineData(null, "Authorized", "PostName")]
        [InlineData(null, "Authorized", "AuthorizedGet")]
        [InlineData(null, "Authorized", "AuthorizedPost")]
        [InlineData(null, "Authorized", "AuthorizedGetName")]
        [InlineData(null, "Authorized", "AuthorizedPostName")]

        [InlineData("Area", "Authorized", "Get")]
        [InlineData("Area", "Authorized", "Post")]
        [InlineData("Area", "Authorized", "GetName")]
        [InlineData("Area", "Authorized", "PostName")]
        [InlineData("Area", "Authorized", "AuthorizedGet")]
        [InlineData("Area", "Authorized", "AuthorizedPost")]
        [InlineData("Area", "Authorized", "AuthorizedGetName")]
        [InlineData("Area", "Authorized", "AuthorizedPostName")]

        [InlineData(null, "AllowAnonymous", "AuthorizedGet")]
        [InlineData(null, "AllowAnonymous", "AuthorizedPost")]
        [InlineData(null, "AllowAnonymous", "AuthorizedGetName")]
        [InlineData(null, "AllowAnonymous", "AuthorizedPostName")]

        [InlineData(null, "AllowUnauthorized", "AuthorizedGet")]
        [InlineData(null, "AllowUnauthorized", "AuthorizedPost")]
        [InlineData(null, "AllowUnauthorized", "AuthorizedGetName")]
        [InlineData(null, "AllowUnauthorized", "AuthorizedPostName")]

        [InlineData(null, "InheritedAuthorized", "InheritanceGet")]
        [InlineData(null, "InheritedAuthorized", "InheritancePost")]
        [InlineData(null, "InheritedAuthorized", "InheritanceGetName")]
        [InlineData(null, "InheritedAuthorized", "InheritancePostName")]
        public void IsAuthorizedFor_DoesNotAuthorizeAuthorizedAction(String area, String controller, String action)
        {
            SetUpDependencyResolver();
            provider.Refresh();

            Assert.False(provider.IsAuthorizedFor(null, area, controller, action));
        }

        #endregion

        #region Authorized with privileges

        [Theory]
        [InlineData(null, "NotAttributed", "AuthorizedGet")]
        [InlineData(null, "NotAttributed", "AuthorizedPost")]
        [InlineData(null, "NotAttributed", "AuthorizedGetName")]
        [InlineData(null, "NotAttributed", "AuthorizedPostName")]

        [InlineData(null, "Authorized", "Get")]
        [InlineData(null, "Authorized", "Post")]
        [InlineData(null, "Authorized", "GetName")]
        [InlineData(null, "Authorized", "PostName")]
        [InlineData(null, "Authorized", "AuthorizedGet")]
        [InlineData(null, "Authorized", "AuthorizedPost")]
        [InlineData(null, "Authorized", "AuthorizedGetName")]
        [InlineData(null, "Authorized", "AuthorizedPostName")]

        [InlineData("Area", "Authorized", "Get")]
        [InlineData("Area", "Authorized", "Post")]
        [InlineData("Area", "Authorized", "GetName")]
        [InlineData("Area", "Authorized", "PostName")]
        [InlineData("Area", "Authorized", "AuthorizedGet")]
        [InlineData("Area", "Authorized", "AuthorizedPost")]
        [InlineData("Area", "Authorized", "AuthorizedGetName")]
        [InlineData("Area", "Authorized", "AuthorizedPostName")]

        [InlineData(null, "AllowAnonymous", "AuthorizedGet")]
        [InlineData(null, "AllowAnonymous", "AuthorizedPost")]
        [InlineData(null, "AllowAnonymous", "AuthorizedGetName")]
        [InlineData(null, "AllowAnonymous", "AuthorizedPostName")]

        [InlineData(null, "AllowUnauthorized", "AuthorizedGet")]
        [InlineData(null, "AllowUnauthorized", "AuthorizedPost")]
        [InlineData(null, "AllowUnauthorized", "AuthorizedGetName")]
        [InlineData(null, "AllowUnauthorized", "AuthorizedPostName")]

        [InlineData(null, "InheritedAuthorized", "InheritanceGet")]
        [InlineData(null, "InheritedAuthorized", "InheritancePost")]
        [InlineData(null, "InheritedAuthorized", "InheritanceGetName")]
        [InlineData(null, "InheritedAuthorized", "InheritancePostName")]
        public void IsAuthorizedFor_AuthorizesAuthorizedAction(String area, String controller, String action)
        {
            Account account = CreateAccountWithPrivilegeFor(area, controller, action);

            Assert.True(provider.IsAuthorizedFor(account.Id, area, controller, action));
        }

        #endregion

        #region Allow anonymous

        [Theory]
        [InlineData(null, "NotAttributed", "AnonymousGet")]
        [InlineData(null, "NotAttributed", "AnonymousPost")]
        [InlineData(null, "NotAttributed", "AnonymousGetName")]
        [InlineData(null, "NotAttributed", "AnonymousPostName")]

        [InlineData(null, "Authorized", "AnonymousGet")]
        [InlineData(null, "Authorized", "AnonymousPost")]
        [InlineData(null, "Authorized", "AnonymousGetName")]
        [InlineData(null, "Authorized", "AnonymousPostName")]

        [InlineData("Area", "Authorized", "AnonymousGet")]
        [InlineData("Area", "Authorized", "AnonymousPost")]
        [InlineData("Area", "Authorized", "AnonymousGetName")]
        [InlineData("Area", "Authorized", "AnonymousPostName")]

        [InlineData(null, "AllowAnonymous", "Get")]
        [InlineData(null, "AllowAnonymous", "Post")]
        [InlineData(null, "AllowAnonymous", "GetName")]
        [InlineData(null, "AllowAnonymous", "PostName")]
        [InlineData(null, "AllowAnonymous", "AnonymousGet")]
        [InlineData(null, "AllowAnonymous", "AnonymousPost")]
        [InlineData(null, "AllowAnonymous", "AnonymousGetName")]
        [InlineData(null, "AllowAnonymous", "AnonymousPostName")]

        [InlineData(null, "AllowUnauthorized", "AnonymousGet")]
        [InlineData(null, "AllowUnauthorized", "AnonymousPost")]
        [InlineData(null, "AllowUnauthorized", "AnonymousGetName")]
        [InlineData(null, "AllowUnauthorized", "AnonymousPostName")]

        [InlineData(null, "InheritedAllowAnonymous", "InheritanceGet")]
        [InlineData(null, "InheritedAllowAnonymous", "InheritancePost")]
        [InlineData(null, "InheritedAllowAnonymous", "InheritanceGetName")]
        [InlineData(null, "InheritedAllowAnonymous", "InheritancePostName")]
        public void IsAuthorizedFor_AuthorizesAnonymousAction(String area, String controller, String action)
        {
            Assert.True(provider.IsAuthorizedFor(null, null, controller, action));
        }

        #endregion

        #region Allow unauthorized

        [Theory]
        [InlineData(null, "NotAttributed", "UnauthorizedGet")]
        [InlineData(null, "NotAttributed", "UnauthorizedPost")]
        [InlineData(null, "NotAttributed", "UnauthorizedGetName")]
        [InlineData(null, "NotAttributed", "UnauthorizedPostName")]

        [InlineData(null, "Authorized", "UnauthorizedGet")]
        [InlineData(null, "Authorized", "UnauthorizedPost")]
        [InlineData(null, "Authorized", "UnauthorizedGetName")]
        [InlineData(null, "Authorized", "UnauthorizedPostName")]

        [InlineData("Area", "Authorized", "UnauthorizedGet")]
        [InlineData("Area", "Authorized", "UnauthorizedPost")]
        [InlineData("Area", "Authorized", "UnauthorizedGetName")]
        [InlineData("Area", "Authorized", "UnauthorizedPostName")]

        [InlineData(null, "AllowAnonymous", "UnauthorizedGet")]
        [InlineData(null, "AllowAnonymous", "UnauthorizedPost")]
        [InlineData(null, "AllowAnonymous", "UnauthorizedGetName")]
        [InlineData(null, "AllowAnonymous", "UnauthorizedPostName")]

        [InlineData(null, "AllowUnauthorized", "Get")]
        [InlineData(null, "AllowUnauthorized", "Post")]
        [InlineData(null, "AllowUnauthorized", "GetName")]
        [InlineData(null, "AllowUnauthorized", "PostName")]
        [InlineData(null, "AllowUnauthorized", "UnauthorizedGet")]
        [InlineData(null, "AllowUnauthorized", "UnauthorizedPost")]
        [InlineData(null, "AllowUnauthorized", "UnauthorizedGetName")]
        [InlineData(null, "AllowUnauthorized", "UnauthorizedPostName")]

        [InlineData(null, "InheritedAllowUnauthorized", "InheritanceGet")]
        [InlineData(null, "InheritedAllowUnauthorized", "InheritancePost")]
        [InlineData(null, "InheritedAllowUnauthorized", "InheritanceGetName")]
        [InlineData(null, "InheritedAllowUnauthorized", "InheritancePostName")]
        public void IsAuthorizedFor_AuthorizesUnauthorizedAction(String area, String controller, String action)
        {
            Assert.True(provider.IsAuthorizedFor(null, area, controller, action));
        }

        #endregion

        [Fact]
        public void IsAuthorizedFor_CanBeAuthorizedAsOtherAction()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizedGetName");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizedAsOther"));
        }

        [Fact]
        public void IsAuthorizedFor_CanBeAuthorizedAsOtherSelf()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "NotAttributed", "AuthorizedAsSelf");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "AuthorizedAsSelf"));
        }

        [Fact]
        public void IsAuthorizedFor_CachesAccountPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor(null, "Authorized", "AuthorizedGet");
            TearDownData();

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AuthorizedGet"));
        }

        [Fact]
        public void IsAuthorizedFor_IgnoresCase()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "AuthorizedGet");

            Assert.True(provider.IsAuthorizedFor(account.Id, "ArEa", "Authorized", "AUTHORIZEDGET"));
        }

        [Fact]
        public void IsAuthorizedFor_OnNotExistingActionThrows()
        {
            String actual = Assert.Throws<Exception>(() => provider.IsAuthorizedFor(null, null, "NotAttributed", "Test")).Message;
            String expected = "'NotAttributedController' does not have 'Test' action.";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Refresh()

        [Fact]
        public void Refresh_RefreshesPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor("Area", "Authorized", "AuthorizedGet");
            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedGet"));

            TearDownData();
            SetUpDependencyResolver();

            provider.Refresh();

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedGet"));
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
                context.Set<RolePrivilege>().RemoveRange(context.Set<RolePrivilege>());
                context.Set<Privilege>().RemoveRange(context.Set<Privilege>());
                context.Set<Account>().RemoveRange(context.Set<Account>());
                context.Set<Role>().RemoveRange(context.Set<Role>());
                context.SaveChanges();
            }
        }

        #endregion
    }
}
