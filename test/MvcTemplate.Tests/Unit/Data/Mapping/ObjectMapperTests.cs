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

        #endregion
    }
}
