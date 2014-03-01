using AutoMapper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Template.Data.Mapping;
using Template.Objects;

namespace Template.Tests.Data.Mapping
{
    [TestFixture]
    public class ObjectMapTests
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
            var account = CreateAccount();
            var profileView = Mapper.Map<Account, ProfileView>(account);

            Assert.AreEqual(account.Id, profileView.Id);
            Assert.AreEqual(account.Username, profileView.Username);

            Assert.IsNull(profileView.CurrentPassword);
            Assert.IsNull(profileView.NewPassword);

            Assert.AreEqual(account.User.FirstName, profileView.UserFirstName);
            Assert.AreEqual(account.User.LastName, profileView.UserLastName);
            Assert.AreEqual(account.User.DateOfBirth, profileView.UserDateOfBirth);
        }

        [Test]
        public void MapUsers_MapsAccountToAccountView()
        {
            var account = CreateAccount();
            var accountView = Mapper.Map<Account, AccountView>(account);

            Assert.AreEqual(account.Id, accountView.Id);
            Assert.AreEqual(account.Username, accountView.Username);

            Assert.IsNull(accountView.Password);
            Assert.IsNull(accountView.NewPassword);
        }

        [Test]
        public void MapUsers_MapsAccountViewToAccount()
        {
            var accountView = CreateAccountView();
            var account = Mapper.Map<AccountView, Account>(accountView);

            Assert.AreEqual(accountView.Id, account.Id);
            Assert.AreEqual(accountView.Username, account.Username);

            Assert.IsNull(account.Passhash);
            Assert.IsNull(account.UserId);
            Assert.IsNull(account.User);
        }

        [Test]
        public void MapUsers_MapsAccountToUserView()
        {
            var account = CreateAccount();
            var userView = Mapper.Map<Account, UserView>(account);

            Assert.AreEqual(account.Id, userView.Id);
            Assert.AreEqual(account.User.FirstName, userView.UserFirstName);
            Assert.AreEqual(account.User.LastName, userView.UserLastName);
            Assert.AreEqual(account.User.DateOfBirth, userView.UserDateOfBirth);

            Assert.AreEqual(account.User.RoleId, userView.UserRoleId);
            Assert.AreEqual(account.User.Role.Id, userView.UserRoleId);
            Assert.AreEqual(account.User.Role.Name, userView.UserRoleName);

            Assert.AreEqual(account.Username, userView.Username);
            Assert.IsNull(userView.NewPassword);
            Assert.IsNull(userView.Password);
        }

        [Test]
        public void MapUsers_MapsUserViewToAccount()
        {
            var userView = CreateUserView();
            var account = Mapper.Map<UserView, Account>(userView);

            Assert.AreEqual(userView.Id, account.Id);
            Assert.AreEqual(userView.Id, account.UserId);
            Assert.AreEqual(userView.UserFirstName, account.User.FirstName);
            Assert.AreEqual(userView.UserLastName, account.User.LastName);
            Assert.AreEqual(userView.UserDateOfBirth, account.User.DateOfBirth);

            Assert.AreEqual(userView.UserRoleId, account.User.RoleId);
            Assert.AreEqual(userView.UserRoleId, account.User.Role.Id);
            Assert.AreEqual(userView.UserRoleName, account.User.Role.Name);

            Assert.AreEqual(userView.Username, account.Username);
            Assert.IsNull(account.User.Role.RolePrivileges);
            Assert.IsNull(account.Passhash);
        }

        [Test]
        public void MapUsers_MapsUserToUserView()
        {
            var user = CreateUser();
            var userView = Mapper.Map<User, UserView>(user);

            Assert.AreEqual(user.Id, userView.Id);
            Assert.AreEqual(user.FirstName, userView.UserFirstName);
            Assert.AreEqual(user.LastName, userView.UserLastName);
            Assert.AreEqual(user.DateOfBirth, userView.UserDateOfBirth);

            Assert.AreEqual(user.RoleId, userView.UserRoleId);
            Assert.AreEqual(user.Role.Name, userView.UserRoleName);

            Assert.IsNull(userView.Username);
            Assert.IsNull(userView.Password);
            Assert.IsNull(userView.NewPassword);
        }

        [Test]
        public void MapUsers_MapsUserViewToUser()
        {
            var userView = CreateUserView();
            var user = Mapper.Map<UserView, User>(userView);

            Assert.AreEqual(userView.Id, user.Id);
            Assert.AreEqual(userView.UserFirstName, user.FirstName);
            Assert.AreEqual(userView.UserLastName, user.LastName);
            Assert.AreEqual(userView.UserDateOfBirth, user.DateOfBirth);

            Assert.AreEqual(userView.UserRoleId, user.RoleId);
            Assert.AreEqual(userView.UserRoleId, user.Role.Id);
            Assert.AreEqual(userView.UserRoleName, user.Role.Name);

            Assert.IsNull(user.Role.RolePrivileges);
        }

        #endregion

        #region Static method: MapRoles()

        [Test]
        public void MapRoles_MapsRoleToRoleView()
        {
            var role = CreateRole();
            var roleView = Mapper.Map<Role, RoleView>(role);

            Assert.AreEqual(role.Id, roleView.Id);
            Assert.AreEqual(role.Name, roleView.Name);
            Assert.IsNotNull(roleView.PrivilegesTree);
            Assert.AreEqual(role.RolePrivileges[0].Id, roleView.RolePrivileges[0].Id);
            Assert.AreEqual(role.RolePrivileges[0].RoleId, roleView.RolePrivileges[0].RoleId);
            Assert.AreEqual(role.RolePrivileges[0].PrivilegeId, roleView.RolePrivileges[0].PrivilegeId);
        }

        [Test]
        public void MapRoles_MapsRoleViewToRole()
        {
            var roleView = CreateRoleView();
            var role = Mapper.Map<RoleView, Role>(roleView);

            Assert.AreEqual(roleView.Id, role.Id);
            Assert.AreEqual(roleView.Name, role.Name);
            Assert.IsNull(role.RolePrivileges[0].Role);
            Assert.IsNull(role.RolePrivileges[0].Privilege);
            Assert.AreEqual(roleView.RolePrivileges[0].Id, role.RolePrivileges[0].Id);
            Assert.AreEqual(roleView.RolePrivileges[0].RoleId, role.RolePrivileges[0].RoleId);
            Assert.AreEqual(roleView.RolePrivileges[0].PrivilegeId, role.RolePrivileges[0].PrivilegeId);
        }

        [Test]
        public void MapRoles_MapsRolePrivilegeToRolePrivilegeView()
        {
            var rolePrivilege = CreateRolePrivilege();
            var rolePrivilegeView = Mapper.Map<RolePrivilege, RolePrivilegeView>(rolePrivilege);

            Assert.AreEqual(rolePrivilege.Id, rolePrivilegeView.Id);
            Assert.AreEqual(rolePrivilege.RoleId, rolePrivilegeView.RoleId);
            Assert.AreEqual(rolePrivilege.PrivilegeId, rolePrivilegeView.PrivilegeId);
        }

        [Test]
        public void MapRoles_MapsRolePrivilegeViewToRolePrivilege()
        {
            var rolePrivilegeView = CreateRolePrivilegeView();
            var rolePrivilege = Mapper.Map<RolePrivilegeView, RolePrivilege>(rolePrivilegeView);

            Assert.AreEqual(rolePrivilegeView.Id, rolePrivilege.Id);
            Assert.AreEqual(rolePrivilegeView.RoleId, rolePrivilege.RoleId);
            Assert.AreEqual(rolePrivilegeView.PrivilegeId, rolePrivilege.PrivilegeId);
        }

        [Test]
        public void MapRoles_MapsPrivilegeToPrivilegeView()
        {
            var privilege = CreatePrivilege();
            var privilegeView = Mapper.Map<Privilege, PrivilegeView>(privilege);

            Assert.AreEqual(privilege.Id, privilegeView.Id);
            Assert.AreEqual(privilege.Area, privilegeView.Area);
            Assert.AreEqual(privilege.Controller, privilegeView.Controller);
            Assert.AreEqual(privilege.Action, privilegeView.Action);
        }

        [Test]
        public void MapRoles_MapsPrivilegeViewToPrivilege()
        {
            var privilegeView = CreatePrivilegeView();
            var privilege = Mapper.Map<PrivilegeView, Privilege>(privilegeView);

            Assert.AreEqual(privilegeView.Id, privilege.Id);
            Assert.AreEqual(privilegeView.Area, privilege.Area);
            Assert.AreEqual(privilegeView.Controller, privilege.Controller);
            Assert.AreEqual(privilegeView.Action, privilege.Action);
        }

        #endregion

        #region Test helpers

        private Account CreateAccount()
        {
            return new Account()
            {
                Id = "AccountId",
                Username = "User",
                Passhash = "Passhash",
                UserId = "UserId",

                User = new User()
                {
                    Id = "InnerUserId",
                    FirstName = "FirstName",
                    LastName = "LastName",
                    RoleId = "RoleId",

                    Role = new Role()
                    {
                        Id = "InnerRoleId",
                        Name = "RoleName"
                    }
                }
            };
        }
        private AccountView CreateAccountView()
        {
            return new AccountView()
            {
                Id = "AccountViewId",
                Username = "User",
                Password = "Password",
                NewPassword = "NewPassword"
            };
        }

        private User CreateUser()
        {
            return new User()
            {
                Id = "UserId",
                FirstName = "FirstName",
                LastName = "LastName",
                DateOfBirth = DateTime.Now,

                RoleId = "RoleId",

                Role = new Role()
                {
                    Id = "InnerRoleId",
                    Name = "RoleName"
                }
            };
        }
        private UserView CreateUserView()
        {
            return new UserView()
            {
                Id = "UserViewId",
                UserFirstName = "UserFirstName",
                UserLastName = "UserLastName",
                UserDateOfBirth = DateTime.Now,

                UserRoleId = "UserRoleId",
                UserRoleName = "UserRoleName",

                Username = "Username",
                Password = "Password",
                NewPassword = "NewPassword"
            };
        }

        private Role CreateRole()
        {
            return new Role()
            {
                Id = "RoleId",
                Name = "Name",

                RolePrivileges = new List<RolePrivilege>()
                {
                    new RolePrivilege()
                    {
                        Id = "RolePrivilegeId",
                        RoleId = "RoleId",
                        PrivilegeId = "PrivilegeId"
                    }
                }
            };
        }
        private RoleView CreateRoleView()
        {
            return new RoleView()
            {
                Id = "RoleViewId",
                Name = "Name",

                RolePrivileges = new List<RolePrivilegeView>()
                {
                    new RolePrivilegeView()
                    {
                        Id = "RolePrivilegeViewId",
                        RoleId = "RoleViewId",
                        PrivilegeId = "PrivilegeId"
                    }
                }
            };
        }

        private RolePrivilege CreateRolePrivilege()
        {
            return new RolePrivilege()
            {
                Id = "RolePrivilegeId",
                RoleId = "RoleId",
                PrivilegeId = "PrivilegeId"
            };
        }
        private RolePrivilegeView CreateRolePrivilegeView()
        {
            return new RolePrivilegeView()
            {
                Id = "RolePrivilegeViewId",
                RoleId = "RoleId",
                PrivilegeId = "PrivilegeId"
            };
        }

        private Privilege CreatePrivilege()
        {
            return new Privilege()
            {
                Id = "PrivilegeId",
                Area = "Area",
                Controller = "Controller",
                Action = "Action"
            };
        }
        private PrivilegeView CreatePrivilegeView()
        {
            return new PrivilegeView()
            {
                Id = "PrivilegeViewId",
                Area = "Area",
                Controller = "Controller",
                Action = "Action"
            };
        }

        #endregion
    }
}
