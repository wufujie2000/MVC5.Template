using NUnit.Framework;
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
            var actualLanguage = context.Set<Language>().SingleOrDefault(language => language.Abbreviation == "en-GB");
            var expectedLanguageName = "English";

            Assert.AreEqual(expectedLanguageName, actualLanguage.Name);
        }

        [Test]
        public void LanguagesTable_HasLithuanianLanguage()
        {
            var actualLanguage = context.Set<Language>().SingleOrDefault(language => language.Abbreviation == "lt-LT");
            var expectedLanguageName = "Lietuvių";

            Assert.AreEqual(expectedLanguageName, actualLanguage.Name);
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
            var adminRole = context.Set<Role>().Single(role => role.Name == "Sys_Admin");
            var allPrivileges = context.Set<Privilege>();

            foreach (var privilege in allPrivileges)
                Assert.IsNotNull(context.Set<RolePrivilege>().SingleOrDefault(rolePrivilege =>
                    rolePrivilege.RoleId == adminRole.Id &&
                    rolePrivilege.PrivilegeId == privilege.Id));
        }

        #endregion

        #region Table: Accounts

        [Test]
        public void AccountsTable_HasSysAdminAccountWithAdminRole()
        {
            var expectedRole = context.Set<Role>().Single(role => role.Name == "Sys_Admin").Id;
            var actualRole = context.Set<Account>().FirstOrDefault(account => account.Username == "admin").Person.RoleId;

            Assert.AreEqual(expectedRole, actualRole);
        }

        #endregion
    }
}
