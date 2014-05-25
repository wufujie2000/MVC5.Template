using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Template.Data.Core;
using Template.Objects;

namespace Template.Tests.Unit.Data.Migrations
{
    [TestFixture]
    public class InitialDataTests
    {
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new Context();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        #region Table: Languages

        [Test]
        public void LanguagesTable_HasEnglishBritishLanguage()
        {
            HasLanguage("en-GB", "English");
        }

        [Test]
        public void LanguagesTable_HasLithuanianLanguage()
        {
            HasLanguage("lt-LT", "Lietuvių");
        }

        private void HasLanguage(String abbrevation, String name)
        {
            Assert.IsNotNull(context.Set<Language>().SingleOrDefault(language =>
                language.Abbreviation == abbrevation &&
                language.Name == name));
        }

        #endregion

        #region Table: Privileges

        [Test]
        public void PrivilegesTable_HasPrivilegeForAdministrationAccountsIndex()
        {
            HasPrivilege("Administration", "Accounts", "Index");
        }

        [Test]
        public void PrivilegesTable_HasPrivilegeForAdministrationAccountsCreate()
        {
            HasPrivilege("Administration", "Accounts", "Create");
        }

        [Test]
        public void PrivilegesTable_HasPrivilegeForAdministrationAccountssDetails()
        {
            HasPrivilege("Administration", "Accounts", "Details");
        }

        [Test]
        public void PrivilegesTable_HasPrivilegeForAdministrationAccountsEdit()
        {
            HasPrivilege("Administration", "Accounts", "Edit");
        }

        [Test]
        public void PrivilegesTable_HasPrivilegeForAdministrationRolesIndex()
        {
            HasPrivilege("Administration", "Roles", "Index");
        }

        [Test]
        public void PrivilegesTable_HasPrivilegeForAdministrationRolesCreate()
        {
            HasPrivilege("Administration", "Roles", "Create");
        }

        [Test]
        public void PrivilegesTable_HasPrivilegeForAdministrationRolesDetails()
        {
            HasPrivilege("Administration", "Roles", "Details");
        }

        [Test]
        public void PrivilegesTable_HasPrivilegeForAdministrationRolesEdit()
        {
            HasPrivilege("Administration", "Roles", "Edit");
        }

        [Test]
        public void PrivilegesTable_HasPrivilegeForAdministrationRolesDelete()
        {
            HasPrivilege("Administration", "Roles", "Delete");
        }

        private void HasPrivilege(String area, String controller, String action)
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Area == area &&
                privilege.Controller == controller &&
                privilege.Action == action));
        }

        #endregion

        #region Table: Roles

        [Test]
        public void RolesTable_HasSysAdminRole()
        {
            Assert.IsNotNull(context.Set<Role>().SingleOrDefault(role => role.Name == "Sys_Admin"));
        }

        #endregion

        #region Table: RolePrivileges

        [Test]
        public void RolesPrivilegesTable_HasAllPrivilegesForAdminRole()
        {
            IEnumerable<String> expected = context.Set<Privilege>()
                .Select(privilege => privilege.Id);
            IEnumerable<String> actual = context.Set<Role>()
                .Single(role => role.Name == "Sys_Admin").RolePrivileges
                .Select(rolePrivilege => rolePrivilege.PrivilegeId);

            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion

        #region Table: Accounts

        [Test]
        public void AccountsTable_HasSysAdminAccountWithAdminRole()
        {
            Assert.IsNotNull(context.Set<Account>()
                .SingleOrDefault(account =>
                    account.Username == "admin" &&
                    account.Role.Name == "Sys_Admin"));
        }

        #endregion
    }
}
