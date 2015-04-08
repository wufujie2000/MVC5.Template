using AutoMapper;
using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;
using Xunit;

namespace MvcTemplate.Tests.Unit.Data.Mapping
{
    public class ObjectMapperTests
    {
        static ObjectMapperTests()
        {
            ObjectMapper.MapObjects();
        }

        #region Static method: MapAccounts()

        [Fact]
        public void MapAccounts_MapsAccountToAccountView()
        {
            Account model = ObjectFactory.CreateAccount();
            model.Role = ObjectFactory.CreateRole();
            model.RoleId = model.Role.Id;

            AccountView actual = Mapper.Map<AccountView>(model);
            Account expected = model;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Role.Name, actual.RoleName);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Null(actual.Password);
        }

        [Fact]
        public void MapAccounts_MapsAccountToAccountEditView()
        {
            Account model = ObjectFactory.CreateAccount();
            model.Role = ObjectFactory.CreateRole();
            model.RoleId = model.Role.Id;

            AccountEditView actual = Mapper.Map<AccountEditView>(model);
            Account expected = model;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.RoleId, actual.RoleId);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void MapAccounts_MapsAccountToProfileEditView()
        {
            Account expected = ObjectFactory.CreateAccount();
            ProfileEditView actual = Mapper.Map<ProfileEditView>(expected);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Null(actual.NewPassword);
            Assert.Null(actual.Password);
        }

        #endregion

        #region Static method: MapRoles()

        [Fact]
        public void MapRoles_MapsRoleToRoleView()
        {
            Role expected = ObjectFactory.CreateRole();
            RoleView actual = Mapper.Map<RoleView>(expected);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.NotNull(actual.PrivilegesTree);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void MapRoles_MapsRoleViewToRole()
        {
            RoleView expected = ObjectFactory.CreateRoleView();
            Role actual = Mapper.Map<Role>(expected);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Null(actual.RolePrivileges);
        }

        #endregion
    }
}
