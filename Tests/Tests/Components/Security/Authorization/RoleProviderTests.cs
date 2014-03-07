using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;
using Tests.Helpers;

namespace Template.Tests.Tests.Security
{
    [TestFixture]
    public class RoleProviderTests
    {
        private HttpContextBase httpContext;
        private RoleProvider provider;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            httpContext = new HttpContextBaseMock().HttpContextBase;
            provider = new RoleProvider(httpContext, new UnitOfWork(context));
        }

        [TearDown]
        public void TearDown()
        {
            TearDownData();

            provider.Dispose();
            context.Dispose();
        }

        #region Method: IsAuthorizedForAction(String action)

        [Test]
        public void IsAuthorizedForAction_IsAuthorizedForActionWithAllowAnonymousAttribute()
        {
            httpContext.Request.RequestContext.RouteData.Values["controller"] = "Account";
            Assert.IsTrue(provider.IsAuthorizedForAction("Login"));
        }

        [Test]
        public void IsAuthorizedForAction_IsAuthorizedForActionWithAllowUnauthorizedAttribute()
        {
            httpContext.Request.RequestContext.RouteData.Values["controller"] = "Account";
            Assert.IsTrue(provider.IsAuthorizedForAction("Logout"));
        }

        [Test]
        [Ignore]
        public void IsAuthorizedForAction_IsAuthorizedForActionOnControllerWithAllowAnonymousAttribute()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void IsAuthorizedForAction_IsAuthorizedForActionOnControllerWithAllowUnauthorizedAttribute()
        {
            httpContext.Request.RequestContext.RouteData.Values["controller"] = "Home";
            Assert.IsTrue(provider.IsAuthorizedForAction("Index"));
        }

        [Test]
        public void IsAuthorizedForAction_IsNotAuthorizedForAction()
        {
            httpContext.Request.RequestContext.RouteData.Values["controller"] = "Users";
            Assert.IsFalse(provider.IsAuthorizedForAction("Index"));
        }

        [Test]
        public void IsAuthorizedForAction_IsAuthorizedForGetAction()
        {
            httpContext.Request.RequestContext.RouteData.Values["area"] = "Administration";
            httpContext.Request.RequestContext.RouteData.Values["controller"] = "Users";
            CreateUserWithPrivilegeFor("Administration", "Users", "Index");

            Assert.IsTrue(provider.IsAuthorizedForAction("Index"));
        }

        [Test]
        [Ignore]
        public void IsAuthorizedForAction_IsAuthorizedForPostAction()
        {
            Assert.Inconclusive();
        }

        #endregion

        #region Test helpers

        private Role CreateUserWithPrivilegeFor(String area, String controller, String action)
        {
            var account = ObjectFactory.CreateAccount();
            account.User = ObjectFactory.CreateUser();
            account.UserId = account.User.Id;

            var role = ObjectFactory.CreateRole();
            account.User.RoleId = role.Id;
            context.Set<Account>().Add(account);

            role.RolePrivileges = new List<RolePrivilege>();
            var rolePrivilege = ObjectFactory.CreateRolePrivilege();
            var privilege = ObjectFactory.CreatePrivilege();
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

            return role;
        }
        private void TearDownData()
        {
            var testId = TestContext.CurrentContext.Test.Name;
            foreach (var user in context.Set<User>().Where(user => user.Id.StartsWith(testId)))
                context.Set<User>().Remove(user);

            foreach (var role in context.Set<Role>().Where(role => role.Id.StartsWith(testId)))
                context.Set<Role>().Remove(role);

            foreach (var privilege in context.Set<Privilege>().Where(privilege => privilege.Id.StartsWith(testId)))
                context.Set<Privilege>().Remove(privilege);

            context.SaveChanges();
        }

        #endregion
    }
}
