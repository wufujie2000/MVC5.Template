using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;

namespace MvcTemplate.Tests.Unit.Data.Migrations
{
    [TestFixture]
    public class InitialDataTests
    {
        private Context context;

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

        #region Table: Privileges

        [Test]
        [TestCase("Administration", "Accounts", "Index")]
        [TestCase("Administration", "Accounts", "Details")]
        [TestCase("Administration", "Accounts", "Edit")]

        [TestCase("Administration", "Roles", "Index")]
        [TestCase("Administration", "Roles", "Create")]
        [TestCase("Administration", "Roles", "Details")]
        [TestCase("Administration", "Roles", "Edit")]
        [TestCase("Administration", "Roles", "Delete")]
        public void PrivilegesTable_HasPrivilege(String area, String controller, String action)
        {
            Assert.IsNotNull(context.Set<Privilege>().SingleOrDefault(privilege =>
                privilege.Controller == controller &&
                privilege.Action == action &&
                privilege.Area == area));
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
            IEnumerable expected = context
                .Set<Privilege>()
                .Select(privilege => privilege.Id);
            IEnumerable actual = context
                .Set<Role>()
                .Single(role => role.Name == "Sys_Admin")
                .RolePrivileges
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
