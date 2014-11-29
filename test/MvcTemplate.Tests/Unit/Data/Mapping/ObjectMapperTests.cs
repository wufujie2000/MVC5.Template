using AutoMapper;
using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Data.Mapping
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
            Account model = ObjectFactory.CreateAccount();
            model.Role = ObjectFactory.CreateRole();
            model.RoleId = model.Role.Id;

            AccountView actual = Mapper.Map<AccountView>(model);
            Account expected = model;

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
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
            Account actual = Mapper.Map<Account>(expected);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.IsNull(actual.RecoveryTokenExpirationDate);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.RecoveryToken);
            Assert.IsNull(actual.Passhash);
            Assert.IsNull(actual.Role);
        }

        [Test]
        public void MapAccounts_MapsAccountToAccountEditView()
        {
            Account model = ObjectFactory.CreateAccount();
            model.Role = ObjectFactory.CreateRole();
            model.RoleId = model.Role.Id;

            AccountEditView actual = Mapper.Map<AccountEditView>(model);
            Account expected = model;

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Role.Name, actual.RoleName);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void MapAccounts_MapsAccountEditViewToAccount()
        {
            AccountEditView expected = ObjectFactory.CreateAccountEditView();
            Account actual = Mapper.Map<Account>(expected);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.IsNull(actual.RecoveryTokenExpirationDate);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.RecoveryToken);
            Assert.IsNull(actual.Passhash);
            Assert.IsNull(actual.Email);
            Assert.IsNull(actual.Role);
        }

        [Test]
        public void MapAccounts_MapsAccountToProfileEditView()
        {
            Account expected = ObjectFactory.CreateAccount();
            ProfileEditView actual = Mapper.Map<ProfileEditView>(expected);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.NewPassword);
            Assert.IsNull(actual.Password);
        }

        [Test]
        public void MapAccounts_MapsProfileEditViewToAccount()
        {
            ProfileEditView expected = ObjectFactory.CreateProfileEditView();
            Account actual = Mapper.Map<Account>(expected);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.IsNull(actual.RecoveryTokenExpirationDate);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.RecoveryToken);
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
            RoleView actual = Mapper.Map<RoleView>(expected);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNotNull(actual.PrivilegesTree);
        }

        [Test]
        public void MapRoles_MapsRoleViewToRole()
        {
            RoleView expected = ObjectFactory.CreateRoleView();
            Role actual = Mapper.Map<Role>(expected);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.RolePrivileges);
        }

        [Test]
        public void MapRoles_MapsRolePrivilegeToRolePrivilegeView()
        {
            RolePrivilege model = ObjectFactory.CreateRolePrivilege();
            model.Privilege = ObjectFactory.CreatePrivilege();
            model.Role = ObjectFactory.CreateRole();
            model.PrivilegeId = model.Privilege.Id;
            model.RoleId = model.Role.Id;

            RolePrivilegeView actual = Mapper.Map<RolePrivilegeView>(model);
            RolePrivilege expected = model;

            Assert.AreEqual(expected.Privilege.CreationDate, actual.Privilege.CreationDate);
            Assert.AreEqual(expected.Privilege.Controller, actual.Privilege.Controller);
            Assert.AreEqual(expected.Privilege.Action, actual.Privilege.Action);
            Assert.AreEqual(expected.Privilege.Area, actual.Privilege.Area);
            Assert.AreEqual(expected.Privilege.Id, actual.Privilege.Id);
            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.PrivilegeId, actual.PrivilegeId);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void MapRoles_MapsRolePrivilegeViewToRolePrivilege()
        {
            RolePrivilegeView view = ObjectFactory.CreateRolePrivilegeView();
            view.Privilege = ObjectFactory.CreatePrivilegeView();
            view.PrivilegeId = view.Privilege.Id;

            RolePrivilege actual = Mapper.Map<RolePrivilege>(view);
            RolePrivilegeView expected = view;

            Assert.AreEqual(expected.Privilege.CreationDate, actual.Privilege.CreationDate);
            Assert.AreEqual(expected.Privilege.Controller, actual.Privilege.Controller);
            Assert.AreEqual(expected.Privilege.Action, actual.Privilege.Action);
            Assert.AreEqual(expected.Privilege.Area, actual.Privilege.Area);
            Assert.AreEqual(expected.Privilege.Id, actual.Privilege.Id);
            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.PrivilegeId, actual.PrivilegeId);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Role);
        }

        [Test]
        public void MapRoles_MapsPrivilegeToPrivilegeView()
        {
            Privilege expected = ObjectFactory.CreatePrivilege();
            PrivilegeView actual = Mapper.Map<PrivilegeView>(expected);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Controller, actual.Controller);
            Assert.AreEqual(expected.Action, actual.Action);
            Assert.AreEqual(expected.Area, actual.Area);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void MapRoles_MapsPrivilegeViewToPrivilege()
        {
            PrivilegeView expected = ObjectFactory.CreatePrivilegeView();
            Privilege actual = Mapper.Map<Privilege>(expected);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Controller, actual.Controller);
            Assert.AreEqual(expected.Action, actual.Action);
            Assert.AreEqual(expected.Area, actual.Area);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion
    }
}
