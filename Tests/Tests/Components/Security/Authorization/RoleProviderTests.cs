using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Tests.Security
{
    [TestFixture]
    public class RoleProviderTests
    {
        private RoleProvider provider;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            provider = new RoleProvider(new UnitOfWork(context));
        }

        [TearDown]
        public void TearDown()
        {
            TearDownData();

            context.Dispose();
            provider.Dispose();
        }

        #region Method: IsAuthorizedFor(String accountId, String area, String controller, String action)

        [Test]
        public void IsAuthorizedFor_IsAuthorizedForActionWithAllowAnonymousAttribute()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "Account", "Login"));
        }

        [Test]
        public void IsAuthorizedFor_IsAuthorizedForActionrWithAllowUnauthorizedAttribute()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "Account", "Logout"));
        }

        [Test]
        [Ignore]
        public void IsAuthorizedFor_IsAuthorizedForActionOnControllerWithAllowAnonymousAttribute()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void IsAuthorizedFor_IsAuthorizedForActionOnControllerWithAllowUnauthorizedAttribute()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(null, null, "Home", "Index"));
        }

        [Test]
        public void IsAuthorizedFor_IsNotAuthorizedForAction()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(null, "Administration", "Users", "Index"));
        }

        [Test]
        public void IsAuthorizedFor_IsAuthorizedForGetAction()
        {
            var account = CreateUserWithPrivilegeFor("Administration", "Users", "Index");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, "Administration", "Users", "Index"));
        }

        [Test]
        [Ignore]
        public void IsAuthorizedFor_IsAuthorizedForPostAction()
        {
            Assert.Inconclusive();
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_CanDisposeMultipleTimes()
        {
            provider.Dispose();
            provider.Dispose();
        }

        #endregion

        #region Test helpers

        private Account CreateUserWithPrivilegeFor(String area, String controller, String action)
        {
            var account = ObjectFactory.CreateAccount();
            account.Person = ObjectFactory.CreatePerson();
            account.PersonId = account.Person.Id;

            var role = ObjectFactory.CreateRole();
            account.Person.RoleId = role.Id;
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

            return account;
        }
        private void TearDownData()
        {
            context = new TestingContext();

            foreach (var person in context.Set<Person>().Where(person => person.Id.StartsWith(ObjectFactory.TestId)))
                context.Set<Person>().Remove(person);

            foreach (var role in context.Set<Role>().Where(role => role.Id.StartsWith(ObjectFactory.TestId)))
                context.Set<Role>().Remove(role);

            foreach (var privilege in context.Set<Privilege>().Where(privilege => privilege.Id.StartsWith(ObjectFactory.TestId)))
                context.Set<Privilege>().Remove(privilege);

            context.SaveChanges();
        }

        #endregion
    }
}
