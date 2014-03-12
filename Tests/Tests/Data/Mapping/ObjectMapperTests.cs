using AutoMapper;
using NUnit.Framework;
using System.Collections.Generic;
using Template.Data.Mapping;
using Template.Objects;
using Template.Tests.Helpers;

namespace Template.Tests.Data.Mapping
{
    [TestFixture]
    public class ObjectMapperTests
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ObjectMapper.MapObjects();
        }

        #region Static method: MapUsers()

        [Test]
        public void MapUsers_MapsAccountToProfileView()
        {
            var expected = ObjectFactory.CreateAccount();
            expected.User = ObjectFactory.CreateUser();

            var actual = Mapper.Map<Account, ProfileView>(expected);

            Assert.AreEqual(expected.User.DateOfBirth, actual.UserDateOfBirth);
            Assert.AreEqual(expected.User.FirstName, actual.UserFirstName);
            Assert.AreEqual(expected.User.LastName, actual.UserLastName);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.CurrentPassword);
            Assert.IsNull(actual.NewPassword);
        }

        [Test]
        public void MapUsers_MapsAccountToAccountView()
        {
            var expected = ObjectFactory.CreateAccount();
            var actual = Mapper.Map<Account, AccountView>(expected);

            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.NewPassword);
            Assert.IsNull(actual.Password);
        }

        [Test]
        public void MapUsers_MapsAccountViewToAccount()
        {
            var expected = ObjectFactory.CreateAccountView();
            var actual = Mapper.Map<AccountView, Account>(expected);

            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Passhash);
            Assert.IsNull(actual.UserId);
            Assert.IsNull(actual.User);
        }

        [Test]
        public void MapUsers_MapsAccountToUserView()
        {
            var expected = ObjectFactory.CreateAccount();
            expected.User = ObjectFactory.CreateUser();
            expected.UserId = expected.User.Id;

            expected.User.Role = ObjectFactory.CreateRole();
            expected.User.RoleId = expected.User.Role.Id;

            var actual = Mapper.Map<Account, UserView>(expected);

            Assert.AreEqual(expected.User.DateOfBirth, actual.UserDateOfBirth);
            Assert.AreEqual(expected.User.FirstName, actual.UserFirstName);
            Assert.AreEqual(expected.User.LastName, actual.UserLastName);

            Assert.AreEqual(expected.User.Role.Name, actual.UserRoleName);
            Assert.AreEqual(expected.User.Role.Id, actual.UserRoleId);
            Assert.AreEqual(expected.User.RoleId, actual.UserRoleId);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.NewPassword);
            Assert.IsNull(actual.Password);
        }

        [Test]
        public void MapUsers_MapsUserViewToAccount()
        {
            var expected = ObjectFactory.CreateUserView();
            var actual = Mapper.Map<UserView, Account>(expected);

            Assert.AreEqual(expected.Id, actual.UserId);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Passhash);
            Assert.IsNull(actual.User);
        }

        [Test]
        public void MapUsers_MapsUserToUserView()
        {
            var expected = ObjectFactory.CreateUser();
            expected.Role = ObjectFactory.CreateRole();
            expected.RoleId = expected.Role.Id;

            var actual = Mapper.Map<User, UserView>(expected);

            Assert.AreEqual(expected.DateOfBirth, actual.UserDateOfBirth);
            Assert.AreEqual(expected.FirstName, actual.UserFirstName);
            Assert.AreEqual(expected.LastName, actual.UserLastName);
            Assert.AreEqual(expected.Id, actual.Id);

            Assert.AreEqual(expected.Role.Name, actual.UserRoleName);
            Assert.AreEqual(expected.RoleId, actual.UserRoleId);

            Assert.IsNull(actual.Username);
            Assert.IsNull(actual.Password);
            Assert.IsNull(actual.NewPassword);
        }

        [Test]
        public void MapUsers_MapsUserViewToUser()
        {
            var expected = ObjectFactory.CreateUserView();
            var actual = Mapper.Map<UserView, User>(expected);

            Assert.AreEqual(expected.UserDateOfBirth, actual.DateOfBirth);
            Assert.AreEqual(expected.UserFirstName, actual.FirstName);
            Assert.AreEqual(expected.UserLastName, actual.LastName);
            Assert.AreEqual(expected.UserRoleId, actual.RoleId);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Role);
        }

        #endregion

        #region Static method: MapRoles()

        [Test]
        public void MapRoles_MapsRoleToRoleView()
        {
            var expected = ObjectFactory.CreateRole();
            expected.RolePrivileges = new List<RolePrivilege>() { ObjectFactory.CreateRolePrivilege() };

            var actual = Mapper.Map<Role, RoleView>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNotNull(actual.PrivilegesTree);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.RolePrivileges[0].Id, actual.RolePrivileges[0].Id);
            Assert.AreEqual(expected.RolePrivileges[0].RoleId, actual.RolePrivileges[0].RoleId);
            Assert.AreEqual(expected.RolePrivileges[0].PrivilegeId, actual.RolePrivileges[0].PrivilegeId);
        }

        [Test]
        public void MapRoles_MapsRoleViewToRole()
        {
            var expected = ObjectFactory.CreateRoleView();
            expected.RolePrivileges.Add(ObjectFactory.CreateRolePrivilegeView());

            var actual = Mapper.Map<RoleView, Role>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.IsNull(actual.RolePrivileges[0].Role);
            Assert.IsNull(actual.RolePrivileges[0].Privilege);
            Assert.AreEqual(expected.RolePrivileges[0].Id, actual.RolePrivileges[0].Id);
            Assert.AreEqual(expected.RolePrivileges[0].RoleId, actual.RolePrivileges[0].RoleId);
            Assert.AreEqual(expected.RolePrivileges[0].PrivilegeId, actual.RolePrivileges[0].PrivilegeId);
        }

        [Test]
        public void MapRoles_MapsRolePrivilegeToRolePrivilegeView()
        {
            var expected = ObjectFactory.CreateRolePrivilege();
            expected.Privilege = ObjectFactory.CreatePrivilege();
            expected.PrivilegeId = expected.Privilege.Id;
            expected.RoleId = expected.Id;

            var actual = Mapper.Map<RolePrivilege, RolePrivilegeView>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.PrivilegeId, actual.PrivilegeId);
            Assert.AreEqual(expected.Privilege.Id, actual.Privilege.Id);
            Assert.AreEqual(expected.Privilege.Area, actual.Privilege.Area);
            Assert.AreEqual(expected.Privilege.Action, actual.Privilege.Action);
            Assert.AreEqual(expected.Privilege.Controller, actual.Privilege.Controller);
        }

        [Test]
        public void MapRoles_MapsRolePrivilegeViewToRolePrivilege()
        {
            var expected = ObjectFactory.CreateRolePrivilegeView();
            expected.Privilege = ObjectFactory.CreatePrivilegeView();
            expected.PrivilegeId = expected.Privilege.Id;

            var actual = Mapper.Map<RolePrivilegeView, RolePrivilege>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.PrivilegeId, actual.PrivilegeId);
            Assert.AreEqual(expected.Privilege.Id, actual.Privilege.Id);
            Assert.AreEqual(expected.Privilege.Area, actual.Privilege.Area);
            Assert.AreEqual(expected.Privilege.Action, actual.Privilege.Action);
            Assert.AreEqual(expected.Privilege.Controller, actual.Privilege.Controller);
        }

        [Test]
        public void MapRoles_MapsPrivilegeToPrivilegeView()
        {
            var expected = ObjectFactory.CreatePrivilege();
            var actual = Mapper.Map<Privilege, PrivilegeView>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Area, actual.Area);
            Assert.AreEqual(expected.Action, actual.Action);
            Assert.AreEqual(expected.Controller, actual.Controller);
        }

        [Test]
        public void MapRoles_MapsPrivilegeViewToPrivilege()
        {
            var expected = ObjectFactory.CreatePrivilegeView();
            var actual = Mapper.Map<PrivilegeView, Privilege>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Area, actual.Area);
            Assert.AreEqual(expected.Action, actual.Action);
            Assert.AreEqual(expected.Controller, actual.Controller);
        }

        #endregion

        #region Static method: MapSystem()

        [Test]
        public void MapSystem_MapsLanguageTolanguageView()
        {
            var expected = ObjectFactory.CreateLanguage();
            var actual = Mapper.Map<Language, LanguageView>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Abbreviation, actual.Abbreviation);
        }

        [Test]
        public void MapRoles_MapsLanguageViewToLanguage()
        {
            var expected = ObjectFactory.CreateLanguageView();
            var actual = Mapper.Map<LanguageView, Language>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Abbreviation, actual.Abbreviation);
        }

        #endregion
    }
}
