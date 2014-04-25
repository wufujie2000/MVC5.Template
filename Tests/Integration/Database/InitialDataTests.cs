using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Template.Data.Core;
using Template.Objects;

namespace Template.Tests.Integration.Database
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
            String expected = "English";
            String actual = context.Set<Language>().SingleOrDefault(language => language.Abbreviation == "en-GB").Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LanguagesTable_HasLithuanianLanguage()
        {
            String expected = "Lietuvių";
            String actual = context.Set<Language>().SingleOrDefault(language => language.Abbreviation == "lt-LT").Name;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Table: Privileges

        [Test]
        public void PrivilegesTable_HasAdministrationUsersIndexPrivilege()
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Area == "Administration" &&
                privilege.Controller == "Users" &&
                privilege.Action == "Index"));
        }

        [Test]
        public void PrivilegesTable_HasAdministrationUsersCreatePrivilege()
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Area == "Administration" &&
                privilege.Controller == "Users" &&
                privilege.Action == "Create"));
        }

        [Test]
        public void PrivilegesTable_HasAdministrationUsersDetailsPrivilege()
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Area == "Administration" &&
                privilege.Controller == "Users" &&
                privilege.Action == "Details"));
        }

        [Test]
        public void PrivilegesTable_HasAdministrationUsersEditPrivilege()
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Area == "Administration" &&
                privilege.Controller == "Users" &&
                privilege.Action == "Edit"));
        }

        [Test]
        public void PrivilegesTable_HasAdministrationUsersDeletePrivilege()
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Area == "Administration" &&
                privilege.Controller == "Users" &&
                privilege.Action == "Delete"));
        }

        [Test]
        public void PrivilegesTable_HasAdministrationRolesIndexPrivilege()
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Area == "Administration" &&
                privilege.Controller == "Roles" &&
                privilege.Action == "Index"));
        }

        [Test]
        public void PrivilegesTable_HasAdministrationRolesCreatePrivilege()
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Area == "Administration" &&
                privilege.Controller == "Roles" &&
                privilege.Action == "Create"));
        }

        [Test]
        public void PrivilegesTable_HasAdministrationRolesDetailsPrivilege()
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Area == "Administration" &&
                privilege.Controller == "Roles" &&
                privilege.Action == "Details"));
        }

        [Test]
        public void PrivilegesTable_HasAdministrationRolesEditPrivilege()
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Area == "Administration" &&
                privilege.Controller == "Roles" &&
                privilege.Action == "Edit"));
        }

        [Test]
        public void PrivilegesTable_HasAdministrationRolesDeletePrivilege()
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Area == "Administration" &&
                privilege.Controller == "Roles" &&
                privilege.Action == "Delete"));
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
                    account.Person.Role.Name == "Sys_Admin"));
        }

        #endregion
    }
}
