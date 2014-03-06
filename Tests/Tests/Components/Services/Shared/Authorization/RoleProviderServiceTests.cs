using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Template.Components.Security;
using Template.Components.Services;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;
using Template.Web.IoC;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Services
{
    [TestFixture]
    public class RoleProviderServiceTests
    {
        private RoleProviderService service;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpContextBaseMock().HttpContext;
            service = new RoleProviderService(new UnitOfWork());
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
            var context = new TestingContext();
            var testId = TestContext.CurrentContext.Test.Name;
            foreach (var user in context.Set<User>().Where(user => user.Id.StartsWith(testId)))
                context.Set<User>().Remove(user);

            foreach (var role in context.Set<Role>().Where(role => role.Id.StartsWith(testId)))
                context.Set<Role>().Remove(role);

            foreach (var privilege in context.Set<Privilege>().Where(privilege => privilege.Id.StartsWith(testId)))
                context.Set<Privilege>().Remove(privilege);

            context.SaveChanges();
        }

        #region Static property: Instance

        [Test]
        public void Instance_ReturnsSameInstance()
        {
            Assert.AreEqual(NinjectContainer.Resolve<IRoleProvider>(), NinjectContainer.Resolve<IRoleProvider>());
        }

        #endregion

        #region Method: IsAuthorizedForAction(String action)

        [Test]
        public void IsAuthorizedForAction_IsAuthorizedForActionWithAllowAnonymousAttribute()
        {
            HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] = "Account";
            Assert.IsTrue(service.IsAuthorizedForAction("Login"));
        }

        [Test]
        public void IsAuthorizedForAction_IsAuthorizedForActionWithAllowUnauthorizedAttribute()
        {
            HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] = "Account";
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
            HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] = "Home";
            Assert.IsTrue(service.IsAuthorizedForAction("Index"));
        }

        [Test]
        public void IsAuthorizedForAction_IsNotAuthorizedForAction()
        {
            HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] = "Users";
            Assert.IsFalse(service.IsAuthorizedForAction("Index"));
        }

        [Test]
        public void IsAuthorizedForAction_IsAuthorizedForAction()
        {
            var account = ObjectFactory.CreateAccount();
            account.User = ObjectFactory.CreateUser();
            var role = ObjectFactory.CreateRole();
            account.UserId = account.User.Id;
            account.User.RoleId = role.Id;

            var context = new TestingContext();

            role.RolePrivileges = new List<RolePrivilege>();
            var rolePrivilege = ObjectFactory.CreateRolePrivilege();
            var privilege = ObjectFactory.CreatePrivilege();
            privilege.Controller = "Users";
            privilege.Action = "Index";
            privilege.Area = null;

            rolePrivilege.PrivilegeId = privilege.Id;
            rolePrivilege.Privilege = privilege;
            rolePrivilege.RoleId = role.Id;
            rolePrivilege.Role = role;

            role.RolePrivileges.Add(rolePrivilege);

            context.Set<Role>().Add(role);
            context.Set<Account>().Add(account);
            context.SaveChanges();

            HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] = "Users";

            Assert.IsTrue(service.IsAuthorizedForAction("Index"));
        }

        #endregion
    }
}
