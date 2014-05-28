using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Template.Components.Security;
using Template.Controllers;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Security
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
            provider = new RoleProvider(typeof(BaseController).Assembly, new UnitOfWork(context));

            TearDownData();
        }

        #region Method: GetAccountPrivileges(String accountId)

        [Test]
        public void GetAccountPrivileges_GetAccoountsPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor("Administration", "Roles", "Index");

            IEnumerable<AccountPrivilege> actual = provider.GetAccountPrivileges(account.Id);
            IEnumerable<AccountPrivilege> expected = context.Set<Account>()
                .First(acc => acc.Id == account.Id)
                .Role
                .RolePrivileges
                .Select(role => new AccountPrivilege
                    {
                        AccountId = account.Id,

                        Area = role.Privilege.Area,
                        Controller = role.Privilege.Controller,
                        Action = role.Privilege.Action
                    });

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetAccountPrivileges_GetsNoPrivilegesForAccountWithoutRole()
        {
            IEnumerable<AccountPrivilege> actual = provider.GetAccountPrivileges(CreateAccount().Id);

            CollectionAssert.IsEmpty(actual);
        }

        [Test]
        public void GetAccountPrivileges_GetsNoPrivilegesForNotExistingAccount()
        {
            IEnumerable<AccountPrivilege> actual = provider.GetAccountPrivileges("Test");

            CollectionAssert.IsEmpty(actual);
        }

        #endregion

        #region Method: IsAuthorizedFor(String accountId, String area, String controller, String action)

        [Test]
        [Ignore]
        public void IsAuthorizedFor_IsAuthorizedForActionWithAllowAnonymousAttribute()
        {
            Assert.Inconclusive("There is no controller with allow anonymous action.");
        }

        [Test]
        [Ignore]
        public void IsAuthorizedFor_IsAuthorizedForActionWithAllowUnauthorizedAttribute()
        {
            Assert.Inconclusive("There is no controller with allow unauthorized action.");
        }

        [Test]
        public void IsAuthorizedFor_IsAuthorizedForActionOnControllerWithAllowAnonymousAttribute()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "Auth", "Login"));
        }

        [Test]
        public void IsAuthorizedFor_IsAuthorizedForActionOnControllerWithAllowUnauthorizedAttribute()
        {
            Assert.IsTrue(provider.IsAuthorizedFor((String)null, null, "Home", "Index"));
        }

        [Test]
        public void IsAuthorizedFor_IsNotAuthorizedForAction()
        {
            Assert.IsFalse(provider.IsAuthorizedFor((String)null, "Administration", "Roles", "Index"));
        }

        [Test]
        public void IsAuthorizedFor_OnNotExistingActionThrows()
        {
            Account account = CreateAccountWithPrivilegeFor("Administration", "Roles", "Test");
            String expectedMessage = "'RolesController' does not have 'Test' action";

            Assert.Throws<Exception>(() => provider.IsAuthorizedFor(account.Id, "Administration", "Roles", "Test"), expectedMessage);
        }

        [Test]
        public void IsAuthorizedFor_IsAuthorizedForGetAction()
        {
            Account account = CreateAccountWithPrivilegeFor("Administration", "Roles", "Index");

            Assert.IsTrue(provider.IsAuthorizedFor(account.Id, "Administration", "Roles", "Index"));
        }

        [Test]
        [Ignore]
        public void IsAuthorizedFor_IsAuthorizedForNonGetAction()
        {
            Assert.Inconclusive();
        }

        #endregion

        #region Method: IsAuthorizedFor(IEnumerable<AccountPrivilege> privileges, String area, String controller, String action)

        [Test]
        [Ignore]
        public void IsAuthorizedFor_IsAuthorizedForActionWithAllowAnonymousAttributeWithoutPrivileges()
        {
            Assert.Inconclusive("There is no controller with allow anonymous action.");
        }

        [Test]
        [Ignore]
        public void IsAuthorizedFor_IsAuthorizedForActionrWithAllowUnauthorizedAttributeWithoutPrivileges()
        {
            Assert.Inconclusive("There is no controller with allow unauthorized action.");
        }

        [Test]
        public void IsAuthorizedFor_IsAuthorizedForActionOnControllerWithAllowAnonymousAttributeWithoutPrivileges()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "Auth", "Login"));
        }

        [Test]
        public void IsAuthorizedFor_IsAuthorizedForActionOnControllerWithAllowUnauthorizedAttributeWithoutPrivileges()
        {
            Assert.IsTrue(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), null, "Home", "Index"));
        }

        [Test]
        public void IsAuthorizedFor_IsNotAuthorizedForActionWithoutPrivileges()
        {
            Assert.IsFalse(provider.IsAuthorizedFor(Enumerable.Empty<AccountPrivilege>(), "Administration", "Roles", "Index"));
        }

        [Test]
        public void IsAuthorizedFor_IsAuthorizedForGetActionWithPrivileges()
        {
            Account account = CreateAccountWithPrivilegeFor("Administration", "Roles", "Index");
            IEnumerable<AccountPrivilege> privileges = new List<AccountPrivilege>()
            {
                new AccountPrivilege()
                {
                    Area = "Administration",
                    Controller = "Roles",
                    Action = "Index"
                }
            };

            Assert.IsTrue(provider.IsAuthorizedFor(privileges, "Administration", "Roles", "Index"));
        }

        [Test]
        [Ignore]
        public void IsAuthorizedFor_IsAuthorizedForNonGetActionWithPrivileges()
        {
            Assert.Inconclusive();
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_CanBeDisposedMultipleTimes()
        {
            provider.Dispose();
            provider.Dispose();
        }

        #endregion

        #region Test helpers

        private Account CreateAccount()
        {
            Account account = ObjectFactory.CreateAccount();
            context.Set<Account>().Add(account);
            context.SaveChanges();

            return account;
        }
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

            return account;
        }
        private void TearDownData()
        {
            context.Set<Privilege>().RemoveRange(context.Set<Privilege>().Where(privilege => privilege.Id.StartsWith(ObjectFactory.TestId)));
            context.Set<Account>().RemoveRange(context.Set<Account>().Where(account => account.Id.StartsWith(ObjectFactory.TestId)));
            context.Set<Role>().RemoveRange(context.Set<Role>().Where(role => role.Id.StartsWith(ObjectFactory.TestId)));

            context.SaveChanges();
        }

        #endregion
    }
}
