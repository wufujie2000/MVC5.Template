using AutoMapper;
using NUnit.Framework;
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

        #region Static method: MapAccounts()

        [Test]
        public void MapAccounts_MapsPersonToPersonView()
        {
            Person expected = ObjectFactory.CreatePerson();
            expected.Role = ObjectFactory.CreateRole();
            PersonView actual = Mapper.Map<Person, PersonView>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.LastName, actual.LastName);
            Assert.AreEqual(expected.Role.Name, actual.RoleName);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.DateOfBirth, actual.DateOfBirth);
        }

        [Test]
        public void MapAccounts_MapsPersonViewToPerson()
        {
            PersonView expected = ObjectFactory.CreatePersonView();
            Person actual = Mapper.Map<PersonView, Person>(expected);

            Assert.IsNull(actual.Role);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.LastName, actual.LastName);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.DateOfBirth, actual.DateOfBirth);
        }

        [Test]
        public void MapAccounts_MapsAccountToAccountView()
        {
            Account expected = ObjectFactory.CreateAccount();
            AccountView actual = Mapper.Map<Account, AccountView>(expected);

            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Password);
        }

        [Test]
        public void MapAccounts_MapsAccountViewToAccount()
        {
            AccountView expected = ObjectFactory.CreateAccountView();
            Account actual = Mapper.Map<AccountView, Account>(expected);

            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Passhash);
            Assert.IsNull(actual.PersonId);
            Assert.IsNull(actual.Person);
        }

        [Test]
        public void MapAccounts_MapsAccountToProfileView()
        {
            Account expected = ObjectFactory.CreateAccount();
            expected.Person = ObjectFactory.CreatePerson();

            ProfileView actual = Mapper.Map<Account, ProfileView>(expected);

            Assert.AreEqual(expected.Person.DateOfBirth, actual.Person.DateOfBirth);
            Assert.AreEqual(expected.Person.FirstName, actual.Person.FirstName);
            Assert.AreEqual(expected.Person.LastName, actual.Person.LastName);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.CurrentPassword);
            Assert.IsNull(actual.NewPassword);
        }

        [Test]
        public void MapAccounts_MapsProfileViewToAccount()
        {
            AccountView expected = ObjectFactory.CreateAccountView();
            Account actual = Mapper.Map<AccountView, Account>(expected);

            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.Passhash);
            Assert.IsNull(actual.PersonId);
            Assert.IsNull(actual.Person);
        }

        #endregion

        #region Static method: MapRoles()

        [Test]
        public void MapRoles_MapsRoleToRoleView()
        {
            Role expected = ObjectFactory.CreateRole();
            RoleView actual = Mapper.Map<Role, RoleView>(expected);

            Assert.IsNotNull(actual.PrivilegesTree);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [Test]
        public void MapRoles_MapsRoleViewToRole()
        {
            RoleView expected = ObjectFactory.CreateRoleView();
            Role actual = Mapper.Map<RoleView, Role>(expected);

            Assert.IsNull(actual.RolePrivileges);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [Test]
        public void MapRoles_MapsRolePrivilegeToRolePrivilegeView()
        {
            RolePrivilege expected = ObjectFactory.CreateRolePrivilege();
            expected.Privilege = ObjectFactory.CreatePrivilege();
            expected.PrivilegeId = expected.Privilege.Id;
            expected.RoleId = expected.Id;

            RolePrivilegeView actual = Mapper.Map<RolePrivilege, RolePrivilegeView>(expected);

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
            RolePrivilegeView expected = ObjectFactory.CreateRolePrivilegeView();
            expected.Privilege = ObjectFactory.CreatePrivilegeView();
            expected.PrivilegeId = expected.Privilege.Id;

            RolePrivilege actual = Mapper.Map<RolePrivilegeView, RolePrivilege>(expected);

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
            Privilege expected = ObjectFactory.CreatePrivilege();
            PrivilegeView actual = Mapper.Map<Privilege, PrivilegeView>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Area, actual.Area);
            Assert.AreEqual(expected.Action, actual.Action);
            Assert.AreEqual(expected.Controller, actual.Controller);
        }

        [Test]
        public void MapRoles_MapsPrivilegeViewToPrivilege()
        {
            PrivilegeView expected = ObjectFactory.CreatePrivilegeView();
            Privilege actual = Mapper.Map<PrivilegeView, Privilege>(expected);

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
            Language expected = ObjectFactory.CreateLanguage();
            LanguageView actual = Mapper.Map<Language, LanguageView>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Abbreviation, actual.Abbreviation);
        }

        [Test]
        public void MapRoles_MapsLanguageViewToLanguage()
        {
            LanguageView expected = ObjectFactory.CreateLanguageView();
            Language actual = Mapper.Map<LanguageView, Language>(expected);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Abbreviation, actual.Abbreviation);
        }

        #endregion
    }
}
