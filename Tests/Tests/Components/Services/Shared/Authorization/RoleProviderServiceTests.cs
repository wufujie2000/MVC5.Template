using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Template.Components.Services;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Services
{
    [TestFixture]
    public class RoleProviderServiceTests
    {
        private RoleProviderService service;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            service = new RoleProviderService(new UnitOfWork(context));
            service.HttpContext = new HttpContextBaseMock().HttpContextBase;
        }

        [TearDown]
        public void TearDown()
        {
            TearDownData();

            service.Dispose();
            context.Dispose();
        }

        #region Method: IsAuthorizedForAction(String action)

        [Test]
        public void IsAuthorizedForAction_IsAuthorizedForActionWithAllowAnonymousAttribute()
        {
            service.HttpContext.Request.RequestContext.RouteData.Values["controller"] = "Account";
            Assert.IsTrue(service.IsAuthorizedForAction("Login"));
        }

        [Test]
        public void IsAuthorizedForAction_IsAuthorizedForActionWithAllowUnauthorizedAttribute()
        {
            service.HttpContext.Request.RequestContext.RouteData.Values["controller"] = "Account";
            Assert.IsTrue(service.IsAuthorizedForAction("Logout"));
        }

        [Test]
        [Ignore]
        public void IsAuthorizedForAction_IsAuthorizedForActionOnControllerWithAllowAnonymousAttribute()
        {
            
        }

        [Test]
        public void IsAuthorizedForAction_IsAuthorizedForActionOnControllerWithAllowUnauthorizedAttribute()
        {
            service.HttpContext.Request.RequestContext.RouteData.Values["controller"] = "Home";
            Assert.IsTrue(service.IsAuthorizedForAction("Index"));
        }

        [Test]
        public void IsAuthorizedForAction_IsNotAuthorizedForAction()
        {
            service.HttpContext.Request.RequestContext.RouteData.Values["controller"] = "Users";
            Assert.IsFalse(service.IsAuthorizedForAction("Index"));
        }

        [Test]
        public void IsAuthorizedForAction_IsAuthorizedForGetAction()
        {
            service.HttpContext.Request.RequestContext.RouteData.Values["area"] = "Administration";
            service.HttpContext.Request.RequestContext.RouteData.Values["controller"] = "Users";
            CreateUserWithPrivilegeFor("Administration", "Users", "Index");

            Assert.IsTrue(service.IsAuthorizedForAction("Index"));
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
