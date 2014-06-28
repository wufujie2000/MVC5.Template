using AutoMapper;
using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;

namespace MvcTemplate.Tests.Data.Mapping
{
    [TestFixture]
    public class ObjectMapperTests
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ObjectMapper.MapObjects();
        }

        #region Static method: MapAccounts()

        [Test]
        public void MapAccounts_MapsAccountToAccountView()
        {
            Account expected = ObjectFactory.CreateAccount();
            expected.Role = ObjectFactory.CreateRole();
            expected.RoleId = expected.Role.Id;

            AccountView actual = Mapper.Map<Account, AccountView>(expected);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Role.Name, actual.RoleName);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Password);
        }

        [Test]
        public void MapAccounts_MapsAccountViewToAccount()
        {
            AccountView expected = ObjectFactory.CreateAccountView();
            Account actual = Mapper.Map<AccountView, Account>(expected);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Passhash);
            Assert.IsNull(actual.Role);
        }

        [Test]
        public void MapAccounts_MapsAccountToAccountEditView()
        {
            Account expected = ObjectFactory.CreateAccount();
            expected.Role = ObjectFactory.CreateRole();
            expected.RoleId = expected.Role.Id;

            AccountEditView actual = Mapper.Map<Account, AccountEditView>(expected);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Role.Name, actual.RoleName);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void MapAccounts_MapsAccountEditViewToAccount()
        {
            AccountEditView expected = ObjectFactory.CreateAccountEditView();
            expected.RoleName = "Not used property";
            expected.RoleId = expected.Id;

            Account actual = Mapper.Map<AccountEditView, Account>(expected);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Passhash);
            Assert.IsNull(actual.Role);
        }

        [Test]
        public void MapAccounts_MapsAccountToProfileEditView()
        {
            Account expected = ObjectFactory.CreateAccount();
            ProfileEditView actual = Mapper.Map<Account, ProfileEditView>(expected);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Password);
            Assert.IsNull(actual.NewPassword);
        }

        [Test]
        public void MapAccounts_MapsProfileEditViewToAccount()
        {
            ProfileEditView expected = ObjectFactory.CreateProfileEditView();
            Account actual = Mapper.Map<ProfileEditView, Account>(expected);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Passhash);
            Assert.IsNull(actual.RoleId);
            Assert.IsNull(actual.Role);
        }

        #endregion

        #region Static method: MapRoles()

        [Test]
        public void MapRoles_MapsRoleToRoleView()
        {
            Role expected = ObjectFactory.CreateRole();
            RoleView actual = Mapper.Map<Role, RoleView>(expected);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNotNull(actual.PrivilegesTree);
        }

        [Test]
        public void MapRoles_MapsRoleViewToRole()
        {
            RoleView expected = ObjectFactory.CreateRoleView();
            Role actual = Mapper.Map<RoleView, Role>(expected);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.RolePrivileges);
        }

        [Test]
        public void MapRoles_MapsRolePrivilegeToRolePrivilegeView()
        {
            RolePrivilege expected = ObjectFactory.CreateRolePrivilege();
            expected.Privilege = ObjectFactory.CreatePrivilege();
            expected.PrivilegeId = expected.Privilege.Id;
            expected.Role = ObjectFactory.CreateRole();
            expected.RoleId = expected.Role.Id;

            RolePrivilegeView actual = Mapper.Map<RolePrivilege, RolePrivilegeView>(expected);

            Assert.AreEqual(expected.Privilege.EntityDate, actual.Privilege.EntityDate);
            Assert.AreEqual(expected.Privilege.Controller, actual.Privilege.Controller);
            Assert.AreEqual(expected.Privilege.Action, actual.Privilege.Action);
            Assert.AreEqual(expected.Privilege.Area, actual.Privilege.Area);
            Assert.AreEqual(expected.Privilege.Id, actual.Privilege.Id);
            Assert.AreEqual(expected.PrivilegeId, actual.PrivilegeId);
            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void MapRoles_MapsRolePrivilegeViewToRolePrivilege()
        {
            RolePrivilegeView expected = ObjectFactory.CreateRolePrivilegeView();
            expected.Privilege = ObjectFactory.CreatePrivilegeView();
            expected.PrivilegeId = expected.Privilege.Id;

            RolePrivilege actual = Mapper.Map<RolePrivilegeView, RolePrivilege>(expected);

            Assert.AreEqual(expected.Privilege.EntityDate, actual.Privilege.EntityDate);
            Assert.AreEqual(expected.Privilege.Controller, actual.Privilege.Controller);
            Assert.AreEqual(expected.Privilege.Action, actual.Privilege.Action);
            Assert.AreEqual(expected.Privilege.Area, actual.Privilege.Area);
            Assert.AreEqual(expected.Privilege.Id, actual.Privilege.Id);
            Assert.AreEqual(expected.PrivilegeId, actual.PrivilegeId);
            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Role);
        }

        [Test]
        public void MapRoles_MapsPrivilegeToPrivilegeView()
        {
            Privilege expected = ObjectFactory.CreatePrivilege();
            PrivilegeView actual = Mapper.Map<Privilege, PrivilegeView>(expected);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Controller, actual.Controller);
            Assert.AreEqual(expected.Action, actual.Action);
            Assert.AreEqual(expected.Area, actual.Area);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void MapRoles_MapsPrivilegeViewToPrivilege()
        {
            PrivilegeView expected = ObjectFactory.CreatePrivilegeView();
            Privilege actual = Mapper.Map<PrivilegeView, Privilege>(expected);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Controller, actual.Controller);
            Assert.AreEqual(expected.Action, actual.Action);
            Assert.AreEqual(expected.Area, actual.Area);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion
    }
}
